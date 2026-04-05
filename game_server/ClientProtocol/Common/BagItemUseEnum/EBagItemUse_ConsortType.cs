using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.BagItemUseEnum
{

/// <summary>
/// 背包使用道具-妃子类型
/// </summary>
public enum EBagItemUse_ConsortType {
	NONE, //0 ==== 
	[InspectorName("INTIMACY - idx[1] - 亲密度")]
	INTIMACY, //1 ==== 亲密度
	[InspectorName("CHARM - idx[2] - 魅力")]
	CHARM, //2 ==== 魅力
	[InspectorName("CHARM_POINT - idx[3] - 加护点")]
	CHARM_POINT, //3 ==== 加护点
}

public class EBagItemUse_ConsortTypeComparer : IEqualityComparer<EBagItemUse_ConsortType>{
	public bool Equals(EBagItemUse_ConsortType x, EBagItemUse_ConsortType y) { return x == y; }
	public int GetHashCode(EBagItemUse_ConsortType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

