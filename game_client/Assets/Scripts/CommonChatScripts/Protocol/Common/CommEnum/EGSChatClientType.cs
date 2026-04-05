using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CommEnum
{

public enum EGSChatClientType {
	NONE, //0 ==== 
	NORMAL_PLAYER, //1 ==== 普通玩家连接
	NORMAL_SERVER, //2 ==== 普通服务器连接
}

public class EGSChatClientTypeComparer : IEqualityComparer<EGSChatClientType>{
	public bool Equals(EGSChatClientType x, EGSChatClientType y) { return x == y; }
	public int GetHashCode(EGSChatClientType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

