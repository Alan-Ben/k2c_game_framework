using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ActivityEnum
{

/// <summary>
/// 活动状态枚举
/// </summary>
public enum EActivityState {
	[InspectorName("PLAN - idx[0] - 待开启")]
	PLAN, //0 ==== 待开启
	[InspectorName("PLAYING - idx[1] - 运行中")]
	PLAYING, //1 ==== 运行中
	[InspectorName("SETTLING - idx[2] - 结算中")]
	SETTLING, //2 ==== 结算中
	[InspectorName("REWARDING - idx[3] - 领奖期")]
	REWARDING, //3 ==== 领奖期
	[InspectorName("CLOSED - idx[4] - 已关闭")]
	CLOSED, //4 ==== 已关闭
	[InspectorName("CAN_DISCARD - idx[5] - 可销毁")]
	CAN_DISCARD, //5 ==== 可销毁
	[InspectorName("RESTORE - idx[6] - 活动状态恢复中")]
	RESTORE, //6 ==== 活动状态恢复中
	[InspectorName("INITIALIZING - idx[7] - 活动初始化中")]
	INITIALIZING, //7 ==== 活动初始化中
	[InspectorName("LOAD_FROM_DB - idx[8] - 从数据库加载中")]
	LOAD_FROM_DB, //8 ==== 从数据库加载中
}

public class EActivityStateComparer : IEqualityComparer<EActivityState>{
	public bool Equals(EActivityState x, EActivityState y) { return x == y; }
	public int GetHashCode(EActivityState obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 9;
}
}

