using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IBlogRepository Blog { get; }
        ISp_Call Sp_Call { get; }

        void Save();
    }
}
