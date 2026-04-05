using System;
using UnityEngine;
using OfficeOpenXml;
using UnityEditor;
using System.IO;
using System.Linq;

public class ExcelCommon 
{
    /// <summary>
    /// 通过文件路径及工作表名获取二维数组Excel内容
    /// </summary>
    /// <param name="_excelPath">excel文件夹的绝对路径</param>
    /// <param name="_worksheetName">页签名字</param>
    /// <returns>返回Excel文件内容即二维数组</returns>
    public static string[,] LoadExcel(string _excelPath,string _worksheetName)
    {
        FileStream fs = new FileStream(_excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//建立文件流fs

        using (ExcelPackage excel = new ExcelPackage(fs))
        {
            ExcelWorksheets workSheets = excel.Workbook.Worksheets;//查找到工作簿内的各工作表
 
            ExcelWorksheet workSheet = workSheets[_worksheetName];          //获取工作表
            if (workSheet ==null)
            {
                Debug.Log(_worksheetName+"工作表未找到");
                return new string[0,0];
            }
        
            // 优化2：检查实际数据范围
            var dimension = workSheet.Dimension;
            if (dimension == null)
            {
                Debug.Log(_worksheetName+"工作表无数据");
                return new string[0,0];
            }
            
            int startRow = dimension.Start.Row;
            int startCol = dimension.Start.Column;
            int endRow = dimension.End.Row;
            int endCol = dimension.End.Column;
            int rowCount = endRow - startRow + 1;
            int columnCount = endCol - startCol + 1;
        
            string[,] excelData=new string[rowCount,columnCount];
            
            // 优化1：使用Range批量读取
            var range = workSheet.Cells[startRow, startCol, endRow, endCol];
            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    excelData[i - startRow, j - startCol] = range[i, j].Text;
                }
            }
            
            fs.Close();
            Debug.Log(_worksheetName+"从Excel表导入数据源完毕");
            return excelData;
        }
    }

    /// <summary>
    /// 传入Excel文件路径，表名，及数据二维数组。更新数据至Excel表
    /// </summary>
    /// <param name="_excelPath">Excel文件绝对路径</param>
    /// <param name="_worksheetName">页签名字</param>
    /// <param name="_excelData">二维数组数据</param>
    public static void SaveExcel(string _excelPath, string _worksheetName, string[,] _excelData)
    {
        try
        {
            FileInfo fs = new FileInfo(_excelPath);   //建立文件流fs
            using (ExcelPackage excel = new ExcelPackage(fs))
            {
                ExcelWorksheets workSheets = excel.Workbook.Worksheets; //查找到工作簿内的各工作表

                ExcelWorksheet workSheet = workSheets[_worksheetName];
                if (workSheet == null)
                {
                    Debug.Log(_worksheetName + "工作表未找到");
                    return;
                }
            
                //删除旧数据
                workSheet.DeleteRow(1, workSheet.Dimension.End.Row);
            
                //填写表
                for (int i = 0; i < _excelData.GetLength(0); i++)
                {
                    for (int j = 0; j < _excelData.GetLength(1); j++)
                    {
                        workSheet.Cells[i + 1, j + 1].Value = _excelData[i, j];
                    }
                }
            
                excel.Save();
                Debug.Log(_worksheetName+",excel表更改完成");
            }
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("导出Excel结果提示", "导出至Excel失败，请关闭Excel表后重试"+e, "确认");
            throw;
        }
        
    }
}
