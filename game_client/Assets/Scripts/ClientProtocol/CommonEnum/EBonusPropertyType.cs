using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 属性加成类型
/// </summary>
public enum EBonusPropertyType {
	NONE, //0 ==== 
	[InspectorName("TALENT - idx[1] - 大臣资质加成")]
	TALENT, //1 ==== 大臣资质加成
	[InspectorName("POWER - idx[2] - 大臣实力绝对值加成")]
	POWER, //2 ==== 大臣实力绝对值加成
	[InspectorName("POWER_PER - idx[3] - 大臣实力加成（万分比）")]
	POWER_PER, //3 ==== 大臣实力加成（万分比）
	[InspectorName("BONUS - idx[4] - 建筑收益绝对值加成")]
	BONUS, //4 ==== 建筑收益绝对值加成
	[InspectorName("EVENING_DUNGEON_POWER_PER - idx[5] - 晚间副本实力加成（万分比）")]
	EVENING_DUNGEON_POWER_PER, //5 ==== 晚间副本实力加成（万分比）
	[InspectorName("FIN_STUDY_BONUS_PER - idx[6] - 毕业收益加成（万分比）")]
	FIN_STUDY_BONUS_PER, //6 ==== 毕业收益加成（万分比）
	[InspectorName("BUILDING_PROFIT_ADD_PER - idx[7] - 建筑收益加成（万分比）")]
	BUILDING_PROFIT_ADD_PER, //7 ==== 建筑收益加成（万分比）
	[InspectorName("INTIMACY - idx[8] - 亲密度")]
	INTIMACY, //8 ==== 亲密度
	[InspectorName("CHARM - idx[9] - 加护力")]
	CHARM, //9 ==== 加护力
	[InspectorName("CHARMPOINT_PER - idx[10] - 加护点加成（万分比）")]
	CHARMPOINT_PER, //10 ==== 加护点加成（万分比）
	[InspectorName("MIDDAY_DUNGEON_HERO_ATTACK_EXTRA_TIMES - idx[11] - 午间副本大臣额外攻击次数")]
	MIDDAY_DUNGEON_HERO_ATTACK_EXTRA_TIMES, //11 ==== 午间副本大臣额外攻击次数
	[InspectorName("ARENA_POWER_ADD_PER - idx[12] - 竞技场实力加成（万分比）")]
	ARENA_POWER_ADD_PER, //12 ==== 竞技场实力加成（万分比）
	[InspectorName("CHILD_TRAIN_COST_PER - idx[13] - 子嗣上课金币消耗减少加成（万分比）")]
	CHILD_TRAIN_COST_PER, //13 ==== 子嗣上课金币消耗减少加成（万分比）
	[InspectorName("CHILD_TRAIN_GAIN_PER - idx[14] - 子嗣上课获得伙伴经验加成（万分比）")]
	CHILD_TRAIN_GAIN_PER, //14 ==== 子嗣上课获得伙伴经验加成（万分比）
	[InspectorName("BUILDING_EMPLOYEE_REDUCE_PER - idx[15] - 建筑雇佣成本减少加成（万分比）")]
	BUILDING_EMPLOYEE_REDUCE_PER, //15 ==== 建筑雇佣成本减少加成（万分比）
	[InspectorName("CHARMPOINT - idx[16] - 加护点（绝对值）")]
	CHARMPOINT, //16 ==== 加护点（绝对值）
	[InspectorName("FIN_STUDY_BONUS - idx[17] - 毕业收益加成（绝对值）")]
	FIN_STUDY_BONUS, //17 ==== 毕业收益加成（绝对值）
	[InspectorName("CHILD_TRAIN_GAIN - idx[18] - 子嗣上课获得伙伴经验加成（绝对值）")]
	CHILD_TRAIN_GAIN, //18 ==== 子嗣上课获得伙伴经验加成（绝对值）
	[InspectorName("ARENA_COINS - idx[19] - 商会谈判战胜伙伴商会币加成（绝对值）")]
	ARENA_COINS, //19 ==== 商会谈判战胜伙伴商会币加成（绝对值）
	[InspectorName("BUILDING_EMPLOYEE_PROFIT_ADD - idx[20] - 建筑员工基础赚速加成（绝对值）")]
	BUILDING_EMPLOYEE_PROFIT_ADD, //20 ==== 建筑员工基础赚速加成（绝对值）
	[InspectorName("GIFT_CHILD_FIN_STUDY_BONUS - idx[21] - 卷王子嗣毕业收益加成（万分比）")]
	GIFT_CHILD_FIN_STUDY_BONUS, //21 ==== 卷王子嗣毕业收益加成（万分比）
	[InspectorName("TREASURE_HUNT_TREASURE_OUTPUT_ADD - idx[22] - 太空寻宝奇物产出加成")]
	TREASURE_HUNT_TREASURE_OUTPUT_ADD, //22 ==== 太空寻宝奇物产出加成
	[InspectorName("ARENA_GAIN_INFLUENCE_PER - idx[23] - 竞技场影响力获得加成（万分比）")]
	ARENA_GAIN_INFLUENCE_PER, //23 ==== 竞技场影响力获得加成（万分比）
}

public class EBonusPropertyTypeComparer : IEqualityComparer<EBonusPropertyType>{
	public bool Equals(EBonusPropertyType x, EBonusPropertyType y) { return x == y; }
	public int GetHashCode(EBonusPropertyType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 24;
}
}

