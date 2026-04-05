using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MailEnum
{

/// <summary>
/// 邮件额外数据类型
/// </summary>
public enum EMailExtType {
	NONE, //0 ==== 
	[InspectorName("TEST_ITEM_LIST - idx[1] - 测试-物品列表")]
	TEST_ITEM_LIST, //1 ==== 测试-物品列表
	[InspectorName("HERO_GAIN - idx[2] - 骑士-骑士获得")]
	HERO_GAIN, //2 ==== 骑士-骑士获得
	[InspectorName("HERO_UP_STEP - idx[3] - 骑士-骑士升阶")]
	HERO_UP_STEP, //3 ==== 骑士-骑士升阶
}

public class EMailExtTypeComparer : IEqualityComparer<EMailExtType>{
	public bool Equals(EMailExtType x, EMailExtType y) { return x == y; }
	public int GetHashCode(EMailExtType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

