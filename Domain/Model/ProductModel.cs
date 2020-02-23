using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string ItemId { get; set; }
        public string Name { get; set; }
        public double PriceNet { get; set; }
        public double PriceGross { get; set; }
        public int Tax { get; set; }
        public CategoryModel PrimaryCategory { get; set; }
        public List<CategoryModel> CategoryList { get; set; }
    }
}
