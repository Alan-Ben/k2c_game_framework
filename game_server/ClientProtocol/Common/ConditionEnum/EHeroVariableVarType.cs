using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ConditionEnum
{

/// <summary>
/// 大臣的参数类型 特殊定义的值类型
/// </summary>
public enum EHeroVariableVarType {
	NONE, //0 ==== 
}

public class EHeroVariableVarTypeComparer : IEqualityComparer<EHeroVariableVarType>{
	public bool Equals(EHeroVariableVarType x, EHeroVariableVarType y) { return x == y; }
	public int GetHashCode(EHeroVariableVarType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 1;
}
}

