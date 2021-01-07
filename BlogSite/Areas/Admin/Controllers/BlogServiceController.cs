using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogSite.DataAccess.Data.Repository.IRepository;
using BlogSite.Models.ViewModels;
using BlogSite.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlogServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Sp_Call.ReturnList<BlogDapperVM>(SD.usp_GetAllBlog, null) });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        {
            if(id != null)
            {
                return Json(new { data = _unitOfWork.Blog.GetFirstOrDefault(x => x.Id == id,includeProperties: "Category,ApplicationUser") });
            }

            return Json(new { data = "Id is required!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                var removingObj = _unitOfWork.Blog.GetFirstOrDefault(x => x.Id == id);
                if (removingObj != null)
                {
                    string webRootPath = _hostEnvironment.WebRootPath;
                    var imagePath = Path.Combine(webRootPath, removingObj.ImagePath.TrimStart('\\'));

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    _unitOfWork.Blog.Remove(removingObj);
                    _unitOfWork.Save();

                    return Json(new { success = true, message = "Delete successful." });
                }
                else
                {
                    return Json(new { success = false, message = "Error while deleting." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
        }
    }
}