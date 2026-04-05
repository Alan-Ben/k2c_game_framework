package Hotfix.V01.Enum.TileMatchEnum;

/*********
 * 三消格子类型
 **/
public enum ETileMatch_BlockType {
	NONE, //0 ==== 
	BOOM, //1 ==== 宝箱
	ROCKET, //2 ==== 火箭
	RAINBOW, //3 ==== 彩虹
	;
public static final ETileMatch_BlockType[]  ETileMatch_BlockType_Values = ETileMatch_BlockType.values();
public static final int ETileMatch_BlockType_Length = ETileMatch_BlockType_Values.length;
public static ETileMatch_BlockType ETileMatch_BlockType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETileMatch_BlockType_Length){ return null; }
	return ETileMatch_BlockType_Values[_ivalue];
}
}

