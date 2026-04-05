using ALPackage;
using System;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;
using GOE;
using NPEnum;



/// <summary>
/// 物品全局表，
/// 在数据库里的表的结构,
/// 注:数据库的表结构里不支持list<>，需要以字符串的方式
/// </summary>
public partial class UniformItemObj
{
	//----------------------数据库表对应的变量---------------------------
	private long _m_lId;//唯一识别ID,由_m_ENPItemType和_m_eCurrency生成

	private string _m_sItemType;//物品类型
	private long _m_lSubId; //子表ID
	private string _m_sName; //名字
	private string _m_sNameArgs;//名字文本翻译的参数
	private string _m_sDesc; //描述
	private string _m_sDescArgs;// 描述参数(作用于desc)
	private string _m_icon;     //物品小图
	private string _m_sSource;// 产出描述
	private string _m_sSourceArgs;//产出描述翻译的参数
	private string _m_sSureAccess;//物品必然产出途径
	private string _m_sPossibleAccess;//物品可能产出途径
	private string _m_sQuality;// 品质

	//-------------------数据使用时的变量
	private ENPItemType _m_ENPItemType;//物品类型
	private List<string> _m_lNameArgsList;// 名字参数(作用于name)
	private List<string> _m_lDescArgsList;// 描述参数(作用于desc)
	private List<string> _m_lSourceArgsList;
	private List<long> _m_lSureAccessList;//物品必然产出途径
	private List<long> _m_lPossibleAccessList;//物品可能产出途径
	private EQuality _m_eQuality;// 品质
	private string _m_sSourceFromAccess;
	//默认构造函数里不能设置字段初始值，
	//从数据库查询使用反射会把依赖的数值修改掉
	public UniformItemObj() { }

	public UniformItemObj(long _refId)
	{
	    // 生成全局ID
	    _m_lId = _refId;
	}

	public UniformItemObj(string _itemType, long _id, UniformItemRefObjBase _refObjBase)
	{
	    _m_sItemType = _itemType;
	    _m_lSubId = _id;
	    _m_sName = _refObjBase.name;
	    _m_sNameArgs = _refObjBase.name_args;
	    _m_sDesc = _refObjBase.desc;
	    _m_sDescArgs = _refObjBase.desc_args;
	    _m_icon = _refObjBase.icon;
	    _m_sSource = _refObjBase.source;
	    _m_sSourceArgs = _refObjBase.source_args;
	    _m_sSureAccess = _refObjBase.sure_access;
	    _m_sPossibleAccess = _refObjBase.possible_access;
	    _m_sQuality = _refObjBase.quality;

	    if (!string.IsNullOrEmpty(_m_sItemType))
	        _m_ENPItemType = (ENPItemType)ALCommon.EnumParse(typeof(ENPItemType), _m_sItemType.Trim(), true);

	    if (!string.IsNullOrEmpty(_m_sQuality))
	        _m_eQuality = (EQuality)ALCommon.EnumParse(typeof(EQuality), _m_sQuality.Trim(), true);

	    // 生成全局ID
	    _m_lId = generateId(_m_ENPItemType, _m_lSubId);
	}

	public static long generateId(ENPItemType _type, long _subId)
	{
	    return (long)_type * 1000000000 + _subId;
	}

	//初始化

#region 数据库的字段

	[PrimaryKey]
	public long Id { get { return _m_lId; } set { _m_lId = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string ItemType { get { return _m_sItemType; } set { _m_sItemType = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public long SubId { get { return _m_lSubId; } set { _m_lSubId = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string Name { get { return _m_sName; } set { _m_sName = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string NameArgs { get { return _m_sNameArgs; } set { _m_sNameArgs = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string Desc { get { return _m_sDesc; } set { _m_sDesc = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string DescArgs { get { return _m_sDescArgs; } set { _m_sDescArgs = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string Icon { get { return _m_icon; } set { _m_icon = value; } }

	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string Source { get { return _m_sSource; } set { _m_sSource = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string SourceArgs { get { return _m_sSourceArgs; } set { _m_sSourceArgs = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string SureAccess { get { return _m_sSureAccess; } set { _m_sSureAccess = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string PossibleAccess { get { return _m_sPossibleAccess; } set { _m_sPossibleAccess = value; } }
	/// <summary>数据库的字段, 使用的时候不要使用这个</summary>
	public string Quality { get { return _m_sQuality; } set { _m_sQuality = value; } }

#endregion

}

public class UniformItemAsset
{
	public const string AssetObjName = "uniform.txt";
	public const string AssetPath = "uniform/uniform.unity3d";
	public static string TableName = typeof(UniformItemObj).FullName;
}

