using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.BagItemUseEnum
{

/// <summary>
/// 背包使用道具-妃子获得展示类型
/// </summary>
public enum EBagItemUse_ConsortDrawShowType {
	NONE, //0 ==== 
	[InspectorName("CHARM - idx[1] - 加护力")]
	CHARM, //1 ==== 加护力
	[InspectorName("INTIMACY - idx[2] - 亲密度")]
	INTIMACY, //2 ==== 亲密度
	[InspectorName("CHARM_POINT - idx[3] - 加护点")]
	CHARM_POINT, //3 ==== 加护点
}

public class EBagItemUse_ConsortDrawShowTypeComparer : IEqualityComparer<EBagItemUse_ConsortDrawShowType>{
	public bool Equals(EBagItemUse_ConsortDrawShowType x, EBagItemUse_ConsortDrawShowType y) { return x == y; }
	public int GetHashCode(EBagItemUse_ConsortDrawShowType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

