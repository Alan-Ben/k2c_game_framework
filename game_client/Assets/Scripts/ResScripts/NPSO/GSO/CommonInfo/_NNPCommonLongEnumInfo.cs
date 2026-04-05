using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;


/// <summary>
/// 通用的值、枚举格式的可序列化信息，自动导出上用
/// </summary>
/// <typeparam name="E"></typeparam>
[System.Serializable]
public class _NNPCommonLongEnumInfo<E>
    where E : Enum
{
    public long longValue;
    public E enumValue;

    /************
    * 读取字符串
    **/
    public static _NNPCommonLongEnumInfo<E> readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":", "-" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        _NNPCommonLongEnumInfo<E> ret = new _NNPCommonLongEnumInfo<E>();

        ret.longValue = long.Parse(strs[0]);
        ret.enumValue = (E)ALCommon.EnumParse(typeof(E), strs[1], true);

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<_NNPCommonLongEnumInfo<E>> readList(string _str)
    {
        List<_NNPCommonLongEnumInfo<E>> list = new List<_NNPCommonLongEnumInfo<E>>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            _NNPCommonLongEnumInfo<E> newItem = _NNPCommonLongEnumInfo<E>.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", longValue, enumValue);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { "-", ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return;
        }

        //需要支持物品类型没有配置的情况
        this.longValue = long.Parse(strs[0]);
        this.enumValue = (E)ALCommon.EnumParse(typeof(E), strs[1], true);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<_NNPCommonLongEnumInfo<E>> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static _NNPCommonLongEnumInfo<E>[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}