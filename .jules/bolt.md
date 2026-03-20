## 2024-06-18 - EF Core Change Tracker Overhead on Read-Only Queries
**Learning:** By default, Entity Framework Core tracks all returned entities in its Change Tracker. For read-only views like dashboards or listing pages that don't modify data, this adds unnecessary memory allocation and CPU overhead.
**Action:** Always append `.AsNoTracking()` to LINQ queries when the fetched entities will only be read and not updated, making the queries faster and more memory-efficient.
