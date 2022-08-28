using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Dtos
{
    public class UploadFileInfoDto
  {
        public int? OrganizeId { set; get; }
        public int? UserId { set; get; }
        public string Mode { set; get; }

        public List<IFormFile> FileUpload { set; get; }

       
    }
}
