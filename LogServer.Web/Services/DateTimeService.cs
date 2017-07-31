using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogServer.Web.Services
{
    /// <summary>
    /// 
    /// </summary>
    [DIGlobal]
    public class DateTimeService
    {
        private static DateTimeService _Instnace;

        /// <summary>
        /// 
        /// </summary>
        public static DateTimeService Instance => _Instnace ?? (_Instnace = DI.Get<DateTimeService>());


        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime Now => DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime UtcNow => DateTime.UtcNow;

    }
}