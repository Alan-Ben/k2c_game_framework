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
	public abstract class NPBasicAutoExportMenuItem<Tobj, TMap> : NPBasicExportMenuItemEX where Tobj : _IALBasicRefObj, new() where TMap : _TALSOBasicRefSet<Tobj>, new()
	{
	    private string assetPath = "";
	    private string objName = "";

	    public NPBasicAutoExportMenuItem(ENPExportSettingEnum _exportEnum
	        , string _assetPath
	        , string _objName, string _tag, Func<string, string, bool> _judgeCanShowFunc)
	    : base(_tag, _exportEnum, _judgeCanShowFunc)
	    {
	        assetPath = _assetPath;
	        objName = _objName;
	        _regSubItem(new NPTextButtonItem(_tag, 15, "copy", () => { GUIUtility.systemCopyBuffer = _tag; }));
	        _regSubItem(new ALSplitLine());
	        _regSubItem(new ALExcelPathListItem("数 据 信 息 Excel 文 件：", _exportEnum.ToString()));
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
	        string excelPathList = ALExportDataCore.instance.getValue(exportEnum.ToString());
	        if(string.IsNullOrEmpty(excelPathList))
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
            List<Tobj> tempList = new List<Tobj>();
            string[] excelPathArr = excelPathList.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string excelPath in excelPathArr)
            {
                if (null == excelPath)
                    continue;

                List<Tobj>  readList = NPExportWnd.autoReadXls<Tobj>(excelPath, tabName, exportEnum);

				if(readList != null && readList.Count > 0)
                    tempList.AddRange(readList);
            }

	        if(tempList.Count == 0)
	        {
	            Debug.LogError(exportEnum + "数据为空,请注意.");
            }

            //输出txt文件
            _writeTxtFile(excelPathArr, tabName);

            TMap refSet = ScriptableObject.CreateInstance<TMap>();
	        refSet.refList = new List<Tobj>(tempList);
	        ALBasicExportFunction.exportAsset(refSet, objName, assetPath, "unity3d");

	        AssetDatabase.Refresh();

	        tempList.Clear();
	        tempList = null;
	        //输出导出完成
	        Debug.LogWarning(string.Format("{0} : Ref Set 导出完成!! {1} : {2}", tabName, assetPath, objName));
        }

        /// <summary>
        /// 输出文本文件的处理，针对general需要特殊处理
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_sheetName"></param>
        public virtual void _writeTxtFile(string[] _pathList, string _sheetName)
        {
            //输出txt文件
            NPExportWnd.WriteToTxtFile(_pathList, _sheetName, exportEnum);
        }

        protected override void _exExport()
	    {
	        exportGeneralRefSet();
	    }
	}

	//第一个类型是模板类,  第二个类型是实际游戏中运用的类,  第三个类型是表的集合类
	public abstract class NPBasicAutoExportMenuItemEx<TTemp, Tobj, TMap> : NPBasicExportMenuItemEX where TTemp : new() where Tobj : _IALBasicRefObj, new() where TMap : _TALSOBasicRefSet<Tobj>, new()
	{
	    private string assetPath = "";
	    private string objName = "";

	    public NPBasicAutoExportMenuItemEx(ENPExportSettingEnum _exportEnum
	        , string _assetPath
	        , string _objName, string _tag, Func<string, string, bool> _judgeCanShowFunc)
	    : base(_tag, _exportEnum, _judgeCanShowFunc)
	    {
	        assetPath = _assetPath;
	        objName = _objName;
	        _regSubItem(new NPTextButtonItem(_tag, 15, "copy", () => { GUIUtility.systemCopyBuffer = _tag; }));
	        _regSubItem(new ALSplitLine());
	        _regSubItem(new ALExcelPathListItem("数 据 信 息 Excel 文 件：", _exportEnum.ToString()));
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
	        string excelPathList = ALExportDataCore.instance.getValue(exportEnum.ToString());
	        if(string.IsNullOrEmpty(excelPathList))
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
            List<TTemp> tempList = new List<TTemp>();

            //解析excel列表
            string[] excelPathArr = excelPathList.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string excelPath in excelPathArr)
            {
                if (null == excelPath)
                    continue;

                //读取数据
                List<TTemp> readList = NPExportWnd.autoReadXls<TTemp>(excelPath, tabName, exportEnum);

                //将数据放入队列
                if (null != readList && readList.Count > 0)
                    tempList.AddRange(readList);
            }

	        if(tempList.Count == 0)
	        {
	            Debug.LogError(exportEnum + "数据为空,请注意.");
            }

            //输出txt文件
            _writeTxtFile(excelPathArr, tabName);

            TMap refSet = ScriptableObject.CreateInstance<TMap>();
	        refSet.refList = new List<Tobj>(_exchangeTemplate(tempList));
	        ALBasicExportFunction.exportAsset(refSet, objName, assetPath, "unity3d");

	        AssetDatabase.Refresh();

	        tempList.Clear();
	        tempList = null;
	        //输出导出完成
	        Debug.LogWarning(string.Format("{0} : Ref Set 导出完成!! {1} : {2}", tabName, assetPath, objName));
	    }

	    //将模板数据转化为实例数据的函数
	    public abstract List<Tobj> _exchangeTemplate(List<TTemp> _tempList);

        /// <summary>
        /// 输出文本文件的处理，针对general需要特殊处理
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_sheetName"></param>
        public virtual void _writeTxtFile(string[] _pathList, string _sheetName)
        {
            //输出txt文件
            NPExportWnd.WriteToTxtFile(_pathList, _sheetName, exportEnum);
        }

        protected override void _exExport()
	    {
	        exportGeneralRefSet();
	    }
	}

}