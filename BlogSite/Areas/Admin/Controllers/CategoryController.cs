using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSite.DataAccess.Data.Repository.IRepository;
using BlogSite.Models;
using BlogSite.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminRole)]
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();

            if(id != null)
            {
                category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            }

            return View(category); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if(category.Id == 0)
            {
                _unitOfWork.Category.Add(category);                
            }
            else
            {
                _unitOfWork.Category.Update(category);
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        #region APICalls

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Sp_Call.ReturnList<Category>(SD.usp_GetAllCategory,null) });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                _unitOfWork.Category.Remove(id);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Delete successful." });
            }
            else
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

        }

        #endregion
    }
}