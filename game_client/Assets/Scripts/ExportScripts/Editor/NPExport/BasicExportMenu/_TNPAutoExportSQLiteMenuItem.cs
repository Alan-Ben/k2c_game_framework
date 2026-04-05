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
	/// <summary>
	/// 将对应数据结构自动从excel中读取出并导出到sqlite中的菜单
	/// </summary>
	/// <typeparam name="Tobj"></typeparam>
	public abstract class _ATNPAutoExportSQLiteMenuItem<Tobj> : NPBasicExportMenuItemEX
	{
	    private string dbPath = "";
	    private string tableName = "";
	    private string assetPath = "";

	    public _ATNPAutoExportSQLiteMenuItem(ENPExportSettingEnum _exportEnum
	        , string _dbPath, string _tableName, string _assetPath
	        , string _tag, Func<string,string, bool> _judgeCanShowFunc)
	    : base(_tag, _exportEnum, _judgeCanShowFunc)
	    {
	        dbPath = _dbPath;
	        tableName = _tableName;

	        assetPath = _assetPath;

	        _regSubItem(new NPTextButtonItem(_tag, 15,"copy",()=> { GUIUtility.systemCopyBuffer = _tag; }));
	        _regSubItem(new ALSplitLine());
	        _regSubItem(new ALExcelPathItem("数 据 信 息 Excel 文 件：", _exportEnum.ToString()));
	        _regSubItem(new ALTextItem("选择的页签名字。", 15));
	        _regSubItem(new NPInputItem(_exportEnum.ToString(), 25));

	        _regSubItem(new NPComfirmItem("导 出", exportGeneralRefSet));

	        NPExportSettingMgr.instance.regSubExportSetting(_exportEnum, exportGeneralRefSet, isSelect, _tag, _judgeCanShowFunc);
	    }

	    //是否每次导出都重新创建文件
	    protected virtual bool alwaysCreateNewFile { get { return false; } }

	    /******************
	     * 具体的导出操作
	     **/
	    private void exportGeneralRefSet()
	    {
	        //读取对应的excel文件
	        string excelPath = ALExportDataCore.instance.getValue(exportEnum.ToString());
	        if (string.IsNullOrEmpty(excelPath))
	        {
	            Debug.LogError("ExcelPath is null or empty!!");
	            return;
	        }

	        string tabName = NPInputTabData.instance.getValue(exportEnum.ToString());
	        //开始读取excel文件
	        List<Tobj> tempList = NPExportWnd.autoReadXls<Tobj>(excelPath, tabName, exportEnum);
	        if (tempList.Count == 0)
	        {
	            Debug.LogError(exportEnum + "数据为空,请注意.");
            }

            //输出txt文件
            _writeTxtFile(excelPath, tabName);

            //输出到对应数据库中
            ALSQLiteExportFunction.exportDB<Tobj>(tempList, dbPath, tableName, alwaysCreateNewFile);

	        //设置导出路径
	        ALSQLiteExportFunction.setDBExportPath(dbPath, assetPath, "unity3d");

	        tempList.Clear();
	        tempList = null;
	        //输出导出完成
	        Debug.LogWarning(string.Format("{0} : Sqlite 导出完成!! {1} - {2} : {3}", tabName, dbPath, tableName, assetPath));
	    }

	    protected override void _exExport()
	    {
	        exportGeneralRefSet();
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
    }

	public class _TNPAutoExportSQLiteMenuItem<Tobj> : _ATNPAutoExportSQLiteMenuItem<Tobj>
	{
	    private string _m_sMenuStr;

	    public _TNPAutoExportSQLiteMenuItem(ENPExportSettingEnum _exprotEnum
	        , string _dbPath, string _tableName, string _assetPath
	        , string _tag, string _menuText, Func<string,string, bool> _judgeCanShowFunc)
	        : base(_exprotEnum, _dbPath, _tableName, _assetPath, _tag, _judgeCanShowFunc)
	    {
	        _m_sMenuStr = _menuText;
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return _m_sMenuStr; } }
	}
}