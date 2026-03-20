## 2024-05-24 - Avoid Client-Side Materialization for Random Sorting in EF Core
**Learning:** Using .AsEnumerable().OrderBy(x => random.Next()) in EF Core queries forces the entire table to be loaded into application memory before sorting. This creates a massive performance bottleneck.
**Action:** Use .OrderBy(p => Guid.NewGuid()) instead, which EF Core translates to a native SQL random sort (e.g., ORDER BY RANDOM()), shifting the workload to the database and preventing large memory allocations.
