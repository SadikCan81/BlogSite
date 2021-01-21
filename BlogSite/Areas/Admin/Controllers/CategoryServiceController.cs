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
            var categoryList = _unitOfWork.Sp_Call.ReturnList<Category>(SD.usp_GetAllCategory, null);
            return Ok(categoryList);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        {
            if (id != null)
            {
                var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
                if(category != null)
                {
                    return Ok(category);
                }
                else
                {
                    return BadRequest(new { message = "Could not found any category for this id!" });
                }
            }

            return BadRequest(new { message = "Id is required!" });
        }       
    }
}