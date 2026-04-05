using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;
using System.IO;
using System.Linq;
using UnityEditor;
using ALPackage;



namespace GOE
{
	public class WCGFilePathData
	{
	    public string exportFloder;
	    public string subFloder;
	    public ENPExportSettingEnum exportEnum;

	    public WCGFilePathData(string _rootPath, string _subPath, ENPExportSettingEnum _exportEnum)
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



	public class NPSOCreater
	{
	    [MenuItem("NPAssets/GeneralExport")]
	    public static void generalExport()
	    {
	        NPGeneralExportWnd.showExportWnd<NPGeneralExportWnd>("全局资源打包面板");
	    }
        

	    [MenuItem("NPAssets/资源版本号校对与修正")]
	    public static void createAssetBundleVersionCheck()
	    {
	        Rect wr = new Rect(100, 100, 300, 300);
	        NPAssetBundleVersionCheckWnd.showExportWnd<NPAssetBundleVersionCheckWnd>(wr, "资源版本号校对与修正");
	    }
        
	    // [MenuItem("NPAssets/NP大地图导出窗口")]
	    // public static void createWorldMapExport()
	    // {
	    //     NPWorldMapExportWnd.showExportWnd();
	    // }


	    [MenuItem("NPAssets/修改ClientVersion", false)]
	    static void EditClientSettingtest() {
	        if (EditorApplication.isPlaying)
	            return;
	        NPEditClientVersionEditorWnd.ShowWindow();
	    }
    

	    /*****************
	    * 导出对应对象，并设置对应对象的assetbundle路径信息
	    **/
	    public static void exportAsset(UnityEngine.Object _assetObj, string _assetPath, string _assetBundlePath, string _varianName, ENPExportSettingEnum _type)
	    {
	        string assetPath = exportAsset(_assetObj, _assetPath, _type);
	        //设置对应的assetbundle路径
	        ALAssetBundleCommon.setAssetBundleName(assetPath, _assetBundlePath.Replace("." + _varianName, ""), _varianName);
	    }


	    /*****************
	    * 根据带入的对象以及导出路径将对象保存到对应路径，并返回最新的对象路径
	    * 默认带入的路径格式为xxx/xxxx
	    **/
	    public static string exportAsset(UnityEngine.Object _assetObj, string _assetPath, ENPExportSettingEnum _type)
	    {
	        //获取完整路径，并根据文件夹进行创建操作
	        string exportFolder = string.Empty;
	        switch (_type)
	        {
	            case ENPExportSettingEnum.ACTOR:
	                exportFolder = "__ActorExport";
	                break;
	            case ENPExportSettingEnum.MAP_TERRAIN:
	                exportFolder = "__TerrainExport";
	                break;
	        }

	        //检查对应文件夹位置是否创建完成
	        if (!Directory.Exists(Application.dataPath + "/Resources/Refdata"))
	        {
	            AssetDatabase.CreateFolder("Assets/Resources", "Refdata");
	        }
	        string path = Application.dataPath + "/Resources/Refdata/" + exportFolder;

	        if (!Directory.Exists(path))
	            AssetDatabase.CreateFolder("Assets/Resources/Refdata", exportFolder);

	        string relativePath = string.Format("Assets/Resources/Refdata/{0}/{1}", exportFolder, _assetPath);
	        //Debug.LogError("relativePath:  " + relativePath);
	        //创建对象
	        AssetDatabase.CreateAsset(_assetObj, relativePath + ".asset");

	        AssetDatabase.SaveAssets();
	        Debug.LogError("导出目录： " + path + "/" + _assetPath + ".asset");
	        //返回导出对象的asset路径
	        return relativePath;
	    }
	}
}