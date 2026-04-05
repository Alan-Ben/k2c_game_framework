
using System;
using GOE;
using UnityEngine;

[System.Serializable]
public abstract class BasicResIndexInfo : _AALBasicLoadResIndexInfo
{
    [SerializeField] private byte _m_indexType;

    public BasicResIndexInfo()
    {

    }
    public BasicResIndexInfo(int _mainId, int _subId)
        : base(_mainId, _subId)
    {
    }

    /// <summary>
    /// 资源索引类型
    /// </summary>
    /// <remarks>
    /// 详细参考 <see cref="EIndexType"/>
    /// </remarks>
    public EIndexType indexType { get { return (EIndexType)_m_indexType; } set { _m_indexType = (byte)value; } }
    
    /// <summary>
    /// 这个资源索引的完整路径
    /// </summary>
    public sealed override string assetPath { get { return indexType.assetPathRoot() + customAssetPath; } }
    /// <summary>
    /// 这个资源索引的完整名字
    /// </summary>
    public sealed override string objName { get { return customObjName; } }
    
    /// <summary>
    /// 资源路径中可自定义的部分
    /// </summary>
    protected abstract string customAssetPath { get; }
    /// <summary>
    /// 资源名字中可自定义的部分
    /// </summary>
    protected abstract string customObjName { get; }

    // public override int GetHashCode()
    // {
    //     unchecked
    //     {
    //         int hash = 17;
    //         hash = hash * 31 + mainId;
    //         hash = hash * 31 + subId;
    //         hash = hash * 31 + _m_indexType;
    //         return hash;
    //     }
    // }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public static bool IsEqual(BasicResIndexInfo _a1, BasicResIndexInfo _a2)
    {
        if (_a1 == _a2)
            return true;
        if (_a1 != null && _a2 != null)
            return _a1.mainId == _a2.mainId && _a1.subId == _a2.subId && _a1._m_indexType == _a2._m_indexType;
        return false;
    }
    
    public override bool Equals(object _other)
    {
        if (_other is not BasicResIndexInfo otherIndex) 
            return false;

        return IsEqual(this, otherIndex);
    }
    
    public override string ToString()
    {
        if (_m_indexType == 0)
            return $"{mainId}:{subId}";
        
        return $"{indexType}#{mainId}:{subId}";
    }
    
    public static bool operator ==(BasicResIndexInfo _a, BasicResIndexInfo _b)
    {
        // If both are null, or both are same instance, return true.
        if(System.Object.ReferenceEquals(_a, _b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if(((object)_a == null) || ((object)_b == null))
        {
            return false;
        }

        // Return true if the fields match:
        if(_a._m_indexType != _b._m_indexType)
            return false;
        if(_a.mainId != _b.mainId)
            return false;
        if(_a.subId != _b.subId)
            return false;

        return true;
    }
    public static bool operator !=(BasicResIndexInfo _a, BasicResIndexInfo _b)
    {
        return !(_a == _b);
    }

    public override void readIndex(string _str, string _columnName = "")
    {
        if(string.IsNullOrEmpty(_str))
        {
            //使用_str来初始化ResIndex，基本只会在excel表里。如果_str不能为空，excel会校验，导表代码也可以加判断，所以这里面不要强行对空值进行报错
            //Debug.LogError($"{_columnName}是空");
            return;
        }
        
        // 完整的字符串格式是 indexType#mainId:subId，如果没有 indexType，那么默认是 0
        string[] typeSplit = _str.Split('#', StringSplitOptions.RemoveEmptyEntries);
        string idStr;
        if (typeSplit.Length > 1)
        {
            idStr = typeSplit[1];
            try
            {
                indexType = Enum.Parse<EIndexType>(typeSplit[0].ToUpper());
            }
            catch (Exception)
            {
                Debug.LogError(string.Format("index type格式错误：{0} columnName: {1}", typeSplit[0], _columnName));
                return;
            }
        }
        else
            idStr = typeSplit[0];
        
        //拆分字符串后进行读取
        string[] idSplit = idStr.Split(new string[] { "||", ":", "_" }, StringSplitOptions.RemoveEmptyEntries);

        if (idSplit.Length < 1)
        {
            //Debug.LogWarning("没有配置 main id! columnName: " + _columnName);
            return;
        }
        try
        {
            mainId = int.Parse(idSplit[0]);
        }
        catch (Exception)
        {
            Debug.LogError(string.Format("index main id格式错误：{0} columnName: {1}", idSplit[0], _columnName));
            return;
        }

        if (idSplit.Length < 2)
        {
            Debug.LogError("没有配置 sub id! columnName: " + _columnName);
            return;
        }
        try
        {
            subId = int.Parse(idSplit[1]);
        }
        catch (Exception)
        {
            Debug.LogError(string.Format("index sub id格式错误：{0} columnName: {1}", idSplit[1], _columnName));
            return;
        }
    }
}

public static class ResIndexExtension
{
    /// <summary>
    /// 配置数据是否有效，只要有一个>0就合法
    /// </summary>
    public static bool isValid(this ALBasicResIndexInfo _this)
    {
        return _this != null && (_this.mainId > 0 || _this.subId > 0);
    }
    public static string assetPathRoot(this EIndexType _type)
    {
        return _type switch
        {
            EIndexType.ADD => ResIndexConst.addPackPathRoot,
            _ => string.Empty
        };
    }
}
