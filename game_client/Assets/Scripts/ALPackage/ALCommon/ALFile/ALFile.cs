using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace ALPackage
{
    public static class ALFile
    {
        public static void Delete(string path)
        {
            try
            {
                if(File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception e)
            {
                if (File.Exists(path))
                    Debug.LogError($"ALFile.Delete:{path},{e}");
            }
        }
    }
}