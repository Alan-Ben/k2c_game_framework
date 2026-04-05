using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ClothesEnum
{

/// <summary>
/// avatar抽卡条件类型
/// </summary>
public enum EAvatarGachaGuaranteeCondType {
	NONE, //0 ==== 
	[InspectorName("QUALITY - idx[1] - 筛选道具品质 QUALITY:min:max")]
	QUALITY, //1 ==== 筛选道具品质 QUALITY:min:max
	[InspectorName("TAG - idx[2] - 筛选道具TAG TAG:EXAMPLE")]
	TAG, //2 ==== 筛选道具TAG TAG:EXAMPLE
	[InspectorName("DONT_HAVE - idx[3] - 筛选还没获得的道具")]
	DONT_HAVE, //3 ==== 筛选还没获得的道具
}

public class EAvatarGachaGuaranteeCondTypeComparer : IEqualityComparer<EAvatarGachaGuaranteeCondType>{
	public bool Equals(EAvatarGachaGuaranteeCondType x, EAvatarGachaGuaranteeCondType y) { return x == y; }
	public int GetHashCode(EAvatarGachaGuaranteeCondType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

