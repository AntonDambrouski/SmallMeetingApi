namespace MinimalMeet.Common.Events;

public class UserDeactivatedAccountEvent : EventBase
{
    public Guid UserId { get; set; }
}
