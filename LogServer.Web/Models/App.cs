using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogServer.Web.Models
{


    /// <summary>
    /// 
    /// </summary>
    [Table("Apps")]
    public class App
    {

        /// <summary>
        /// 
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(200, MinimumLength = 5)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(400)]
        [Index(IsUnique = true)]
        public string AppPushKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(100)]
        public string Platform { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(AppVersion.App))]
        [JsonIgnore]
        public List<AppVersion> AppVersions { get; }
            = new List<AppVersion>();


        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(AppUser.App))]
        [JsonIgnore]
        public List<AppUser> AppUsers { get; }
            = new List<AppUser>();

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(LogGroup.App))]
        [JsonIgnore]
        public List<LogGroup> LogGroups { get; }
            = new List<LogGroup>();




        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string PushUrl {
            get;
            internal set;
        }
    }
}