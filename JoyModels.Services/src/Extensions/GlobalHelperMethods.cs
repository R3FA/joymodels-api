using JoyModels.Services.Extensions.Query;

namespace JoyModels.Services.Extensions;

public static class GlobalHelperMethods<TEntity>
    where TEntity : class
{
    public static IQueryable<TEntity> OrderBy(IQueryable<TEntity> source, string? orderBy)
    {
        if (orderBy != null)
        {
            IQueryable<TEntity> response;

            var orderByParts = orderBy.Split(":");

            if (orderByParts.Length < 2)
                throw new ArgumentException("You must pass both a property and direction!");

            var orderByProperty = orderByParts[0];
            var orderByDirection = orderByParts[1];

            switch (orderByDirection)
            {
                case "asc":
                    response = source.OrderByProperty(orderByProperty);
                    break;
                case "desc":
                    response = source.OrderByProperty(orderByProperty, false);
                    break;
                default:
                    throw new ArgumentException("OrderBy must be asc or desc");
            }

            return response;
        }

        return source;
    }
}