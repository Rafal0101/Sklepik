using Domain.IRepository;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class ExcelSchemaRepository : IExcelSchemaRepository
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ExcelSchemaRepository(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public List<ExcelSchemaModel> GetExcelColumnsDefinition(string schema)
        {
            string sql = "SELECT ColumnName, ColumnId, ColumnTitle FROM [ReadFileDefinition] WHERE SchemaTitle = @SchemaTitle";
            var result = _sqlDataAccess.LoadData<ExcelSchemaModel, dynamic>(sql, new { SchemaTitle = schema });

            return result;
        }
    }
}
