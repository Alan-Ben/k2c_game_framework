using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家组合具体称号
/// </summary>
public enum ENPPlayerComboTitleType {
	[InspectorName("PRE - idx[0] - 前缀")]
	PRE, //0 ==== 前缀
	[InspectorName("SFX - idx[1] - 后缀")]
	SFX, //1 ==== 后缀
	[InspectorName("BG - idx[2] - 底色")]
	BG, //2 ==== 底色
}

public class ENPPlayerComboTitleTypeComparer : IEqualityComparer<ENPPlayerComboTitleType>{
	public bool Equals(ENPPlayerComboTitleType x, ENPPlayerComboTitleType y) { return x == y; }
	public int GetHashCode(ENPPlayerComboTitleType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

