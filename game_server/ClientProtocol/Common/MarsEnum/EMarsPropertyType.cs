using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MarsEnum
{

/// <summary>
/// 火星属性
/// </summary>
public enum EMarsPropertyType {
	NONE, //0 ==== 
	[InspectorName("HEALTH_ADD_PER - idx[1] - 健康指数万分比加成")]
	HEALTH_ADD_PER, //1 ==== 健康指数万分比加成
	[InspectorName("HAPPY_ADD_PER - idx[2] - 幸福指数万分比加成")]
	HAPPY_ADD_PER, //2 ==== 幸福指数万分比加成
	[InspectorName("OXYGEN_ADD_PER - idx[3] - 氧气指数万分比加成")]
	OXYGEN_ADD_PER, //3 ==== 氧气指数万分比加成
	[InspectorName("SATIETY_ADD_PER - idx[4] - 饱腹指数万分比加成")]
	SATIETY_ADD_PER, //4 ==== 饱腹指数万分比加成
	[InspectorName("SLEEP_ADD_PER - idx[5] - 睡眠指数万分比加成")]
	SLEEP_ADD_PER, //5 ==== 睡眠指数万分比加成
	[InspectorName("COMFORT_ADD_PER - idx[6] - 舒适指数万分比加成")]
	COMFORT_ADD_PER, //6 ==== 舒适指数万分比加成
	[InspectorName("MOOD_ADD_PER - idx[7] - 心情指数万分比加成")]
	MOOD_ADD_PER, //7 ==== 心情指数万分比加成
}

public class EMarsPropertyTypeComparer : IEqualityComparer<EMarsPropertyType>{
	public bool Equals(EMarsPropertyType x, EMarsPropertyType y) { return x == y; }
	public int GetHashCode(EMarsPropertyType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 8;
}
}

