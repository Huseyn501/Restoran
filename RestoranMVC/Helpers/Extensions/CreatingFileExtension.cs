namespace RestoranMVC.Helpers.Extensions
{
    public static class CreatingFileExtension
    {
        public static string CreatingFile(this IFormFile file,string root,string folderName)
        {
            string filename = "";
            if (file.FileName.Length > 100)
            {
                filename = Guid.NewGuid() + file.FileName.Substring(file.FileName.Length-64);
            }
            else
            {
                filename = Guid.NewGuid() + file.FileName;
            }
            string path = Path.Combine(root,folderName, filename);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                fileStream.CopyTo(fileStream);
            }
            return filename;
        }
        public static void DeletingFile(this string filename, string root, string folderName)
        {
            string path = Path.Combine(root, folderName, filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
