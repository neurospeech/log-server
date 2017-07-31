using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LogServer.Web.Models
{

    /// <summary>
    /// 
    /// </summary>
    [Table("AppVersions")]
    public class AppVersion
    {

        /// <summary>
        /// 
        /// </summary>
        [Key, Column(Order = 1)]
        public long AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key, Column(Order = 2)]
        public string Version { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(AppID))]
        [InverseProperty(nameof(Models.App.AppVersions))]
        [JsonIgnore]
        public App App { get; set; }

    }
}