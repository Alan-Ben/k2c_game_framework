package Common.DinnerEnum;

/*********
 * 宴会标签
 **/
public enum EDinnerType {
	NONE, //0 ==== 
	CEREMONY, //1 ==== 典礼
	PARTY, //2 ==== 酒会
	;
public static final EDinnerType[]  EDinnerType_Values = EDinnerType.values();
public static final int EDinnerType_Length = EDinnerType_Values.length;
public static EDinnerType EDinnerType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EDinnerType_Length){ return null; }
	return EDinnerType_Values[_ivalue];
}
}

