
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SQLite4Unity3d;
using UnityEditor;
using UnityEngine;

//导出相关的代码，只在Editor下用
#if UNITY_EDITOR
public class UniformToTxt
{
	private static string LINE_DELIMITER = "     ";//行内分隔符
	private static string LINE = "\r\n";//行间分隔符

	/// <summary>
	/// 将Uniform数据库中数据写入uniform_review.txt中
	/// </summary>
	/// <param name="_path"></param>
	/// <param name="_sheetName"></param>
	/// <param name="_exportEnum"></param>
	/// <param name="_onDone"></param>
	public static void WriteToTxtFile(List<UniformItemObj> _uniformItemList)
	{
	    string fileName = UniformItemAsset.AssetObjName + "_review";
    
	    //检查对应文件夹位置是否创建完成
	    FilePathData pathData = FilePathData.getSavePathData(fileName);
	    if (pathData == null)
	    {
	        Debug.LogError("save path data is null" + fileName);
	        return;
	    }

	    pathData.fileDirectoryCheck();
	    string filePath = pathData.getAbsolutePath(fileName);

	    //当前TXT文件是否存在，存在就先删除
	    if (File.Exists(filePath))
	    {
	        File.Delete(Path.GetFullPath(filePath));
	    }

	    //重新创建一个文件读写
	    FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
	    StreamWriter strmWriter = new StreamWriter(fs); //存入到文本文件中

	    StringBuilder lineSB = new StringBuilder();
    
	    try
	    {
        
#if !NETFX_CORE
	        var props = typeof(UniformItemObj).GetProperties (BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
#else
			var props = from p in typeof(UniformItemObj).GetRuntimeProperties()
						where ((p.GetMethod != null && p.GetMethod.IsPublic) || (p.SetMethod != null && p.SetMethod.IsPublic) || (p.GetMethod != null && p.GetMethod.IsStatic) || (p.SetMethod != null && p.SetMethod.IsStatic))
						select p;
#endif
        
	        var cols = new List<PropertyInfo> ();
	        foreach (var p in props) {
#if !NETFX_CORE
	            var ignore = p.GetCustomAttributes (typeof(IgnoreAttribute), true).Length > 0;
#else
				var ignore = p.GetCustomAttributes (typeof(IgnoreAttribute), true).Count() > 0;
#endif
	            if (p.CanWrite && !ignore) {
	                cols.Add (p);
	            }
	        }

	        bool isFirst = true;
	        foreach (var prosItem in cols)
	        {
	            if (!isFirst) lineSB.Append(LINE_DELIMITER);
	            else isFirst = false;
            
	            lineSB.Append(prosItem.Name);
	        }
	        lineSB.Append(LINE);

	        foreach (UniformItemObj uniformItemObj in _uniformItemList)
	        {
	            isFirst = true;
	            foreach (var prosItem in cols)
	            {
	                if (!isFirst) lineSB.Append(LINE_DELIMITER);
	                else isFirst = false;
                
	                lineSB.Append(prosItem.GetValue(uniformItemObj));
	            }
	            lineSB.Append(LINE);
	        }
	        strmWriter.Write(lineSB);
	    }
	    catch (Exception e)
	    {
	        Debug.LogError(e);
	    }
	    finally
	    {
	        strmWriter.Close();
	        fs.Close();
	        strmWriter.Dispose();
	        fs.Dispose();
	    }
	}

	private class FilePathData
	{
	    public static FilePathData getSavePathData(string _fileName)
	    {
	      return new FilePathData("Refdata", "__DLExport", _fileName);
	    }

	    public static FilePathData getBackUpPathData(string _fileName)
	    {
	      return new FilePathData("Refdata/__dlserverref", "", _fileName);
	    }

	    public string exportFloder;
	    public string subFloder;
	    public string fileName;

	    public FilePathData(string _rootPath, string _subPath, string _fileName)
	    {
	      exportFloder = _rootPath;
	      subFloder = _subPath;
	      fileName = _fileName;
	    }

	    private string getFolderPath(string _floder)
	    {
	      return _floder.Length > 0 ? "/" + _floder : _floder;
	    }

	    //相对路径
	    public string getRelativePath(string _fileName)
	    {
	      return "/Assets/Resources" + getFolderPath(exportFloder) + getFolderPath(subFloder) + string.Format("/{0}.txt", _fileName);
	    }

	    //绝对路径
	    public string getAbsolutePath(string _fileName)
	    {
	      return Application.dataPath + "/Resources" + getFolderPath(exportFloder) + getFolderPath(subFloder) + string.Format("/{0}.txt", _fileName);
	    }

	    //文件夹目录判断
	    public void fileDirectoryCheck()
	    {
	      //检查对应文件夹位置是否创建完成
	      string folderPath = getFolderPath(exportFloder);
	      string tempFullPath = Application.dataPath + "/Resources";//绝对目录
	      string tempRelatviePath = "Assets/Resources";//相对
	      if (folderPath.Length > 0)
	      {
	          tempFullPath += folderPath;
	          tempRelatviePath += folderPath;
	          if (!Directory.Exists(tempFullPath))
	          {
	              AssetDatabase.CreateFolder(tempRelatviePath, exportFloder);

	          }
	      }

	      string[] subFloderArray = subFloder.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

	      for (int i = 0; i < subFloderArray.Length; ++i)
	      {
	          folderPath = getFolderPath(subFloderArray[i]);
	          string parentFloder = tempRelatviePath;
	          tempFullPath += folderPath;
	          tempRelatviePath += folderPath;
	          if (!Directory.Exists(tempFullPath))
	          {
	              AssetDatabase.CreateFolder(parentFloder, subFloderArray[i]);
	          }
	      }
	    }
	}
}
#endif
