using System.Linq.Expressions;

namespace MeetingsService.Core.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDesceding { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
