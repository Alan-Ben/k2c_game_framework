using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 分享码类型
/// </summary>
public enum EShareCodeType {
	[InspectorName("MAKE_FACE - idx[0] - 捏脸数据 PlayerShow_MakeFace")]
	MAKE_FACE, //0 ==== 捏脸数据 PlayerShow_MakeFace
	[InspectorName("CLOTHES_SHARE - idx[1] - 服装分享 Clothes_ClothesShareRawInfo to Clothes_ClothesShareInfo")]
	CLOTHES_SHARE, //1 ==== 服装分享 Clothes_ClothesShareRawInfo to Clothes_ClothesShareInfo
}

public class EShareCodeTypeComparer : IEqualityComparer<EShareCodeType>{
	public bool Equals(EShareCodeType x, EShareCodeType y) { return x == y; }
	public int GetHashCode(EShareCodeType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

