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
        public List<ProductModel> ReadPriceListFile(MemoryStream fileBody)
        {
            List<ProductModel> model = new List<ProductModel>();

            List<ExcelSchemaModel> columnsId = new List<ExcelSchemaModel>(_excelSchemaReader.GetExcelColumnsDefinition(schema));

            using (ExcelPackage xlPackage = new ExcelPackage(fileBody))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First();
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;


                if (schema == Const.DPD_SCHEMA)
                {
                    if (!(myWorksheet.Cells["A1"].Value.ToString() == "id" && myWorksheet.Cells["B1"].Value.ToString() == "Nr_Faktury"))
                    {
                        throw new Exception("Bledne pliki");
                    }
                }

                if (schema == Const.EROTRANS_SCHEMA)
                {
                    if (!(myWorksheet.Cells["A1"].Value.ToString() == "WARTOŚĆ" && myWorksheet.Cells["B1"].Value.ToString() == "WALUTA"))
                    {
                        throw new Exception("Bledne pliki");
                    }
                }


                for (int rowNum = 2; rowNum < totalRows; rowNum++)
                {
                    try
                    {
                        model.Add(new TransOrderModel
                        {
                            SenderEmployee = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "SenderEmployee").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                            PostingDate = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "PostingDate").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault().Substring(0, 10),

                            LetterNumber = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "LetterNumber").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                            OrginalLetter = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "OrginalLetter").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                            Amount = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "Amount").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                            PKNumber = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "PKNumber").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                            CustomerName = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "CustomerName").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                            CustomerStreet = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "CustomerStreet").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),

                            CustomerCity = myWorksheet.Cells[rowNum, columnsId.Where(x => x.ColumnName == "CustomerCity").FirstOrDefault().ColumnId]
                            .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault(),
                        });
                    }
                    catch
                    {
                        int h = 1;
                    }
                }
            }

            //model.RemoveAll(x => (x.OrginalLetter == null || x.OrginalLetter.Length < 4));

            for (int i = 0; i < model.Count; i++)
            {
                model[i].Id = i + 1;
            }

            return model;
        }
    }
}
