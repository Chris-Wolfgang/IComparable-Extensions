type: fix

`WhereBetween` and `WhereInRange` on a `string` key now build a `string.CompareTo(string)` call that EF Core translates to SQL, instead of an `IComparable<string>` call that failed translation.
