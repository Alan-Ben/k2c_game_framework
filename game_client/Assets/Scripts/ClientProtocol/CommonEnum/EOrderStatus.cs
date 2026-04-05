using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 订单状态
/// </summary>
public enum EOrderStatus {
	[InspectorName("WAIT_PAY - idx[0] - 待支付")]
	WAIT_PAY, //0 ==== 待支付
	[InspectorName("PAY_SUCCESS - idx[1] - 支付成功")]
	PAY_SUCCESS, //1 ==== 支付成功
	[InspectorName("DELIVERY_COMPLETED - idx[2] - 支付成功发货完成")]
	DELIVERY_COMPLETED, //2 ==== 支付成功发货完成
	[InspectorName("DELIVERY_FAILED - idx[3] - 支付成功发货失败")]
	DELIVERY_FAILED, //3 ==== 支付成功发货失败
	[InspectorName("PAY_CANCELED - idx[4] - 支付取消")]
	PAY_CANCELED, //4 ==== 支付取消
}

public class EOrderStatusComparer : IEqualityComparer<EOrderStatus>{
	public bool Equals(EOrderStatus x, EOrderStatus y) { return x == y; }
	public int GetHashCode(EOrderStatus obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}

