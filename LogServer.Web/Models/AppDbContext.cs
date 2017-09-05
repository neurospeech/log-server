using LogServer.Web.Services;
using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LogServer.Web.Models
{

    /// <summary>
    /// 
    /// </summary>
    [DIScoped(typeof(AppDbContext))]
    public class AppDbContext: DbContext
    {

        /// <summary>
        /// 
        /// </summary>
        public AppDbContext() : base("AppDbContext")
        {
            Configuration.ValidateOnSaveEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public AppDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configuration.ValidateOnSaveEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<WebUser> WebUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<App> Apps { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<AppVersion> AppVersions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<AppUser> AppUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<LogGroup> LogGroups { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<LogItem> LogItems { get; set; }

        /// <summary>
        /// Table for Configuration Values
        /// </summary>
        public DbSet<ConfigItem> ConfigItems { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public async Task<T> GetConfigurationAsync<T>(string name, T def = default(T)) {
            string v = await ConfigItems.Where(x => x.Key == name).Select(x => x.Value).FirstOrDefaultAsync();
            if (v == null)
                return def;
            Type type = typeof(T);
            if (type == typeof(string))
                return (T)(object)v;
            return (T)Convert.ChangeType(v,type);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                throw new InvalidOperationException(GetErrorMessage(ex.EntityValidationErrors));
            }
        }

        private static string GetErrorMessage(IEnumerable<DbEntityValidationResult> errors)
        {
            if (errors == null || !errors.Any())
                return null;
            var list = errors.Where(x => !x.IsValid)
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x =>
                                    $"{x.PropertyName} ({x.ErrorMessage})"
                                ).ToList();

            string message = string.Join(",", list);

            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alwaysCreate"></param>
        public static void InitializeMigration(bool alwaysCreate = false)
        {
            if (alwaysCreate)
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<AppDbContext>());
            }
            else
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, AppDbContextConfiguration>());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class AppDbContextConfiguration : DbMigrationsConfiguration<AppDbContext>
        {

            /// <summary>
            /// 
            /// </summary>
            public AppDbContextConfiguration()
            {
                AutomaticMigrationsEnabled = true;
                //AutomaticMigrationDataLossAllowed = true;

            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            protected override void Seed(AppDbContext context)
            {
                base.Seed(context);

                var hashService = DI.Get<IHashService>();

                if (!context.WebUsers.Any(x => x.IsAdmin == true))
                {
                    context.WebUsers.Add(new Models.WebUser
                    {
                        Username = "admin",
                        PasswordHash = hashService.Hash("exceptions"),
                        IsActive = true,
                        IsAdmin = true
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}