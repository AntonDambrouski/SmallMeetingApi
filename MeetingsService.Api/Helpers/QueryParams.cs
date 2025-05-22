using Microsoft.AspNetCore.Mvc;

namespace MeetingsService.Api.Helpers;

public class QueryParams
{
    public int Take { get; set; } = 50;
    public int Skip { get; set; } = 0;
    public string? SearchTerm { get; set; }
}

public class LocationQueryParams : QueryParams
{
    [FromQuery(Name = "isLocationOnline")]
    public bool IsLocationOnline { get; set; }
}

