namespace AutoRealizator
{
    public static class SaveAsName
    {
        public static string CreateSavePath(string path)
        {
            var newPath = path.Split(".docx")[0];
            newPath = newPath+"-ispunjeno"+".docx";
            
            return newPath;

        }
    }
}