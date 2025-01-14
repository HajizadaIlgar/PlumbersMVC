namespace PlumberzMVC.Extensions;

public static class FileExtension
{
    public static bool IsValidType(this IFormFile file, string type)
       => file.ContentType.StartsWith(type);

    public static bool IsValidSize(this IFormFile file, int kb)
    {
        return file.Length <= kb * 1024 * 1024;
    }
    public static async Task<string> UploadAsync(this IFormFile file, string filename)
    {
        string path = Path.Combine(filename);
        if (!Path.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string newfilename = Path.GetExtension(file.FileName);
        using (Stream st = File.Create(Path.Combine(path, newfilename)))
        {
            await file.CopyToAsync(st);
        }
        return newfilename;
    }
}
