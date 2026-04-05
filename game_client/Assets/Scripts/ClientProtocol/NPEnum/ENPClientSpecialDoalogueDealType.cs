using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

public enum ENPClientSpecialDoalogueDealType {
	NONE, //0 ==== 
	[InspectorName("CONFIRM - idx[1] - 确认")]
	CONFIRM, //1 ==== 确认
	[InspectorName("CANCEL - idx[2] - 取消")]
	CANCEL, //2 ==== 取消
}

public class ENPClientSpecialDoalogueDealTypeComparer : IEqualityComparer<ENPClientSpecialDoalogueDealType>{
	public bool Equals(ENPClientSpecialDoalogueDealType x, ENPClientSpecialDoalogueDealType y) { return x == y; }
	public int GetHashCode(ENPClientSpecialDoalogueDealType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

