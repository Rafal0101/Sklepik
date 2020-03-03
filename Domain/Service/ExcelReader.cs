using Domain.IRepository;
using Domain.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Domain.Service
{
    public class ExcelReader : IExcelReaderService
    {
        private readonly IExcelSchemaRepository _excelSchemaRepository;
        public ExcelReader(IExcelSchemaRepository excelSchemaRepository)
        {
            _excelSchemaRepository = excelSchemaRepository;
        }
        public List<ProductModel> ReadPriceListFile(MemoryStream fileBody)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            List<ProductModel> model = new List<ProductModel>();

            List<ExcelSchemaModel> columns = new List<ExcelSchemaModel>(_excelSchemaRepository.GetExcelColumnsDefinition("PriceList"));

            using (ExcelPackage xlPackage = new ExcelPackage(fileBody))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First();
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                string category = "";
                int categoryId = 0;
                for (int rowNum = 2; rowNum < totalRows; rowNum++)
                {
                    try
                    {

                        string testColumn = myWorksheet.Cells[rowNum, 1].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault();
                        if (testColumn.Length == 0)
                        {
                            category = myWorksheet.Cells[rowNum, columns.Where(x => x.ColumnTitle == "CategoryId").FirstOrDefault().ColumnId]
                                .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault();

                            model.Add(new ProductModel { PrimaryCategory = new CategoryModel { Name = category } });
                            categoryId++;
                        }
                        else
                        {
                            model.Add(new ProductModel
                            {
                                ItemId = myWorksheet.Cells[rowNum, columns.Where(x => x.ColumnTitle == "ItemId").FirstOrDefault().ColumnId]
                                .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                                Name = myWorksheet.Cells[rowNum, columns.Where(x => x.ColumnTitle == "ItemName").FirstOrDefault().ColumnId]
                                .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                                PriceGross = double.Parse(myWorksheet.Cells[rowNum, columns.Where(x => x.ColumnTitle == "PriceGross").FirstOrDefault().ColumnId]
                                .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault()),

                                PrimaryCategory = new CategoryModel { Id = categoryId, Code = category, Name = category }
                            });
                        }
                    }
                    catch
                    {

                    }
                }
            }

            return model;
        }
    }
}
