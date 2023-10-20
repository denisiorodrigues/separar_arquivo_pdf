public static class Helper 
{
    public static void DeleteFolder(string path)
    {
        string[] files = Directory.GetFiles(path, "*.*");

        foreach (string file in files)
        {
            File.Delete(file);
        }

        Directory.Delete(path);
    }
}