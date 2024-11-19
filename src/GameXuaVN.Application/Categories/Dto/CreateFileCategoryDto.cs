using System.ComponentModel.DataAnnotations;

namespace GameXuaVN.Categories.Dto
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}
