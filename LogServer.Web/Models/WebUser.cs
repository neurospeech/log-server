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
    [Table("WebUsers")]
    public class WebUser
    {

        /// <summary>
        /// 
        /// </summary>
        [Key, StringLength(200, MinimumLength = 5)]
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(200)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(200)]
        [JsonIgnore]
        public string PasswordHash { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string Password { get;set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(AppUser.WebUser))]
        [JsonIgnore]
        public List<AppUser> AppUsers { get; }
            = new List<AppUser>();


    }


}