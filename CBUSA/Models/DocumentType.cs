using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Models
{
    public static class DocumentType
    {
        public static bool CheckMimeTypeFiles(string MimeType)
        {
            if (MimeType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" ||
               MimeType == "application/msword" ||
                 MimeType == "image/jpeg" ||
                 MimeType == "application/pdf" ||
                  MimeType == "application/octet-stream" ||
                 MimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                 MimeType == "application/vnd.ms-excelx" ||
                 MimeType == "text/rtf" ||
                 MimeType == "text/plain" ||
                 MimeType == "image/png" ||
                 MimeType == "image/bmp" ||
                 MimeType == "image/gif" ||
                 MimeType == "image/x-icon" ||
                 MimeType == "application/vnd.ms-powerpoint" ||
                 MimeType == "application/vnd.openxmlformats-officedocument.presentationml.presentation" ||
                 MimeType == "application/vnd.ms-excel"

                 )
            {
                return true;
            }
            return false;
        }

        public static bool CheckFilestypes(string FileExtention)
        {
            if (FileExtention == ".jpg" || FileExtention == ".doc" || FileExtention == ".docx" || FileExtention == ".rtf" || FileExtention == ".pdf" || FileExtention == ".png" || FileExtention == ".xlsx")
            {
                return true;
            }
            return false;
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }


    }
}