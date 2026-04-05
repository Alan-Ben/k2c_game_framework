using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.RechargeRebateEnum
{

/// <summary>
/// 充值返利类型枚举
/// </summary>
public enum ERechargeRebateType {
	[InspectorName("DAILY - idx[0] - 每日充值")]
	DAILY, //0 ==== 每日充值
	[InspectorName("TOTAL - idx[1] - 累计充值")]
	TOTAL, //1 ==== 累计充值
	[InspectorName("DAYS - idx[2] - 累天充值")]
	DAYS, //2 ==== 累天充值
}

public class ERechargeRebateTypeComparer : IEqualityComparer<ERechargeRebateType>{
	public bool Equals(ERechargeRebateType x, ERechargeRebateType y) { return x == y; }
	public int GetHashCode(ERechargeRebateType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

