using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 日志记录类型，增删改
/// </summary>
public enum ENpLogType {
	NONE, //0 ==== 
	[InspectorName("ADD - idx[1] - 增加")]
	ADD, //1 ==== 增加
	[InspectorName("DEL - idx[2] - 删除")]
	DEL, //2 ==== 删除
	[InspectorName("SET - idx[3] - 设置")]
	SET, //3 ==== 设置
}

public class ENpLogTypeComparer : IEqualityComparer<ENpLogType>{
	public bool Equals(ENpLogType x, ENpLogType y) { return x == y; }
	public int GetHashCode(ENpLogType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

