
using System;
using System.IO;
using System.Security.Cryptography;

namespace AudioBuilder
{
    public static class BuildUtility
    {

        /// <summary>
        /// 传入一个路径，为路径末尾补上斜杠符号
        /// </summary>
        public static string directoryInsure(string _path)
        {
            if (string.IsNullOrEmpty(_path))
            {
                Debug.Log("传入的path不是一个路径");
                return string.Empty;
            }
            // 判断末尾是不是斜杠结尾
            if (_path[_path.Length - 1] != '\\' || _path[_path.Length - 1] != '/')
            {
                _path += "/"; // 如果不是就加上斜杠
            }
            return _path;
        }

        /// <summary>
        /// 从一个根目录开始，对里面所有的文件进行遍历处理
        /// </summary>
        public static void actionRecursiveFileByRootPath(string _rootPath, Action<FileInfo> _fileAction)
        {
            if (_fileAction == null)
                return;

            //在指定目录及子目录下查找文件,在list中列出子目录及文件
            DirectoryInfo rootDir = new DirectoryInfo(_rootPath);
            if (!rootDir.Exists)
                return;

            FileInfo[] files = rootDir.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                _fileAction(files[i]);
            }

            DirectoryInfo[] subDirection = rootDir.GetDirectories();
            for (int i = 0; i < subDirection.Length; i++)
            {
                actionRecursiveFileByRootPath(subDirection[i].FullName, _fileAction);
            }
        }

        /// <summary>
        /// 生成对应文件的md5值
        /// </summary>
        public static string generateMD5(string _filePath)
        {
            FileInfo file = new FileInfo(_filePath);
            if (!file.Exists)
            {
                Debug.Log($" generateMD5 目标文件不存在：\"{_filePath}");
                return string.Empty;
            }

            string output = string.Empty;
            try
            {
                using (FileStream fileStream = file.OpenRead())
                {
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    byte[] md5Byte = md5.ComputeHash(fileStream);
                    output = BitConverter.ToString(md5Byte).Replace("-", "");
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"generateMD5 生成MD5发生错误：\"{_ex}");
            }

            return output;
        }
        public static string generateMD5(FileInfo _file)
        {
            if (!_file.Exists)
            {
                Debug.Log($"generateMD5 目标文件不存在");
                return string.Empty;
            }

            string output = string.Empty;
            try
            {
                using (FileStream fileStream = _file.OpenRead())
                {
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    byte[] md5Byte = md5.ComputeHash(fileStream);
                    output = BitConverter.ToString(md5Byte).Replace("-", "");
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"generateMD5 生成MD5发生错误：\"{_ex}");
            }

            return output;
        }
    }
}
