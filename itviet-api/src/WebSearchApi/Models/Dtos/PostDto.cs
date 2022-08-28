using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Dtos
{
    public class PostDto
    {
        public int PostId { get; set; }

        public string Title { get; set; }

        public string FilterTitle { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public int CategoryId { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public string ImagePath { get; set; }

        public string PublicId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class PostForCreateDto: PostDto
    {
        public List<IFormFile> Files { get; set; }
    }
}
