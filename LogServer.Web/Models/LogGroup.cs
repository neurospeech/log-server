using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogServer.Web.Models
{

    /// <summary>
    /// 
    /// </summary>
    [Table("LogGroups")]
    public class LogGroup
    {
        /// <summary>
        /// 
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogGroupID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Index("IX_LogGroups_LastUpdate", Order = 1)]
        public long AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Index("IX_LogGroups_LastUpdate", Order = 2)]
        public DateTime LastTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(Models.App.LogGroups))]
        [ForeignKey(nameof(AppID))]
        [JsonIgnore]
        public App App { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(LogItem.LogGroup))]
        [JsonIgnore]
        public List<LogItem> LogItems { get; set; }
            = new List<LogItem>();
    }
}