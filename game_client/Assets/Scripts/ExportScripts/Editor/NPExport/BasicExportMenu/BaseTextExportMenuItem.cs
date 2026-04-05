using System;
using System.Collections.Generic;
using System.IO;
using ALPackage;
using UnityEditor;
using UnityEngine;


namespace GOE
{
	/// <summary>
	///  固定导出基类（专门导出只给服务端用的表）
	/// </summary>
	public abstract class BaseTextExportMenuItem : NPBasicExportMenuItemEX
	{
	    private string _m_sAssetPath = NPABString.C_RefdataPath;
	    public BaseTextExportMenuItem(string _tag, ENPExportSettingEnum _exportEnum, Func<string,string, bool> _judgeCanShowFunc)
	        : base(_tag, _exportEnum, _judgeCanShowFunc)
	    {
	        _regSubItem(new ALSplitLine());
	        _regSubItem(new ALExcelPathItem("数 据 信 息 Excel 文 件：", _exportEnum.ToString()));
	        // _regSubItem(new ALTextItem("选择的页签名字。", 15));
	        // _regSubItem(new NPInputItem(_tag, 25));
	        _regSubItem(new ALComfirmItem("导 出", realExportGeneralRefSet));

	        NPExportSettingMgr.instance.regSubExportSetting(_exportEnum, realExportGeneralRefSet, isSelect, _tag, _judgeCanShowFunc);
	    }


	    /// <summary>
	    /// 导出热更工程使用的配置
	    /// </summary>
	    protected void doHotfixExportRefSet(string _excelProjectPath, string _excelSheetName, string _assetPath)
	    {
		    string excelPath = Application.dataPath + _excelProjectPath;
		    doExportRefSet(excelPath, _excelSheetName, _assetPath);
	    }
	    
	    /// <summary>
	    /// 导出热更工程使用的配置
	    /// </summary>
	    protected void doExportRefSet(string _excelProjectPath, string _excelSheetName, string _assetPath)
	    {
	        //读取对应的excel文件
	        string excelPath = _excelProjectPath;
	        if (string.IsNullOrEmpty(excelPath))
	        {
	            Debug.LogError(string.Format("【{0}】的ExcelPath是空", exportEnum));
	            return;
	        }
	        if (!File.Exists(excelPath))
	        {
	            Debug.LogError(string.Format("【{0}】的ExcelPath填写错误: {1}", exportEnum, excelPath));
	            return;
	        }

	        //开始读取excel文件
	        NPExportWnd.WriteToTxtFile(excelPath, _excelSheetName, exportEnum, (_filePath) =>
	        {
	            AssetDatabase.Refresh();

	            if (string.IsNullOrEmpty(_assetPath))
	                return;

	            if (string.IsNullOrEmpty(_filePath))
	                return;

	            //获取完整路径，并根据文件夹进行创建操作
	            //string folderPath = _assetPath.Substring(0, _assetPath.LastIndexOf('/') + 1);
	            string fileName = System.IO.Path.GetFileName(_filePath);

	            string path = "Assets/Resources/Refdata/__DLExport/" + fileName;
	            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
	            if (null == assetImporter)
	            {
	                UnityEngine.Debug.LogError("get AssetImporter is null: " + path);
	                return;
	            }

	            //如果没传用默认的ab包名
	            if (string.IsNullOrEmpty(_assetPath))
	                _assetPath = _m_sAssetPath;

	            assetImporter.assetBundleName = _assetPath.Replace(".unity3d", "");//设置Bundle文件的名称
	            assetImporter.assetBundleVariant = _assetPath.Substring(_assetPath.LastIndexOf(".") + 1);//设置Bundle文件的扩展名

	        });

	        //输出导出完成
	        Debug.LogWarning(string.Format("{0} : Ref Set 导出完成!!", _excelSheetName));
	    }

	    /// <summary>
	    /// 导出主工程使用的refSet
	    /// </summary>
	    public void doExportMainRefSet<TObj, TSet>(string _excelProjectPath, string _excelSheetName, string _assetPath, string _objName)
	        where TObj : _IALBasicRefObj, new()
	        where TSet : _TALSOBasicRefSet<TObj>, new()
	    {
	        //读取对应的excel文件
	        string excelPath = _excelProjectPath;
	        if (string.IsNullOrEmpty(excelPath))
	        {
	            Debug.LogError(string.Format("【{0}】的ExcelPath是空", exportEnum));
	            return;
	        }
	        if (!File.Exists(excelPath))
	        {
	            Debug.LogError(string.Format("【{0}】的ExcelPath填写错误: {1}", exportEnum, excelPath));
	            return;
	        }

	        //开始读取excel文件
	        List<TObj> tempList = NPExportWnd.autoReadXls<TObj>(excelPath, _excelSheetName, exportEnum);
	        if (tempList == null)
	        {
	            Debug.LogError(exportEnum + "Excel读取失败");
	            return;
            }

            //输出txt文件
            _writeTxtFile(excelPath, _excelSheetName);

            if (tempList.Count == 0)
	        {
	            Debug.LogError(exportEnum + "数据为空,请注意.");
	        }

	        TSet refSet = ScriptableObject.CreateInstance<TSet>();
	        if (refSet == null)
	        {
	            Debug.LogError(exportEnum + "创建资源失败");
	            return;
	        }
	        refSet.refList = new List<TObj>(tempList);
	        ALBasicExportFunction.exportAsset(refSet, _objName, _assetPath, "unity3d");

	        tempList.Clear();
	        tempList = null;

	        //输出导出完成
	        Debug.LogWarning($"{_excelSheetName} : Ref Set 导出完成!!");
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

        protected abstract void realExportGeneralRefSet();
	}
}