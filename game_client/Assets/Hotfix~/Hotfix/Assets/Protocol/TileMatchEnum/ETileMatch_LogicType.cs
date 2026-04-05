using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.TileMatchEnum
{

/// <summary>
/// 三消逻辑类型
/// </summary>
public enum ETileMatch_LogicType {
	NONE, //0 ==== 
	[InspectorName("NORMAL - idx[1] - 普通 Composite")]
	NORMAL, //1 ==== 普通 Composite
	[InspectorName("BOX - idx[2] - 宝箱 Remove")]
	BOX, //2 ==== 宝箱 Remove
	[InspectorName("ROCKET - idx[3] - 火箭 Remove")]
	ROCKET, //3 ==== 火箭 Remove
	[InspectorName("RAINBOW - idx[4] - 彩虹 Remove")]
	RAINBOW, //4 ==== 彩虹 Remove
	[InspectorName("BOX_BOX - idx[5] - 宝箱+宝箱 CombineRemove")]
	BOX_BOX, //5 ==== 宝箱+宝箱 CombineRemove
	[InspectorName("BOX_ROCKET - idx[6] - 宝箱+火箭 CombineRemove")]
	BOX_ROCKET, //6 ==== 宝箱+火箭 CombineRemove
	[InspectorName("ROCKET_ROCKET - idx[7] - 火箭+火箭 CombineRemove")]
	ROCKET_ROCKET, //7 ==== 火箭+火箭 CombineRemove
	[InspectorName("BOX_RAINBOW - idx[8] - 宝箱+彩虹 RainbowTrans+Remove")]
	BOX_RAINBOW, //8 ==== 宝箱+彩虹 RainbowTrans+Remove
	[InspectorName("ROCKET_RAINBOW - idx[9] - 火箭+彩虹 RainbowTrans+Remove")]
	ROCKET_RAINBOW, //9 ==== 火箭+彩虹 RainbowTrans+Remove
	[InspectorName("RAINBOW_RAINBOW - idx[10] - 彩虹+彩虹 CombineRemove")]
	RAINBOW_RAINBOW, //10 ==== 彩虹+彩虹 CombineRemove
	[InspectorName("DROP - idx[11] - 掉落 Drop")]
	DROP, //11 ==== 掉落 Drop
}

public class ETileMatch_LogicTypeComparer : IEqualityComparer<ETileMatch_LogicType>{
	public bool Equals(ETileMatch_LogicType x, ETileMatch_LogicType y) { return x == y; }
	public int GetHashCode(ETileMatch_LogicType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 12;
}
}

