using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 商店商品刷新来源类型
/// </summary>
public enum EShopItemRefreshSourceType {
	NONE, //0 ==== 
	[InspectorName("PLAYER - idx[1] - 玩家")]
	PLAYER, //1 ==== 玩家
	[InspectorName("SERVER - idx[2] - 服务器")]
	SERVER, //2 ==== 服务器
}

public class EShopItemRefreshSourceTypeComparer : IEqualityComparer<EShopItemRefreshSourceType>{
	public bool Equals(EShopItemRefreshSourceType x, EShopItemRefreshSourceType y) { return x == y; }
	public int GetHashCode(EShopItemRefreshSourceType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

