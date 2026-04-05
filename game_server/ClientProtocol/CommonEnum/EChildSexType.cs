using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 子嗣性别
/// </summary>
public enum EChildSexType {
	NONE, //0 ==== 
	BOY, //1 ==== 
	GIRL, //2 ==== 
}

public class EChildSexTypeComparer : IEqualityComparer<EChildSexType>{
	public bool Equals(EChildSexType x, EChildSexType y) { return x == y; }
	public int GetHashCode(EChildSexType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

