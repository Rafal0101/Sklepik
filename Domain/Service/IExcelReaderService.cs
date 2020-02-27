using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Domain.Service
{
    public interface IExcelReaderService
    {
        List<ProductModel> ReadPriceListFile(MemoryStream fileBody);
    }
}
