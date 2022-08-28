using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
    public class CategoryForSelectDto
    {
        public int CategoryId { set; get; }

        public string CategoryName { set; get; }
    }

    public class CategoryDto: CategoryForSelectDto
    {

        public ECategoryType TypeCode { get; set; }

        public int? ParentId { get; set; }

        public string CategoryCode { get; set; }

        public string Link { get; set; }

        public string Description { set; get; }

        public string Icon { get; set; }
    }
}
