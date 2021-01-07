using BlogSite.DataAccess.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Blog = new BlogRepository(_db);
            Sp_Call = new Sp_Call(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public IBlogRepository Blog { get; private set; }
        public ISp_Call Sp_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
