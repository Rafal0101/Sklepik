using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Domain
{
    public interface IProductRepository
    {
        List<ProductModel> GetAll(string searchPattern = "");
        void AddProduct(ProductModel product);
        void EditProduct(ProductModel product);
        void DeleteProduct(ProductModel product);
        int ImportProductFromFile(MemoryStream memoryStream, bool purgeCategories = true, bool purgeProducts = true);
    }
}
