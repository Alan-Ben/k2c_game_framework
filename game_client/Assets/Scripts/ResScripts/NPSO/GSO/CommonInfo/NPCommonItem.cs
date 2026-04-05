using System;
using System.Collections.Generic;

using ALPackage;
using NPCommon;
using NPEnum;


/********************
 * 信息结构体
 **/
[System.Serializable]
public class NPCommonItem
{
    public NPEnum.ENPItemType itemType;
    public long itemId;

    public NPCommonItem()
    {

    }

    public NPCommonItem(NPEnum.ENPItemType _itemType, long _itemId)
    {
        itemType = _itemType;
        itemId = _itemId;
    }

    public NPCommonItem(NPCommonItem _item)
    {
        if (_item == null)
            return;

        itemType = _item.itemType;
        itemId = _item.itemId;
    }

    public NPCommonItem(NPCommon_ItemInfo _item)
    {
        if (_item == null)
            return;

        itemType = (ENPItemType)_item.getItemType();
        itemId = _item.getSubId();
    }
    
    public bool IsValid { get { return itemType != ENPItemType.NONE; } }

    /************
    * 读取字符串
    **/
    public static NPCommonItem readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        NPCommonItem ret = new NPCommonItem();

        ret.itemType = (NPEnum.ENPItemType)ALCommon.EnumParse(typeof(NPEnum.ENPItemType), strs[0], true);
        ret.itemId = long.Parse(strs[1]);

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<NPCommonItem> readList(string _str)
    {
        List<NPCommonItem> list = new List<NPCommonItem>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            NPCommonItem newItem = NPCommonItem.readFromStr(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", itemType, itemId);
    }
    
    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return;
        }

        //需要支持物品类型没有配置的情况
        if (strs.Length == 1)
        {
            this.itemType = ENPItemType.BAG_ITEM;
            this.itemId = long.Parse(strs[0]);
        }
        else
        {
            this.itemType = (ENPItemType)ALCommon.EnumParse(typeof(ENPItemType), strs[0], true);
            this.itemId = long.Parse(strs[1]);
        }
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<NPCommonItem> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static NPCommonItem[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
    
    //重载运算符 == 的任何类型还应重载运算符 !=,否则会产生编译错误
    public static bool operator ==(NPCommonItem _a, NPCommonItem _b)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(_a, _b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)_a == null) || ((object)_b == null))
        {
            return false;
        }

        // Return true if the fields match:
        if (_a.itemType != _b.itemType)
            return false;
        if (_a.itemId != _b.itemId)
            return false;

        return true;
    }

    public static bool operator !=(NPCommonItem _a, NPCommonItem _b)
    {
        return !(_a == _b);
    }

    public override bool Equals(object obj)
    {
        if (obj is NPCommonItem)
        {
            NPCommonItem item = obj as NPCommonItem;
            // Return true if the fields match:
            if (item.itemType != itemType)
                return false;
            if (item.itemId != itemId)
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
        return itemType.GetHashCode() ^ itemId.GetHashCode();
    }

    /// <summary>
    /// 获取一个唯一识别的id
    /// </summary>
    /// <returns></returns>
    public long getId()
    {
        return getId(itemType, itemId);
    }

    public static long getId(ENPItemType _itemType, long _itemId)
    {
        return (int)_itemType * 100000 + _itemId;
    }
}