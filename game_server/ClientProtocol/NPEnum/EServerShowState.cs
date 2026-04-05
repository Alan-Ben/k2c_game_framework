using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 服务器对外显示状态
/// </summary>
public enum EServerShowState {
	[InspectorName("NEW - idx[0] - 新服")]
	NEW, //0 ==== 新服
	[InspectorName("FULL - idx[1] - 爆满")]
	FULL, //1 ==== 爆满
	[InspectorName("RECOMMEND - idx[2] - 推荐")]
	RECOMMEND, //2 ==== 推荐
}

public class EServerShowStateComparer : IEqualityComparer<EServerShowState>{
	public bool Equals(EServerShowState x, EServerShowState y) { return x == y; }
	public int GetHashCode(EServerShowState obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

