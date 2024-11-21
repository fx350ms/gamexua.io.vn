﻿using GameXuaVN.Controllers;
using GameXuaVN.Games;
using GameXuaVN.Games.Dto;
using GameXuaVN.Web.Models.Games;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace GameXuaVN.Web.Controllers
{
    public class NesController : GameXuaVNControllerBase
    {

        private readonly IGameAppService _fileAppService;

        public NesController(IGameAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }


        public async Task<IActionResult> TopPlay()
        {
            return View();
        }


        public async Task<IActionResult> TopLike()
        {
            return View();
        }

        public async Task<IActionResult> TopPlayListItem(string page = "")
        {
            var dto = await _fileAppService.GetTopPlayList(new ListGameRequestDto()
            {
                CategoryId = -1,
                Page = page,
                MaxResultCount = 8
            });
            var model = new GameListViewModel()
            {
                Data = dto.Data,
            };
            return View(model);
        }

        public async Task<IActionResult> ListItem(string page = "#" )
        {
            var dto = await _fileAppService.GetListAsync(new ListGameRequestDto()
            {
                CategoryId = -1,
                Page = page,
            });
            var model = new GameListViewModel()
            {
                Data = dto.Data,
            };
            return View(model);
        }

        public async Task<IActionResult> Play(int id, string title)
        {
            var dto = await _fileAppService.GetAsync(new EntityDto<int>(id));
            var model = new PlayingGameViewModel()
            {
                Id = id,
                Data = dto
            };
            return View(model);
        }


        public async Task<IActionResult> Detail(int id, string title)
        {
            var dto = await _fileAppService.GetAsync(new EntityDto<int>(id));
            var topList = await _fileAppService.GetTopPlayList(new ListGameRequestDto()
            {
                MaxResultCount = 8
            });
            var model = new PlayingGameViewModel()
            {
                Id = id,
                Data = dto,
                TopList = topList.Data
            };
            return View(model);
        }
    }
}