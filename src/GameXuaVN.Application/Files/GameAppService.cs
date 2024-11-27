using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using GameXuaVN.Authorization.Roles;
using GameXuaVN.Categories.Dto;
using GameXuaVN.Entities;
using GameXuaVN.Games.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections;
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


        public override async Task<PagedResultDto<GameDto>> GetAllAsync(PagedGameResultRequestDto input)
        {
            var query = (await _gameRepository.GetAllAsync())

                .Where(u =>
                    (input.CategoryId == -1 || u.CategoryId == input.CategoryId)
                    && (string.IsNullOrEmpty(input.PageName) || u.Page == input.PageName)
                    && (string.IsNullOrEmpty(input.Keyword) || (u.Name.Contains(input.Keyword))
                ));
            query = ApplySorting(query, input);
            var total = query.Count();
            query = ApplyPaging(query, input);
            var list = query.ToList();
            var result = new PagedResultDto<GameDto>(total, ObjectMapper.Map<List<GameDto>>(list));
            return result;
        }

        public override async Task<GameDto> UpdateAsync(GameDto input)
        {
            var existingGame = await _gameRepository.GetAsync(input.Id);
            existingGame.Name = input.Name;
            existingGame.Description = input.Description;
            existingGame.TotalPlay = input.TotalPlay;
            existingGame.TotalLike = input.TotalLike;
            existingGame.TotalDislike = input.TotalDislike;
            existingGame.ContentType = input.ContentType;
            existingGame.EmbedUrl = input.EmbedUrl;
            existingGame.Page = input.Name.Substring(0, 1).ToUpper();
            existingGame.CategoryId = input.CategoryId;

            // Update Thumbnail nếu không null
            if (input.Thumbnail != null && input.Thumbnail.Length > 0)
            {
                existingGame.Thumbnail = input.Thumbnail;
            }

            // Update Data nếu không null
            if (input.Data != null && input.Data.Length > 0)
            {
                existingGame.Data = input.Data;
            }

            var updateDto = MapToEntityDto(existingGame);
            return await base.UpdateAsync(updateDto);
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
