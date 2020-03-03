using BlazorInputFile;
using Domain;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.Pages.AdminPages
{
    public partial class ManagementPage
    {
        [Inject]
        IProductRepository _productRepository { get; set; }
        public string ImportMessage { get; set; } = string.Empty;

        async Task ReadExcelPriceList(IFileListEntry[] files)
        {
            ImportMessage = "Czytam plik Excel";
            IFileListEntry file;
            file = files.FirstOrDefault();
            var ms = new MemoryStream();
            await file.Data.CopyToAsync(ms);
            ImportMessage = "Importuję do bazy danych";
            int result = _productRepository.ImportProductFromFile(ms);
            ImportMessage = $"Zaimportowano {result} produktów do bazy danych.";
            file = null;
        }
    }
}
