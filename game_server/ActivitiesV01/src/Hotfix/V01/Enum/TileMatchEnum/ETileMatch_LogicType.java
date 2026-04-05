package Hotfix.V01.Enum.TileMatchEnum;

/*********
 * 三消逻辑类型
 **/
public enum ETileMatch_LogicType {
	NONE, //0 ==== 
	NORMAL, //1 ==== 普通 Composite
	BOX, //2 ==== 宝箱 Remove
	ROCKET, //3 ==== 火箭 Remove
	RAINBOW, //4 ==== 彩虹 Remove
	BOX_BOX, //5 ==== 宝箱+宝箱 CombineRemove
	BOX_ROCKET, //6 ==== 宝箱+火箭 CombineRemove
	ROCKET_ROCKET, //7 ==== 火箭+火箭 CombineRemove
	BOX_RAINBOW, //8 ==== 宝箱+彩虹 RainbowTrans+Remove
	ROCKET_RAINBOW, //9 ==== 火箭+彩虹 RainbowTrans+Remove
	RAINBOW_RAINBOW, //10 ==== 彩虹+彩虹 CombineRemove
	DROP, //11 ==== 掉落 Drop
	;
public static final ETileMatch_LogicType[]  ETileMatch_LogicType_Values = ETileMatch_LogicType.values();
public static final int ETileMatch_LogicType_Length = ETileMatch_LogicType_Values.length;
public static ETileMatch_LogicType ETileMatch_LogicType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETileMatch_LogicType_Length){ return null; }
	return ETileMatch_LogicType_Values[_ivalue];
}
}

