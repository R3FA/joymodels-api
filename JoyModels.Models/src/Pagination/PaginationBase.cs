using Microsoft.EntityFrameworkCore;

namespace JoyModels.Models.Pagination;

public class PaginationBase<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public string? OrderBy { get; set; }
    public List<T> Data { get; set; }

    public PaginationBase(List<T> data, int totalRecords, int pageNumber, int pageSize, string? orderBy)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((decimal)totalRecords / pageSize);
        OrderBy = orderBy;
    }

    public static async Task<PaginationBase<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize,
        string? orderBy)
    {
        var totalRecords = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginationBase<T>(items, totalRecords, pageNumber, pageSize, orderBy);
    }
}