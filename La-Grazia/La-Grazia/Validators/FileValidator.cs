﻿using System;
namespace La_Grazia.Validators
{
    public class FileValidator
    {
        public static string Save(string rootPath, string folder, IFormFile file)
        {
            string newFileName = file.FileName;
            newFileName = newFileName.Length > 64 ? newFileName.Substring(newFileName.Length - 64, 64) : newFileName;
            newFileName = Guid.NewGuid().ToString() + newFileName;

            string path = Path.Combine(rootPath, folder, newFileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return newFileName;
        }

        public static bool Delete(string rootPath, string folder, string fileName)
        {
            string path = Path.Combine(rootPath, folder, fileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }

            return false;
        }
    }
}