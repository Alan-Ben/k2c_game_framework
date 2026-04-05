using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.RegularActivityEnum
{

/// <summary>
/// 万能活动事件
/// </summary>
public enum ERegularActivity_GameEvent {
	NONE, //0 ==== 
	[InspectorName("REGULAR_SHOP_BUY_ITEM - [100001] - idx[1] - 万能活动商店购买物品")]
	REGULAR_SHOP_BUY_ITEM=100001, //1 ==== 万能活动商店购买物品
	[InspectorName("REGULAR_USE_ITEM - [100002] - idx[2] - 万能活动使用道具")]
	REGULAR_USE_ITEM=100002, //2 ==== 万能活动使用道具
}

public class ERegularActivity_GameEventComparer : IEqualityComparer<ERegularActivity_GameEvent>{
	public bool Equals(ERegularActivity_GameEvent x, ERegularActivity_GameEvent y) { return x == y; }
	public int GetHashCode(ERegularActivity_GameEvent obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

