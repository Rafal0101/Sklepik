using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ProductRepository(ISqlDataAccess sqlDataAccess)
        {
             _sqlDataAccess = sqlDataAccess;
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
    }
}
