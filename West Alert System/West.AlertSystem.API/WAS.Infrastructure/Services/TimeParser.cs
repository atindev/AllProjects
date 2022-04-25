using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class TimeParser : ITimeParser
    {
        /// <summary>
        /// Get relative time, like 2 hours ago, 24 day ago 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string RelativeTime(DateTime dateTime)
        {
            var differenceHours = Math.Floor((DateTime.UtcNow - dateTime).TotalHours);
            var differenceMinutes = Math.Floor((DateTime.UtcNow - dateTime).TotalMinutes);

            if (differenceHours < 1 && differenceMinutes < 1)
                return "Just now";
            else if (differenceHours < 1 && differenceMinutes <= 59)
                return differenceMinutes + ((differenceMinutes == 1) ? " minute ago" : " minutes ago");
            else if (differenceHours <= 23)
                return differenceHours + ((differenceHours == 1) ? " hour ago" : " hours ago");
            else
            {
                int daycount = (int)(differenceHours / 24);
                return daycount + ((daycount == 1) ? " day ago" : " days ago");
            }
        }

        /// <summary>
        /// returning time based on seconds
        /// </summary>
        /// <param name="totalseconds"></param>
        /// <returns></returns>
        public string GetTime(int totalseconds)
        {
            int hours = totalseconds / 3600;
            int minutes = (totalseconds % 3600) / 60;
            int seconds = (totalseconds % 60);

            if (hours > 0)
                return String.Format("{0} hours, {1} minutes, {2} seconds", hours, minutes, seconds);
            else if (minutes > 0)
                return String.Format("{0} minutes, {1} seconds", minutes, seconds);
            else
                return String.Format("{0} seconds", seconds);
        }
    }
}
