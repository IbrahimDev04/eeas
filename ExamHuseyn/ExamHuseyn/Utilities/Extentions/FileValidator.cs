namespace ExamHuseyn.Utilities.Extentions
{
    public static class FileValidator
    {
        public static bool CheckType(this IFormFile photo,string type)
        {
            return !photo.ContentType.Contains(type);
        }
        public static bool CheckSize(this IFormFile photo, int mb)
        {
            return photo.Length > mb*1024*1024;
        }
        public static async Task<string> CreateFileAsync(this IFormFile photo,string root, params string[] folders)
        {
            string filename = Guid.NewGuid().ToString()+photo.FileName;
            for (int i = 0; i < folders.Length; i++)
            {
                root = Path.Combine(root, folders[i]);
            }
            root = Path.Combine(root,filename);
            using (FileStream stream = new FileStream(root,FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }
            return filename;
        }
        public static void DeleteFile(this string filename, string root, params string[] folders)
        {
            for (int i = 0; i < folders.Length; i++)
            {
                root = Path.Combine(root, folders[i]);
            }
            root = Path.Combine(root, filename);
            if (File.Exists(root))
            {
                File.Delete(root);
            }
        }
    }
}
