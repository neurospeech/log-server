using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogServer.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonDate
    {
        /// <summary>
        /// 
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Date, Hours, Minutes, Seconds);
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ToDate()
        {
            return new DateTime(Year, Month, Date);
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ToUtcTime()
        {
            var dt = ToDateTime();
            return dt.AddMinutes(Offset);
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ToUtcTime(TimeZoneInfo timeZone)
        {
            var dt = ToDateTime();
            return TimeZoneInfo.ConvertTimeToUtc(dt, timeZone);
        }

    }
}