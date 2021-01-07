using BlogSite.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.DataAccess.Data.Repository.IRepository
{
    public interface IBlogRepository : IRepository<Blog>
    {
        void Update(Blog blog);
    }
}
