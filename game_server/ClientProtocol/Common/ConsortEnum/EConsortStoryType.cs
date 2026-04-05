using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ConsortEnum
{

/// <summary>
/// 家人剧情类型
/// </summary>
public enum EConsortStoryType {
	NONE, //0 ==== 
	[InspectorName("PLAY - idx[1] - 游玩")]
	PLAY, //1 ==== 游玩
	[InspectorName("CALL - idx[2] - 邀约")]
	CALL, //2 ==== 邀约
	[InspectorName("MARRY - idx[3] - 成为家人")]
	MARRY, //3 ==== 成为家人
	[InspectorName("FIRST_CALL - idx[4] - 首次邀约")]
	FIRST_CALL, //4 ==== 首次邀约
	[InspectorName("SECOND_CALL - idx[5] - 二次邀约")]
	SECOND_CALL, //5 ==== 二次邀约
}

public class EConsortStoryTypeComparer : IEqualityComparer<EConsortStoryType>{
	public bool Equals(EConsortStoryType x, EConsortStoryType y) { return x == y; }
	public int GetHashCode(EConsortStoryType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 6;
}
}

