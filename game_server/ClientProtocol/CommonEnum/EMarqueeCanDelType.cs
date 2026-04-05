using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 跑马灯是否可删除类型
/// </summary>
public enum EMarqueeCanDelType {
	[InspectorName("READ_REF - idx[0] - 读表, 即不覆盖")]
	READ_REF, //0 ==== 读表, 即不覆盖
	[InspectorName("TRUE - idx[1] - 可删除")]
	TRUE, //1 ==== 可删除
	[InspectorName("FALSE - idx[2] - 不可删除")]
	FALSE, //2 ==== 不可删除
}

public class EMarqueeCanDelTypeComparer : IEqualityComparer<EMarqueeCanDelType>{
	public bool Equals(EMarqueeCanDelType x, EMarqueeCanDelType y) { return x == y; }
	public int GetHashCode(EMarqueeCanDelType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

