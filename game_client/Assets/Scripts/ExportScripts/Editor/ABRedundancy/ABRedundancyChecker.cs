using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

/// <summary>
/// Author: inkiu0@gmail.com
/// Date: 2016/07/28
/// Repository: https://github.com/inkiu0/ABRedundancyChecker
/// </summary>
class ABRedundancyChecker
{
    /// <summary>
    /// AB文件名匹配规则
    /// </summary>
    public string searchPattern = "*.unity3d";
    /// <summary>
    /// 冗余资源类型白名单
    /// </summary>
    public List<Type> assetTypeList = new List<Type> { typeof(Material), typeof(Texture2D), typeof(AnimationClip), typeof(AudioClip), typeof(Sprite), typeof(Shader), typeof(Font), typeof(Mesh) };
    /// <summary>
    /// 输出路径
    /// </summary>
    public string outPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
    /// <summary>
    /// AB文件存放路径，会从这个文件夹下递归查找符合查找规则searchPattern的文件。
    /// </summary>
#if UNITY_ANDROID
    public string abPath = $"Assets/Resources/__ALAssetBundle/{BuildTarget.Android.ToString()}";
#elif UNITY_STANDALONE_WIN
    public string abPath = $"Assets/Resources/__ALAssetBundle/{BuildTarget.StandaloneWindows.ToString()}";
#elif UNITY_IPHONE
    public string abPath = $"Assets/Resources/__ALAssetBundle/{BuildTarget.iOS.ToString()}";
#endif
    string CsvSavePath = Application.dataPath + "/AnalyzeABResource.csv";

    Dictionary<string, AssetInfo> _AssetMap = new Dictionary<string, AssetInfo>();
    List<string> _FilesList = new List<string>();
    List<string> _AbFilesList = new List<string>();

    [MenuItem("Tools/检查工具/AB冗余检测")]
    public static void Launch()
    {
        ABRedundancyChecker checker = new ABRedundancyChecker();
        checker.StartCheck();
    }

    #region CheckAB

    private string ApplicationPath;
    private void StartCheck()
    {
        ApplicationPath = Application.dataPath;
        ApplicationPath = ApplicationPath.Remove(ApplicationPath.Length - 6, 6);
        GetFileListFromFolderPath(_FilesList, abPath);
        byte[] fileBytes = new byte[] { };
        int startIndex = 0;
        bool hasCollectAbAssetDone = false;
        int collectIndex = 0;

        EditorApplication.update = delegate ()
        {
            if (!hasCollectAbAssetDone)
            {
                string file = _FilesList[collectIndex];

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("AB资源收集中", file, 0.5f * collectIndex/ _FilesList.Count);

                fileBytes = File.ReadAllBytes(_FilesList[collectIndex]);
                try
                {
                    AssetBundle ab = AssetBundle.LoadFromMemory(fileBytes);
                    EditorSettings.serializationMode = SerializationMode.ForceText;
                    string[] names = ab.GetAllAssetNames();//这里如果是预制体-》ab的包，里面包含只有预制体一个
                    _AbFilesList.AddRange(names);
                    ab.Unload(true);
                }
                catch (Exception e)
                {
                    Debug.Log("<color=red>" + e + "</color>");
                }

                collectIndex++;
                if (isCancel || collectIndex >= _FilesList.Count)
                {
                    Resources.UnloadUnusedAssets();
                    hasCollectAbAssetDone = true;
                }
            }
            else
            {
                string file = _FilesList[startIndex];

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("AB资源检测中", file, ((float)startIndex + collectIndex) / 2f/_FilesList.Count);

                fileBytes = File.ReadAllBytes(_FilesList[startIndex]);
                try
                {
                    AssetBundle ab = AssetBundle.LoadFromMemory(fileBytes);
                    string[] abFilePathArr = _FilesList[startIndex].Split('/');
                    CheckABInfo(ab, abFilePathArr[abFilePathArr.Length - 1]);
                    ab.Unload(true);
                }
                catch (Exception e)
                {
                    Debug.Log("<color=red>" + e + "</color>");
                }

                startIndex++;
                if (isCancel || startIndex >= _FilesList.Count)
                {
                    ConvertMapToMarkDown();
                    _AssetMap = null;
                    _FilesList = null;
                    Resources.UnloadUnusedAssets();
                    GC.Collect();

                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    Debug.Log($"检测结束,请打开csv文件检查ab资源冗余：{CsvSavePath}");
                }
            }
         
        };
    }

    public void CheckABInfo(AssetBundle ab, string abName)
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        string[] names = ab.GetAllAssetNames();//这里如果是预制体-》ab的包，里面包含只有预制体一个
        string[] dependencies = AssetDatabase.GetDependencies(names);//预制体包含的所有资源
        string[] allDepen = dependencies.Length > 0 ? dependencies : names;
        // string[] allDepen = names;
        //Dictionary<string, UnityEngine.Object> assetMap = new Dictionary<string, UnityEngine.Object>();
        for (int i = 0; i < allDepen.Length; ++i)
        {
            //实例化不出来且卡，直接采用字符串加载方式
            //UnityEngine.Object obj = ab.LoadAsset(allDepen[i]);
            //if (obj != null && assetTypeList.Contains(obj.GetType()))
            //    TryAddAssetToMap(obj.name, allDepen[i], abName, GetObjectType(obj));
            string lowerDepen = allDepen[i].ToLowerInvariant();
            if(lowerDepen.EndsWith(".cs"))
                continue;

            if(_AbFilesList.Contains(lowerDepen))
                continue;
            //直接加字符串
            string[] typename = allDepen[i].Split('.');
            string type = "";
            if (typename.Length > 1)
                type = typename[typename.Length - 1];
            else
            {
                // File.Exists(Application.dataPath)
                if(!File.Exists(ApplicationPath + allDepen[i]))
                    continue;
            }
            TryAddAssetToMap(allDepen[i], allDepen[i], abName, type);
        }
    }

    /// <summary>
    /// 加入到字典保存标记
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="assetPath">散资源路径，作为key</param>
    /// <param name="abName">ab包名</param>
    /// <param name="type">类型</param>
    private void TryAddAssetToMap(string assetName, string assetPath, string abName, string type)
    {
        if (_AssetMap.ContainsKey(assetPath))
        {
            AssetInfo assetInfo = _AssetMap[assetPath];
            if (!assetInfo.referenceABNames.Contains(abName))
            {
                assetInfo.referenceCount += 1;
                assetInfo.referenceABNames += "`" + abName + "` ";
                _AssetMap[assetPath] = assetInfo;
            }
        }
        else
        {
            AddAssetToMap(assetName, assetPath, abName, type);
        }
    }

    private void AddAssetToMap(string assetName, string assetPath, string abName, string type)
    {
        AssetInfo assetInfo = new AssetInfo();
        assetInfo.name = assetName;
        assetInfo.abType = type;
        assetInfo.referenceCount += 1;
        assetInfo.referenceABNames += "`" + abName + "` ";
        var obj = AssetDatabase.LoadMainAssetAtPath(assetPath);
        assetInfo.memory = GetStorageMemory(obj);
        _AssetMap.Add(assetPath, assetInfo);
    }

    /// <summary>
    /// 从文件夹中递归读取符合查找规则的文件名
    /// </summary>
    /// <param name="files"></param>
    /// <param name="folder"></param>
    private List<string> GetFileListFromFolderPath(List<string> files, string folder)
    {
        folder = AppendSlash(folder);
        if (files == null)
            files = new List<string>();
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folder);
        foreach (var file in dir.GetFiles(searchPattern))
        {
            files.Add(folder + file.Name);
        }
        foreach (var sub in dir.GetDirectories())
        {
            files = GetFileListFromFolderPath(files, folder + sub.Name);
        }
        return files;
    }

    private string GetObjectType(UnityEngine.Object obj)
    {
        string longType = obj.GetType().ToString();
        string[] longTypeArr = longType.Split('.');
        return longTypeArr[longTypeArr.Length - 1];
    }

    /// <summary>
    /// 在路径后面加上斜杠
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>路径</returns>
    private string AppendSlash(string path)
    {
        if (path == null || path == "")
            return "";
        int idx = path.LastIndexOf('/');
        if (idx == -1)
            return path + "/";
        if (idx == path.Length - 1)
            return path;
        return path + "/";
    }

    struct AssetInfo
    {
        public string name;
        public string abType;
        public string memory;
        public int referenceCount;
        public string referenceABNames;
    }

    static string GetStorageMemory(UnityEngine.Object obj)
    {
        if (obj == null)
        {
            Debug.Log("obj为空，无法获取硬盘大小");
            return "";
        }
        Sprite sprite = obj as Sprite;
        Texture texture = null;
        if (sprite != null)
        {
            texture = sprite.texture;
        }
        if (texture == null)
        {
            texture = obj as Texture;
        }
        if (texture != null)
        {
            Type type = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");
            MethodInfo methodInfo = type.GetMethod("GetStorageMemorySize", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
            return (((int)methodInfo.Invoke(null, new object[] { texture }))/1024f).ToString();
        }
        return "";
    }
    #endregion

    #region ConvertMapToMarkDown

    private void ConvertMapToMarkDown()
    {
        // string path = outPath + "/ABRedundency" + DateTime.Now.ToString("yyMMddHHmm") + ".md";
        if (File.Exists(CsvSavePath))
        {
            File.Delete(CsvSavePath);
        }
        using (FileStream fs = File.Create(CsvSavePath))
        {
            // AddText(fs, "# ABRedundency_" + DateTime.Now.ToString("yyMMddHHmm") + "  \r\n");
            AddText(fs, "资源名称,资源类型,内存(kb),AB文件数量,AB文件名\r\n");
            // AddText(fs, "---|---|---|---\r\n");
            string single = "";
            string repeat = "";
            string split = ",";
            foreach (AssetInfo assetInfo in _AssetMap.Values)
            {
                if (assetInfo.referenceCount > 1)
                    repeat += assetInfo.name + split + assetInfo.abType + split + assetInfo.memory + split + assetInfo.referenceCount + split + assetInfo.referenceABNames + "\r\n";
                else
                    single += assetInfo.name + split + assetInfo.abType + split + assetInfo.memory + split + assetInfo.referenceCount + split + assetInfo.referenceABNames + "\r\n";
            }
            AddText(fs, repeat);
            // AddText(fs, single);
        }
    }

    private void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }

    #endregion

}