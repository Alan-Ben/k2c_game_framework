using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 物品转换生效时机
/// </summary>
public enum EExchangeItemDealType {
	NONE, //0 ==== 
	[InspectorName("GAIN_ITEM_TIME - idx[1] - 获得物品时")]
	GAIN_ITEM_TIME, //1 ==== 获得物品时
	[InspectorName("MUSEUM_ITEM_STAR_MAX - idx[2] - 博物馆藏品满星")]
	MUSEUM_ITEM_STAR_MAX, //2 ==== 博物馆藏品满星
	[InspectorName("GAIN_ACTIVE_ITEM - idx[3] - 获得活跃度时")]
	GAIN_ACTIVE_ITEM, //3 ==== 获得活跃度时
}

public class EExchangeItemDealTypeComparer : IEqualityComparer<EExchangeItemDealType>{
	public bool Equals(EExchangeItemDealType x, EExchangeItemDealType y) { return x == y; }
	public int GetHashCode(EExchangeItemDealType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

