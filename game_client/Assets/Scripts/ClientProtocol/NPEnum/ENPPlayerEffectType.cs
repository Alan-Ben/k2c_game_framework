using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家效果类型
/// </summary>
public enum ENPPlayerEffectType {
	NONE, //0 ==== 
	[InspectorName("C_UI_TO_SYS_SCE - idx[1] - 跳转到系统界面  C_UI_TO_SYS_SCE:ESysSceneType")]
	C_UI_TO_SYS_SCE, //1 ==== 跳转到系统界面  C_UI_TO_SYS_SCE:ESysSceneType
	[InspectorName("C_SPECIAL - idx[2] - 客户端部分特殊效果  C_SPECIAL:EClientSpecialDealType")]
	C_SPECIAL, //2 ==== 客户端部分特殊效果  C_SPECIAL:EClientSpecialDealType
	[InspectorName("C_T_ENTER_DUNGEON - idx[3] - 客户端测试进入dungeon地图  C_T_ENTER_DUNGEON:dungeonId")]
	C_T_ENTER_DUNGEON, //3 ==== 客户端测试进入dungeon地图  C_T_ENTER_DUNGEON:dungeonId
	[InspectorName("C_USE_BAG_ITEM - idx[4] - 使用背包物品  C_USE_BAG_ITEM:itemId:（usecount）")]
	C_USE_BAG_ITEM, //4 ==== 使用背包物品  C_USE_BAG_ITEM:itemId:（usecount）
	[InspectorName("C_ENTER_ACTOR_TEST - idx[5] - 客户端进入单位测试地图 C_ENTER_ACTOR_TEST:dungeonId")]
	C_ENTER_ACTOR_TEST, //5 ==== 客户端进入单位测试地图 C_ENTER_ACTOR_TEST:dungeonId
	[InspectorName("C_DEAL_ID - idx[6] - 针对ID的数据进行处理  C_DEAL_ID:EClientIDDealType:id")]
	C_DEAL_ID, //6 ==== 针对ID的数据进行处理  C_DEAL_ID:EClientIDDealType:id
	[InspectorName("C_SHOW_ERR - idx[7] - 展示错误信息  C_SHOW_ERR:key:args:args...")]
	C_SHOW_ERR, //7 ==== 展示错误信息  C_SHOW_ERR:key:args:args...
	[InspectorName("C_UI_SHOW_CUST_WND - idx[8] - 显示自定义窗口 C_UI_SHOW_CUST_WND:assetPath:objName")]
	C_UI_SHOW_CUST_WND, //8 ==== 显示自定义窗口 C_UI_SHOW_CUST_WND:assetPath:objName
	[InspectorName("C_ANI_PLAY - idx[9] - 动作控制，对应动作播放，可自定义从某帧开始播normalizedTime：0~1， C_ANI_PLAY:animatorObjName:animatorStateName（:normalizedTime）")]
	C_ANI_PLAY, //9 ==== 动作控制，对应动作播放，可自定义从某帧开始播normalizedTime：0~1， C_ANI_PLAY:animatorObjName:animatorStateName（:normalizedTime）
	[InspectorName("C_ANI_SET_BOOL - idx[10] - 动作控制，设置动作布尔值  C_ANI_SET_BOOL:animatorObjName:variableName:value")]
	C_ANI_SET_BOOL, //10 ==== 动作控制，设置动作布尔值  C_ANI_SET_BOOL:animatorObjName:variableName:value
	[InspectorName("C_ANI_SET_FLOAT - idx[11] - 动作控制，设置动作浮点值 C_ANI_SET_FLOAT:animatorObjName:variableName:value")]
	C_ANI_SET_FLOAT, //11 ==== 动作控制，设置动作浮点值 C_ANI_SET_FLOAT:animatorObjName:variableName:value
	[InspectorName("C_ANI_SET_INT - idx[12] - 动作控制，设置动作整形值 C_ANI_SET_INT:animatorObjName:variableName:value")]
	C_ANI_SET_INT, //12 ==== 动作控制，设置动作整形值 C_ANI_SET_INT:animatorObjName:variableName:value
	[InspectorName("C_ANI_G_PLAY - idx[13] - 动作控制，对应动作播放 C_ANI_G_PLAY:enum:animatorGroupName:animatorStateName")]
	C_ANI_G_PLAY, //13 ==== 动作控制，对应动作播放 C_ANI_G_PLAY:enum:animatorGroupName:animatorStateName
	[InspectorName("C_ANI_G_SET_BOOL - idx[14] - 动作控制，设置动作布尔值  C_ANI_G_SET_BOOL:animatorGroupName:variableName:value")]
	C_ANI_G_SET_BOOL, //14 ==== 动作控制，设置动作布尔值  C_ANI_G_SET_BOOL:animatorGroupName:variableName:value
	[InspectorName("C_ANI_G_SET_FLOAT - idx[15] - 动作控制，设置动作浮点值  C_ANI_G_SET_FLOAT:animatorGroupName:variableName:value")]
	C_ANI_G_SET_FLOAT, //15 ==== 动作控制，设置动作浮点值  C_ANI_G_SET_FLOAT:animatorGroupName:variableName:value
	[InspectorName("C_ANI_G_SET_INT - idx[16] - 动作控制，设置动作整形值  C_ANI_G_SET_INT:animatorGroupName:variableName:value")]
	C_ANI_G_SET_INT, //16 ==== 动作控制，设置动作整形值  C_ANI_G_SET_INT:animatorGroupName:variableName:value
	[InspectorName("UNUSE_S_S_EFFECT - idx[17] - 已废弃")]
	UNUSE_S_S_EFFECT, //17 ==== 已废弃
	[InspectorName("S_R_EFFECT - idx[18] - 执行远端效果 S_R_EFFECT:id")]
	S_R_EFFECT, //18 ==== 执行远端效果 S_R_EFFECT:id
	[InspectorName("S_GAIN_ITEM - idx[19] - 获取物品 S_GAIN_ITEM:itemType-itemId:count#itemType-itemId:count")]
	S_GAIN_ITEM, //19 ==== 获取物品 S_GAIN_ITEM:itemType-itemId:count#itemType-itemId:count
	[InspectorName("S_SPEND_ITEM - idx[20] - 扣除物品（无需保证物品数量满足）S_SPEND_ITEM:itemType-itemId:count#itemType-itemId:count")]
	S_SPEND_ITEM, //20 ==== 扣除物品（无需保证物品数量满足）S_SPEND_ITEM:itemType-itemId:count#itemType-itemId:count
	[InspectorName("S_GAIN_ITEM_FORM - idx[21] - 获取物品（高级公式） S_GAIN_ITEM_FORM:itemType:count（高级公式）:倍数（高级公式）")]
	S_GAIN_ITEM_FORM, //21 ==== 获取物品（高级公式） S_GAIN_ITEM_FORM:itemType:count（高级公式）:倍数（高级公式）
	[InspectorName("S_CHG_BUFF - idx[22] - 修改buff（不存在就新增，已存在在原基础上进行调整）  S_CHG_BUFF:buff_id:layer:timeS（秒：-1永久）")]
	S_CHG_BUFF, //22 ==== 修改buff（不存在就新增，已存在在原基础上进行调整）  S_CHG_BUFF:buff_id:layer:timeS（秒：-1永久）
	[InspectorName("S_SET_BUFF - idx[23] - 修改buff（不存在就新增，已存在在则替换原基础）  S_CHG_BUFF:buff_id:layer:timeS（秒：-1永久）")]
	S_SET_BUFF, //23 ==== 修改buff（不存在就新增，已存在在则替换原基础）  S_CHG_BUFF:buff_id:layer:timeS（秒：-1永久）
	[InspectorName("UNUSE_S_CHG_BUFF_TIME - idx[24] - 已废弃")]
	UNUSE_S_CHG_BUFF_TIME, //24 ==== 已废弃
	[InspectorName("S_DEL_BUFF - idx[25] - 移除buff  S_DEL_BUFF:buff_id")]
	S_DEL_BUFF, //25 ==== 移除buff  S_DEL_BUFF:buff_id
	[InspectorName("S_GAIN_REWARD - idx[26] - 获取奖励 S_GAIN_REWARD:reward_id#reward_id...")]
	S_GAIN_REWARD, //26 ==== 获取奖励 S_GAIN_REWARD:reward_id#reward_id...
	[InspectorName("S_START_QUEST - idx[27] - 开启任务 S_START_QUEST:quest_id")]
	S_START_QUEST, //27 ==== 开启任务 S_START_QUEST:quest_id
	[InspectorName("S_QUEST_COUNTER_CHG - idx[28] - 修改任务步骤计数 S_QUEST_COUNTER_CHG:quest_id:quest_step:quest_step_target:计数的变化数值（:ENCounterDealType默认ADD）")]
	S_QUEST_COUNTER_CHG, //28 ==== 修改任务步骤计数 S_QUEST_COUNTER_CHG:quest_id:quest_step:quest_step_target:计数的变化数值（:ENCounterDealType默认ADD）
	[InspectorName("S_RECORD_CHG - idx[29] - 修改玩家计数 S_RECORD_CHG:ENPPlayerRecordParam:计数的变化数值（:ENCounterDealType默认ADD）")]
	S_RECORD_CHG, //29 ==== 修改玩家计数 S_RECORD_CHG:ENPPlayerRecordParam:计数的变化数值（:ENCounterDealType默认ADD）
	[InspectorName("S_UNLOCK_ARENA - idx[30] - 解锁竞技场功能 S_UNLOCK_ARENA")]
	S_UNLOCK_ARENA, //30 ==== 解锁竞技场功能 S_UNLOCK_ARENA
	[InspectorName("S_EVENT_RECORD_CHG - idx[31] - 修改玩家事件计数（操作枚举：） S_EVENT_RECORD_CHG:ENPPlayerEventRecordType:sub_id:计数的变化数值（:ENCounterDealType默认ADD）")]
	S_EVENT_RECORD_CHG, //31 ==== 修改玩家事件计数（操作枚举：） S_EVENT_RECORD_CHG:ENPPlayerEventRecordType:sub_id:计数的变化数值（:ENCounterDealType默认ADD）
	[InspectorName("S_DEAL_ID - idx[32] - 服务端针对Id的数据进行处理的统一接口 S_DEAL_ID:ENPServerIDDealType:id")]
	S_DEAL_ID, //32 ==== 服务端针对Id的数据进行处理的统一接口 S_DEAL_ID:ENPServerIDDealType:id
	[InspectorName("C_SHOWCASE_LOAD - idx[33] - showcase加载npc资源 C_SHOWCASE_LOAD:NPGShowcaseIndex:单位下标:NpcId")]
	C_SHOWCASE_LOAD, //33 ==== showcase加载npc资源 C_SHOWCASE_LOAD:NPGShowcaseIndex:单位下标:NpcId
	[InspectorName("C_SHOWCASE_GRAY - idx[34] - showcase指定下标单位置灰 C_SHOWCASE_GRAY:NPGShowcaseIndex:单位下标:是否置灰")]
	C_SHOWCASE_GRAY, //34 ==== showcase指定下标单位置灰 C_SHOWCASE_GRAY:NPGShowcaseIndex:单位下标:是否置灰
	[InspectorName("C_SPECIAL_DIALOGUE - idx[35] - 客户端部分特殊对话效果 C_SPECIAL_DIALOGUE:ENPClientSpecialDoalogueDealType")]
	C_SPECIAL_DIALOGUE, //35 ==== 客户端部分特殊对话效果 C_SPECIAL_DIALOGUE:ENPClientSpecialDoalogueDealType
	[InspectorName("C_WISH_QUEST_SHOWCASE_LOAD - idx[36] - showcase加载心愿任务宠物资源 C_WISH_QUEST_SHOWCASE_LOAD:NPGShowcaseIndex:单位下标")]
	C_WISH_QUEST_SHOWCASE_LOAD, //36 ==== showcase加载心愿任务宠物资源 C_WISH_QUEST_SHOWCASE_LOAD:NPGShowcaseIndex:单位下标
	[InspectorName("C_SHOWCASE_LOAD_PET - idx[37] - showcase加载宠物资源 C_SHOWCASE_LOAD_PET:ENPPlayerParam:NPGShowcaseIndex:单位下标")]
	C_SHOWCASE_LOAD_PET, //37 ==== showcase加载宠物资源 C_SHOWCASE_LOAD_PET:ENPPlayerParam:NPGShowcaseIndex:单位下标
	[InspectorName("C_SEND_MSG - idx[38] - 发送客户端自定义监听消息 C_SEND_MSG:WinMsgType")]
	C_SEND_MSG, //38 ==== 发送客户端自定义监听消息 C_SEND_MSG:WinMsgType
	[InspectorName("C_DEAL_PRIVATE_ITEM_ACTION - idx[39] - 执行某个个人物件的某个action C_DEAL_PRIVATE_ITEM_ACTION:个人物件refId:action的refId:对应op的索引（从0开始，大部分action都只有一个op）")]
	C_DEAL_PRIVATE_ITEM_ACTION, //39 ==== 执行某个个人物件的某个action C_DEAL_PRIVATE_ITEM_ACTION:个人物件refId:action的refId:对应op的索引（从0开始，大部分action都只有一个op）
	[InspectorName("C_CAMERA_MOVE_FOCUS_TO - idx[40] - 摄像头移动聚焦到某个位置（视野焦点世界坐标） C_CAMERA_MOVE_FOCUS_TO:（x,y,z）:duration")]
	C_CAMERA_MOVE_FOCUS_TO, //40 ==== 摄像头移动聚焦到某个位置（视野焦点世界坐标） C_CAMERA_MOVE_FOCUS_TO:（x,y,z）:duration
	[InspectorName("C_GOTO_EFFECT - idx[41] - 执行effect_goto表里的effect: C_GOTO_EFFECT:id1:id2:...")]
	C_GOTO_EFFECT, //41 ==== 执行effect_goto表里的effect: C_GOTO_EFFECT:id1:id2:...
	[InspectorName("C_ENTER_PRIVATE_ITEM_SIMPLE_PVE_BATTLE - idx[42] - 进入一个个人物件挑战，布阵位置是固定的玩家初始阵型，宠物是玩家当前按战力顺序前5只")]
	C_ENTER_PRIVATE_ITEM_SIMPLE_PVE_BATTLE, //42 ==== 进入一个个人物件挑战，布阵位置是固定的玩家初始阵型，宠物是玩家当前按战力顺序前5只
	[InspectorName("C_DEAL_STRING - idx[43] - 针对String的数据进行处理  C_DEAL_STRING:EClientStringDealType:string")]
	C_DEAL_STRING, //43 ==== 针对String的数据进行处理  C_DEAL_STRING:EClientStringDealType:string
	[InspectorName("C_SPECIAL_SEND_EVENT - idx[44] - 发送第三方埋点 C_SPECIAL_SEND_EVENT:EThirdCustomEventType")]
	C_SPECIAL_SEND_EVENT, //44 ==== 发送第三方埋点 C_SPECIAL_SEND_EVENT:EThirdCustomEventType
	[InspectorName("C_PREFAB_LOAD - idx[45] - 为注册的加载对象，配合脚本PrefabControlMono使用 加载子对象 C_PREFAB_LOAD:tag:uiResPathId")]
	C_PREFAB_LOAD, //45 ==== 为注册的加载对象，配合脚本PrefabControlMono使用 加载子对象 C_PREFAB_LOAD:tag:uiResPathId
	[InspectorName("S_RND_GAIN_X_BUSINESS_WORKERS - idx[46] - 为随机建筑招募X位员工（数量） S_RND_GAIN_X_BUSINESS_WORKERS:数量")]
	S_RND_GAIN_X_BUSINESS_WORKERS, //46 ==== 为随机建筑招募X位员工（数量） S_RND_GAIN_X_BUSINESS_WORKERS:数量
	[InspectorName("S_ANECDOTE_REFRESH - idx[47] - 对政务指定刷新组进行刷新 S_ANECDOTE_REFRESH:刷新组id")]
	S_ANECDOTE_REFRESH, //47 ==== 对政务指定刷新组进行刷新 S_ANECDOTE_REFRESH:刷新组id
	[InspectorName("S_UNLOCK_SEVEN_DAYS_LOGIN - idx[48] - 解锁七日登录 S_UNLOCK_SEVEN_DAYS_LOGIN")]
	S_UNLOCK_SEVEN_DAYS_LOGIN, //48 ==== 解锁七日登录 S_UNLOCK_SEVEN_DAYS_LOGIN
	[InspectorName("S_GAIN_X_CONSORT_CHARM_FROM - idx[49] - 给予已拥有的情人魅力值（情人id,数量） S_GAIN_X_CONSORT_CHARM_FROM:情人id（高级公式）:数量（高级公式）")]
	S_GAIN_X_CONSORT_CHARM_FROM, //49 ==== 给予已拥有的情人魅力值（情人id,数量） S_GAIN_X_CONSORT_CHARM_FROM:情人id（高级公式）:数量（高级公式）
	[InspectorName("S_GAIN_X_CONSORT_INTIMACY_FROM - idx[50] - 给予已拥有的情人亲密度（情人id,数量） S_GAIN_X_CONSORT_INTIMACY_FROM:情人id（高级公式）:数量（高级公式）")]
	S_GAIN_X_CONSORT_INTIMACY_FROM, //50 ==== 给予已拥有的情人亲密度（情人id,数量） S_GAIN_X_CONSORT_INTIMACY_FROM:情人id（高级公式）:数量（高级公式）
	[InspectorName("S_GAIN_LAZY_CD - idx[51] - 给予LAZY_CD（CdId,数量）S_GAIN_LAZY_CD:CdId:数量")]
	S_GAIN_LAZY_CD, //51 ==== 给予LAZY_CD（CdId,数量）S_GAIN_LAZY_CD:CdId:数量
	[InspectorName("C_CLOSE_NODE - idx[52] - 关闭窗口节点，可指定节点 C_CLOSE_NODE（:nodeTag）")]
	C_CLOSE_NODE, //52 ==== 关闭窗口节点，可指定节点 C_CLOSE_NODE（:nodeTag）
	[InspectorName("C_SHOWCASE_LOAD_SPEC - idx[53] - showcase加载指定类型资源 C_SHOWCASE_LOAD_SPEC:NPGShowcaseIndex:单位下标:EShowcaseLoadType")]
	C_SHOWCASE_LOAD_SPEC, //53 ==== showcase加载指定类型资源 C_SHOWCASE_LOAD_SPEC:NPGShowcaseIndex:单位下标:EShowcaseLoadType
	[InspectorName("S_ADD_P_V - idx[54] - 增加玩家数值（数量：定值），S_ADD_P_V:EEffectPlayerValueType:数量")]
	S_ADD_P_V, //54 ==== 增加玩家数值（数量：定值），S_ADD_P_V:EEffectPlayerValueType:数量
	[InspectorName("S_ADD_P_V_FORM - idx[55] - 增加玩家数值（数量：高级公式），S_ADD_P_V:EEffectPlayerValueType:高级公式")]
	S_ADD_P_V_FORM, //55 ==== 增加玩家数值（数量：高级公式），S_ADD_P_V:EEffectPlayerValueType:高级公式
	[InspectorName("C_CAMERA_MOVE_FOCUS_TO_ENTRYPOINT - idx[56] - 摄像头聚焦到当前存在的指定 entry_point C_Camera_Move_Focus_To_EntryPoint:entry_point_id:duration")]
	C_CAMERA_MOVE_FOCUS_TO_ENTRYPOINT, //56 ==== 摄像头聚焦到当前存在的指定 entry_point C_Camera_Move_Focus_To_EntryPoint:entry_point_id:duration
	[InspectorName("S_LEVY_ADD_COUNT - idx[57] - 征收增加次数，S_LEVY_ADD_COUNT:征收类型枚举ELevy_Type（当前只有士兵征收有效）:数量")]
	S_LEVY_ADD_COUNT, //57 ==== 征收增加次数，S_LEVY_ADD_COUNT:征收类型枚举ELevy_Type（当前只有士兵征收有效）:数量
	[InspectorName("S_GAIN_AND_SET_CUTE_ACTOR - idx[58] - 获得且设置Q版形象 S_GAIN_AND_SET_CUTE_ACTOR:Q版形象ID（:时间（秒），默认为0）")]
	S_GAIN_AND_SET_CUTE_ACTOR, //58 ==== 获得且设置Q版形象 S_GAIN_AND_SET_CUTE_ACTOR:Q版形象ID（:时间（秒），默认为0）
	[InspectorName("C_SHOWCASE_MAGICA_CLOTH - idx[59] - showcase指定下标单位开启关闭魔法布料物理效果 C_SHOWCASE_MAGICA_CLOTH:NPGShowcaseIndex:单位下标:是否开启")]
	C_SHOWCASE_MAGICA_CLOTH, //59 ==== showcase指定下标单位开启关闭魔法布料物理效果 C_SHOWCASE_MAGICA_CLOTH:NPGShowcaseIndex:单位下标:是否开启
	[InspectorName("C_SHOW_BATCH_USE_ITEM_WND - idx[60] - 打开批量使用道具窗口 C_SHOW_BATCH_USE_ITEM_WND:itemId")]
	C_SHOW_BATCH_USE_ITEM_WND, //60 ==== 打开批量使用道具窗口 C_SHOW_BATCH_USE_ITEM_WND:itemId
	[InspectorName("C_SHOWCASE_PLAY_SFX - idx[61] - showcase在指定index播放sfx，要求这个index有对象了 C_SHOWCASE_LOAD:NPGShowcaseIndex:单位下标:sfxId")]
	C_SHOWCASE_PLAY_SFX, //61 ==== showcase在指定index播放sfx，要求这个index有对象了 C_SHOWCASE_LOAD:NPGShowcaseIndex:单位下标:sfxId
	[InspectorName("C_CAMERA_MOVE_TO - idx[62] - 摄像头移动到某个位置（世界坐标） C_CAMERA_MOVE_TO:（x,y,z）:duration")]
	C_CAMERA_MOVE_TO, //62 ==== 摄像头移动到某个位置（世界坐标） C_CAMERA_MOVE_TO:（x,y,z）:duration
	[InspectorName("C_PLAY_FUNC_UNLOCK_PROCESS - idx[63] - 播放系统解锁表现过程，C_PLAY_FUNC_UNLOCK_PROCESS:ENPFunctionType（:是否使用notice）")]
	C_PLAY_FUNC_UNLOCK_PROCESS, //63 ==== 播放系统解锁表现过程，C_PLAY_FUNC_UNLOCK_PROCESS:ENPFunctionType（:是否使用notice）
	[InspectorName("C_REFRESH_ENTRY_STATE - idx[64] - 刷新入口状态，C_REFRESH_ENTRY_STATE:ENPFunctionType")]
	C_REFRESH_ENTRY_STATE, //64 ==== 刷新入口状态，C_REFRESH_ENTRY_STATE:ENPFunctionType
	[InspectorName("C_PREFAB_DISCARD - idx[65] - 配合脚本PrefabControlMono使用，销毁子对象，C_PREFAB_DISCARD:tag")]
	C_PREFAB_DISCARD, //65 ==== 配合脚本PrefabControlMono使用，销毁子对象，C_PREFAB_DISCARD:tag
	[InspectorName("C_SET_CLOTHES_UNIT - idx[66] - 设置avatar穿戴部件，如果多个部件后面直接接上id，C_SET_CLOTHES_UNIT:部件id:部件id...")]
	C_SET_CLOTHES_UNIT, //66 ==== 设置avatar穿戴部件，如果多个部件后面直接接上id，C_SET_CLOTHES_UNIT:部件id:部件id...
	[InspectorName("C_PLAY_DIALOG_AUDIO - idx[67] - 播放对话音效 C_PLAY_DIALOG_AUDIO:音效refId:音效layer:需要停止的音效layer列表（列表中元素用,分割）")]
	C_PLAY_DIALOG_AUDIO, //67 ==== 播放对话音效 C_PLAY_DIALOG_AUDIO:音效refId:音效layer:需要停止的音效layer列表（列表中元素用,分割）
	[InspectorName("S_CHG_LEVY_SILVER_TIME - idx[68] - 修改可征收金币时间，S_CHG_LEVY_SILIVER_TIME:ENPTimeAddType（SET、ADD）:时间（秒）")]
	S_CHG_LEVY_SILVER_TIME, //68 ==== 修改可征收金币时间，S_CHG_LEVY_SILIVER_TIME:ENPTimeAddType（SET、ADD）:时间（秒）
	[InspectorName("C_SEND_MSG_WITH_PARAM - idx[69] - 发送客户端自定义监听消息 C_SEND_MSG:WinMsgType:自定义参数")]
	C_SEND_MSG_WITH_PARAM, //69 ==== 发送客户端自定义监听消息 C_SEND_MSG:WinMsgType:自定义参数
	[InspectorName("S_GAIN_X_CONSORT_LIKE_FROM - idx[70] - 给予未拥有的情人好感度（情人id,数量） S_GAIN_X_CONSORT_LIKE_FROM:情人id:数量（高级公式）")]
	S_GAIN_X_CONSORT_LIKE_FROM, //70 ==== 给予未拥有的情人好感度（情人id,数量） S_GAIN_X_CONSORT_LIKE_FROM:情人id:数量（高级公式）
	[InspectorName("C_ANI_SET_ANI - idx[71] - 动作控制，强制设置动画 C_ANI_SET_ANI:animatorObjName:aniName")]
	C_ANI_SET_ANI, //71 ==== 动作控制，强制设置动画 C_ANI_SET_ANI:animatorObjName:aniName
	[InspectorName("C_ANI_G_SET_ANI - idx[72] - 动作控制，强制设置动画 C_ANI_G_SET_ANI:animatorGroupName:aniName")]
	C_ANI_G_SET_ANI, //72 ==== 动作控制，强制设置动画 C_ANI_G_SET_ANI:animatorGroupName:aniName
	[InspectorName("C_HOTFIX_EFFECT - idx[73] - 热更操作类型 C_HOTFIX_EFFECT:操作类型:参数（参数可不填）")]
	C_HOTFIX_EFFECT, //73 ==== 热更操作类型 C_HOTFIX_EFFECT:操作类型:参数（参数可不填）
	[InspectorName("S_GAIN_X_CONSORT_CHARM_POINT_FROM - idx[74] - 给予已拥有的情人加护力点（情人id，数量，倍数） S_GAIN_X_CONSORT_CHARM_POINT_FROM:情人id（高级公式）:数量（高级公式）(:倍数，默认1)")]
	S_GAIN_X_CONSORT_CHARM_POINT_FROM, //74 ==== 给予已拥有的情人加护力点（情人id，数量，倍数） S_GAIN_X_CONSORT_CHARM_POINT_FROM:情人id（高级公式）:数量（高级公式）(:倍数，默认1)
	[InspectorName("S_GAIN_X_CHILD_EXP_POINT_FROM - idx[75] - 给予未成年子嗣增加经验（子嗣实例id，数量，倍数） S_GAIN_X_CHILD_EXP_POINT_FROM:未成年id（高级公式）:数量（高级公式）(:倍数，默认1)")]
	S_GAIN_X_CHILD_EXP_POINT_FROM, //75 ==== 给予未成年子嗣增加经验（子嗣实例id，数量，倍数） S_GAIN_X_CHILD_EXP_POINT_FROM:未成年id（高级公式）:数量（高级公式）(:倍数，默认1)
	[InspectorName("S_MARS_CHG_MOOD - idx[76] - 火星系统：修改心情指数（额外），S_MARS_CHG_MOOD:count数量")]
	S_MARS_CHG_MOOD, //76 ==== 火星系统：修改心情指数（额外），S_MARS_CHG_MOOD:count数量
	[InspectorName("S_MARS_CURE - idx[77] - 火星系统：治疗数量，S_MARS_CURE:数量（-1 全部病人）")]
	S_MARS_CURE, //77 ==== 火星系统：治疗数量，S_MARS_CURE:数量（-1 全部病人）
	[InspectorName("S_MARS_RND_B_RND_SICK - idx[78] - 火星系统：随机建筑（需要有工作居民）随机1~n个居民进入医疗室，S_MARS_RND_B_RND_SICK")]
	S_MARS_RND_B_RND_SICK, //78 ==== 火星系统：随机建筑（需要有工作居民）随机1~n个居民进入医疗室，S_MARS_RND_B_RND_SICK
	[InspectorName("S_MARS_RND_B_RND_LOST - idx[79] - 火星系统：随机建筑随机（需要有工作居民）减少1~n个居民，S_MARS_RND_B_RND_LOST")]
	S_MARS_RND_B_RND_LOST, //79 ==== 火星系统：随机建筑随机（需要有工作居民）减少1~n个居民，S_MARS_RND_B_RND_LOST
	[InspectorName("S_MARS_GAIN_ENERGY - idx[80] - 火星系统：立即获得N小时产出的能源，S_MARS_GAIN_ENERGY:mins（分钟）")]
	S_MARS_GAIN_ENERGY, //80 ==== 火星系统：立即获得N小时产出的能源，S_MARS_GAIN_ENERGY:mins（分钟）
	[InspectorName("S_GAIN_REWARD_FORM - idx[81] - 获取奖励（高级公式） S_GAIN_REWARD_FORM:reward_id（高级公式）:倍数（高级公式）")]
	S_GAIN_REWARD_FORM, //81 ==== 获取奖励（高级公式） S_GAIN_REWARD_FORM:reward_id（高级公式）:倍数（高级公式）
	[InspectorName("S_INN_ADD_GUEST - idx[82] - 旅店系统：增加接待客人数量，S_INN_ADD_GUEST:数量")]
	S_INN_ADD_GUEST, //82 ==== 旅店系统：增加接待客人数量，S_INN_ADD_GUEST:数量
	[InspectorName("C_TRY_TRIGGER_PUSH_GIFT_PACK - idx[83] - 尝试触发推送礼包 C_TRY_TRIGGER_PUSH_GIFT_PACK:礼包组id")]
	C_TRY_TRIGGER_PUSH_GIFT_PACK, //83 ==== 尝试触发推送礼包 C_TRY_TRIGGER_PUSH_GIFT_PACK:礼包组id
	[InspectorName("C_PLAY_AUDIO - idx[84] - 客户端枚举")]
	C_PLAY_AUDIO, //84 ==== 客户端枚举
	[InspectorName("S_CHG_BIRTH_GIFTDE_COUM - idx[85] - 增加卷王子嗣次数 S_CHG_BIRTH_GIFTDE_COUM:数量（:ENCounterDealType默认ADD）")]
	S_CHG_BIRTH_GIFTDE_COUM, //85 ==== 增加卷王子嗣次数 S_CHG_BIRTH_GIFTDE_COUM:数量（:ENCounterDealType默认ADD）
}

public class ENPPlayerEffectTypeComparer : IEqualityComparer<ENPPlayerEffectType>{
	public bool Equals(ENPPlayerEffectType x, ENPPlayerEffectType y) { return x == y; }
	public int GetHashCode(ENPPlayerEffectType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 86;
}
}

