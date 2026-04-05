using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 宠物资质品质类型
/// </summary>
public enum ENPAttrGrade {
	NONE, //0 ==== 
	N, //1 ==== 
	R, //2 ==== 
	SR, //3 ==== 
	SSR, //4 ==== 
	UR, //5 ==== 
	SP, //6 ==== 
}

public class ENPAttrGradeComparer : IEqualityComparer<ENPAttrGrade>{
	public bool Equals(ENPAttrGrade x, ENPAttrGrade y) { return x == y; }
	public int GetHashCode(ENPAttrGrade obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 7;
}
}

