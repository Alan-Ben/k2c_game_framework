using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.TreasureHuntEnum
{

/// <summary>
/// 太空寻宝获得类型
/// </summary>
public enum ETreasureHuntGainType {
	NONE, //0 ==== 
	[InspectorName("ORE - idx[1] - 矿石")]
	ORE, //1 ==== 矿石
	[InspectorName("TREASURE - idx[2] - 奇物")]
	TREASURE, //2 ==== 奇物
	[InspectorName("REWARD - idx[3] - 奖励")]
	REWARD, //3 ==== 奖励
}

public class ETreasureHuntGainTypeComparer : IEqualityComparer<ETreasureHuntGainType>{
	public bool Equals(ETreasureHuntGainType x, ETreasureHuntGainType y) { return x == y; }
	public int GetHashCode(ETreasureHuntGainType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

