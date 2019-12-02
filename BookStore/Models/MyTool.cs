using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public static class MyTool
    {
        public static string UploadHinh(IFormFile fHinh, string folder)
        {
            string fileNameReturn = string.Empty;
            if (fHinh != null)
            {
                fileNameReturn = $"_{DateTime.Now.Ticks}{fHinh.FileName}";
                var fileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", folder, fileNameReturn);
                using (var file = new FileStream(fileName, FileMode.Create))
                {
                    fHinh.CopyTo(file);
                }
            }
            return fileNameReturn;
        }

        public static void MoveImage(string Unit, string NameImage, string NameFolder)
        {
            /*----Start Move file from one folder to another folder*/
            var sourcePath = "wwwroot/Image/" + NameFolder + "/" + NameImage;
            var destinationPath = "wwwroot/Image/"+Unit + "/" + NameImage;
            if (System.IO.File.Exists(sourcePath))
            {
                System.IO.File.Move(sourcePath, destinationPath);
            }
            /*----End Move file from one folder to another folder*/

            /*----Start Delete folder*/
            DeleteFolder(NameFolder);
            /*----End Delete folder*/
        }

        public static void DeleteFolder(string NameFolder)
        {
            if (NameFolder != null)
            {

                /*----Start Delete folder*/
                var path = "wwwroot/Image/" + NameFolder;
                if (Directory.Exists(path))
                {

                    Directory.Delete(path, true);
                }
                /*----End Delete folder*/
            }
        }
    }

    public static class StaticClass
    {
        public static string ToVND(this double dongia)
        {
            return $"{dongia.ToString("#,##0")} đ";
        }

        public static string ToUrlFriendly(this string tieuDe)
        {
            string str = tieuDe.ToLower().Trim();

            //thay thế tiếng Việt
            str = Regex.Replace(str, @"[áàảạãăắằẳẵặâấầẩẫậ]", "a");

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-").Trim();
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }
    }
}
