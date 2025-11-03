namespace CorrespondenceTracker.Shared.Extensions
{
    public static class DateExtensions
    {
        public static string FormatDate(this DateOnly input)
        {
            var year = input.Year;
            var month = input.Month;
            var day = input.Day;
            return $"{year}/{month}/{day}";
        }

        public static string FormatDate(this DateOnly? input)
        {
            if (!input.HasValue) return "";
            DateOnly date = (DateOnly)input;
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            return $"{year}/{month}/{day}";
        }
    }
}
