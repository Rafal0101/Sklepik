using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class CategoryModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}
