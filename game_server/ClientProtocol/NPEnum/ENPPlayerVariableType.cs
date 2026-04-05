using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家的参数类型 高级公式类型
/// </summary>
public enum ENPPlayerVariableType {
	NONE, //0 ==== 
	[InspectorName("C_RND - idx[1] - 随机值 c_rnd@max")]
	C_RND, //1 ==== 随机值 c_rnd@max
	[InspectorName("S_RND - idx[2] - 服务端随机值 s_rnd@max")]
	S_RND, //2 ==== 服务端随机值 s_rnd@max
	[InspectorName("CS_NUM - idx[3] - 指定数字 cs_num@num")]
	CS_NUM, //3 ==== 指定数字 cs_num@num
	[InspectorName("CS_VALUE - idx[4] - 指定类型的值，如等级等 cs_value@ENPPlayerValueType")]
	CS_VALUE, //4 ==== 指定类型的值，如等级等 cs_value@ENPPlayerValueType
	[InspectorName("CS_PROPERTY - idx[5] - 玩家属性 cs_property@ENPPlayerPropertyType")]
	CS_PROPERTY, //5 ==== 玩家属性 cs_property@ENPPlayerPropertyType
	[InspectorName("CS_VAR_V - idx[6] - 对应的动态值 cs_var_v@ENPPlayerVariableVarType")]
	CS_VAR_V, //6 ==== 对应的动态值 cs_var_v@ENPPlayerVariableVarType
	[InspectorName("CS_BUF_L - idx[7] - buf层级 cs_buf_l@buf_id")]
	CS_BUF_L, //7 ==== buf层级 cs_buf_l@buf_id
	[InspectorName("CS_BUFF_HAD_ACTIVE_DAY - idx[8] - buff已生效自然天 从1开始 cs_buff_had_active_day@buff_id")]
	CS_BUFF_HAD_ACTIVE_DAY, //8 ==== buff已生效自然天 从1开始 cs_buff_had_active_day@buff_id
	[InspectorName("CS_ITEM_COUNT - idx[9] - 物品数量 cs_item_count@item_type@sub_id")]
	CS_ITEM_COUNT, //9 ==== 物品数量 cs_item_count@item_type@sub_id
	[InspectorName("CS_HAS_BUILDING - idx[10] - 是否拥有建筑 CS_HAS_BUILDING@建筑id 1=有 0=无")]
	CS_HAS_BUILDING, //10 ==== 是否拥有建筑 CS_HAS_BUILDING@建筑id 1=有 0=无
	[InspectorName("CS_PARAM - idx[11] - 玩家系数 cs_param@ENPPlayerParam")]
	CS_PARAM, //11 ==== 玩家系数 cs_param@ENPPlayerParam
	[InspectorName("CS_VALUE_ID - idx[12] - 玩家对应Id的某一个值 cs_value_id@ENPPlayerIdValueType@value_id")]
	CS_VALUE_ID, //12 ==== 玩家对应Id的某一个值 cs_value_id@ENPPlayerIdValueType@value_id
	[InspectorName("CS_BUSINESS_BUILDING_WORKER_NUM - idx[13] - 指定经营建筑员工数量 CS_BUSINESS_BUILDING_WORKER_NUM@建筑id）")]
	CS_BUSINESS_BUILDING_WORKER_NUM, //13 ==== 指定经营建筑员工数量 CS_BUSINESS_BUILDING_WORKER_NUM@建筑id）
	[InspectorName("CS_BUILDING_LEVEL - idx[14] - 建筑等级 CS_BUILDING_LEVEL@建筑功能类型EBuildingFuncEnum@建筑id（如果传0为该类型下所有）")]
	CS_BUILDING_LEVEL, //14 ==== 建筑等级 CS_BUILDING_LEVEL@建筑功能类型EBuildingFuncEnum@建筑id（如果传0为该类型下所有）
	[InspectorName("CS_CONDITION_BOOLEAN_V - idx[15] - 返回条件的布尔值 CS_CONDITION_BOOLEAN_V@（条件）条件外的括号比较特殊，不可省略")]
	CS_CONDITION_BOOLEAN_V, //15 ==== 返回条件的布尔值 CS_CONDITION_BOOLEAN_V@（条件）条件外的括号比较特殊，不可省略
	[InspectorName("CS_RECORD_PARAM - idx[16] - 玩家记录 CS_RECORD_PARAM@ENPPlayerRecordParam")]
	CS_RECORD_PARAM, //16 ==== 玩家记录 CS_RECORD_PARAM@ENPPlayerRecordParam
	[InspectorName("CS_EVENT_RECORD - idx[17] - 玩家事件记录计数 CS_EVENT_RECORD@ENPPlayerEventRecordType（@sub_id）注意，sub_id使用负数时需要用括号括起来，否则会被当成分隔符无法读取例： '（-1）'")]
	CS_EVENT_RECORD, //17 ==== 玩家事件记录计数 CS_EVENT_RECORD@ENPPlayerEventRecordType（@sub_id）注意，sub_id使用负数时需要用括号括起来，否则会被当成分隔符无法读取例： '（-1）'
	[InspectorName("CS_BUILDING_LVL - idx[18] - 已放置指定建筑的等级 CS_BUILDING_LVL@建筑ID")]
	CS_BUILDING_LVL, //18 ==== 已放置指定建筑的等级 CS_BUILDING_LVL@建筑ID
	[InspectorName("CS_SCOPE_ITEM_COUNT - idx[19] - 范围内物品数量 CS_SCOPE_ITEM_COUNT@item_type@startSubIdS:endSubIdS")]
	CS_SCOPE_ITEM_COUNT, //19 ==== 范围内物品数量 CS_SCOPE_ITEM_COUNT@item_type@startSubIdS:endSubIdS
	[InspectorName("S_RND_FROM_SCOPE - idx[20] - 范围内随机值 S_RND_FROM_SCOPE@startCount@endCount")]
	S_RND_FROM_SCOPE, //20 ==== 范围内随机值 S_RND_FROM_SCOPE@startCount@endCount
	[InspectorName("S_RND_BASIC_ATTR - idx[21] - 已废弃 - 随机基础属性类型 S_RND_BASIC_ATTR（@EBasicAttrType） 如果传了则是指定类型，没传则为随机")]
	S_RND_BASIC_ATTR, //21 ==== 已废弃 - 随机基础属性类型 S_RND_BASIC_ATTR（@EBasicAttrType） 如果传了则是指定类型，没传则为随机
	[InspectorName("S_RND_ATTR_HERO - idx[22] - 随机已拥有特长大臣 S_RND_ATTR_HERO（@ESpecAttrType） 如果传了则是指定类型，没传则为随机")]
	S_RND_ATTR_HERO, //22 ==== 随机已拥有特长大臣 S_RND_ATTR_HERO（@ESpecAttrType） 如果传了则是指定类型，没传则为随机
	[InspectorName("S_RND_CONSORT - idx[23] - 随机已拥有情人 S_RND_CONSORT")]
	S_RND_CONSORT, //23 ==== 随机已拥有情人 S_RND_CONSORT
	[InspectorName("CS_HERO_NUM - idx[24] - 已拥有大臣数量（可配置等级上下限，无限制即-1） CS_HERO_NUM（@min_lv@max_lv）")]
	CS_HERO_NUM, //24 ==== 已拥有大臣数量（可配置等级上下限，无限制即-1） CS_HERO_NUM（@min_lv@max_lv）
	[InspectorName("S_RND_VALUE_BY_WEIGHT - idx[25] - 通过权重随机数值 S_RND_VALUE_BY_WEIGHT@value1：weight1；value2：weight2...")]
	S_RND_VALUE_BY_WEIGHT, //25 ==== 通过权重随机数值 S_RND_VALUE_BY_WEIGHT@value1：weight1；value2：weight2...
	[InspectorName("CS_LEVY_SUM - idx[26] - 征收累计数量 CS_LEVY_SUM@ELevy_Type")]
	CS_LEVY_SUM, //26 ==== 征收累计数量 CS_LEVY_SUM@ELevy_Type
	[InspectorName("CS_CLOTHES_UNIT_QUALITY_NUM - idx[27] - 指定品质服装数量 CS_CLOTHES_UNIT_QUALITY_NUM@ENPQuality")]
	CS_CLOTHES_UNIT_QUALITY_NUM, //27 ==== 指定品质服装数量 CS_CLOTHES_UNIT_QUALITY_NUM@ENPQuality
	[InspectorName("CS_CONSORT_RES_COUNT - idx[28] - 指定家人指定类型资源数量 CS_CONSORT_RES_COUNT@家人ID@EBagItemUse_ConsortDrawShowType")]
	CS_CONSORT_RES_COUNT, //28 ==== 指定家人指定类型资源数量 CS_CONSORT_RES_COUNT@家人ID@EBagItemUse_ConsortDrawShowType
	[InspectorName("CS_CONSORT_FETTER_LEVLE - idx[29] - 指定家人的羁绊等级 C_CONSORT_FETTER_LEVLE@家人ID")]
	CS_CONSORT_FETTER_LEVLE, //29 ==== 指定家人的羁绊等级 C_CONSORT_FETTER_LEVLE@家人ID
	[InspectorName("CS_MARS_BUILDING_LVL - idx[30] - 指定火星建筑等级 CS_MARS_BUILDING_LVL@火星建筑ID")]
	CS_MARS_BUILDING_LVL, //30 ==== 指定火星建筑等级 CS_MARS_BUILDING_LVL@火星建筑ID
	[InspectorName("CS_MARS_BUILDING_EQUIP_LVL - idx[31] - 指定火星建筑指定部件等级 CS_MARS_BUILDING_EQUIP_LVL@火星建筑ID@部件ID")]
	CS_MARS_BUILDING_EQUIP_LVL, //31 ==== 指定火星建筑指定部件等级 CS_MARS_BUILDING_EQUIP_LVL@火星建筑ID@部件ID
	[InspectorName("CS_MARS_BUILDING_DISPATCH_NUM - idx[32] - 指定火星建筑派遣居民数量 CS_MARS_BUILDING_DISPATCH_NUM@火星建筑ID")]
	CS_MARS_BUILDING_DISPATCH_NUM, //32 ==== 指定火星建筑派遣居民数量 CS_MARS_BUILDING_DISPATCH_NUM@火星建筑ID
}

public class ENPPlayerVariableTypeComparer : IEqualityComparer<ENPPlayerVariableType>{
	public bool Equals(ENPPlayerVariableType x, ENPPlayerVariableType y) { return x == y; }
	public int GetHashCode(ENPPlayerVariableType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 33;
}
}

