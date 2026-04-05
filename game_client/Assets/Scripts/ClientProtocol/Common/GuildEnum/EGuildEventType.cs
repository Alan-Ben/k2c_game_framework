using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟事件类型
/// </summary>
public enum EGuildEventType {
	NONE, //0 ==== 
	[InspectorName("IMPEACH_LEADER - idx[1] - 弹劾盟主")]
	IMPEACH_LEADER, //1 ==== 弹劾盟主
}

public class EGuildEventTypeComparer : IEqualityComparer<EGuildEventType>{
	public bool Equals(EGuildEventType x, EGuildEventType y) { return x == y; }
	public int GetHashCode(EGuildEventType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

