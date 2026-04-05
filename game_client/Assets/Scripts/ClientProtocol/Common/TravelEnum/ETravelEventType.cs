using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.TravelEnum
{

/// <summary>
/// 游历事件类型
/// </summary>
public enum ETravelEventType {
	NONE, //0 ==== 
	[InspectorName("REWARD - idx[1] - 对话奖励事件")]
	REWARD, //1 ==== 对话奖励事件
	[InspectorName("CONSORT_LIKE - idx[2] - 妃子好感度事件")]
	CONSORT_LIKE, //2 ==== 妃子好感度事件
	[InspectorName("CONSORT_INTIMACY - idx[3] - 妃子亲密度事件")]
	CONSORT_INTIMACY, //3 ==== 妃子亲密度事件
	[InspectorName("CONSORT_BAR - idx[4] - 酒馆妃子事件")]
	CONSORT_BAR, //4 ==== 酒馆妃子事件
	[InspectorName("CHANGE - idx[5] - 兑换事件")]
	CHANGE, //5 ==== 兑换事件
	[InspectorName("INVITATION - idx[6] - 指定邀约事件")]
	INVITATION, //6 ==== 指定邀约事件
	[InspectorName("GIFTDE - idx[7] - 获得卷王事件")]
	GIFTDE, //7 ==== 获得卷王事件
	[InspectorName("ADD_POWER - idx[8] - 增加实力事件")]
	ADD_POWER, //8 ==== 增加实力事件
	[InspectorName("GAMBLING - idx[9] - 博彩事件")]
	GAMBLING, //9 ==== 博彩事件
}

public class ETravelEventTypeComparer : IEqualityComparer<ETravelEventType>{
	public bool Equals(ETravelEventType x, ETravelEventType y) { return x == y; }
	public int GetHashCode(ETravelEventType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 10;
}
}

