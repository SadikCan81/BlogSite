using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSite.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var blogList = _unitOfWork.Blog.GetAll(includeProperties: "Category,ApplicationUser").Where(x=> x.IsPublished == true);

            return View(blogList);
        }

        public IActionResult Details(int id)
        {
            var blog = _unitOfWork.Blog.GetFirstOrDefault(x => x.Id == id);
            if(blog != null)
            {
                return View(blog);
            }

            return NotFound();
        }
    }
}