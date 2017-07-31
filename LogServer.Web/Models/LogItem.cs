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
    [Table("LogItems")]
    public class LogItem
    {
        /// <summary>
        /// 
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogItemID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Index("IX_LogItem_Time", Order = 1)]
        public long LogGroupID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [StringLength(400)]
        [Index("IX_LogItem_User")]
        public string User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(100)]
        public string UserIPAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Index("IX_LogItem_Time", Order = 2)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(200)]
        [Index("IX_LogItem_Application")]
        public string Application { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string Platform { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string PlatformVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string Device { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(10)]
        [Index("IX_LogItem_LogType")]
        public string LogType { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(10)]
        public string Method { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Headers { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }



        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(LogGroupID))]
        [InverseProperty(nameof(Models.LogGroup.LogItems))]
        [JsonIgnore]
        public LogGroup LogGroup { get; set; }

    }
}