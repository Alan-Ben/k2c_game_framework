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
using System.Data.Odbc;


namespace GOE
{
	//文件目录对象
	public class NPFolderPathItem : ALFolderPathItem {
	    public NPFolderPathItem (string _text, string _dataKey)
	        : base(_text, _dataKey) {

	    }

	    public string path { get { return ALExportDataCore.instance.getValue(_dataKey); } }
	}

	//文件选择对象
	public class NPExcelPathItem : ALExcelPathItem {
	    public NPExcelPathItem (string _text, string _dataKey)
	        : base(_text, _dataKey) {

	    }

	   public string path { get { return ALExportDataCore.instance.getValue(_dataKey); } }
	}

	public class NPFileRename : EditorWindow {
	    public static void showExportWnd<T> (Rect _rect, string _title) where T : NPFileRename {
	        //创建窗口
	        T window = (T)EditorWindow.GetWindowWithRect(typeof(T), _rect, true, _title);
	        window.Show();
	    }

	    public class FileNameLinker {
	        public string srcName;
	        public string destName;
	    }

	     [MenuItem("NPAssets/打开重命名窗口")]
	    static void ResExportWnd () {
	        //创建窗口
	        Rect wr = new Rect(100, 100, 700, 700);
	        NPFileRename.showExportWnd<NPFileRename>(wr, "WCG 文件重命名工具窗口");
	    }

	    private static NPFileRename _g_instance = null;
	    public static NPFileRename instance { get { return _g_instance; } }

	    //滚动区域位置
	    private Vector2 _m_vScrollViewPos;
	    private NPExcelPathItem _m_fpiFolderPathExcel;//重命名关系excel目录
	    private NPFolderPathItem _m_fpiFolderPathRename;//文件目录

	    //导出所有的对象
	    protected ALComfirmItem _m_eaiConfirm;
	    string tabName = "Sheet1";

	    public NPFileRename () {
	        _g_instance = this;


	        _m_fpiFolderPathExcel = new NPExcelPathItem("xml 文 件  路 径 ：", ENPExportSettingEnum.FILE_RENAME_FILE.ToString());

	        _m_fpiFolderPathRename = new NPFolderPathItem("重 命 名 文 件 夹 路 径 ：",  ENPExportSettingEnum.FILE_RENAME_FLODER.ToString());

	        _m_eaiConfirm = new ALComfirmItem("重命名", () => { 
	            Debug.LogError("开始重命名....");
	            if (string.IsNullOrEmpty(_m_fpiFolderPathExcel.path)) {
	                Debug.LogError("获取到底目录是错了, path:  " +_m_fpiFolderPathExcel.path );
	                return;
	            }

	            List<FileNameLinker> fileNameLinkerList = getXlsData(_m_fpiFolderPathExcel.path, tabName);
	            Debug.LogError("读取excel 重命名关系表成功,fileNameLinkerList.Count：  " + fileNameLinkerList.Count);
	            //for(int i = 0;i < fileNameLinkerList.Count;++i)
	            //    Debug.LogError(fileNameLinkerList[i].srcName + "\t\t" + fileNameLinkerList[i].destName);

	            //获取所有源文件的名字
	            DirectoryInfo fileDirInfo = new DirectoryInfo(_m_fpiFolderPathRename.path);
	            //Debug.LogError("需要改名的目录下的文件名字有：  " + fileDirInfo.GetFiles().Length);
	            for (int i = 0; i < fileDirInfo.GetFiles().Length; ++i) {
	                FileInfo fileInfo = fileDirInfo.GetFiles()[i];
       
	                string fullName = fileInfo.Name;
	                string fileName = string.Empty;
	                int idx = fullName.IndexOf('.');
	                if(idx > 0)
	                    fileName = fullName.Substring(0, idx);
	                //Debug.LogError("     " + fileInfo.Name + "\t\tfileName:  " + fileName);

	                //从改名关系表获取对应的数据
	                for (int k = 0; k < fileNameLinkerList.Count; ++k) {
	                    FileNameLinker tmpNameLinker = fileNameLinkerList[k];
	                    //根据文件名字查询到对应的新名字
	                    if (fileName.Equals(tmpNameLinker.srcName)) {
	                        //Debug.LogError("根据文件名字查询到对应的新名字：  " + tmpNameLinker.srcName);
	                        fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, tmpNameLinker.destName + fileInfo.Extension));
	                    }
	                }
	            }
	        });
	    }

	    /*********
	    * gui处理函数
	    **/
	    void OnGUI () {
	        _m_vScrollViewPos = GUILayout.BeginScrollView(_m_vScrollViewPos);

	        //开始纵向布局
	        EditorGUILayout.BeginVertical();

	        //显示文本
	        _m_fpiFolderPathExcel.onGUI();

	        _m_fpiFolderPathRename.onGUI();

	        EditorGUILayout.BeginHorizontal();
	        _m_eaiConfirm.onGUI();

	        EditorGUILayout.EndHorizontal();

	        //结束最外围纵向布局
	        EditorGUILayout.EndVertical();

	        GUILayout.EndScrollView();
	    }


	    private List<FileNameLinker> getXlsData (string _excelPath, string _tabName)
	    {
	        Dictionary<string, string> lineValue = new Dictionary<string, string>();
	        List<FileNameLinker> fileNameLinkerList =
	            readXls(_excelPath
	            , _tabName
	            , () => { return new FileNameLinker(); }
	            , (_tmpLinker, _line, _lineData) => {
	                lineValue = _lineData;

	                //给每一行取值
	                string strValue = "";
	                if (lineValue.TryGetValue("src", out strValue))
	                    _tmpLinker.srcName = strValue;

	                strValue = "";
	                if (lineValue.TryGetValue("dest", out strValue))
	                    _tmpLinker.destName = strValue;
	            });

	        return fileNameLinkerList;
	    }

	    public static List<T> readXls<T> (string _path, string _sheetName, Func<T> _createDelegate, Action<T, int, Dictionary<string, string>> _readDelegate) {
	        //判断带入的函数对象是否有效
	        if (null == _createDelegate || null == _readDelegate)
	            return null;
	        string exitension = Path.GetExtension(_path);
	        //Debug.LogError("===path:  " + _path + "\t\texitension:  " + exitension);
	        //开启文件并进行读取
	        string tmpPath = _path.Replace(exitension, ".tmpX");
	        //拷贝一个文件，避免开启excel时无法读取的问题
	        File.Copy(_path, tmpPath, true);
	        //开启文件并进行读取
	        FileStream stream = File.OpenRead(tmpPath);
	        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

	        //创建结果队列
	        List<T> resList = new List<T>();

	        do {
	            //判断sheet name是否data
	            if (excelReader.Name.Equals(_sheetName, StringComparison.OrdinalIgnoreCase)) {
	                Debug.Log("Start Read Sheet: " + excelReader.Name);
	                //存储列名称,用于分析数据并进行读取
	                List<string> columnNameList = new List<string>();
	                int lineIdx = 0;
	                //对表进行处理
	                while (excelReader.Read()) {
	                    if (1 == lineIdx) {
	                        //取列名
	                        for (int i = 0; i < excelReader.FieldCount; i++) {
	                            //添加列名称, 第一行不能有空列，空列意味着隔断
	                            if (excelReader.IsDBNull(i)) {
	                                break;
	                            }
	                            else {
	                                columnNameList.Add(excelReader.GetString(i));
	                            }

	                        }
	                    }
	                    else if (lineIdx > 1) {

	                        //进行行处理
	                        //创建新的接受数据节点
	                        T dataObj = _createDelegate();

	                        Dictionary<string, string> lineData = new Dictionary<string, string>();
	                        int keyCount = columnNameList.Count;

	                        //记录本行是否有效
	                        bool isLineEnable = false;

	                        //当前行中的列数目，每列值进行处理
	                        for (int i = 0; i < excelReader.FieldCount; i++) {
	                            if (i >= keyCount)
	                                continue;

	                            if (excelReader.IsDBNull(i)) {
	                                lineData[columnNameList[i].ToLowerInvariant()] = "";
	                                continue;
	                            }

	                            //获取对应值，进行后续处理
	                            string value = excelReader.GetString(i);
	                            try {
	                                //处理读取操作
	                                lineData[columnNameList[i].ToLowerInvariant()] = value;
	                                //设置本行有效
	                                isLineEnable = true;

	                            }
	                            catch (Exception ex) {
	                                Debug.LogError(ex.Message + " Error Position:" + "[" + (i + 1) + "," + (lineIdx + 1) + "]" + "----name:" + columnNameList[i] + "   value:" + value);
	                            }
	                        }

	                        //添加到结果集合
	                        if (isLineEnable) {
	                            _readDelegate(dataObj, lineIdx, lineData);
	                            resList.Add(dataObj);
	                        }
	                    }

	                    //增加行号
	                    lineIdx++;
	                }
	            }
	        } while (excelReader.NextResult());

	        //释放文件资源
	        stream.Close();
	        stream.Dispose();
	        excelReader.Dispose();
	        stream = null;
	        excelReader = null;

	        //删除拷贝的文件
	        File.Delete(tmpPath);

	        return resList;
	    }


	}
}