using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 活动物品过期时间类型
/// </summary>
public enum EActivityItemExpireTimeType {
	NONE, //0 ==== 
	[InspectorName("REWARDING - idx[1] - 活动进入领奖期")]
	REWARDING, //1 ==== 活动进入领奖期
	[InspectorName("CLOSED - idx[2] - 活动关闭")]
	CLOSED, //2 ==== 活动关闭
}

public class EActivityItemExpireTimeTypeComparer : IEqualityComparer<EActivityItemExpireTimeType>{
	public bool Equals(EActivityItemExpireTimeType x, EActivityItemExpireTimeType y) { return x == y; }
	public int GetHashCode(EActivityItemExpireTimeType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

