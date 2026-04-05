using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.BagItemUseEnum
{

/// <summary>
/// 背包使用道具-选择目标类型
/// </summary>
public enum EBagItemUse_TargetType {
	NONE, //0 ==== 
	[InspectorName("SELECT - idx[1] - 选择")]
	SELECT, //1 ==== 选择
	[InspectorName("RAND - idx[2] - 随机")]
	RAND, //2 ==== 随机
}

public class EBagItemUse_TargetTypeComparer : IEqualityComparer<EBagItemUse_TargetType>{
	public bool Equals(EBagItemUse_TargetType x, EBagItemUse_TargetType y) { return x == y; }
	public int GetHashCode(EBagItemUse_TargetType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

