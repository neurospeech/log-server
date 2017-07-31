using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogServer.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("AppUsers")]
    public class AppUser
    {

        /// <summary>
        /// 
        /// </summary>
        [Key, Column(Order =1)]
        public long AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key, Column(Order = 2)]
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(Models.App.AppUsers))]
        [ForeignKey(nameof(AppID))]
        [JsonIgnore]
        public App App { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(Models.WebUser.AppUsers))]
        [ForeignKey(nameof(Username))]
        [JsonIgnore]
        public WebUser WebUser { get; set; }
    }
}