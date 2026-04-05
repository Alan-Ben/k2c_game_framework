using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 建筑细分类型
/// </summary>
public enum ENPCityBuildDetailType {
	NONE, //0 ==== 
	[InspectorName("NORMAL - idx[1] - 未分类")]
	NORMAL, //1 ==== 未分类
	[InspectorName("HATCH - idx[2] - 孵化场")]
	HATCH, //2 ==== 孵化场
	[InspectorName("MUSEUM - idx[3] - 博物馆")]
	MUSEUM, //3 ==== 博物馆
	[InspectorName("COOKING - idx[4] - 烹饪")]
	COOKING, //4 ==== 烹饪
	[InspectorName("FIGHT - idx[5] - 关卡")]
	FIGHT, //5 ==== 关卡
	[InspectorName("MINI_GAME_QUEST - idx[6] - 悬赏任务")]
	MINI_GAME_QUEST, //6 ==== 悬赏任务
	[InspectorName("PLAYER_CARVING - idx[7] - 玩家雕像")]
	PLAYER_CARVING, //7 ==== 玩家雕像
	[InspectorName("FIGHT_REWARD - idx[8] - 关卡奖励")]
	FIGHT_REWARD, //8 ==== 关卡奖励
	[InspectorName("STELLA - idx[9] - 家族星座")]
	STELLA, //9 ==== 家族星座
	[InspectorName("SIGN_IN - idx[10] - 签到建筑")]
	SIGN_IN, //10 ==== 签到建筑
	[InspectorName("ELF - idx[11] - 精灵爱心工坊建筑")]
	ELF, //11 ==== 精灵爱心工坊建筑
}

public class ENPCityBuildDetailTypeComparer : IEqualityComparer<ENPCityBuildDetailType>{
	public bool Equals(ENPCityBuildDetailType x, ENPCityBuildDetailType y) { return x == y; }
	public int GetHashCode(ENPCityBuildDetailType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 12;
}
}

