using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.BagItemUseEnum
{

/// <summary>
/// 背包使用道具-大臣获得展示类型
/// </summary>
public enum EBagItemUse_HeroDrawShowType {
	NONE, //0 ==== 
	[InspectorName("POWER - idx[1] - 实力")]
	POWER, //1 ==== 实力
}

public class EBagItemUse_HeroDrawShowTypeComparer : IEqualityComparer<EBagItemUse_HeroDrawShowType>{
	public bool Equals(EBagItemUse_HeroDrawShowType x, EBagItemUse_HeroDrawShowType y) { return x == y; }
	public int GetHashCode(EBagItemUse_HeroDrawShowType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

