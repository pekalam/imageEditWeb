using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ImageEdit.WebAPI.Extensions
{
    public static class FormFileExtensions
    {
        public static string GetFileExtension(this IFormFile formFile)
        {
            if (formFile == null)
            {
                throw new ArgumentException();
            }
            var ext = formFile.FileName.Split(".", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (ext == null)
            {
                throw new ArgumentException();
            }

            return ext;
        }
    }
}
