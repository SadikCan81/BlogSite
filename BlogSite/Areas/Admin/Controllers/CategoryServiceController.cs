using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSite.DataAccess.Data.Repository.IRepository;
using BlogSite.Models;
using BlogSite.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Sp_Call.ReturnList<Category>(SD.usp_GetAllCategory, null) });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        {
            if (id != null)
            {
                return Json(new { data = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id) });
            }

            return Json(new { data = "Id is required!" });
        }

        [HttpDelete("{id}")]
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
    }
}