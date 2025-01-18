using LastDance.Utils.Enums;

namespace LastDance.Utils
{
    public static class FileValidator
    {
        public static string BuildPath(string fileName, params string[] roots)
        {
            string path = string.Empty;
            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            path = Path.Combine(path, fileName);
            return path;
        }

        public static bool ValidateType(this IFormFile file, string type)
        {
            if(file.ContentType.Contains(type)) 
                return true;
            return false;
        }

        public static bool ValidateSize(this IFormFile file, SizeEnum fileSize ,int size)
        {
            switch(fileSize)
            {
                case SizeEnum.Kb:
                    return file.Length<=size*1024;
                    

                case SizeEnum.Mb:
                    return file.Length <= size * 1024 *1024;

                case SizeEnum.Gb:
                    return file.Length <= size *1024*1024*1024;
            }

            return false;
        }


        public async static Task<string> CreateFileAsync(this IFormFile file,params string[] roots)
        {
            string orginalFileName = file.FileName;
            int lastDotIndex = orginalFileName.LastIndexOf('.');
            string fileExtension = orginalFileName.Substring(lastDotIndex);

            string fileName = string.Concat(Guid.NewGuid().ToString(), fileExtension);

            string path = BuildPath(fileName,roots);

            using(FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public static void DeleteFile(this string fileName,params string[] roots)
        {
            string path = BuildPath(fileName,roots);
            File.Delete(path);

        }
    }
}
