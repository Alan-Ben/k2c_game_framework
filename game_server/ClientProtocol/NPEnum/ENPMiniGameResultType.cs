using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 小游戏结果类型
/// </summary>
public enum ENPMiniGameResultType {
	NONE, //0 ==== 
	[InspectorName("SUCCESS - idx[1] - 胜利")]
	SUCCESS, //1 ==== 胜利
	[InspectorName("FAIL - idx[2] - 失败")]
	FAIL, //2 ==== 失败
}

public class ENPMiniGameResultTypeComparer : IEqualityComparer<ENPMiniGameResultType>{
	public bool Equals(ENPMiniGameResultType x, ENPMiniGameResultType y) { return x == y; }
	public int GetHashCode(ENPMiniGameResultType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

