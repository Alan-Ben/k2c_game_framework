using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 游戏奖励获取展示样式
/// </summary>
public enum ENpRewardShowType {
	[InspectorName("DEFAULT - idx[0] - 默认样式")]
	DEFAULT, //0 ==== 默认样式
	[InspectorName("TIP - idx[1] - tip提示")]
	TIP, //1 ==== tip提示
	[InspectorName("NOT_DISPLAY - idx[2] - 不展示")]
	NOT_DISPLAY, //2 ==== 不展示
}

public class ENpRewardShowTypeComparer : IEqualityComparer<ENpRewardShowType>{
	public bool Equals(ENpRewardShowType x, ENpRewardShowType y) { return x == y; }
	public int GetHashCode(ENpRewardShowType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

