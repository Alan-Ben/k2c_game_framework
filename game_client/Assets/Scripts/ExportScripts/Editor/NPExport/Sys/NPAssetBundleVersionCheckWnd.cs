using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using ALPackage;



namespace GOE
{
	public class NPAssetBundleVersionCheckWnd : EditorWindow
	{
	    public static void showExportWnd<T>(Rect _rect, string _title) where T : NPAssetBundleVersionCheckWnd
	    {
	        T window = (T)EditorWindow.GetWindowWithRect(typeof(T), _rect, true, _title);
	        window.Show();
	    }


	    private string _m_sAssetPath = "";

	    public NPAssetBundleVersionCheckWnd()
	    { }

	    void OnGUI()
	    {
	        //开始纵向布局
	        EditorGUILayout.BeginVertical();
        
	        if (GUILayout.Button("粘贴", GUILayout.Width(60)))
	        {
	            _m_sAssetPath = GUIUtility.systemCopyBuffer;
	        }

	        _m_sAssetPath = GUILayout.TextField(_m_sAssetPath);

	        if (GUILayout.Button("版本号修正", GUILayout.Width(100), GUILayout.Height(40)))
	        {

	            CheckVersion(_m_sAssetPath);
	        }
        
	        //结束最外围纵向布局
	        EditorGUILayout.EndVertical();
	    }



	    public static void CheckVersion(string _versionPath)
	    {
	        ALSOAssetBundleVersionSet newSet;
	        ALSOAssetBundleVersionSet oldSet;
	        AssetBundle abVersion = null;
	        oldSet = Resources.Load<ALSOAssetBundleVersionSet>("__ALAssetBundle/version");

	        if (abVersion != null)
	        {
	            abVersion.Unload(true);
	        }

	        //加载资源
	        if (! File.Exists(_versionPath)) {

	            Debug.LogError("TC------------------------versionPath----不存在");
	        }
	        abVersion = AssetBundle.LoadFromFile(_versionPath);
	        if (oldSet == null || abVersion == null)
	        {
	            UnityEngine.Debug.LogError("load version: " + _versionPath + " fail!");
	            return;
	        }

	        newSet = abVersion.LoadAsset<ALSOAssetBundleVersionSet>("version");


	        if (newSet == null)
	        {
	            UnityEngine.Debug.LogError("load version: " + _versionPath + " version obj fail!");
	            return;
	        }

	        //资源外部版总本号
	        oldSet.versionNum = newSet.versionNum;
        
	        List<ALAssetBundleVersionInfo> addInfos = new List<ALAssetBundleVersionInfo>();

	        //逐个遍历设置
	        Debug.LogError("TC------------------ oldSet    1    " + oldSet.versionInfoList.Count);
	        for (int i = 0; i < newSet.versionInfoList.Count; i++)
	        {
	            ALAssetBundleVersionInfo vInfo = newSet.versionInfoList[i];
	            bool hasChecked = false;

	            for (int j = 0; j < oldSet.versionInfoList.Count; j++)
	            {
               
	                if (oldSet.versionInfoList[j].assetPath == vInfo.assetPath)
	                {
	                    if (oldSet.versionInfoList[j].hashTag == vInfo.hashTag)
	                    {
	                        oldSet.versionInfoList[j].versionNum = vInfo.versionNum;
	                        hasChecked = true;
	                        break;
	                    }
	                    else
	                    {
	                        Debug.Log("hash tag error ! assetbundle name is :" + vInfo.assetPath);
	                        Debug.Log(" ---------------oldSet.versionNum :" + oldSet.versionInfoList[j].versionNum  );
	                        Debug.Log(" ---------------newSet.versionNum :" + vInfo.versionNum);
	                        oldSet.versionInfoList[j].versionNum = vInfo.versionNum;
	                        hasChecked = true;
	                    }
	                }
	            }
	            if (!hasChecked)
	            {
	                addInfos.Add(vInfo);
	            }
	        }

	        if (addInfos.Count > 0)
	        {
	            for ( int i=1; i <addInfos.Count; i++ ) {
	                Debug.LogError(" ------------------ addInfos VersionNum       " + addInfos[i].assetPath + "    " + addInfos[i].versionNum);
	            }
	            oldSet.versionInfoList.AddRange(addInfos);
	        }

	        abVersion.Unload(false);

	        EditorUtility.SetDirty(oldSet);//告诉Uinty 数据已经修改过需要进行保存
	        AssetDatabase.SaveAssets();

	        Debug.LogError("TC------------------ oldSet    2   " + oldSet.versionInfoList.Count);
	        Debug.LogError("AssetBundle Version Check Over !");

	    }
	}
}