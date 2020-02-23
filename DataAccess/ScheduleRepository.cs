using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public ScheduleRepository(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            Categories = _categoryRepository.GetAll();
            Products = _productRepository.GetAll();
        }

        public List<CategoryModel> Categories { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}
