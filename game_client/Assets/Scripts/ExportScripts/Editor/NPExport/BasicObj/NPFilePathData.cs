using UnityEngine;
using UnityEditor;
using System.IO;


namespace GOE
{
	public class NPFilePathData
	{
	    public static NPFilePathData getSavePathData(ENPExportSettingEnum _exportEnum)
	    {
	        return new NPFilePathData("Refdata", "__DLExport", _exportEnum);
	    }

	    public static NPFilePathData getBackUpPathData(ENPExportSettingEnum _exportEnum)
	    {
	        return new NPFilePathData("Refdata/__dlserverref", "", _exportEnum);
	    }

	    public string exportFloder;
	    public string subFloder;
	    public ENPExportSettingEnum exportEnum;

	    public NPFilePathData(string _rootPath, string _subPath, ENPExportSettingEnum _exportEnum)
	    {
	        exportFloder = _rootPath;
	        subFloder = _subPath;
	        exportEnum = _exportEnum;
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

	        //if (exportEnum == EWCGExportSettingEnum.MapTerrain) 
	        //{
	        //    string[] subFloderArray = subFloder.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

	        //    for (int i = 0; i < subFloderArray.Length; ++i) {
	        //        folderPath = getFolderPath(subFloderArray[i]);
	        //        string parentFloder = tempRelatviePath;
	        //        tempFullPath += folderPath;
	        //        tempRelatviePath += folderPath;
	        //        if (!Directory.Exists(tempFullPath)) {

	        //            AssetDatabase.CreateFolder(parentFloder, subFloderArray[i]);
	        //        }
	        //    }

	        //}
	        //else
	        //{
	        //    folderPath = getFolderPath(subFloder);
	        //    if (folderPath.Length > 0)
	        //    {
	        //        tempFullPath += folderPath;
	        //        tempRelatviePath += folderPath;
	        //        if (!Directory.Exists(tempFullPath)) 
	        //        {
	        //            AssetDatabase.CreateFolder(tempFullPath, subFloder);
	        //        }
	        //    }
	        //}

	    }
	}
}