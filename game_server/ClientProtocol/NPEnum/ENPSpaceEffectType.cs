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
public enum ENPSpaceEffectType {
	NONE, //0 ==== 
	[InspectorName("S_DSI_GROUP_REFRESH - idx[1] - 执行动态对象刷新组的刷新  S_DSI_GROUP_REFRESH:dynamic_item_refresh的id")]
	S_DSI_GROUP_REFRESH, //1 ==== 执行动态对象刷新组的刷新  S_DSI_GROUP_REFRESH:dynamic_item_refresh的id
	[InspectorName("S_DSI_GROUP_DISCARD - idx[2] - 执行动态对象刷新组的销毁  S_DSI_GROUP_DISCARD:dynamic_item_refresh的id")]
	S_DSI_GROUP_DISCARD, //2 ==== 执行动态对象刷新组的销毁  S_DSI_GROUP_DISCARD:dynamic_item_refresh的id
}

public class ENPSpaceEffectTypeComparer : IEqualityComparer<ENPSpaceEffectType>{
	public bool Equals(ENPSpaceEffectType x, ENPSpaceEffectType y) { return x == y; }
	public int GetHashCode(ENPSpaceEffectType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

