using Abp.Application.Services;
using Abp.Domain.Repositories;
using GameXuaVN.Authorization.Roles;
using GameXuaVN.Categories.Dto;
using GameXuaVN.Entities;
using GameXuaVN.Games.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameXuaVN.Games
{

    public class GameAppService : AsyncCrudAppService<Game, GameDto, int, PagedGameResultRequestDto, CreateGameDto, GameDto>, IGameAppService
    {
        private readonly IRepository<Game, int> _gameRepository;

        public GameAppService(IRepository<Game, int> gameRepository)
            : base(gameRepository)
        {
            _gameRepository = gameRepository;
        }

 

        public async Task<ListGameResultRequestDto> GetListAsync(ListGameRequestDto request)
        {
            var query = (await _gameRepository.GetAllAsync())
                .Where(u =>
                    (request.CategoryId == -1 || u.CategoryId == request.CategoryId)
                    && (string.IsNullOrEmpty(request.Page) || u.Page == request.Page))
                .OrderBy(u => u.Name);
            var data = ObjectMapper.Map<List<GameDto>>(query);
            var dto = new ListGameResultRequestDto()
            {
                Data = data
            };

            return dto;
        }

        public async Task<ListGameResultRequestDto> GetTopLikeList(ListGameRequestDto request)
        {
            var query = (await _gameRepository.GetAllAsync())
                   .Where(u =>
                       (request.CategoryId == -1 || u.CategoryId == request.CategoryId)
                       && (string.IsNullOrEmpty(request.Page) || u.Page == request.Page))
                   .OrderByDescending(u => u.TotalLike).Take(request.MaxResultCount);
            var data = ObjectMapper.Map<List<GameDto>>(query);
            var dto = new ListGameResultRequestDto()
            {
                Data = data
            };

            return dto;
        }

        public async Task<ListGameResultRequestDto> GetTopPlayList(ListGameRequestDto request)
        {
            var query = (await _gameRepository.GetAllAsync())
                 .Where(u =>
                     (request.CategoryId == -1 || u.CategoryId == request.CategoryId)
                     && (string.IsNullOrEmpty(request.Page) || u.Page == request.Page))
                 .OrderByDescending(u => u.TotalPlay)
                 .Take(request.MaxResultCount);
            var data = ObjectMapper.Map<List<GameDto>>(query);
            var dto = new ListGameResultRequestDto()
            {
                Data = data
            };

            return dto;
        }
    }
}
