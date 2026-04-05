using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 品质类型，用于客户端和策划定义客户端显示部分数据
/// </summary>
public enum ENPQualityClass {
	[InspectorName("NONE - idx[0] - 无")]
	NONE, //0 ==== 无
	[InspectorName("EQUIP - idx[1] - 藏品")]
	EQUIP, //1 ==== 藏品
	[InspectorName("BAG_ITEM - idx[2] - 道具")]
	BAG_ITEM, //2 ==== 道具
	[InspectorName("RECIPE - idx[3] - 烹饪食谱")]
	RECIPE, //3 ==== 烹饪食谱
	[InspectorName("MUSEUM_ITEM - idx[4] - 博物馆藏品")]
	MUSEUM_ITEM, //4 ==== 博物馆藏品
	[InspectorName("AVATAR - idx[5] - 服装")]
	AVATAR, //5 ==== 服装
	[InspectorName("MUSEUM_ITEM_CHIP - idx[6] - 博物馆藏品碎片")]
	MUSEUM_ITEM_CHIP, //6 ==== 博物馆藏品碎片
	[InspectorName("ICON - idx[7] - 玩家头像")]
	ICON, //7 ==== 玩家头像
	[InspectorName("SPACE_CAPTURE_ITEM - idx[8] - 宠物捕捉道具")]
	SPACE_CAPTURE_ITEM, //8 ==== 宠物捕捉道具
	[InspectorName("MINI_GAME_QUEST - idx[9] - 悬赏任务")]
	MINI_GAME_QUEST, //9 ==== 悬赏任务
	[InspectorName("PET_RAND - idx[10] - 宠物随机资质")]
	PET_RAND, //10 ==== 宠物随机资质
	[InspectorName("SEED - idx[11] - 种子")]
	SEED, //11 ==== 种子
	[InspectorName("HERO_SKIN - idx[12] - 骑士皮肤")]
	HERO_SKIN, //12 ==== 骑士皮肤
	[InspectorName("CONSORT_SKIN - idx[13] - 妃子皮肤")]
	CONSORT_SKIN, //13 ==== 妃子皮肤
	[InspectorName("CLOTHES_SUIT - idx[14] - 套装")]
	CLOTHES_SUIT, //14 ==== 套装
	[InspectorName("HERO - idx[15] - 伙伴")]
	HERO, //15 ==== 伙伴
	[InspectorName("CONSORT - idx[16] - 家人")]
	CONSORT, //16 ==== 家人
}

public class ENPQualityClassComparer : IEqualityComparer<ENPQualityClass>{
	public bool Equals(ENPQualityClass x, ENPQualityClass y) { return x == y; }
	public int GetHashCode(ENPQualityClass obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 17;
}
}

