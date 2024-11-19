using Abp.Application.Services;
using Abp.Domain.Repositories;
using GameXuaVN.Categories.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameXuaVN.Entities;

namespace GameXuaVN.Categories
{
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly IRepository<Category, int> _fileCategoryRepository;

        public CategoryAppService(IRepository<Category, int> fileRepository)
        {
            _fileCategoryRepository = fileRepository;
        }

        public async Task Create(CreateCategoryDto input)
        {
            var file = new Category
            {
                Name = input.Name,
                ParentId = input.ParentId
            };
            await _fileCategoryRepository.InsertAsync(file);
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var data = await _fileCategoryRepository.GetAllAsync();
            var result = ObjectMapper.Map<List<CategoryDto>>(data);
            return result;
        }
        
    }
}
