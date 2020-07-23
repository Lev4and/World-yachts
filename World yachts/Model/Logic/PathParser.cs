namespace World_yachts.Model.Logic
{
    public class PathParser
    {
        public static string GetFileName(string path)
        {
            return path.Substring(path.LastIndexOfAny(new char[2] { @"\"[0], '/' }) + 1, path.LastIndexOf('.') - path.LastIndexOfAny(new char[2] { @"\"[0], '/' }) - 1);
        }

        public static string GetFileExtension(string path)
        {
            return path.Substring(path.LastIndexOf('.'));
        }

        public static string GetFileNameWithExtension(string path)
        {
            return path.Substring(path.LastIndexOfAny(new char[2] { @"\"[0], '/' }) + 1);
        }
    }
}
