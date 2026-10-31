#if NET8_0_OR_GREATER
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

/// <summary>
/// Runs <see cref="QueryableRangeExtensions.WhereBetween{TSource, TKey}"/> and
/// <see cref="QueryableRangeExtensions.WhereInRange{TSource, TKey}"/> through a real
/// EF Core provider (SQLite, in memory). EF Core throws when a <c>Where</c> predicate
/// cannot be translated, so a query that returns rows was evaluated in SQL, and
/// <c>ToQueryString</c> shows the comparison and the parameterized bounds.
/// </summary>
public sealed class QueryableRangeEfCoreTranslationTests
{
    [Fact]
    public async Task WhereBetween_when_key_is_int_is_evaluated_by_the_database()
    {
        await using var context = await CreateSeededContextAsync();

        var names = await context.Products
            .WhereBetween(p => p.Quantity, 10, 100)
            .OrderBy(p => p.Id)
            .Select(p => p.Name)
            .ToArrayAsync();

        Assert.Equal(new[] { "stapler" }, names);
    }



    [Fact]
    public async Task WhereInRange_when_key_is_int_is_evaluated_by_the_database()
    {
        await using var context = await CreateSeededContextAsync();

        var names = await context.Products
            .WhereInRange(p => p.Quantity, 10, 100)
            .OrderBy(p => p.Id)
            .Select(p => p.Name)
            .ToArrayAsync();

        Assert.Equal(new[] { "lamp", "ruler", "stapler" }, names);
    }



    [Fact]
    public async Task WhereInRange_when_key_is_DateTime_is_evaluated_by_the_database()
    {
        await using var context = await CreateSeededContextAsync();

        var names = await context.Products
            .WhereInRange(p => p.Added, new DateTime(2026, 4, 1), new DateTime(2026, 6, 30))
            .OrderBy(p => p.Id)
            .Select(p => p.Name)
            .ToArrayAsync();

        Assert.Equal(new[] { "lamp", "pencil", "ruler" }, names);
    }



    [Fact]
    public async Task WhereBetween_when_key_is_string_is_evaluated_by_the_database()
    {
        await using var context = await CreateSeededContextAsync();

        var names = await context.Products
            .WhereBetween(p => p.Name, "d", "r")
            .OrderBy(p => p.Id)
            .Select(p => p.Name)
            .ToArrayAsync();

        Assert.Equal(new[] { "lamp", "pencil" }, names);
    }



    [Fact]
    public async Task WhereInRange_when_key_is_string_is_evaluated_by_the_database()
    {
        await using var context = await CreateSeededContextAsync();

        var names = await context.Products
            .WhereInRange(p => p.Name, "lamp", "ruler")
            .OrderBy(p => p.Id)
            .Select(p => p.Name)
            .ToArrayAsync();

        Assert.Equal(new[] { "lamp", "pencil", "ruler" }, names);
    }



    [Fact]
    public async Task WhereBetween_when_key_has_operators_translates_to_parameterized_comparisons()
    {
        await using var context = await CreateSeededContextAsync();

        var sql = context.Products
            .WhereBetween(p => p.Quantity, 10, 100)
            .ToQueryString();

        Assert.Matches
        (
            @"WHERE ""p""\.""Quantity"" > @\w+ AND ""p""\.""Quantity"" < @\w+",
            sql
        );
    }



    [Fact]
    public async Task WhereInRange_when_key_has_operators_translates_to_inclusive_comparisons()
    {
        await using var context = await CreateSeededContextAsync();

        var sql = context.Products
            .WhereInRange(p => p.Quantity, 10, 100)
            .ToQueryString();

        Assert.Matches
        (
            @"WHERE ""p""\.""Quantity"" >= @\w+ AND ""p""\.""Quantity"" <= @\w+",
            sql
        );
    }



    [Fact]
    public async Task WhereBetween_when_key_is_string_translates_to_parameterized_comparisons()
    {
        await using var context = await CreateSeededContextAsync();

        var sql = context.Products
            .WhereBetween(p => p.Name, "d", "r")
            .ToQueryString();

        Assert.Matches
        (
            @"WHERE ""p""\.""Name"" > @\w+ AND ""p""\.""Name"" < @\w+",
            sql
        );
    }



    // Each test gets its own in-memory database; it lives as long as the open connection,
    // which the context owns and closes on dispose.
    private static async Task<ShopContext> CreateSeededContextAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var context = new ShopContext
        (
            connection,
            new DbContextOptionsBuilder<ShopContext>()
                .UseSqlite(connection)
                .Options
        );

        await context.Database.EnsureCreatedAsync();

        context.Products.AddRange
        (
            new Product { Id = 1, Name = "chair", Quantity = 150, Added = new DateTime(2026, 3, 31) },
            new Product { Id = 2, Name = "lamp", Quantity = 100, Added = new DateTime(2026, 4, 1) },
            new Product { Id = 3, Name = "pencil", Quantity = 5, Added = new DateTime(2026, 5, 15) },
            new Product { Id = 4, Name = "ruler", Quantity = 10, Added = new DateTime(2026, 6, 30) },
            new Product { Id = 5, Name = "stapler", Quantity = 50, Added = new DateTime(2026, 7, 1) }
        );

        await context.SaveChangesAsync();

        return context;
    }



    [ExcludeFromCodeCoverage]
    private sealed class ShopContext : DbContext
    {
        private readonly SqliteConnection _connection;



        public ShopContext(SqliteConnection connection, DbContextOptions<ShopContext> options)
            : base(options)
        {
            _connection = connection;
        }



        public DbSet<Product> Products => Set<Product>();



        public override void Dispose()
        {
            base.Dispose();
            _connection.Dispose();
        }



        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }



    [ExcludeFromCodeCoverage]
    private sealed class Product
    {
        public int Id { get; set; }



        public string Name { get; set; } = string.Empty;



        public int Quantity { get; set; }



        public DateTime Added { get; set; }
    }
}
#endif
