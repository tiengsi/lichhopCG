using System;
using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
    public class SettingDto
    {
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }

        public string SettingName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string SettingComment { get; set; }

        public ESettingTypeControl SettingTypeControl { get; set; }

        public int SortOrder { get; set; }
    }
}
