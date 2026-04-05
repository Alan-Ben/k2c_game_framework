using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.TileMatchEnum
{

/// <summary>
/// 三消连接类型
/// </summary>
public enum ETileMatch_LinkType {
	NONE, //0 ==== 
	LINK_3, //1 ==== 
	LINK_4, //2 ==== 
	LINK_5, //3 ==== 
	LINK_T, //4 ==== 
	LINK_L, //5 ==== 
}

public class ETileMatch_LinkTypeComparer : IEqualityComparer<ETileMatch_LinkType>{
	public bool Equals(ETileMatch_LinkType x, ETileMatch_LinkType y) { return x == y; }
	public int GetHashCode(ETileMatch_LinkType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 6;
}
}

