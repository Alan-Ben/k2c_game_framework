using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 登录服务器的客户端连接类型
/// </summary>
public enum ENPLSGameClientType {
	NONE, //0 ==== 
	[InspectorName("USER - idx[1] - 正常的用户名登录")]
	USER, //1 ==== 正常的用户名登录
	[InspectorName("USER_CHECK_CODE - idx[2] - 用户使用验证串登录")]
	USER_CHECK_CODE, //2 ==== 用户使用验证串登录
	[InspectorName("VISITORS - idx[3] - 游客登录")]
	VISITORS, //3 ==== 游客登录
	[InspectorName("CHEAT - idx[4] - 作弊登录，username为uid")]
	CHEAT, //4 ==== 作弊登录，username为uid
	[InspectorName("SDK - idx[5] - SDK登录")]
	SDK, //5 ==== SDK登录
}

public class ENPLSGameClientTypeComparer : IEqualityComparer<ENPLSGameClientType>{
	public bool Equals(ENPLSGameClientType x, ENPLSGameClientType y) { return x == y; }
	public int GetHashCode(ENPLSGameClientType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 6;
}
}

