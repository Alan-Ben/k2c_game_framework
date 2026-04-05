using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 服务端ID处理效果枚举
/// </summary>
public enum ENPServerIDDealType {
	NONE, //0 ==== 
	[InspectorName("TAKE_MAIL_ATT - idx[1] - 取出对应邮件Id的附件")]
	TAKE_MAIL_ATT, //1 ==== 取出对应邮件Id的附件
}

public class ENPServerIDDealTypeComparer : IEqualityComparer<ENPServerIDDealType>{
	public bool Equals(ENPServerIDDealType x, ENPServerIDDealType y) { return x == y; }
	public int GetHashCode(ENPServerIDDealType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

