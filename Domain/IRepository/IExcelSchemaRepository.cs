using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.IRepository
{
    public interface IExcelSchemaRepository
    {
        List<ExcelSchemaModel> GetExcelColumnsDefinition(string schema);
    }
}
