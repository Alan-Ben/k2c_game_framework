using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace ClientEnum
{

/// <summary>
/// 背包中的物品点击下拉弹窗的类型
/// </summary>
public enum EBagClickShowView {
	NONE, //0 ==== 
	[InspectorName("DETAIL - idx[1] - 详情")]
	DETAIL, //1 ==== 详情
	[InspectorName("USE - idx[2] - 使用")]
	USE, //2 ==== 使用
	[InspectorName("CONVERT - idx[3] - 合成")]
	CONVERT, //3 ==== 合成
	[InspectorName("USE_AND_CONVERT - idx[4] - 使用和合成")]
	USE_AND_CONVERT, //4 ==== 使用和合成
}

public class EBagClickShowViewComparer : IEqualityComparer<EBagClickShowView>{
	public bool Equals(EBagClickShowView x, EBagClickShowView y) { return x == y; }
	public int GetHashCode(EBagClickShowView obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}

