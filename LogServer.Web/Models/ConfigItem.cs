using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LogServer.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("Configuration")]
    public class ConfigItem
    {

        /// <summary>
        /// 
        /// </summary>
        [Key, StringLength(400)]
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

    }
}