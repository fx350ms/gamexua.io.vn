using Abp.Application.Services;
using GameXuaVN.Categories.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services.Dto;
using GameXuaVN.Roles.Dto;

namespace GameXuaVN.Categories
{

    public interface ICategoryAppService : IApplicationService
    {
        Task Create(CreateCategoryDto input);
        Task<List<CategoryDto>> GetAllAsync();
    }
}
