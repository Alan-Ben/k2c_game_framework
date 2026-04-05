using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CommEnum
{

public enum EMSMonitorClientType {
	NONE, //0 ==== 
	PHP, //1 ==== 
}

public class EMSMonitorClientTypeComparer : IEqualityComparer<EMSMonitorClientType>{
	public bool Equals(EMSMonitorClientType x, EMSMonitorClientType y) { return x == y; }
	public int GetHashCode(EMSMonitorClientType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

