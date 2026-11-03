type: docs

The NuGet package description now says the methods require `IComparable<T>`; it previously also promised non-generic `IComparable` support, which the `where T : IComparable<T>` constraint never offered.
