using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 礼包日志类型
/// </summary>
public enum EGiftPackLogType {
	NONE, //0 ==== 
	[InspectorName("GEM - idx[1] - 钻石充值")]
	GEM, //1 ==== 钻石充值
	[InspectorName("VOUCHER - idx[2] - 代金券充值")]
	VOUCHER, //2 ==== 代金券充值
}

public class EGiftPackLogTypeComparer : IEqualityComparer<EGiftPackLogType>{
	public bool Equals(EGiftPackLogType x, EGiftPackLogType y) { return x == y; }
	public int GetHashCode(EGiftPackLogType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

