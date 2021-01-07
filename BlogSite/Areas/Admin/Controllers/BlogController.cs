using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogSite.DataAccess.Data.Repository.IRepository;
using BlogSite.Models;
using BlogSite.Models.ViewModels;
using BlogSite.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlogController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public BlogVM BlogVM { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            BlogVM = new BlogVM()
            {
                Blog = new Blog(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropdown()
            };

            if(id != null)
            {
                BlogVM.Blog = _unitOfWork.Blog.GetFirstOrDefault(x => x.Id == id);
            }

            return View(BlogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (!ModelState.IsValid)
            {
                BlogVM.CategoryList = _unitOfWork.Category.GetCategoryListForDropdown();

                return View(BlogVM);
            }

            string webRootPath = _hostEnvironment.WebRootPath;
            var files = Request.Form.Files;
            string active = Request.Form["rdActive"].ToString();

            if (BlogVM.Blog.Id == 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"images\blogs");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload,fileName + extension),FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                BlogVM.Blog.ImagePath = @"\images\blogs\" + fileName + extension;

                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                BlogVM.Blog.ApplicationUserId = claims.Value;
                if(active == SD.Active)
                {
                    BlogVM.Blog.IsPublished = true;
                }
                else
                {
                    BlogVM.Blog.IsPublished = false;
                }

                BlogVM.Blog.CreatedDate = DateTime.Now;

                _unitOfWork.Blog.Add(BlogVM.Blog);
            }
            else
            {
                var blogFromDb = _unitOfWork.Blog.GetFirstOrDefault(x => x.Id == BlogVM.Blog.Id);

                if(files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootPath, @"images\blogs");
                    var extension = Path.GetExtension(files[0].FileName);

                    var imagePath = Path.Combine(webRootPath, blogFromDb.ImagePath.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    BlogVM.Blog.ImagePath = @"\images\blogs\" + fileName + extension;
                }
                else
                {
                    BlogVM.Blog.ImagePath = blogFromDb.ImagePath;
                }

                if (active == SD.Active)
                {
                    BlogVM.Blog.IsPublished = true;
                }
                else
                {
                    BlogVM.Blog.IsPublished = false;
                }

                _unitOfWork.Blog.Update(BlogVM.Blog);
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        #region APICalls

        [HttpGet]       
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Sp_Call.ReturnList<BlogDapperVM>(SD.usp_GetAllBlog,null) });
        }

        [HttpDelete]
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

        #endregion
    }
}