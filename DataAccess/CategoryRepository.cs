using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{


    public class CategoryRepository : ICategoryRepository
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public CategoryRepository(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public void AddCategory(CategoryModel category)
        {
            string sql = "INSERT INTO Categories (Code, Name) VALUES (@Code, @Name)";
            _sqlDataAccess.SaveData(sql, new { category.Code, category.Name});
        }

        public void DeleteCategory(CategoryModel category)
        {
            string sql = "DELETE FROM Categories WHERE Id = @Id";
            _sqlDataAccess.SaveData(sql, new { category.Id });
        }

        public void EditCategory(CategoryModel category)
        {
            string sql = "UPDATE Categories SET Code = @Code, Name = @Name WHERE Id = @Id";
            _sqlDataAccess.SaveData(sql, new { category.Code, category.Name, category.Id });
        }

        public List<CategoryModel> GetAll()
        {
            string sql = "SELECT Id, Code, Name FROM Categories ORDER BY Code";
            return _sqlDataAccess.LoadData<CategoryModel, dynamic>(sql, new { });
        }

        public CategoryModel GetOne(object id)
        {
            string sql = "SELECT * FROM Category WHERE Id = @Id";
            return _sqlDataAccess.LoadData<CategoryModel, dynamic>(sql, new { Id = id }).FirstOrDefault();
   
        }

    }
}
