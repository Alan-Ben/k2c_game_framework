package Hotfix.V01.Enum.TileMatchEnum;

/*********
 * 三消连接类型
 **/
public enum ETileMatch_LinkType {
	NONE, //0 ==== 
	LINK_3, //1 ==== 
	LINK_4, //2 ==== 
	LINK_5, //3 ==== 
	LINK_T, //4 ==== 
	LINK_L, //5 ==== 
	;
public static final ETileMatch_LinkType[]  ETileMatch_LinkType_Values = ETileMatch_LinkType.values();
public static final int ETileMatch_LinkType_Length = ETileMatch_LinkType_Values.length;
public static ETileMatch_LinkType ETileMatch_LinkType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETileMatch_LinkType_Length){ return null; }
	return ETileMatch_LinkType_Values[_ivalue];
}
}

