using GameXuaVN.Games;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GameXuaVN.Controllers;
using System.Linq;
using GameXuaVN.Web.Models.File;
using GameXuaVN.Games.Dto;
using System.IO;


namespace GameXuaVN.Web.Controllers
{
    public class FileController : GameXuaVNControllerBase
    {
        private readonly IGameAppService _fileAppService;

        public FileController(IGameAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        
        public async Task<IActionResult> Index()
        {
            var model = new FileListViewModel
            {

            };
            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(CreateFileModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // Chuyển đổi file thành byte[]
        //    byte[] fileData = null;
        //    byte[] thumb = null;
        //    string contentType = null;
        //    if (model.FileData != null && model.FileData.Length > 0)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await model.FileData.CopyToAsync(memoryStream);
        //            fileData = memoryStream.ToArray();
        //            contentType = model.FileData.ContentType;
        //        }
        //    }


        //    if(model.Thumbnail != null && model.Thumbnail.Length > 0)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await model.Thumbnail.CopyToAsync(memoryStream);
        //            thumb = memoryStream.ToArray();
        //        }
        //    }    
        //    // Tạo đối tượng FileDto và gửi tới service để lưu vào DB
        //    var fileDto = new CreateFileDto
        //    {
        //        FileName = model.FileName,
        //        Description = model.Description,
        //        CategoryId = model.CategoryId,
        //        Data = fileData,
        //        Thumbnail = thumb,
        //        ContentType = contentType,
        //        DownloadCount = 0, // Bắt đầu với 0 lượt tải
        //        TotalRateCount = 0, // Bắt đầu với 0 lượt đánh giá
        //        TotalRate = 0,
        //        DownloadUrl = "", // Có thể thêm sau khi xử lý lưu trữ file thực tế
        //    };

        //    await _fileAppService.Create(fileDto);
        //    return RedirectToAction("Index"); // Redirect về danh sách file hoặc trang thành công
        //}

      

        //[HttpPost]
        //public async Task<IActionResult> Rate(int fileId, float rating)
        //{
        //    await _fileAppService.Rate(fileId, rating);
        //    return RedirectToAction("Index");
        //}
    }
}
