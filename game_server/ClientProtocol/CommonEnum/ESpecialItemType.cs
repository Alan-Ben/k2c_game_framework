using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 特殊物品类型
/// </summary>
public enum ESpecialItemType {
	NONE, //0 ==== 
	[InspectorName("GOLD - idx[1] - 金币")]
	GOLD, //1 ==== 金币
	[InspectorName("ARENA_STATION - idx[2] - 竞技场贸易站")]
	ARENA_STATION, //2 ==== 竞技场贸易站
	[InspectorName("FARM_MULTIPLE - idx[3] - 农场暴击")]
	FARM_MULTIPLE, //3 ==== 农场暴击
	[InspectorName("MARS_ENERGY - idx[4] - 火星系统-能量")]
	MARS_ENERGY, //4 ==== 火星系统-能量
	[InspectorName("PAID_GEM - idx[5] - 付费钻石")]
	PAID_GEM, //5 ==== 付费钻石
	[InspectorName("PAID_VOUCHER - idx[6] - 付费代金券")]
	PAID_VOUCHER, //6 ==== 付费代金券
}

public class ESpecialItemTypeComparer : IEqualityComparer<ESpecialItemType>{
	public bool Equals(ESpecialItemType x, ESpecialItemType y) { return x == y; }
	public int GetHashCode(ESpecialItemType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 7;
}
}

