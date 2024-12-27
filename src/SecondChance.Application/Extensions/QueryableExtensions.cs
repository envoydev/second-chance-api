using System.Linq.Expressions;

namespace SecondChance.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
    
    public static IQueryable<T> TakeIf<T>(this IQueryable<T> source, bool condition, int count)
    {
        return condition ? source.Take(count) : source;
    }
    
    public static IQueryable<T> SkipIf<T>(this IQueryable<T> source, bool condition, int count)
    {
        return condition ? source.Skip(count) : source;
    }
}