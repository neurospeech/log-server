using CoreDI;
using LogServer.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;
using LogServer.Web.Services;

namespace UnitTests
{
    public class BaseTest
    {
        private SQLiteConnection conn;

        public AppDbContext db { get; private set; }
        private string TempFile { get; set; }

        public T ExpectException<T>(Action action)
            where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                return ex;
            }
            throw new AssertFailedException($"Could not catch exception of type {typeof(T).Name}");
        }

        public async Task<T> ExpectExceptionAsync<T>(Func<Task> action)
            where T : Exception
        {
            try
            {
                await action();
            }
            catch (T ex)
            {
                return ex;
            }
            throw new AssertFailedException($"Could not catch exception of type {typeof(T).Name}");
        }

        public async Task<T> ExpectExceptionAsync<T>(Task action)
            where T : Exception
        {
            try
            {
                await action;
            }
            catch (T ex)
            {
                return ex;
            }
            catch (Exception ex)
            {
                throw new AssertFailedException($"Found exception {ex.GetType().FullName} instead of {typeof(T).Name}", ex);
            }
            throw new AssertFailedException($"Could not catch exception of type {typeof(T).Name}");
        }

        [TestInitialize]
        public void Init()
        {

            DI.RegisterGlobal<IAuthService, MemoryAuthService>();
            DI.RegisterGlobal<IHashService, HashService>();
            StartSqlServerLocalDB();
            //StartSQLite();




        }

        private static void StartSqlServerLocalDB()
        {
            AppDbContext.InitializeMigration(false);

            



            //var cnstr = new System.Data.SqlClient.SqlConnectionStringBuilder();
            //cnstr.AttachDBFilename = "db.mdf";
            //cnstr.DataSource = "(localdb)\\MSSQLLocalDB";
            //cnstr.InitialCatalog = "SideBusiness";

            var cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
            using (var conn = new System.Data.SqlClient.SqlConnection(cnstr))
            {
                Database.Delete(conn);
            }
            DI.RegisterScoped<AppDbContext, AppDbContext>();
        }

        private void StartSQLite()
        {
            var tmp = System.IO.Path.GetTempFileName();
            this.TempFile = tmp;
            var cnstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            cnstr.DataSource = tmp;


            this.conn = new System.Data.SQLite.SQLiteConnection(cnstr.ToString());
            db = new SqliteDbContext(conn);
            DI.ReplaceGlobal<AppDbContext, SqliteDbContext>(db);
        }

        [TestCleanup]
        public void Dispose()
        {
            ((IDisposable)db)?.Dispose();
            conn?.Dispose();
            if (this.TempFile != null)
            {
                System.IO.File.Delete(this.TempFile);
            }
        }
    }

    public class SqliteDbContext : AppDbContext {

        public SqliteDbContext(DbConnection conn) : base(conn)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            var i = new DbInitializer(modelBuilder);
            
            Database.SetInitializer(i);
        }
    }

    public class DbInitializer : SQLite.CodeFirst.SqliteDropCreateDatabaseWhenModelChanges<SqliteDbContext> {

        public DbInitializer(DbModelBuilder modelBuilder):base(modelBuilder)
        {
               
        }

    }
}
