using MinimalMeet.Common.Enums;

namespace MinimalMeet.Common.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string? TimeZone { get; set; }
    public UserActivationStatuses ActivationStatus { get; set; }
    public UserRoles Role { get; set; }
}
