using LogServer.Web.Models;
using NeuroSpeech.CoreDI;
using NeuroSpeech.EFLocalDBMock;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Model
{
    [DIScoped(typeof(AppDbContext))]
    public class MockAppDbContext: AppDbContext
    {

        public MockAppDbContext()
            : base(MockDatabaseContext.Current.ConnectionString)
        {

        }

    }

    public class MockAppDbContextConfiguration : DbMigrationsConfiguration<MockAppDbContext> {

        public MockAppDbContextConfiguration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }
    }
}
