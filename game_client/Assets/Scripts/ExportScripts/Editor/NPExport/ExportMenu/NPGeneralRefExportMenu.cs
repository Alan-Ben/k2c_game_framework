using ALPackage;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace GOE
{
	//导出的值结构体
	public class NPGeneralExportValuePair
	{
	    public string name;
	    public string value;
	}

	public class NPGeneralRefExportMenu : NPBasicExportMenuItem_excelList<NPGeneralExportValuePair, NPGeneralRefObj, NPSOGeneralRefSet>
	{
	    public NPGeneralRefExportMenu(string _tag, Func<string, string,bool> _judgeCanShowFunc)
	        : base("General", ENPExportSettingEnum.GENERAL, NPSOGeneralRefSet.assetPath, NPSOGeneralRefSet.objName, _tag, _judgeCanShowFunc)
	    {
	    }

	    protected override string _menuText
	    {
	        get
	        {
	            return "General 常量信息(General)";
	        }
	    }

	    public override List<NPGeneralRefObj> _exchangeTemplate(List<NPGeneralExportValuePair> _tempList)
        {
            //缓存去重后原本的数据，用于写入Excel
            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            if (lineValue == null)
	            lineValue = new Dictionary<string, string>();

	        //将数据放入集合，用于用基本读取方式读取数据
	        lineValue.Clear();
	        for (int i = 0; i < _tempList.Count; i++)
	        {
                if (lineValue.ContainsKey(_tempList[i].name.ToLowerInvariant()))
                {
                    if (lineValue[_tempList[i].name.ToLowerInvariant()] == _tempList[i].value)
                    {
                        Debug.LogError($"NPGeneralRefExportMenu重复的key值：{_tempList[i].name} - 值一致");
                    }
                    else
                    {
                        Debug.LogError($"NPGeneralRefExportMenu重复的key值，值还不匹配：{_tempList[i].name} 值- [{lineValue[_tempList[i].name.ToLowerInvariant()]}]:[{_tempList[i].value}] - 当前使用[{lineValue[_tempList[i].name.ToLowerInvariant()]}]");
                    }
                    continue;
                }

                lineValue.Add(_tempList[i].name.ToLowerInvariant(), _tempList[i].value);
                tempDic.Add(_tempList[i].name, _tempList[i].value);
            }

			//获取路径
            string excelPathList = ALExportDataCore.instance.getValue(exportEnum.ToString());
            string[] excelPathArr = excelPathList.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            string transposeExcelPath = excelPathArr[0];
            string exitension = Path.GetExtension(transposeExcelPath);
            transposeExcelPath = transposeExcelPath.Replace(exitension, ".transposeTemp");

            //将general表数据转置，写入到临时Excel文件
            try
            {
                FileInfo fs = new FileInfo(transposeExcelPath);
                // 如果文件存在，先删除
                if (fs.Exists)
                    fs.Delete();

                using (ExcelPackage excel = new ExcelPackage(fs))
                {
                    // 创建工作表
                    ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add(NPSOGeneralRefSet.objName);

                    // 写入表头（第一行为注释行）
                    workSheet.Cells[1, 1].Value = "";
                    workSheet.Cells[1, 2].Value = "";

                    // 写入数据（从第二行开始），第二行字段，第三行值
                    int i = 0;
                    foreach (KeyValuePair<string, string> keyValuePair in tempDic)
                    {
                        workSheet.Cells[2, i + 1].Value = keyValuePair.Key;
                        workSheet.Cells[3, i + 1].Value = keyValuePair.Value;
                        i++;
                    }

                    excel.Save();
                    UnityEngine.Debug.Log($"Excel 文件保存成功: {transposeExcelPath}, 工作表: {NPSOGeneralRefSet.objName}, 数据行数: {tempDic.Count}");
                }
            }
            catch (Exception e)
            {
                UnityEditor.EditorUtility.DisplayDialog("导出Excel结果提示", "导出至Excel失败: " + e.Message, "确认");
                return null;
            }

            //创建队列
            List<NPGeneralRefObj> list = NPExportWnd.autoReadXls<NPGeneralRefObj>(transposeExcelPath, NPSOGeneralRefSet.objName, exportEnum);

			//删除临时文件
			File.Delete(transposeExcelPath);

            return list;
        }

	    public override void _readRefInfo()
	    {
	        //读取所有数据集合
	        obj.name = GetString("name");
	        obj.value = GetString("value");
        }

        /// <summary>
        /// 输出文本文件的处理，针对general需要特殊处理
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_sheetName"></param>
        public override void _writeTxtFile(string[] _path, string _sheetName)
        {
            //输出txt文件
            NPExportWnd.WriteToTxtFile(_path, "general", exportEnum);
            //NPExportWnd.exportGeneralText(_path, "general_txt", exportEnum);
        }
    }
}