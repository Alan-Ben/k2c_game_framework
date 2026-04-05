using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 孵化场种子状态V2
/// </summary>
public enum ENPHatchSeedStatusTypeV2 {
	NONE, //0 ==== 
	[InspectorName("PREPARE - idx[1] - 等待中")]
	PREPARE, //1 ==== 等待中
	[InspectorName("INCUBATING - idx[2] - 孵化中")]
	INCUBATING, //2 ==== 孵化中
	[InspectorName("MATURE - idx[3] - 已成熟")]
	MATURE, //3 ==== 已成熟
}

public class ENPHatchSeedStatusTypeV2Comparer : IEqualityComparer<ENPHatchSeedStatusTypeV2>{
	public bool Equals(ENPHatchSeedStatusTypeV2 x, ENPHatchSeedStatusTypeV2 y) { return x == y; }
	public int GetHashCode(ENPHatchSeedStatusTypeV2 obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

