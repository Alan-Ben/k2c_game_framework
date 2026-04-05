using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CommEnum
{

public enum EISChatClientType {
	NONE, //0 ==== 
	SDK, //1 ==== SDK连接
}

public class EISChatClientTypeComparer : IEqualityComparer<EISChatClientType>{
	public bool Equals(EISChatClientType x, EISChatClientType y) { return x == y; }
	public int GetHashCode(EISChatClientType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

