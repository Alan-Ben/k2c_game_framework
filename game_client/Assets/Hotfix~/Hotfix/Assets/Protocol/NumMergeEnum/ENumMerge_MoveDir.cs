using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.NumMergeEnum
{

/// <summary>
/// 数字合并移动方向
/// </summary>
public enum ENumMerge_MoveDir {
	NONE, //0 ==== 
	[InspectorName("UP - idx[1] - 向上")]
	UP, //1 ==== 向上
	[InspectorName("DOWN - idx[2] - 向下")]
	DOWN, //2 ==== 向下
	[InspectorName("LEFT - idx[3] - 向左")]
	LEFT, //3 ==== 向左
	[InspectorName("RIGHT - idx[4] - 向右")]
	RIGHT, //4 ==== 向右
}

public class ENumMerge_MoveDirComparer : IEqualityComparer<ENumMerge_MoveDir>{
	public bool Equals(ENumMerge_MoveDir x, ENumMerge_MoveDir y) { return x == y; }
	public int GetHashCode(ENumMerge_MoveDir obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}

