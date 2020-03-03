using Dapper;
using Domain;
using Domain.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly IExcelReaderService _excelReaderService;

        public ProductRepository(ISqlDataAccess sqlDataAccess, IExcelReaderService excelReaderService)
        {
            _sqlDataAccess = sqlDataAccess;
            _excelReaderService = excelReaderService;
        }
        public void AddProduct(ProductModel product)
        {
            string sql = "INSERT INTO Products (CategoryId, ItemId, Name, PriceNet, PriceGross, Tax) " +
                "VALUES (@CategoryId, @ItemId, @Name, @PriceNet, @PriceGross, @Tax)";

            _sqlDataAccess.SaveData(sql
                , new { product.CategoryId, product.ItemId, product.Name, product.PriceNet, product.PriceGross, product.Tax });
        }

        public void DeleteProduct(ProductModel product)
        {
            string sql = "DELETE FROM Products WHERE Id = @Id";
            _sqlDataAccess.SaveData(sql, new { product.Id });
        }

        public void EditProduct(ProductModel product)
        {
            string sql = "UPDATE Products SET " +
                "CategoryId = @CategoryId " +
                ", ItemId = @ItemId " +
                ", Name = @Name " +
                ", PriceNet = @PriceNet " +
                ", PriceGross = @PriceGross " +
                ", Tax = @Tax " +
                " WHERE Id = @Id";

            _sqlDataAccess.SaveData(sql
                , new { product.CategoryId, product.ItemId, product.Name, product.PriceNet, product.PriceGross, product.Tax, product.Id });
        }

        public List<ProductModel> GetAll(string searchPattern = "")
        {
            using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
            {
                string sql = "SELECT P.Id, P.CategoryId, P.ItemId, P.Name, P.PriceNet, P.PriceGross, P.Tax, " +
                    "C.Id, C.Code, C.Name " +
                    "FROM Products P LEFT JOIN Categories C ON C.Id = P.CategoryId " +
                    $"WHERE P.ItemId LIKE '%{searchPattern}%' OR P.Name LIKE '%{searchPattern}%' " +
                    "ORDER BY C.[Name], P.ItemId ";

                var result = cnn.Query<ProductModel, CategoryModel, ProductModel>(sql,
                    (product, category) => { product.PrimaryCategory = category; return product; });

                return result.ToList();
            }
        }

        public int ImportProductFromFile(MemoryStream fileBody, bool purgeCategories = true, bool purgeProducts = true)
        {
            #warning Dodac zrzut obu tabel do kopii

            List<ProductModel> products = _excelReaderService.ReadPriceListFile(fileBody);
            if (products != null && products.Count > 0)
            {
                using (IDbConnection cnn = new SqlConnection(_sqlDataAccess.GetConnectionString()))
                {
                    if (purgeProducts)
                    {
                        string sql = "DELETE FROM Products; DBCC CHECKIDENT ('[Products]', RESEED, 0);";
                        cnn.Execute(sql);
                    }
                    if (purgeCategories)
                    {
                        string sql = "DELETE FROM Categories; DBCC CHECKIDENT ('[Categories]', RESEED, 0);";
                        cnn.Execute(sql);
                    }

                    List<CategoryModel> categories = products.Where(x => x.ItemId == null).Select(x => new CategoryModel { Code = x.PrimaryCategory.Name, Name = x.PrimaryCategory.Name }).ToList();

                    foreach (var item in categories)
                    {
                        string sql = $"INSERT INTO Categories (Code, Name) VALUES ('{item.Code.Trim()}', '{item.Name.Trim()}')";
                        cnn.Execute(sql);
                    }

                    foreach (var item in products)
                    {
                        if (item.ItemId != null)
                        {
                            string name = item.Name == null ? "" : item.Name;
                            string sql = $"INSERT INTO Products (CategoryId, ItemId, Name, PriceGross) " +
                                $"VALUES (@CategoryId, @ItemId, @Name, @PriceGross)";
                            cnn.Execute(sql, new { CategoryId = item.PrimaryCategory.Id, ItemId = item.ItemId.Trim(), Name = name.Trim(), PriceGross = Math.Round(item.PriceGross, 2) });
                        }
                    }

                }
            }
            return products.Count;
        }
    }
}
