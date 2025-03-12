namespace Common.Helpers
{
    public class PaginatedList<T>(List<T> items, int totalItems, int currentPage, int pageSize)
    {
        public List<T> Items { get; } = items;
        public int TotalItems { get; } = totalItems;
        public int TotalPages { get; } = (int)Math.Ceiling((double)totalItems / pageSize);
        public int CurrentPage { get; } = currentPage;
        public int PageSize { get; } = pageSize;

        public static PaginatedList<T> Create(IQueryable<T> source, int page, int pageSize = 9)
        {
            var totalItems = source.Count();
            var items = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new PaginatedList<T>(items, totalItems, page, pageSize);
        }
    }
}