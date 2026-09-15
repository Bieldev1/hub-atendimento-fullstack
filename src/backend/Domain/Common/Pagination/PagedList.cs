namespace EnterpriseFlow.Domain.Common.Pagination;

/// <summary>
/// Representa uma coleção paginada de itens.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedList<T>
{
    public PagedList() { }

    public PagedList(List<T> items, int page, int itemsPerPage, int totalItems)
    {
        Items = items;
        Page = page;
        ItemsPerPage = itemsPerPage;
        TotalItems = totalItems;
        TotalPages = itemsPerPage == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)itemsPerPage);
    }

    public List<T> Items { get; set; } = new();

    public int Page { get; set; }

    public int ItemsPerPage { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages { get; set; }
}
