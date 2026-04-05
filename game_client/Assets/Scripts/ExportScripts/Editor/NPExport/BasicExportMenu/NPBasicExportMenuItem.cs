using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using ALPackage;
using Excel;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;


namespace GOE
{
	//第一个类型是模板类,  第二个类型是实际游戏中运用的类,  第三个类型是表的集合类
	public abstract class NPBasicExportMenuItem<TTemp, Tobj, TMap> : NPBasicExportMenuItemEX where TTemp : new() where Tobj : _IALBasicRefObj, new() where TMap : _TALSOBasicRefSet<Tobj>, new()
	{
	    private string assetPath = "";
	    private string objName = "";

	    public NPBasicExportMenuItem(string _exportName
	        , ENPExportSettingEnum _exportEnum
	        , string _assetPath
	        , string _objName, string _tag, Func<string,string, bool> _judgeCanShowFunc)
	    : base(_tag, _exportEnum, _judgeCanShowFunc)
	    {
	        assetPath = _assetPath;
	        objName = _objName;
	        _regSubItem(new NPTextButtonItem(_exportName, 15, "copy", () => { GUIUtility.systemCopyBuffer = _exportName; }));
	        _regSubItem(new ALSplitLine());
            _regSubItem(new ALExcelPathItem("数 据 信 息 Excel 文 件：", _exportEnum.ToString()));
            _regSubItem(new ALTextItem("选择的页签名字。", 15));
	        _regSubItem(new NPInputItem(_exportEnum.ToString(), 25));

	        _regSubItem(new NPComfirmItem("导 出", exportGeneralRefSet));

	        NPExportSettingMgr.instance.regSubExportSetting(_exportEnum, exportGeneralRefSet, isSelect, _tag, _judgeCanShowFunc);
	    }

	    /******************
	     * 具体的导出操作
	     **/
	    private void exportGeneralRefSet()
	    {
	        //读取对应的excel文件
	        string excelPath = ALExportDataCore.instance.getValue(exportEnum.ToString());
	        if(string.IsNullOrEmpty(excelPath))
	        {
	            Debug.LogError("ExcelPath is null or empty!!");
	            return;
	        }

	        string tabName = NPInputTabData.instance.getValue(exportEnum.ToString());

	        if (string.IsNullOrEmpty(objName) || string.IsNullOrEmpty(assetPath))
	        {
	            Debug.LogError($"{tabName}:Ref Set 导出失败，包名为空，objName:{objName} assetPath:{assetPath}");
	            return;
	        }

	        //开始读取excel文件
	        List<TTemp> tempList = NPExportWnd.readXls<TTemp>(
	                                        excelPath
	                                        , tabName
	                                        , exportEnum
	                                        , _readRefInfo);

            //输出txt文件
            _writeTxtFile(excelPath, tabName);

            if (tempList.Count == 0)
	        {
	            Debug.LogError(exportEnum + "数据为空,请注意.");
	        }

	        TMap refSet = ScriptableObject.CreateInstance<TMap>();
	        refSet.refList = new List<Tobj>(_exchangeTemplate(tempList));
	        ALBasicExportFunction.exportAsset(refSet, objName, assetPath, "unity3d");
	        AssetDatabase.Refresh();

	        tempList.Clear();
	        tempList = null;
	        //输出导出完成
	        Debug.LogWarning(string.Format("{0} : Ref Set 导出完成!! {1} : {2}", tabName, assetPath, objName));
	    }

	    protected TTemp obj;

	    public void _readRefInfo(TTemp _tmpInfo, int _line, Dictionary<string, string> _lineData)
	    {
	        lineValue = _lineData;
	        line = _line;
	        obj = _tmpInfo;
	        _readRefInfo();
	        lineValue = null;
	    }

		/// <summary>
		/// 输出文本文件的处理，针对general需要特殊处理
		/// </summary>
		/// <param name="_path"></param>
		/// <param name="_sheetName"></param>
		public virtual void _writeTxtFile(string _path, string _sheetName)
        {
            //输出txt文件
            NPExportWnd.WriteToTxtFile(_path, _sheetName, exportEnum);
        }

	    public abstract void _readRefInfo();

	    public abstract List<Tobj> _exchangeTemplate(List<TTemp> _tempList);


	    protected override void _exExport()
	    {
	        exportGeneralRefSet();
	    }
	}
}