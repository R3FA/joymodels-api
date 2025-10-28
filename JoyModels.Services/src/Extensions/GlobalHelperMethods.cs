using JoyModels.Services.Extensions.Query;

namespace JoyModels.Services.Extensions;

public static class GlobalHelperMethods<TEntity>
    where TEntity : class
{
    public static IQueryable<TEntity> OrderBy(IQueryable<TEntity> source, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return source;

        var orderByParts = orderBy.Split(":");
        if (orderByParts.Length < 2)
            throw new ArgumentException("You must pass both a property and direction!");

        var orderByProperty = orderByParts[0];
        var orderByDirection = orderByParts[1];

        var response = orderByDirection switch
        {
            "asc" => source.OrderByProperty(orderByProperty),
            "desc" => source.OrderByProperty(orderByProperty, false),
            _ => throw new ArgumentException("OrderBy must be asc or desc")
        };

        return response;
    }
}