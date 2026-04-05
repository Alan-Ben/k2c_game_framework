using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace ClientEnum
{

/// <summary>
/// 客户端用，玩家 Avatar 编辑场景的视点类型
/// </summary>
public enum EPlayerAvatarEditViewType {
	NONE, //0 ==== 
	[InspectorName("NORMAL - idx[1] - 正常视点")]
	NORMAL, //1 ==== 正常视点
	[InspectorName("FACE - idx[2] - 脸部视点")]
	FACE, //2 ==== 脸部视点
	[InspectorName("DYE - idx[3] - 染色视点")]
	DYE, //3 ==== 染色视点
}

public class EPlayerAvatarEditViewTypeComparer : IEqualityComparer<EPlayerAvatarEditViewType>{
	public bool Equals(EPlayerAvatarEditViewType x, EPlayerAvatarEditViewType y) { return x == y; }
	public int GetHashCode(EPlayerAvatarEditViewType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

