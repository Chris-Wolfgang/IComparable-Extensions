using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit.Abstractions;

namespace Wolfgang.Extensions.IComparable.Tests.DocExamples;

/// <summary>
/// Extracts every <c>&lt;example&gt;&lt;code&gt;...&lt;/code&gt;&lt;/example&gt;</c> block
/// from the src project's XML doc comments and compiles each one against the real library
/// assembly. Fails on any error-severity Roslyn diagnostic — so an example that references
/// a renamed / removed API breaks the build the same day the API changes, instead of
/// drifting silently for months.
/// </summary>
public sealed class DocExampleRotTests
{
    public static IEnumerable<object[]> Examples() =>
        DocExampleSource.Extract().Select(e => new object[] { e });


    [Theory]
    [MemberData(nameof(Examples))]
    public void doc_example_compiles(DocExample example)
    {
        ArgumentNullException.ThrowIfNull(example);

        var diagnostics = DocExampleCompiler.Compile(example);
        var errors = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();

        Assert.True
        (
            errors.Length == 0,
            $"Example at {example.File}:{example.Line} failed to compile:\n" +
            string.Join(Environment.NewLine, errors.Select(d => "  " + d))
        );
    }


    [Fact]
    public void at_least_one_example_was_extracted()
    {
        // Floor guard — a broken extractor would silently yield zero examples,
        // and a vacuously-passing Theory would look green. Explicitly assert
        // that the scan found the examples we know exist in the src project.
        var count = DocExampleSource.Extract().Count();
        Assert.True
        (
            count >= 2,
            $"Expected at least 2 <example> blocks in the src project; extractor found {count}."
        );
    }
}


/// <summary>
/// Locates the src project relative to the test host and enumerates every
/// <c>&lt;example&gt;&lt;code&gt;</c> block in its .cs files.
/// </summary>
internal static class DocExampleSource
{
    public static IEnumerable<DocExample> Extract()
    {
        var srcDir = FindSrcDirectory();
        var binSeg = $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
        var objSeg = $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}";
        var files = Directory.EnumerateFiles(srcDir, "*.cs", SearchOption.AllDirectories)
                             .Where(p => !p.Contains(binSeg, StringComparison.Ordinal) &&
                                         !p.Contains(objSeg, StringComparison.Ordinal))
                             .OrderBy(p => p, StringComparer.Ordinal);

        foreach (var file in files)
        {
            foreach (var example in ExtractFromFile(file))
            {
                yield return example;
            }
        }
    }


    private static IEnumerable<DocExample> ExtractFromFile(string file)
    {
        var lines = File.ReadAllLines(file);
        var inExample = false;
        var inCode = false;
        var codeStartLine = 0;
        var buffer = new StringBuilder();

        for (var i = 0; i < lines.Length; i++)
        {
            var trimmed = lines[i].TrimStart();
            if (!trimmed.StartsWith("///", StringComparison.Ordinal))
            {
                continue;
            }

            var content = trimmed.Length > 3 && trimmed[3] == ' '
                ? trimmed.Substring(4)
                : trimmed.Substring(3);

            if (!inExample)
            {
                if (content.Contains("<example>", StringComparison.Ordinal))
                {
                    inExample = true;
                }
                continue;
            }

            if (content.Contains("</example>", StringComparison.Ordinal))
            {
                inExample = false;
                inCode = false;
                continue;
            }

            if (!inCode)
            {
                if (content.Contains("<code>", StringComparison.Ordinal))
                {
                    inCode = true;
                    codeStartLine = i + 2; // 1-based, first content line follows the opening tag
                    buffer.Clear();
                }
                continue;
            }

            if (content.Contains("</code>", StringComparison.Ordinal))
            {
                var snippet = WebUtility.HtmlDecode(buffer.ToString());
                yield return new DocExample(file, codeStartLine, snippet);
                inCode = false;
                buffer.Clear();
                continue;
            }

            buffer.AppendLine(content);
        }
    }


    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "The prose comment describes the design rationale for why the walk uses AppContext.BaseDirectory rather than CallerFilePath; it is not commented-out code, but the analyzer's heuristic pattern-matches the member-access + semicolon shape.")]
    private static string FindSrcDirectory()
    {
        // Walk up from AppContext.BaseDirectory looking for a directory that
        // contains src/Wolfgang.Extensions.IComparable/Wolfgang.Extensions.IComparable.csproj.
        // AppContext.BaseDirectory is deterministic under CI's remapped paths;
        // the CallerFilePath attribute would bake in the build-machine path and
        // resolve to /_/ prefixes under CI deterministic builds.
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "src", "Wolfgang.Extensions.IComparable");
            if (Directory.Exists(candidate) &&
                File.Exists(Path.Combine(candidate, "Wolfgang.Extensions.IComparable.csproj")))
            {
                return candidate;
            }
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException
        (
            "Could not locate src/Wolfgang.Extensions.IComparable/ walking up from " + AppContext.BaseDirectory
        );
    }
}


/// <summary>
/// Wraps a snippet in a compilable harness and runs the Roslyn C# compiler over it against
/// the real library's assembly + the test host's trusted-platform-assemblies closure.
/// </summary>
internal static class DocExampleCompiler
{
    public static IReadOnlyList<Diagnostic> Compile(DocExample example)
    {
        ArgumentNullException.ThrowIfNull(example);

        // Split the snippet into its leading `using` directives (which must live at
        // the top of the compilation unit) and the executable body (which must live
        // inside a method). `#line` remaps compiler diagnostics onto the original
        // doc-comment location so any error names the right file:line, not our wrapper.
        var (usings, body, bodyLineOffset) = SplitUsingsAndBody(example.Code);
        var bodyLine = example.Line + bodyLineOffset;
        var fileForwardSlash = example.File.Replace('\\', '/');

        var source = new StringBuilder();
        foreach (var u in usings)
        {
            source.AppendLine(u);
        }
        source.AppendLine("namespace __DocExamples");
        source.AppendLine("{");
        source.AppendLine("    internal static class __Wrapper");
        source.AppendLine("    {");
        source.AppendLine("        internal static void __Run()");
        source.AppendLine("        {");
        source.AppendLine($"#line {bodyLine} \"{fileForwardSlash}\"");
        source.Append(body);
        source.AppendLine();
        source.AppendLine("#line default");
        source.AppendLine("        }");
        source.AppendLine("    }");
        source.AppendLine("}");

        var syntax = CSharpSyntaxTree.ParseText(source.ToString());

        var references = BuildReferences();
        var compilation = CSharpCompilation.Create
        (
            assemblyName: "DocExampleCompilation_" + Guid.NewGuid().ToString("N"),
            syntaxTrees: new[] { syntax },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        return compilation.GetDiagnostics();
    }


    private static (IReadOnlyList<string> Usings, string Body, int BodyLineOffset) SplitUsingsAndBody(string code)
    {
        var lines = code.Split('\n');
        var usings = new List<string>();
        var bodyStart = 0;

        for (var i = 0; i < lines.Length; i++)
        {
            var trimmed = lines[i].TrimStart();
            var trimmedEnd = lines[i].TrimEnd();
            if (trimmed.StartsWith("using ", StringComparison.Ordinal) && trimmedEnd.EndsWith(";", StringComparison.Ordinal))
            {
                usings.Add(trimmedEnd);
                bodyStart = i + 1;
                continue;
            }
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                // Allow blank lines interspersed with the leading usings.
                if (usings.Count > 0 && bodyStart == i)
                {
                    bodyStart = i + 1;
                }
                continue;
            }
            break;
        }

        var body = string.Join("\n", lines.Skip(bodyStart));
        return (usings, body, bodyStart);
    }


    private static IReadOnlyList<MetadataReference> BuildReferences()
    {
        var references = new List<MetadataReference>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Test-host trusted-platform-assemblies gives us the BCL closure the test is running against.
        if (AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") is string tpa)
        {
            foreach (var path in tpa.Split(Path.PathSeparator))
            {
                if (string.IsNullOrWhiteSpace(path)) continue;
                if (!path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)) continue;
                if (!seen.Add(path)) continue;
                references.Add(MetadataReference.CreateFromFile(path));
            }
        }

        // The library under test — snippets `using Wolfgang.Extensions.IComparable;`
        // and then call IsBetween/IsInRange on real instances.
        var libAssembly = typeof(IComparableExtensions).Assembly.Location;
        if (!string.IsNullOrEmpty(libAssembly) && seen.Add(libAssembly))
        {
            references.Add(MetadataReference.CreateFromFile(libAssembly));
        }

        return references;
    }
}


/// <summary>
/// One extracted <c>&lt;example&gt;&lt;code&gt;</c> block. Serializable so xunit can name
/// the theory case with the source location.
/// </summary>
public sealed class DocExample : IXunitSerializable
{
    // Parameterless ctor required by IXunitSerializable.
    public DocExample()
    {
        File = string.Empty;
        Code = string.Empty;
    }


    public DocExample(string file, int line, string code)
    {
        File = file ?? throw new ArgumentNullException(nameof(file));
        Line = line;
        Code = code ?? throw new ArgumentNullException(nameof(code));
    }


    public string File { get; private set; }

    public int Line { get; private set; }

    public string Code { get; private set; }


    public void Serialize(IXunitSerializationInfo info)
    {
        ArgumentNullException.ThrowIfNull(info);
        info.AddValue(nameof(File), File);
        info.AddValue(nameof(Line), Line);
        info.AddValue(nameof(Code), Code);
    }


    public void Deserialize(IXunitSerializationInfo info)
    {
        ArgumentNullException.ThrowIfNull(info);
        File = info.GetValue<string>(nameof(File));
        Line = info.GetValue<int>(nameof(Line));
        Code = info.GetValue<string>(nameof(Code));
    }


    public override string ToString() =>
        $"{Path.GetFileName(File)}:{Line}";
}
