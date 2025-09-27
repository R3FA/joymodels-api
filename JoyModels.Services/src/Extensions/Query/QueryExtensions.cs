using System.Linq.Expressions;

namespace JoyModels.Services.Extensions.Query;

public static class QueryExtensions
{
    public static IQueryable<TEntity> OrderByProperty<TEntity>(this IQueryable<TEntity> query, string propertyName,
        bool ascending = true)
        where TEntity : class
    {
        var entityType = typeof(TEntity);
        var properties = propertyName.Split(".");
        var parameter = Expression.Parameter(entityType, "x");
        Expression propertyAccess = parameter;

        foreach (var name in properties)
        {
            var property = entityType.GetProperty(name);

            if (property == null)
                throw new ArgumentException($"Property '{name}' not found in type '{entityType.FullName}'");

            propertyAccess = Expression.Property(propertyAccess, property);
            entityType = property.PropertyType;
        }

        var lambda = Expression.Lambda(propertyAccess, parameter);
        var methodName = ascending ? "OrderBy" : "OrderByDescending";
        var methodCallExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(TEntity), propertyAccess.Type },
            query.Expression,
            Expression.Quote(lambda)
        );

        return query.Provider.CreateQuery<TEntity>(methodCallExpression);
    }
}