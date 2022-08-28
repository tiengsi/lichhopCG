using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebApi.Helpers.Extensions
{
    public static class RepositoryExtension
    {
        public static IQueryable<TEntity> OrderByCustom<TEntity>(this IQueryable<TEntity> source, string sortExpression) where TEntity : class
        {
            if (string.IsNullOrEmpty(sortExpression))
                return source;

            var entityType = typeof(TEntity);
            string ascSortMethodName = "OrderBy";
            string descSortMethodName = "OrderByDescending";
            string[] sortExpressionParts = sortExpression.Split(' ');
            string sortProperty = sortExpressionParts[0];
            string sortMethod = ascSortMethodName;

            if (sortExpressionParts.Length > 1 && sortExpressionParts[1] == "DESC")
                sortMethod = descSortMethodName;

            var property = entityType.GetProperty(sortProperty);
            if (property == null) return source;

            var parameter = Expression.Parameter(entityType, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            MethodCallExpression resultExp = Expression.Call(
                                                typeof(Queryable),
                                                sortMethod,
                                                new Type[] { entityType, property.PropertyType },
                                                source.Expression,
                                                Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<TEntity>(resultExp);
        }
    }
}
