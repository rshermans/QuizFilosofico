## 2024-05-24 - Database-Side Random Sorting in Entity Framework Core
**Learning:** Using `.AsEnumerable().OrderBy(x => random.Next())` causes client-side materialization of the entire table, leading to massive memory overhead and performance bottlenecks.
**Action:** Always use `.OrderBy(p => Guid.NewGuid())` for random selection in Entity Framework Core to keep sorting on the database side.
