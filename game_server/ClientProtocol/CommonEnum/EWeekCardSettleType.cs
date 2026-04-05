using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 周卡处理类型
/// </summary>
public enum EWeekCardSettleType {
	NONE, //0 ==== 
	[InspectorName("LEVY - idx[1] - 征收")]
	LEVY, //1 ==== 征收
	[InspectorName("ANECDOTE - idx[2] - 政务")]
	ANECDOTE, //2 ==== 政务
	[InspectorName("CONSORT_RND_CALL - idx[3] - 妃子倾诉")]
	CONSORT_RND_CALL, //3 ==== 妃子倾诉
	[InspectorName("CHILD_TRAIN - idx[4] - 子嗣培养")]
	CHILD_TRAIN, //4 ==== 子嗣培养
	[InspectorName("COLLEGE_STUDY - idx[5] - 大学学习")]
	COLLEGE_STUDY, //5 ==== 大学学习
	[InspectorName("TRAVEL - idx[6] - 游历")]
	TRAVEL, //6 ==== 游历
}

public class EWeekCardSettleTypeComparer : IEqualityComparer<EWeekCardSettleType>{
	public bool Equals(EWeekCardSettleType x, EWeekCardSettleType y) { return x == y; }
	public int GetHashCode(EWeekCardSettleType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 7;
}
}

