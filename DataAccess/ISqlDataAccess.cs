using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ISqlDataAccess
    {
        string GetConnectionString();
        List<T> LoadData<T, U>(string sql, U parameters);
        void SaveData<T>(string sql, T parameters);

    }
}