using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 孵化场种子状态
/// </summary>
public enum ENPHatchSeedStatusType {
	NONE, //0 ==== 
	[InspectorName("IDLE - idx[1] - 空闲")]
	IDLE, //1 ==== 空闲
	[InspectorName("PREPARE - idx[2] - 准备队列中")]
	PREPARE, //2 ==== 准备队列中
	[InspectorName("INCUBATING - idx[3] - 孵化中")]
	INCUBATING, //3 ==== 孵化中
	[InspectorName("MATURE - idx[4] - 成熟")]
	MATURE, //4 ==== 成熟
}

public class ENPHatchSeedStatusTypeComparer : IEqualityComparer<ENPHatchSeedStatusType>{
	public bool Equals(ENPHatchSeedStatusType x, ENPHatchSeedStatusType y) { return x == y; }
	public int GetHashCode(ENPHatchSeedStatusType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}

