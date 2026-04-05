using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ALPackage;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Editor工具类
/// </summary>
public class EditorUtil
{
    public const string LocalResRootPath = "Resources";

    /// <summary>
    /// 查找Variant未空的Ab包
    /// </summary>
    /// <returns></returns>
    public static List<string> checkAssetBundleVariant(string[] _searchInFolders = null)
    {
        var guids = AssetDatabase.FindAssets("b:", _searchInFolders);
        List<string> results = new List<string>();
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if(null != assetImporter)
            {
                if((string.IsNullOrEmpty(assetImporter.assetBundleVariant) || assetImporter.assetBundleVariant == "None")  && !string.IsNullOrEmpty(assetImporter.assetBundleName))
                {
                    results.Add(path);                    
                }
            }
        }
        return results;
    }

    /// <summary>
    /// 查找同一个包内的同名资源
    /// </summary>
    /// <returns></returns>
    public static List<string> checkSameNameResource()
    {
        Dictionary<string, string> nameDict = new Dictionary<string, string>();
        List<string> results = new List<string>();
        foreach (var assetBundleName in AssetDatabase.GetAllAssetBundleNames())
        {
            foreach (var assetPathAndName in AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName))
            {
                if (assetPathAndName == null)
                    continue;
                
                string nameWithoutPath = assetPathAndName.Substring(assetPathAndName.LastIndexOf("/") + 1);
                if (!nameDict.ContainsKey(nameWithoutPath))
                    nameDict.Add(nameWithoutPath,assetBundleName);
                else
                {
                    string str = string.Format("包：{0} 存在重复的资源类型名称：{1}", assetBundleName, nameWithoutPath);
                    Debug.LogError(str, AssetDatabase.LoadAssetAtPath<GameObject>(assetPathAndName));
                    results.Add(str);
                }
            }
            nameDict.Clear();
        }
        Debug.Log("查找结束");
        return results;
    }

    class PMeshGoInfoCollect
    {
        public enum PWriteType
        {
            NONE,
            SHAPE,
            RENDER,
            BOTH,
        }
        class PMeshGoInfo
        {
            public PMeshGoInfo(Mesh _mesh, GameObject _go, PWriteType _type)
            {
                mesh = _mesh;
                gos = new List<GameObject>();
                gos.Add(_go);
                type = _type;
            }
            public Mesh mesh;
            public List<GameObject> gos;
            public PWriteType type;

            public void setType(PWriteType _type)
            {
                if (type == PWriteType.SHAPE && _type == PWriteType.RENDER)
                    type = PWriteType.BOTH;
                else if (type == PWriteType.RENDER && _type == PWriteType.SHAPE)
                    type = PWriteType.BOTH;
                else
                    type = _type;
            }
        }
        
        List<PMeshGoInfo> results = new List<PMeshGoInfo>();

        public void addInfo(Mesh _mesh, GameObject _go, PWriteType _type)
        {
            foreach (var info in results)
            {
                if (info.mesh == _mesh)
                {
                    info.gos.Add(_go);
                    info.setType(_type);
                    return;
                }
            }
            results.Add(new PMeshGoInfo(_mesh, _go, _type));
        }
        
        public void Log()
        {
            Debug.Log("查找特效上使用，未开启读写的Mesh 完成");
            foreach (var info in results)
            {
                string prefabList = "";
                foreach (var go in info.gos)
                {
                    prefabList += $"go:{go.name},path:{AssetDatabase.GetAssetPath(go)}\n";
                }
                UnityEngine.Debug.LogError($"这个mesh在特效中使用的设置，需要开启Read/Write:name{info.mesh.name},{info.type}path:{AssetDatabase.GetAssetPath(info.mesh)}\n {prefabList}", info.mesh);
            }
        }

        public string errorInfo()
        {
            string eInfo = "";

            foreach (var info in results)
            {
                eInfo += $"name{info.mesh.name},{info.type}path:{AssetDatabase.GetAssetPath(info.mesh)}\n";
                
            }

            return eInfo;
        }

        public bool isSuc()
        {
            return results.Count == 0;
        }
    }
    
    /// <summary>
    /// 查找在特效上使用，未开启mesh的Read/Write设置的模型
    /// </summary>
    /// <returns></returns>
    public static bool checkAssetParticleUsedMeshSetting(out string _errorInfo, string[] _searchInFolders = null)
    {
        var guids = AssetDatabase.FindAssets("t:Prefab", _searchInFolders);
        PMeshGoInfoCollect collect = new PMeshGoInfoCollect();
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (var obj in toCheck)
            {
                var go = obj as GameObject;
                if(go == null)
                    continue;

                var comp = go.GetComponent<ParticleSystem>();
                var shape = comp.shape;
                if (shape.mesh != null && !shape.mesh.isReadable )
                {
                    collect.addInfo(shape.mesh, go, PMeshGoInfoCollect.PWriteType.SHAPE);
                }
                if(shape.meshRenderer != null )
                {
                    var meshFilter = shape.meshRenderer.GetComponent<MeshFilter>();
                    if(meshFilter != null && meshFilter.sharedMesh != null && !meshFilter.sharedMesh.isReadable)
                        collect.addInfo(meshFilter.sharedMesh, go, PMeshGoInfoCollect.PWriteType.SHAPE);
                }
                if(shape.skinnedMeshRenderer != null )
                {
                    if(shape.skinnedMeshRenderer.sharedMesh != null && !shape.skinnedMeshRenderer.sharedMesh.isReadable)
                        collect.addInfo(shape.skinnedMeshRenderer.sharedMesh, go, PMeshGoInfoCollect.PWriteType.SHAPE);
                }
                
                // 对render中的mesh也要做检查
                ParticleSystemRenderer renderer = go.GetComponent<ParticleSystemRenderer>();

                if (renderer != null && renderer.mesh != null )
                {
                    SerializedObject renderSource = new SerializedObject(renderer);
                    //SerializedProperty useCustomVertexSteamsProperty = renderSource.FindProperty("m_UseCustomVertexStreams");
                    
                    if (!renderer.mesh.isReadable)
                        collect.addInfo(renderer.mesh, go, PMeshGoInfoCollect.PWriteType.RENDER);
                }
            }
        }
        collect.Log();
        _errorInfo = collect.errorInfo();
        return collect.isSuc();
    }
    
        /// <summary>
    /// 查找丢失引用的prefabV2
    /// </summary>
    /// <returns></returns>
    public static List<GameObject> checkMissingPrefabV2(string[] _searchInFolders = null)
    {
        List<GameObject> stringList = new List<GameObject>();
        var guids = AssetDatabase.FindAssets("t:Prefab", _searchInFolders);
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if(null == path)
                continue;
            
            var toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
            if(null == toCheck)
                continue;
                
            foreach (var obj in toCheck)
            {
                var go = obj as GameObject;
                if(go == null)
                {
                    continue;
                }
                if(!EditorUtil.IsActiveInAssetHierarchy(go))
                {
                    continue;
                }
                var components = go.GetComponents<Component>();

                foreach (var c in components)
                {
                    // Missing components will be null, we can't find their type, etc.
                    if (!c)
                    {
                        stringList.Add(go);
                        continue;
                    }

                    // SerializedObject so = new SerializedObject(c);
                    // var sp = so.GetIterator();
                    //
                    // // Iterate over the components' properties.
                    // while (sp.NextVisible(true))
                    // {
                    //     if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    //     {
                    //         if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
                    //         {
                    //             stringList.Add(go);
                    //         }
                    //     }
                    // }
                }
            }
           
        }

        return stringList;
    }
  
    public static bool IsActiveInAssetHierarchy(GameObject _go) {
        bool active = _go.activeSelf;
        Transform parent = _go.transform.parent;
        while (parent != null ) {
            if (!parent.gameObject.activeSelf) {
                return false;
            }
            parent = parent.parent;
        }
        return active;
    }
    
    /// <summary>
    /// 项目内查找
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_searchChild">是否查找子目录</param>
    /// <param name="_searchInactive">未激活也查找</param>
    /// <param name="_searchInFolders">查找指定目录</param>
    /// <returns></returns>
    public static List<T> findAssetsWithComponent<T>(bool _searchChild = false, bool _searchInactive = false, string[] _searchInFolders = null) where T : Component
    {
        var guids = AssetDatabase.FindAssets("t:Prefab", _searchInFolders);
        var results = new List<T>();
        var tempTList = new List<T>();
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if(!_searchChild)
            {
                var go = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;

                var comp = go.GetComponent<T>();
                if(comp != null)
                {
                    results.Add(comp);
                }
            }
            else
            {
                var toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
                foreach (var obj in toCheck)
                {
                    var go = obj as GameObject;
                    if(go == null)
                    {
                        continue;
                    }
                    if(!_searchInactive && !IsActiveInAssetHierarchy(go))
                    {
                        continue;
                    }

                    var comp = go.GetComponent<T>();
                    if(comp != null)
                    {
                        results.Add(comp);
                    }
                }
            }
        }
        return results;
    }

    public static string GetHierarchyPath(GameObject _go)
    {
        if (_go != null) {
            string path = _go.name;
            Transform tr = _go.transform;
            while (tr.parent != null) {
                path = tr.parent.name + "/" + path;
                tr = tr.parent;
            }
            return path;
        }
        return "";
    }
    
    public static bool IsDescendantOf(Transform parent, Transform child)
    {
        // 检查子对象是否为 null
        if (child == null || null == parent)
        {
            return false;
        }

        // 如果子对象的父对象就是目标对象，则返回 true
        if (child.parent == parent)
        {
            return true;
        }
        // 否则，递归检查子对象的父对象直到找到目标对象或到达根节点为止
        else
        {
            return IsDescendantOf(parent, child.parent);
        }
    }
    
    public static string GetFullPath(GameObject go)
    {
        return go.transform.parent == null
            ? go.name
            : GetFullPath(go.transform.parent.gameObject) + "/" + go.name;
    }

    #region Editir下资源加载

    public static T loadPrefab<T>(string _assetPath, string _objName, string _exname, string _unitySiftStr, Func<string, List<string>> _getValuesFunc = null, Action<string, List<string>> _addKVAction = null) where T : UnityEngine.Object
    {
        string judgeS = _objName + _exname;
        judgeS = judgeS.ToLowerInvariant();

        //声明临时变量
        T finalGo = null;
            
        //使用GetAssetPathsFromAssetBundleAndAssetName方式加载比FindAssets匹配加载快很多，FindAssets底层逻辑是全遍历去匹配
        //直接先使用这个方式获取ab路径，不需要缓存，耗时很短可以忽略
        string[] paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(_assetPath, _objName);
        foreach (string pathItem in paths)
        {
            if(string.IsNullOrEmpty(pathItem))
                continue;
                
            finalGo = _getPathAssetsMatch<T>(pathItem, _objName, judgeS);
            //如果有对象直接返回   
            if (null != finalGo)
                return finalGo;
        }

        return _loadPrefabByFindAssets<T>(_assetPath, _objName, _exname, _unitySiftStr, _getValuesFunc, _addKVAction);
    }
    
    // 检查资源是否匹配，增加了检查生成的物体是否名字一致的判断，来实现加载TexturePacker的sprite的功能
    private static T _getPathAssetsMatch<T>(string _path, string _objName, string judgeS) where T : UnityEngine.Object
    {
        T finalGo = default(T);
        if(_path.ToLowerInvariant().EndsWith(judgeS))
        {
            finalGo = AssetDatabase.LoadAssetAtPath(_path, typeof(T)) as T;
            if(finalGo != null )
                return finalGo;
        }
  
        UnityEngine.Object[] allAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(_path);
        if(allAssets == null)
            return null;

        for (int i = 0; i < allAssets.Length; i++)
        {
            UnityEngine.Object obj = allAssets[i];
            if (null == obj || !(obj is T))
                continue;

            finalGo = obj as T;
            if(finalGo != null && finalGo.name == _objName)
                return finalGo;
        }

        return null;
    }
    
    private static T _loadPrefabByFindAssets<T>(string _assetPath, string _objName, string _exname, string _unitySiftStr, Func<string, List<string>> _getValuesFunc = null, Action<string, List<string>> _addKVAction = null) where T : UnityEngine.Object
    {
        string judgeS = _objName + _exname;
        judgeS = judgeS.ToLowerInvariant();

        //声明临时变量
        T finalGo = null;
        
        List<string> totalValue = _getValuesFunc?.Invoke(_assetPath + _exname);
        if(null != totalValue)
        {
            //已经有全部值则直接处理并返回
            for (int i = 0; i < totalValue.Count; i++)
            {
                string path = totalValue[i];
                if(null == path)
                    continue;

                finalGo = _getPathAssetsMatch<T>(path, _objName, judgeS);
                if(null == finalGo)
                    continue;

                break;
            }

            return finalGo;
        }
        else
        {
            //创建新队列，等待后续赋值
            totalValue = new List<string>();
        }

        //检索目录名
        string[] guids = AssetDatabase.FindAssets("b:" + _assetPath, new string[] { "Assets/" + LocalResRootPath });

        if(guids.Length <= 0)
        {
            //将值队列加入数据
            _addKVAction?.Invoke(_assetPath + _exname, totalValue);
            return null;
        }

        //加入值队列

        string[] resFolders = new string[guids.Length];

        for(int f = 0; f < guids.Length; f++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[f]);
            resFolders[f] = path;
            if(null == path || totalValue.Contains(path))
                continue;
            //加入值队列
            totalValue.Add(path);

            if (null == finalGo)
                finalGo = _getPathAssetsMatch<T>(path, _objName, judgeS);
        }

        if(null == finalGo)
        {
            //检索文件夹内信息
            string[] ids = AssetDatabase.FindAssets(_unitySiftStr, resFolders);

            //遍历对象加入列表中
            for (int i = 0; i < ids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(ids[i]);
                if(null == path || totalValue.Contains(path))
                    continue;
                //加入值队列
                totalValue.Add(path);
                if (null == finalGo)
                    finalGo = _getPathAssetsMatch<T>(path, _objName, judgeS);
            }
        }
      
        //将值队列加入数据
        _addKVAction?.Invoke(_assetPath + _exname, totalValue);

        return finalGo;
    }

    #endregion
}