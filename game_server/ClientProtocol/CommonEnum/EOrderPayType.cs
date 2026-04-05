using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 订单支付方式类型
/// </summary>
public enum EOrderPayType {
	[InspectorName("NONE - idx[0] - 无")]
	NONE, //0 ==== 无
	[InspectorName("PLATFORM - idx[1] - 平台支付")]
	PLATFORM, //1 ==== 平台支付
	[InspectorName("VOUCHER - idx[2] - 代金券支付")]
	VOUCHER, //2 ==== 代金券支付
	[InspectorName("GM - idx[3] - GM支付")]
	GM, //3 ==== GM支付
}

public class EOrderPayTypeComparer : IEqualityComparer<EOrderPayType>{
	public bool Equals(EOrderPayType x, EOrderPayType y) { return x == y; }
	public int GetHashCode(EOrderPayType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

