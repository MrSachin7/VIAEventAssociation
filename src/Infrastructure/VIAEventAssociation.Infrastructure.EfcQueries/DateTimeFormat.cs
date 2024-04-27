using System.Globalization;

namespace VIAEventAssociation.Infrastructure.EfcQueries;

public static class DateTimeFormat {
    public const string DateFormat = "yyyy-MM-dd";
    public const string TimeFormat = @"hh\:mm";

    public static DateTime ParseFromJsonString(string jsonDateTime) {
        return DateTime.ParseExact(jsonDateTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
    }
    
}