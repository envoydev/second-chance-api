using SecondChance.Application.Services;

namespace SecondChance.Infrastructure.Services;

internal class DateTimeService : IDateTimeService
{
    public DateTime GetUtc()
    {
        return DateTime.UtcNow;
    }

    public int FromDateTimeToUnixTimeStamp(DateTime dateTime)
    {
        return (int)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public DateTime FromUnixTimeStampToDateTime(int unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);

        return dateTime;
    }
}