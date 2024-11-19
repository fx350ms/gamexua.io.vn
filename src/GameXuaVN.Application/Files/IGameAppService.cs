using Abp.Application.Services;
using GameXuaVN.Categories.Dto;
using GameXuaVN.Games.Dto;
using GameXuaVN.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameXuaVN.Games
{
    public interface IGameAppService : IAsyncCrudAppService<GameDto, int, PagedGameResultRequestDto, CreateGameDto, GameDto>
    {
        //  Task<FileDto> GetOneAsync(int id);
        Task<ListGameResultRequestDto> GetListAsync(ListGameRequestDto request);


        Task<ListGameResultRequestDto> GetTopPlayList(ListGameRequestDto request);


        Task<ListGameResultRequestDto> GetTopLikeList(ListGameRequestDto request);
    }
}
