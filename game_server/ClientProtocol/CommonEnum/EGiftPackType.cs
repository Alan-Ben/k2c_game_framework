using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 礼包类型
/// </summary>
public enum EGiftPackType {
	NONE, //0 ==== 
	[InspectorName("PUSH_GIFT - idx[1] - 推送礼包")]
	PUSH_GIFT, //1 ==== 推送礼包
	[InspectorName("FIRST_RECHARGE - idx[2] - 首充礼包")]
	FIRST_RECHARGE, //2 ==== 首充礼包
}

public class EGiftPackTypeComparer : IEqualityComparer<EGiftPackType>{
	public bool Equals(EGiftPackType x, EGiftPackType y) { return x == y; }
	public int GetHashCode(EGiftPackType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

