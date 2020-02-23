using Domain;
using System.Collections.Generic;

namespace Domain
{
    public interface IScheduleRepository
    {
        List<CategoryModel> Categories { get; set; }
        List<ProductModel> Products { get; set; }
    }
}