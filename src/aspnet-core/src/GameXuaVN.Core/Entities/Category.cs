using Abp.Domain.Entities;

namespace GameXuaVN.Entities
{
    public class Category : Entity<int>
    {
        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}
