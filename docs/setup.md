# Development Setup Guide

This guide will help you set up your development environment to contribute to Wolfgang.Extensions.IComparable.

## Prerequisites

Before you begin, ensure you have the following installed:

### Required Software

1. **.NET 8.0 SDK** or later
   - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
   - Verify installation: `dotnet --version`

2. **Git**
   - Download from [git-scm.com](https://git-scm.com/)
   - Verify installation: `git --version`

3. **A Code Editor** (choose one):
   - [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community, Professional, or Enterprise)
   - [Visual Studio Code](https://code.visualstudio.com/) with C# extension
   - [JetBrains Rider](https://www.jetbrains.com/rider/)

### Recommended Tools

1. **ReportGenerator** (for code coverage reports)
   ```bash
   dotnet tool install -g dotnet-reportgenerator-globaltool
   ```

2. **DevSkim CLI** (for security scanning)
   ```bash
   dotnet tool install --global Microsoft.CST.DevSkim.CLI
   ```

## Getting the Source Code

### 1. Fork the Repository

1. Navigate to [https://github.com/Chris-Wolfgang/IComparable-Extensions](https://github.com/Chris-Wolfgang/IComparable-Extensions)
2. Click the **Fork** button in the upper right
3. Select your GitHub account as the destination

### 2. Clone Your Fork

```bash
# Clone your fork
git clone https://github.com/YOUR-USERNAME/IComparable-Extensions.git

# Navigate to the repository
cd IComparable-Extensions

# Add upstream remote
git remote add upstream https://github.com/Chris-Wolfgang/IComparable-Extensions.git
```

### 3. Verify Your Setup

```bash
# Check remotes
git remote -v

# Should show:
# origin    https://github.com/YOUR-USERNAME/IComparable-Extensions.git (fetch)
# origin    https://github.com/YOUR-USERNAME/IComparable-Extensions.git (push)
# upstream  https://github.com/Chris-Wolfgang/IComparable-Extensions.git (fetch)
# upstream  https://github.com/Chris-Wolfgang/IComparable-Extensions.git (push)
```

## Building the Project

### 1. Restore Dependencies

```bash
dotnet restore
```

### 2. Build the Solution

```bash
# Build in Debug mode
dotnet build

# Build in Release mode (recommended for testing)
dotnet build --configuration Release
```

Expected output:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

## Running Tests

### Run All Tests

```bash
dotnet test --configuration Release
```

### Run Tests with Code Coverage

```bash
# Run tests and collect coverage
dotnet test --configuration Release --collect:"XPlat Code Coverage" --results-directory "./TestResults"

# Generate coverage report (if ReportGenerator is installed)
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" \
                -targetdir:"CoverageReport" \
                -reporttypes:"Html;TextSummary"

# View the report
# Open CoverageReport/index.html in your browser
```

### Run Specific Test Projects

```bash
# Run only unit tests
dotnet test tests/Wolfgang.Extensions.IComparable.Tests/Wolfgang.Extensions.IComparable.Tests.csproj
```

## Code Quality Checks

### Run Security Scanning

```bash
# Run DevSkim security analysis
devskim analyze --source-code . -f text --output-file devskim-results.txt

# View results
cat devskim-results.txt
```

### Check Code Style

The project uses `.editorconfig` for consistent code style. Most IDEs will automatically apply these rules.

To manually check:
```bash
# Format code
dotnet format

# Verify formatting
dotnet format --verify-no-changes
```

## Running Benchmarks

If you're working on performance improvements:

```bash
# Navigate to benchmarks
cd benchmarks

# Run benchmarks
dotnet run --configuration Release
```

## Project Structure

```
IComparable-Extensions/
├── src/
│   └── Wolfgang.Extensions.IComparable/
│       ├── Wolfgang.Extensions.IComparable.csproj
│       └── IComparableExtensions.cs
├── tests/
│   └── Wolfgang.Extensions.IComparable.Tests/
│       ├── Wolfgang.Extensions.IComparable.Tests.csproj
│       └── IComparableExtensionsTests.cs
├── benchmarks/
│   └── (Benchmark projects)
├── examples/
│   └── (Example projects)
├── docs/
│   ├── introduction.md
│   ├── getting-started.md
│   ├── readme.md
│   └── setup.md
├── .github/
│   └── workflows/
│       └── pr.yaml (CI/CD pipeline)
├── .editorconfig (Code style rules)
├── .gitignore
├── IComparable Extensions.slnx (Solution file)
├── README.md
└── LICENSE
```

## Development Workflow

### 1. Create a Feature Branch

```bash
# Update your main branch
git checkout main
git pull upstream main

# Create a new feature branch
git checkout -b feature/your-feature-name
```

### 2. Make Your Changes

- Write code following the existing style
- Add or update tests for your changes
- Ensure tests pass: `dotnet test`
- Ensure coverage meets requirements (≥80%)

### 3. Commit Your Changes

```bash
# Stage your changes
git add .

# Commit with a descriptive message
git commit -m "Add feature: description of your changes"
```

### 4. Push and Create Pull Request

```bash
# Push to your fork
git push origin feature/your-feature-name
```

Then:
1. Go to your fork on GitHub
2. Click "Compare & pull request"
3. Fill out the PR template
4. Submit the pull request

## CI/CD Pipeline

The project uses GitHub Actions for continuous integration. On every pull request:

1. **Build Check**: Code is built in Release mode
2. **Test Execution**: All tests are run
3. **Code Coverage**: Coverage is collected and must be ≥80%
4. **Security Scan**: DevSkim analyzes for security vulnerabilities
5. **Artifacts**: Coverage reports and scan results are uploaded

### Local CI Simulation

To simulate the CI pipeline locally:

```bash
# Clean previous builds
dotnet clean

# Restore dependencies
dotnet restore

# Build in Release mode
dotnet build --no-restore --configuration Release

# Run tests with coverage
find ./tests -type f -name '*Test*.csproj' | while read proj; do
  dotnet test "$proj" --no-build --configuration Release \
    --collect:"XPlat Code Coverage" \
    --results-directory "./TestResults"
done

# Generate coverage report
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" \
                -targetdir:"CoverageReport" \
                -reporttypes:"Html;TextSummary;MarkdownSummaryGithub;CsvSummary"

# Run security scan
devskim analyze --source-code . -f text --output-file devskim-results.txt -E
```

## Troubleshooting

### Build Errors

**Problem**: `The SDK 'Microsoft.NET.Sdk' specified could not be found`
- **Solution**: Install .NET 8.0 SDK from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)

**Problem**: `Package restore failed`
- **Solution**: 
  ```bash
  dotnet nuget locals all --clear
  dotnet restore
  ```

### Test Failures

**Problem**: Tests pass locally but fail in CI
- **Solution**: Ensure you're building in Release mode: `dotnet test --configuration Release`

**Problem**: Code coverage below 80%
- **Solution**: Add tests for uncovered code paths

### Git Issues

**Problem**: Merge conflicts
- **Solution**:
  ```bash
  git fetch upstream
  git merge upstream/main
  # Resolve conflicts in your editor
  git add .
  git commit -m "Merge upstream changes"
  ```

## Editor Configuration

### Visual Studio Code

Recommended extensions:
- C# (Microsoft)
- C# Dev Kit (Microsoft)
- EditorConfig for VS Code

Settings (`.vscode/settings.json`):
```json
{
  "dotnet.defaultSolution": "IComparable Extensions.slnx",
  "editor.formatOnSave": true,
  "editor.codeActionsOnSave": {
    "source.fixAll": true
  }
}
```

### Visual Studio

The solution should work out of the box. Ensure:
- Code cleanup is configured to use `.editorconfig` rules
- Code analysis is enabled

### JetBrains Rider

Rider automatically respects `.editorconfig`. Additional settings:
- Enable "Reformat code on save"
- Enable "Optimize imports on save"

## Getting Help

If you encounter issues:

1. Check [existing GitHub issues](https://github.com/Chris-Wolfgang/IComparable-Extensions/issues)
2. Review the [CONTRIBUTING.md](../CONTRIBUTING.md) guide
3. Ask questions in a new GitHub issue

## Next Steps

- Read the [Contributing Guidelines](../CONTRIBUTING.md)
- Review the [Code of Conduct](../CODE_OF_CONDUCT.md)
- Explore the [examples](../examples/) folder
- Join the community discussions

Happy coding! 🚀
