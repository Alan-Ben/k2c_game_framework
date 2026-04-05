package NPEnum;

/*********
 * 服务端ID处理效果枚举
 **/
public enum ENPServerIDDealType {
	NONE, //0 ==== 
	TAKE_MAIL_ATT, //1 ==== 取出对应邮件Id的附件
	;
public static final ENPServerIDDealType[]  ENPServerIDDealType_Values = ENPServerIDDealType.values();
public static final int ENPServerIDDealType_Length = ENPServerIDDealType_Values.length;
public static ENPServerIDDealType ENPServerIDDealType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPServerIDDealType_Length){ return null; }
	return ENPServerIDDealType_Values[_ivalue];
}
}

