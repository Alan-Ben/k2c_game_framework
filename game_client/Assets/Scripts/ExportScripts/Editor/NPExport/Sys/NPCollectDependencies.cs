using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;
using System.IO;
using UnityEditor;
using ALPackage;
using System.Text;


namespace GOE
{
	public class NPObjectFiledItem : _IALExportMenuInterface {
	    public string txt;
	    public Object obj;

	    public NPObjectFiledItem (string _txt) {
	        txt = _txt;
	    }


	    public virtual bool needShow { get { return true; } }

	    //具体的gui绘制函数
	    public void onGUI () {
	        obj = EditorGUILayout.ObjectField(txt, obj, typeof(Object), true, GUILayout.Height(25));
	    }
	}


	public class NPCollectDependencies : EditorWindow {
	    private static string EXCEL_LINE_DELIMITER = "\t";//行内分隔符
	    private static string EXCEL_LINE = "\r\n";//行间分隔符

	    public static void showExportWnd<T> (Rect _rect, string _title) where T : NPCollectDependencies {
	        //创建窗口
	        T window = (T)EditorWindow.GetWindowWithRect(typeof(T), _rect, true, _title);
	        window.Show();
	    }

	    public class FileNameLinker {
	        public string srcName;
	        public string destName;
	    }

	     [MenuItem("NPAssets/打开GameObject依赖查找窗口")]
	    static void ResExportWnd () {
	        //创建窗口
	        Rect wr = new Rect(100, 100, 700, 700);
	        NPCollectDependencies.showExportWnd<NPCollectDependencies>(wr, "选择找到游戏物体的依赖关系");
	    }

	    private static NPCollectDependencies _g_instance = null;
	    public static NPCollectDependencies instance { get { return _g_instance; } }

	    //滚动区域位置
	    private Vector2 _m_vScrollViewPos;
    
	    private NPObjectFiledItem _m_ofiSelectObj;
	    private NPFolderPathItem _m_fpiFolderPath;//文件目录
	   // private WCGExcelPathItem _m_fpiExportFileFolderPath;//保存导出内容的文本文件路径

	    //导出所有的对象
	    protected ALComfirmItem _m_eaiConfirm;

	    public NPCollectDependencies () {
	        _g_instance = this;

	        _m_ofiSelectObj = new NPObjectFiledItem("直 接 拖 拽 选 择 单 个 对 象");

	        _m_fpiFolderPath = new NPFolderPathItem("选 择 文 件 夹 路 径 ：", ENPExportSettingEnum.COLLECT_DEPENDENCIES.ToString());//-1不保存路径

	        _m_eaiConfirm = new ALComfirmItem("导 出 到 txt 文 件", () => {
	            string resLog = string.Empty;
	            string filePath = string.Empty;

	            if (_m_ofiSelectObj.obj != null) {
	                resLog = getDependenciesLog(_m_ofiSelectObj.obj);

	                //根据文件对象生成一个对应名字的文本文件名字
	                string tmpPath =  AssetDatabase.GetAssetPath(_m_ofiSelectObj.obj);//relative
	                //根据相对路径获取到绝对路径
	                tmpPath = GetAbsolutePath(tmpPath);
	                FileInfo fileInfo = new FileInfo(tmpPath);
	                //Debug.LogError(fileInfo.FullName + "\t\t" + fileInfo.Extension + "\t\t" + fileInfo.Name + "\t\t" + fileInfo.DirectoryName);
	                string fileName = string.Empty;
	                int idx = fileInfo.Name.IndexOf('.');
	                if (idx > 0)
	                    fileName = fileInfo.Name.Substring(0, idx);
	                filePath = Path.Combine(fileInfo.DirectoryName, fileName + ".txt");
	            }
	            else {
	                resLog = getDirectorInfoLog(_m_fpiFolderPath.path);
	                //_m_fpiFolderPath.path = D:/WCG/project_res/Assets/Resources/Refdata/Effect/sfx_2000
	                filePath = GetRightFormatPath(_m_fpiFolderPath.path) + ".txt";

	            }

	            //Debug.LogError(filePath);

	            WriteLogToTextFile(filePath, resLog);
	        });
	    }

	    /*********
	    * gui处理函数
	    **/
	    void OnGUI () {
	        _m_vScrollViewPos = GUILayout.BeginScrollView(_m_vScrollViewPos);

	        //开始纵向布局
	        EditorGUILayout.BeginVertical();

	        EditorGUILayout.Space();
	        _m_ofiSelectObj.onGUI();
	        EditorGUILayout.Space();
	        _m_fpiFolderPath.onGUI();
	        //_m_fpiExportFileFolderPath.onGUI();

	        EditorGUILayout.BeginHorizontal();
	        _m_eaiConfirm.onGUI();

	        EditorGUILayout.EndHorizontal();

	        //结束最外围纵向布局
	        EditorGUILayout.EndVertical();

	        GUILayout.EndScrollView();
	    }

	    private string getDependenciesLog (Object _selectObj) {
	        string log = string.Empty;
	        if (_selectObj == null) {
	            Debug.LogError("_selectObj == null");
	            return log;
	        }            

	        log += _selectObj.name;
	        log += EXCEL_LINE_DELIMITER;

	        Selection.objects = EditorUtility.CollectDependencies(new Object[] { _selectObj });
	        foreach (Object obj in Selection.objects) {

	            if (!obj.name.StartsWith("E-") && !obj.name.StartsWith("M-") && !obj.name.StartsWith("sfx"))
	                continue;

	            log += obj.name;
	            log += EXCEL_LINE_DELIMITER;
	        }
	        return log;
	    }


	    //单个目录下的log
	    private string getDirectorInfoLog (DirectoryInfo _fileDirInfo) {
	        string log = string.Empty;

	        if(_fileDirInfo == null)
	            return log;

	        string tmpFileLog = string.Empty;
	        foreach (FileInfo fileInfo in _fileDirInfo.GetFiles()) {

	            //过滤掉其他文件，保留*.prefab
	            if (!fileInfo.Extension.Equals(".prefab"))
	                continue;

	            //替换路径中的反斜杠为正斜杠       
	            string strTempPath = fileInfo.FullName.Replace(@"\", "/");
	            //截取我们需要的路径
	            strTempPath = strTempPath.Substring(strTempPath.IndexOf("Assets"));
	            tmpFileLog = getDependenciesLog(AssetDatabase.LoadAssetAtPath(strTempPath, typeof(Object)));
	            if (tmpFileLog.Equals(string.Empty)) {
	                Debug.LogError("[" + fileInfo.FullName + "] 获取依赖关系失败");
	                continue;
	            }

	            log += tmpFileLog;
	            log += EXCEL_LINE;
	        }

	        foreach (DirectoryInfo directoryInfo in _fileDirInfo.GetDirectories()) {
	            //Debug.LogError(directoryInfo.Name);
	            if(directoryInfo.Name.StartsWith("sfx"))
	                continue;
	            log += getDirectorInfoLog(directoryInfo);
	        }

	        return log;
	    }

	    private string getDirectorInfoLog (string _path) {
	        string log = string.Empty;
        
	        if (string.IsNullOrEmpty(_path)) {
	            Debug.LogError("获取到底目录是错了, path:  " + _path);
	            return log;
	        }

	        log += getDirectorInfoLog(new DirectoryInfo(_m_fpiFolderPath.path));
	        //Debug.LogError(log);
	        return log;
	    }

	    //字符串写到传入目录的文本里
	    private void WriteLogToTextFile (string _path, string _log) {
	        if (string.IsNullOrEmpty(_path)) {
	            Debug.LogError("请选择目录");
	            return;
	        }
	        Debug.LogError("写入的文本路径:  " + _path);

	        //当前TXT文件是否存在，存在就先删除
	        if (File.Exists(_path)) {
	            File.Delete(Path.GetFullPath(_path));
	        }

	        //开启文件并进行读取
	        FileStream fs = new FileStream(_path, FileMode.CreateNew, FileAccess.ReadWrite);
	        StreamWriter strmWriter = new StreamWriter(fs);    //存入到文本文件中
	        strmWriter.Write(_log);
	        strmWriter.Close();
	        fs.Close();
	        strmWriter.Dispose();
	        fs.Dispose();
	    }

	    //相对路径获取绝对路径
	    static string GetAbsolutePath (string _relativePath) {
	        _relativePath = GetRightFormatPath(_relativePath);
	        int idx = _relativePath.IndexOf("/");
	        string absolutePath = Application.dataPath + _relativePath.Substring(idx);
	        return absolutePath;
	    }

	    //绝对路径获取相对路径
	    static string GetRelativeAssetPath (string _fullPath) {
	        _fullPath = GetRightFormatPath(_fullPath);
	        int idx = _fullPath.IndexOf("Assets");
	        string assetRelativePath = _fullPath.Substring(idx);
	        return assetRelativePath;
	    }

	    static string GetRightFormatPath (string _path) {
	        return _path.Replace("\\", "/");
	    }
	}

	//public class EditorGUIObjectFiled : EditorWindow {
	//    public GameObject obj = null;
	//    public string txtLog = "";
	//    public Object[] dependenciesList;


	//    [MenuItem("Examples/Select Dependencies")]
	//    public static void Init () {
	//        UnityEditor.EditorWindow window = EditorWindow.GetWindow(typeof(EditorGUIObjectFiled));
	//        window.position = new Rect(0, 0, 250, 580);
	//        window.Show();
	//    }

	//    void OnInspectorUpdate () {
	//        Repaint();
	//    }

	//    void OnGUI() {
	//        obj = (GameObject)EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Find Dependency", obj, typeof(GameObject));

	//        if (obj == null) {
	//            EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "Missing:", "Select an object first");
	//            return;
	//        }

	//        if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Check Dependencies")) {
	//            Selection.objects = EditorUtility.CollectDependencies(new GameObject[] { obj });
	//            dependenciesList = Selection.objects;
	//        }

	//        if (GUI.Button(new Rect(3, 55, position.width - 6, 20), "Cancel Select")) {
	//            obj = null;
	//            Selection.objects = EditorUtility.CollectDependencies(new GameObject[] {  });
	//        }

	//        if (Selection.objects != null) {
	//            for (int i = 0; i < Selection.objects.Length; ++i) {
	//                EditorGUI.ObjectField(new Rect(3, 80 + i * 20, position.width - 6, 20), "", Selection.objects[i], typeof(Object));
	//            }
	//        }
       
	//    }

	//}
}