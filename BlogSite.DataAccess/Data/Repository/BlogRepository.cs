using BlogSite.DataAccess.Data.Repository.IRepository;
using BlogSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSite.DataAccess.Data.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Blog blog)
        {
            var blogFromDb = _db.Blogs.FirstOrDefault(x => x.Id == blog.Id);

            if(blogFromDb != null)
            {
                blogFromDb.Title = blog.Title;
                blogFromDb.SubTitle = blog.SubTitle;
                blogFromDb.Content = blog.Content;
                blogFromDb.ImagePath = blog.ImagePath;
                blogFromDb.IsPublished = blog.IsPublished;
                blogFromDb.CategoryId = blog.CategoryId;
                blogFromDb.ApplicationUserId = blog.ApplicationUserId;

                _db.SaveChanges();
            }           
        }
    }
}
