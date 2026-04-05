using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using ALPackage;
using GOE;


namespace GOE
{
	/******************
	 * WCG导出面板统一操作窗口
	 **/
	public class NPGeneralExportWnd : EditorWindow {
    
	    public static void showExportWnd<T> (string _title) where T : NPGeneralExportWnd
	    {
	        //T window = (T)EditorWindow.GetWindow(typeof(T), true, _title);
	        T window = (T)EditorWindow.GetWindowWithRect(typeof(T), new Rect(10, 10, 500, 250), true, _title);
	        window.Show();
	    }

	    private string _m_sVersionNum = "";
	    private bool _m_bExportServerRefdata = true;

	    public NPGeneralExportWnd()
	    {
	    }

	    void OnGUI () {
	        GUILayout.BeginArea(new Rect(10, 10, 480, 230));
	        //开始纵向布局
	        EditorGUILayout.BeginVertical();

	        //显示当前版本
#if UNITY_IPHONE
	        GUILayout.Label("当前平台： Ios");
#elif UNITY_ANDROID
	        GUILayout.Label("当前平台： Android");
#elif UNITY_WEBPLAYER
	        GUILayout.Label("当前平台： Web");
#elif UNITY_STANDALONE_WIN
	        GUILayout.Label("当前平台： Pc");
#endif

	        //空白
	        GUILayout.Label("---------------------------------");

	        _m_bExportServerRefdata = GUILayout.Toggle(_m_bExportServerRefdata, "是否导出ServerRef");

	        //输入资源版本号，当不输入时则自动取当前时间
	        GUILayout.Space(10);
	        GUILayout.Label("在此输入版本号（当不输入时则自动取最新时间戳作为版本号）", GUILayout.Height(30));

	        GUILayout.BeginHorizontal();
	        GUILayout.Label("版本号：", GUILayout.Width(60));
	        _m_sVersionNum = GUILayout.TextField(_m_sVersionNum, GUILayout.Height(30));
	        GUILayout.EndHorizontal();

	        //点击导出当前版本资源
	        if(GUILayout.Button("1- 导出全部资源", GUILayout.Height(30)))
	        {
	            long versionNum = 0;
	            if(_m_sVersionNum.Trim().Length > 0 && !long.TryParse(_m_sVersionNum.Trim(), out versionNum))
	                EditorUtility.DisplayDialog("填写错误", "版本号需要是整数！请正确填写！", "确定");
	            else
	            {
	                //打包资源
	                Build.exportAssetBundle(TCSetting.resVer, _m_bExportServerRefdata, false, String.Empty);
	//
	//                //拷贝服务端数据文件
	//                NPSOCreater.dlServerRef();

	                AssetDatabase.Refresh();

	                if(0 == versionNum)
	                    EditorUtility.DisplayDialog("提示", "自动版本号导出完成！", "确定");
	                else
	                    EditorUtility.DisplayDialog("提示", "使用固定版本号：" + versionNum + "\n导出完成！", "确定");
	            }
	        }
	        if(GUILayout.Button("2- 拷贝服务器数据", GUILayout.Height(30)))
	        {

	            //拷贝服务端数据文件
	            FireCreater.dlServerRef();

	            AssetDatabase.Refresh();

	            EditorUtility.DisplayDialog("提示", "导出成功！", "确定");
	        }

	        //结束最外围纵向布局
	        EditorGUILayout.EndVertical();

	        GUILayout.EndArea();
	    }
	}
}