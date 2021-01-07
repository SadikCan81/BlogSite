using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.DataAccess.Data.Repository.IRepository
{
    public interface ISp_Call : IDisposable
    {
        IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null);
    }
}
