using System.Collections.Generic;
using System;
using GOE;
using NPEnum;
using UnityEngine;


/// <summary>
/// uniform 工具类
/// </summary>
public class UniformItemSqliteAssistant
{

	/// <summary>
	/// 获取对象数据
	/// </summary>
	/// <param name="_str"></param>
	/// <returns></returns>
	public static UniformItemObj getUnifromItem(ENPItemType _type, long _subId)
	{
	    return GRefdataCoreMgr.instance.uniformRefCore.getRef(UniformItemObj.generateId(_type, _subId));
	}

	/// <summary>获取翻译前的key</summary>
	public static string getTransNameKey(ENPItemType _type, long _subId)
	{
	    UniformItemObj uniformItemObj = getUnifromItem(_type, _subId);
	    if(uniformItemObj != null)
	    {
	        return uniformItemObj.transNameKey;
	    }
	    else
	    {
#if UNITY_EDITOR
	        Debug.LogError("uniformItemSqlite 找不到对应的UniformItemObj, " + _type + "\t" + _subId);
#endif
	        return String.Empty;
	    }
	}
	/// <summary>获取翻译后的名字</summary>
	public static string getTransName(ENPItemType _type, long _subId)
	{
	    UniformItemObj uniformItemObj = getUnifromItem(_type, _subId);
	    if(uniformItemObj != null)
	    {
	        return uniformItemObj.transName;
	    }
	    else
	    {
#if UNITY_EDITOR
	        Debug.LogError("uniformItemSqlite 找不到对应的UniformItemObj, " + _type + "\t" + _subId);
#endif
	        return String.Empty;
	    }
	}
	/// <summary>获取翻译后的描述</summary>
	public static string getTransDesc(ENPItemType _type, long _subId)
	{
	    UniformItemObj uniformItemObj = getUnifromItem(_type, _subId);
	    if (uniformItemObj != null)
	    {
	        return uniformItemObj.transDesc;
	    }
	    else
	    {
#if UNITY_EDITOR
	        Debug.LogError("uniformItemSqlite 找不到对应的UniformItemObj, " + _type + "\t" + _subId);
#endif
	        return String.Empty;
	    }
	}
	/// <summary>获取翻译后的来源</summary>
	public static string getTransSource(ENPItemType _type, long _subId)
	{
	    UniformItemObj uniformItemObj = getUnifromItem(_type, _subId);
	    if (uniformItemObj != null)
	    {
	        return uniformItemObj.transSource;
	    }
	    else
	    {
#if UNITY_EDITOR
	        Debug.LogError("uniformItemSqlite 找不到对应的UniformItemObj, " + _type + "\t" + _subId);
#endif
	        return String.Empty;
	    }
	}

	/// <summary>获取大图</summary>
	public static NPGTextureIndex getIcon(ENPItemType _type, long _subId)
	{
	    UniformItemObj uniformItemObj = getUnifromItem(_type, _subId);
	    if (uniformItemObj != null)
	    {
	        return uniformItemObj.icon;
	    }
	    else
	    {
#if UNITY_EDITOR
	        Debug.LogError("uniformItemSqlite 找不到对应的UniformItemObj, " + _type + "\t" + _subId);
#endif
	        return null;
	    }
	}
	/// <summary>获取品质</summary>
	public static EQuality getQuality(ENPItemType _type, long _subId)
	{
	    UniformItemObj uniformItemObj = getUnifromItem(_type, _subId);
	    if (uniformItemObj != null)
	    {
	        return uniformItemObj.quality;
	    }
	    else
	    {
#if UNITY_EDITOR
	        Debug.LogError("uniformItemSqlite 找不到对应的UniformItemObj, " + _type + "\t" + _subId);
#endif
	        return EQuality.NONE;
	    }
	}

	/// <summary>
	/// 获取必然获取途径
	/// </summary>
	/// <param name="_type"></param>
	/// <param name="_subId"></param>
	/// <returns></returns>
	public static List<long> getSureAccessWays(ENPItemType _type, long _subId)
	{
	    UniformItemObj uniformItemObj = getUnifromItem(_type, _subId);
	    if (uniformItemObj != null)
	    {
	        return uniformItemObj.sure_access;
	    }
	    else
	    {
#if UNITY_EDITOR
	        Debug.LogError("uniformItemSqlite 找不到对应的UniformItemObj, " + _type + "\t" + _subId);
#endif
	        return null;
	    }
	}

	/// <summary>
	/// 获取可能获取途径
	/// </summary>
	/// <param name="_type"></param>
	/// <param name="_subId"></param>
	/// <returns></returns>
	public static List<long> getPossibleAccessWays(ENPItemType _type, long _subId)
	{
	    UniformItemObj uniformItemObj = getUnifromItem(_type, _subId);
	    if (uniformItemObj != null)
	    {
	        return uniformItemObj.possible_access;
	    }
	    else
	    {
#if UNITY_EDITOR
	        Debug.LogError("uniformItemSqlite 找不到对应的UniformItemObj, " + _type + "\t" + _subId);
#endif
	        return null;
	    }
	}

}
