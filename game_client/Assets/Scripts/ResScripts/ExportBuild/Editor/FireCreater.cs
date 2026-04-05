using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;
using System.IO;
using System.Linq;
using UnityEditor;
using ALPackage;



public class FireCreater
{
    //private static string EXCEL_LINE = "\r\n";//行间分隔符

    //==== 导出给服务器使用的所有表数据
    //[MenuItem("ALMenu/DLServerRef")]
    public static void dlServerRef()
    {
        //检查对应文件夹位置是否创建完成
        if(!Directory.Exists(Application.dataPath + "/Resources/Refdata"))
            AssetDatabase.CreateFolder("Assets/Resources", "Refdata");

        if (!Directory.Exists(Application.dataPath + "/Resources/Refdata/__DLExport"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Refdata", "__DLExport");
        }
        string srcPath = Application.dataPath + "/Resources/Refdata/__DLExport";

        if (!Directory.Exists(Application.dataPath + "/Resources/Refdata/__dlserverref"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Refdata", "__dlserverref");
        }
        string destPath = Application.dataPath + "/Resources/Refdata/__dlserverref";

        CopyDirectory(srcPath, destPath);
        DeleteFilesInDirectory(destPath, "asset");
        
        //将version_num.txt拷贝到__dlserverref目录下
        string sourceFileName = Application.dataPath + "/Resources/__ALAssetBundle/version_num.txt";
        string destFileName = Application.dataPath + "/Resources/Refdata/__dlserverref/version_num.txt";
        if (File.Exists(sourceFileName))
        {
            File.Copy(sourceFileName, destFileName, true);
            AssetDatabase.Refresh();
        }
    }
    
    /// <summary>
    /// 把一个文件夹下所有文件复制到另一个文件夹下
    /// </summary>
    /// <param name="srcPath">源目录</param>
    /// <param name="destPath">目标目录</param>
    public static void CopyDirectory(string srcPath, string destPath)
    {
        if (!Directory.Exists(srcPath))
        {
            Debug.LogError("错了目录不存在，srcPath：　" + srcPath);
            return;
        }

        if (!Directory.Exists(destPath))
        {
            Debug.LogError("错了目录不存在，destPath：　" + destPath);
            return;
        }

        DirectoryInfo dir = new DirectoryInfo(srcPath);
        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
        foreach (FileSystemInfo info in fileinfo)
        {
            //Debug.LogError("---"  + info.FullName + "\t\t\t" + (info is DirectoryInfo));
            if (info is DirectoryInfo)     //判断是否文件夹
            {
                if (!Directory.Exists(destPath + "\\" + info.Name.ToLowerInvariant()))
                {
                    Directory.CreateDirectory(destPath + "\\" + info.Name.ToLowerInvariant());   //目标目录下不存在此文件夹即创建子文件夹
                }
                CopyDirectory(info.FullName, destPath + "\\" + info.Name.ToLowerInvariant());    //递归调用复制子文件夹
            }
            else
            {
                //判断后缀名，不拷贝meta
                if (info.FullName.Substring(info.FullName.Length - 4).Equals("meta", System.StringComparison.OrdinalIgnoreCase))
                    continue;

                //过滤掉非文本文件
                bool isTextFile = CheckIsTextFile(info.FullName);
                if (!isTextFile)
                {
                    continue;
                }
                //文本文件拷贝
                File.Copy(info.FullName, destPath + "\\" + info.Name, true); //不是文件夹即复制文件，true表示可以覆盖同名文件
            }
        }

    }
    
    //删除指定类型文件
    public static void DeleteFilesInDirectory(string _path, string _extension)
    {
        DirectoryInfo di = new DirectoryInfo(_path);
        FileInfo[] files = di.GetFiles("*." + _extension)
            .Where(p => p.Extension == "." + _extension).ToArray();
        foreach (FileInfo file in files)
            try
            {
                file.Attributes = FileAttributes.Normal;
                File.Delete(file.FullName);
            }
            catch (Exception)
            {
                Debug.LogError("删除文件失败： " + file.FullName);
            }
    }

    /// <summary>
    /// Checks the file is textfile or not.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns></returns>
    public static bool CheckIsTextFile(string fileName)
    {
        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        bool isTextFile = true;
        try
        {
            int i = 0;
            int length = (int)fs.Length;
            byte data;
            while (i < length && isTextFile)
            {
                data = (byte)fs.ReadByte();
                isTextFile = (data != 0);
                i++;
            }
            return isTextFile;
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
            }
        }
    }
}
