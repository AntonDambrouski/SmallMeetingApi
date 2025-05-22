using MeetingsService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingsService.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Meeting> SearchByTerm(this IQueryable<Meeting> query, string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return query;
        }

        term = term.Trim().ToLower();
        return query.Where(x =>
            EF.Functions.Like(x.Title.ToLower(), $"%{term}%")
            || x.Topics.Any(t => t.Name.ToLower() == term)
            || EF.Functions.Like(x.Location.Place.ToLower(), $"%{term}%"));
    }

    public static IQueryable<Topic> SearchByTerm(this IQueryable<Topic> query, string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return query;
        }

        term = term.Trim().ToLower();
        return query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{term}%"));
    }

    public static IQueryable<Location> SearchByTermAndType(this IQueryable<Location> query, string term, bool isOnline)
    {
        if (string.IsNullOrEmpty(term))
        {
            return query.Where(x => x.Online == isOnline);
        }

        term = term.Trim().ToLower();
        return query.Where(x => x.Online == isOnline && EF.Functions.Like(x.Place.ToLower(), $"%{term}%"));
    }
}

