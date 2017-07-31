using NeuroSpeech.CoreDI;
using NeuroSpeech.EFLocalDBMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Model;
using Xunit.Abstractions;

namespace UnitTests
{

    public abstract class BaseTest : MockSqlDatabaseContext<MockAppDbContext, MockAppDbContextConfiguration>
    {

        private static object lockObject = new object();
        private static bool registered = false;
        public BaseTest(ITestOutputHelper writer)
        {
            this.Writer = writer;

            lock (lockObject) {
                if (!registered) {
                    DI.Register(typeof(BaseTest).Assembly);
                    registered = true;
                }
            }
        }

        protected override void DumpLogs()
        {
            this.Writer.WriteLine(base.GeneratedLog);
        }

        public ITestOutputHelper Writer { get; private set; }

        public override string Version => typeof(BaseTest).Assembly.GetName().Version.ToString();
    }
}
