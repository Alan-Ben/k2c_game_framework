using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;


/// <summary>
/// 通用的枚举、CommonItem格式的可序列化信息，自动导出上用
/// </summary>
/// <typeparam name="E"></typeparam>
[System.Serializable]
public class _NNPCommonEnumItemInfo<E>
    where E : Enum
{
    public E enumValue;
    public NPCommonItem itemValue;
    
    /************
    * 读取字符串
    **/
    public static _NNPCommonEnumItemInfo<E> readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        _NNPCommonEnumItemInfo<E> ret = new _NNPCommonEnumItemInfo<E>();

        ret.enumValue = (E)ALCommon.EnumParse(typeof(E), strs[0], true);
        ret.itemValue = NPCommonItem.readFromStr(strs[1]);

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<_NNPCommonEnumItemInfo<E>> readList(string _str)
    {
        List<_NNPCommonEnumItemInfo<E>> list = new List<_NNPCommonEnumItemInfo<E>>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            _NNPCommonEnumItemInfo<E> newItem = _NNPCommonEnumItemInfo<E>.readFromStr(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", enumValue, itemValue);
    }
    
    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] {":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return;
        }

        //需要支持物品类型没有配置的情况
        this.enumValue = (E)ALCommon.EnumParse(typeof(E), strs[0], true);
        this.itemValue = NPCommonItem.readFromStr(strs[1]);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<_NNPCommonEnumItemInfo<E>> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static _NNPCommonEnumItemInfo<E>[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}