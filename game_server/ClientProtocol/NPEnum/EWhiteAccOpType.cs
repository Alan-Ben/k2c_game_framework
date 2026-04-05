using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 白名单操作类型
/// </summary>
public enum EWhiteAccOpType {
	[InspectorName("ADD - idx[0] - 添加账号")]
	ADD, //0 ==== 添加账号
	[InspectorName("REMOVE - idx[1] - 删除账号")]
	REMOVE, //1 ==== 删除账号
}

public class EWhiteAccOpTypeComparer : IEqualityComparer<EWhiteAccOpType>{
	public bool Equals(EWhiteAccOpType x, EWhiteAccOpType y) { return x == y; }
	public int GetHashCode(EWhiteAccOpType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

