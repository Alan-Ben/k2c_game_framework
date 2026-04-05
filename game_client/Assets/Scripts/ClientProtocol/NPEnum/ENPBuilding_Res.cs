using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 资源建筑分类枚举
/// </summary>
public enum ENPBuilding_Res {
	NONE, //0 ==== 
	[InspectorName("DEFAULT - idx[1] - 默认产出，获取不到其他类型的时候获取本类型")]
	DEFAULT, //1 ==== 默认产出，获取不到其他类型的时候获取本类型
	[InspectorName("GOLD - idx[2] - 金币产出")]
	GOLD, //2 ==== 金币产出
	[InspectorName("FOOD - idx[3] - 食物产出")]
	FOOD, //3 ==== 食物产出
	[InspectorName("P_EXP - idx[4] - 玩家经验")]
	P_EXP, //4 ==== 玩家经验
	[InspectorName("DUST - idx[5] - 粉尘")]
	DUST, //5 ==== 粉尘
	[InspectorName("ENERGY - idx[6] - 注能道具")]
	ENERGY, //6 ==== 注能道具
	[InspectorName("CEREALS - idx[7] - 粮食")]
	CEREALS, //7 ==== 粮食
	[InspectorName("FIGHT_REWARD - idx[8] - 关卡征收")]
	FIGHT_REWARD, //8 ==== 关卡征收
	[InspectorName("WOOD - idx[9] - 建筑材料")]
	WOOD, //9 ==== 建筑材料
}

public class ENPBuilding_ResComparer : IEqualityComparer<ENPBuilding_Res>{
	public bool Equals(ENPBuilding_Res x, ENPBuilding_Res y) { return x == y; }
	public int GetHashCode(ENPBuilding_Res obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 10;
}
}

