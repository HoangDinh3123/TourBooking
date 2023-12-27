using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatTour.Upload
{
    public class UploadFile
    {
        public static string UploadAnh(int id, string controller, IFormFile file)
        {
            if (file != null)
            {
                
                var folderPath = Path.GetFullPath("./wwwroot/img");
                folderPath = string.Format(@"{0}\{1}\{2}", folderPath, controller, id);
                Directory.CreateDirectory(folderPath);

                var filePath = string.Format(@"{0}\{1}", folderPath, file.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);

                return string.Format(@"{0}/{1}/{2}",controller,id , file.FileName);
            }
            return string.Empty;
        }
    }
}
