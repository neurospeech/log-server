using LogServer.Web.Services;
using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    [DIGlobal(typeof(DateTimeService))]
    public class MockDateTimeService: DateTimeService
    {

        public static DateTime Reference { get; set; } 
            = DateTime.Now;

        public override DateTime Now => Reference;

        public override DateTime UtcNow => Reference.ToUniversalTime(); 

    }
}
