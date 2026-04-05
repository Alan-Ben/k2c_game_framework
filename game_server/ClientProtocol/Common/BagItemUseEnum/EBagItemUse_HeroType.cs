using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.BagItemUseEnum
{

/// <summary>
/// 背包使用道具-大臣类型
/// </summary>
public enum EBagItemUse_HeroType {
	NONE, //0 ==== 
	[InspectorName("POWER - idx[1] - 实力")]
	POWER, //1 ==== 实力
}

public class EBagItemUse_HeroTypeComparer : IEqualityComparer<EBagItemUse_HeroType>{
	public bool Equals(EBagItemUse_HeroType x, EBagItemUse_HeroType y) { return x == y; }
	public int GetHashCode(EBagItemUse_HeroType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

