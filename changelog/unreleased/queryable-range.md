type: feature

Add `WhereBetween` and `WhereInRange` for `IQueryable<T>`, which build provider-translatable expression trees (e.g. SQL `>`/`<` with parameterized bounds) instead of forcing client-side evaluation.
