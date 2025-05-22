using MeetingsService.Core.Entities;
using MinimalMeet.Common.Interfaces;

namespace MeetingsService.Core.Interfaces;

public interface IFeedbacksRepository : IRepository<Feedback, int>
{
}
