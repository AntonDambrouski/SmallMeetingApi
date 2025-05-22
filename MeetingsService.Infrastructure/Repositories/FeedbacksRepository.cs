using MeetingsService.Core.Entities;
using MeetingsService.Core.Interfaces;
using MeetingsService.Infrastructure.Data;
using MinimalMeet.Common.Repositories;

namespace MeetingsService.Infrastructure.Repositories;

public class FeedbacksRepository(MeetingsContext context)
    : RepositoryBase<Feedback, int, MeetingsContext>(context), IFeedbacksRepository
{
}
