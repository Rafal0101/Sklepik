using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class ExcelSchemaRepository
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ExcelSchemaRepository(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public List<ExcelSchemaModel> GetExcelColumnsDefinition(string schema)
        {
            string sql = "SELECT ColumnName, ColumnId FROM [Accountancy].[CarrierFieldsSchemaDefinition] WHERE FileSchema = @FileSchema";
            var result = _sqlDataAccess.LoadData<ExcelSchemaModel, dynamic>(sql, new { FileSchema = schema });

            return result;
        }
    }
}
