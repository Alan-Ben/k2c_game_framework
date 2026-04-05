using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟弹劾盟主事件状态
/// </summary>
public enum EGuildImpeachLeaderEventState {
	[InspectorName("CAN_IMPEACH - idx[0] - 可弹劾")]
	CAN_IMPEACH, //0 ==== 可弹劾
	[InspectorName("IN_IMPEACH - idx[1] - 弹劾中")]
	IN_IMPEACH, //1 ==== 弹劾中
	[InspectorName("IMPEACH_SUCCESS - idx[2] - 弹劾成功")]
	IMPEACH_SUCCESS, //2 ==== 弹劾成功
	[InspectorName("IMPEACH_FAIL - idx[3] - 弹劾失败")]
	IMPEACH_FAIL, //3 ==== 弹劾失败
}

public class EGuildImpeachLeaderEventStateComparer : IEqualityComparer<EGuildImpeachLeaderEventState>{
	public bool Equals(EGuildImpeachLeaderEventState x, EGuildImpeachLeaderEventState y) { return x == y; }
	public int GetHashCode(EGuildImpeachLeaderEventState obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

