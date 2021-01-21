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
            var blogList = _unitOfWork.Sp_Call.ReturnList<BlogDapperVM>(SD.usp_GetAllBlog, null);
            return Ok(blogList);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        {
            if(id != null)
            {
                var blog = _unitOfWork.Blog.GetFirstOrDefault(x => x.Id == id, includeProperties: "Category,ApplicationUser");
                if(blog != null)
                {
                    return Ok(blog);
                }
                else
                {
                    return BadRequest(new { message = "Could not found any blog for this id!" });
                }
            }

            return BadRequest(new { message = "Id is required!" });
        }
    }
}