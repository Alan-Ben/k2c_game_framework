using System;
using System.Collections.Generic;
using GOE;


/********************
 * 信息结构体
 **/
[System.Serializable]
public class NPCommonAssetPathInfo
{
    public NPCommonAssetPathInfo()
    {
        
    }
    
    public NPCommonAssetPathInfo(string _assetPath, string _wndObjName)
    {
        asset_path = _assetPath;
        obj_name = _wndObjName;
    }

    public string asset_path;//资源ab路径
    public string obj_name;//资源名字



    public bool enable { get { return (!string.IsNullOrEmpty(asset_path) && !string.IsNullOrEmpty(obj_name)); } }

    //读取函数
    public void ParseFromString(string _str)
    {
        readIndex(_str);
    }

    /*******************
     * 从字符串初始化数据
     **/
    public void readIndex(string _str, string _columnName = "")
    {
        if (null == _str)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名! columnName: " + _columnName);
            return;
        }
        
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if(strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名! columnName: " + _columnName);
            return;
        }
        asset_path = strs[0];
        if(!asset_path.EndsWith(".unity3d"))
            asset_path += ".unity3d";

        obj_name = strs[1];
    }

    /// <summary>
    /// 根据资源id构造数据
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static NPCommonAssetPathInfo readFromUiResId(long _id)
    {
        NPCommonAssetPathInfo ret = new NPCommonAssetPathInfo();

        ret.asset_path = UIResPathAssistant.getAssetPath(_id);
        ret.obj_name = UIResPathAssistant.getObjName(_id);

        return ret;
    }

    /************
    * 读取字符串
    **/
    public static NPCommonAssetPathInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        NPCommonAssetPathInfo ret = new NPCommonAssetPathInfo();

        ret.asset_path = strs[0];
        if (!ret.asset_path.EndsWith(".unity3d"))
            ret.asset_path += ".unity3d";

        ret.obj_name = strs[1];

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<NPCommonAssetPathInfo> readInfoList(string _str)
    {
        List<NPCommonAssetPathInfo> list = new List<NPCommonAssetPathInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            NPCommonAssetPathInfo info = new NPCommonAssetPathInfo();
            info.readIndex(strs[i]);

            list.Add(info);
        }
        return list;
    }

    /***********
     * 构造索引名称
     **/
    public string makeIndexName()
    {
        return asset_path + "/" + obj_name;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", asset_path, obj_name);
    }

    //重载运算符 == 的任何类型还应重载运算符 !=,否则会产生编译错误
    public static bool operator ==(NPCommonAssetPathInfo a, NPCommonAssetPathInfo b)
    {
        // If both are null, or both are same instance, return true.
        if(System.Object.ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if(((object)a == null) || ((object)b == null))
        {
            return false;
        }

        // Return true if the fields match:
        if(a.asset_path != b.asset_path)
            return false;
        if(a.obj_name != b.obj_name)
            return false;

        return true;
    }

    public static bool operator !=(NPCommonAssetPathInfo a, NPCommonAssetPathInfo b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if(obj is NPCommonAssetPathInfo)
        {
            NPCommonAssetPathInfo info = obj as NPCommonAssetPathInfo;
            // Return true if the fields match:
            if(info.asset_path != asset_path)
                return false;
            if(info.obj_name != obj_name)
                return false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return GetAssetPathHashCode(asset_path, obj_name);
    }
    
    public static int GetAssetPathHashCode(string _assetPath, string _objName)
    {
        return _assetPath.GetHashCode() ^ _objName.GetHashCode();
    }
}