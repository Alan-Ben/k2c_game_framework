package Common.DinnerEnum;

/*********
 * 宴会凭证类型
 **/
public enum EDinnerPermitType {
	NONE, //0 ==== 
	FAMILY, //1 ==== 家庭宴
	CHILD_CELE, //2 ==== 子嗣庆功宴
	TOWER_CELE, //3 ==== 爬塔庆功宴
	GIFTDE_CHILD_CELE, //4 ==== 卷王子嗣庆功宴
	;
public static final EDinnerPermitType[]  EDinnerPermitType_Values = EDinnerPermitType.values();
public static final int EDinnerPermitType_Length = EDinnerPermitType_Values.length;
public static EDinnerPermitType EDinnerPermitType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EDinnerPermitType_Length){ return null; }
	return EDinnerPermitType_Values[_ivalue];
}
}

