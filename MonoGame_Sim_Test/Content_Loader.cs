using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoGame_Sim_Test
{
    public static class Content_Loader
    {
        public static Dictionary<string, T> Load_Content<T>(this ContentManager contentManager, string contentFolder, string FileType = "*", string FileName = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, T> result = new Dictionary<String, T>();

            FileType = FileType.TrimStart('.');
            FileInfo[] files = dir.GetFiles(FileName + "." + FileType, searchOption);
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                try
                {
                    result[key] = contentManager.Load<T>(contentFolder + "/" + key);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to convert " + file.ToString() + " to " + typeof(T) + "\r\n" + ex);
                }

            }
            return result;
        }
    }
}
