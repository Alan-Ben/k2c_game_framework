using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 时间刷新类型
/// </summary>
public enum ENPTimeRefreshType {
	NONE, //0 ==== 
	[InspectorName("REF_BY_LIVE - idx[1] - 按存在时间到期刷新")]
	REF_BY_LIVE, //1 ==== 按存在时间到期刷新
	[InspectorName("REF_IF_DIS - idx[2] - 消失立马刷新")]
	REF_IF_DIS, //2 ==== 消失立马刷新
	[InspectorName("REF_DURATION - idx[3] - 按照间隔时间刷新 REF_DURATION:MIN（S）:MAX（S）")]
	REF_DURATION, //3 ==== 按照间隔时间刷新 REF_DURATION:MIN（S）:MAX（S）
	[InspectorName("REF_CLOCK - idx[4] - 按照定时时间刷新 REF_CLOCK:24进制小时数")]
	REF_CLOCK, //4 ==== 按照定时时间刷新 REF_CLOCK:24进制小时数
	[InspectorName("REF_WEEK_CLOCK - idx[5] - 按照周定时时间刷新 REF_WEEK_CLOCK:周几:24进制小时数")]
	REF_WEEK_CLOCK, //5 ==== 按照周定时时间刷新 REF_WEEK_CLOCK:周几:24进制小时数
	[InspectorName("REF_NOT_REFRESH - idx[6] - 不刷新")]
	REF_NOT_REFRESH, //6 ==== 不刷新
	[InspectorName("REF_MONTH_CLOCK - idx[7] - 按照月定时时间刷新 REF_MONTH_CLOCK:几号:24进制小时数")]
	REF_MONTH_CLOCK, //7 ==== 按照月定时时间刷新 REF_MONTH_CLOCK:几号:24进制小时数
	[InspectorName("REF_MIN_CLOCK - idx[8] - 按照定时时间刷新 REF_MIN_CLOCK:24进制小时数:60进制分钟数")]
	REF_MIN_CLOCK, //8 ==== 按照定时时间刷新 REF_MIN_CLOCK:24进制小时数:60进制分钟数
}

public class ENPTimeRefreshTypeComparer : IEqualityComparer<ENPTimeRefreshType>{
	public bool Equals(ENPTimeRefreshType x, ENPTimeRefreshType y) { return x == y; }
	public int GetHashCode(ENPTimeRefreshType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 9;
}
}

