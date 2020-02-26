using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface ICategoryRepository
    {
        List<CategoryModel> GetAll(string searchPattern = "");

        CategoryModel GetOne(object id);
        void AddCategory(CategoryModel category);
        void EditCategory(CategoryModel category);
        void DeleteCategory(CategoryModel category);

    }
}
