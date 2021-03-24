using System;
using System.Text;
using System.Text.RegularExpressions;

namespace eStoryContainer.Core.Utils
{
    public static class StringExtension
    {
        public static string UTF8Convert(this String text)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = text.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static string ToRelativeDate(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return string.Format("{0} giây trước", timeSpan.Seconds);

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes > 1 ? String.Format("{0} phút trước", timeSpan.Minutes) : "1 phút trước";

            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours > 1 ? String.Format("{0} giờ trước", timeSpan.Hours) : "1 giờ trước";

            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days > 1 ? String.Format("{0} ngày trước", timeSpan.Days) : "hôm qua";

            if (timeSpan <= TimeSpan.FromDays(365))
                return timeSpan.Days > 30 ? String.Format("{0} tháng trước", timeSpan.Days / 30) : "1 tháng trước";

            return timeSpan.Days > 365 ? String.Format("{0} năm trước", timeSpan.Days / 365) : "1 năm trước";
        }

    }
}
