using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApi.Models.Enums;

namespace WebApi.Models
{
    [Table("Setting")]
    public class SettingModel
    {
        [Key]
        public string SettingKey { get; set; }

        [AllowHtml]
        public string SettingValue { get; set; }

        [StringLength(100, ErrorMessage = "Bạn chỉ được nhập tối đa 100 ký tự !")]
        public string SettingName { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(200, ErrorMessage = "Bạn chỉ được nhập tối đa 200 ký tự !")]
        public string SettingComment { get; set; }

        public ESettingTypeControl SettingTypeControl { get; set; }

        public int SortOrder { get; set; }
    }
}
