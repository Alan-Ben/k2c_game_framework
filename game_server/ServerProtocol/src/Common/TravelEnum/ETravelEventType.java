package Common.TravelEnum;

/*********
 * 游历事件类型
 **/
public enum ETravelEventType {
	NONE, //0 ==== 
	REWARD, //1 ==== 对话奖励事件
	CONSORT_LIKE, //2 ==== 妃子好感度事件
	CONSORT_INTIMACY, //3 ==== 妃子亲密度事件
	CONSORT_BAR, //4 ==== 酒馆妃子事件
	CHANGE, //5 ==== 兑换事件
	INVITATION, //6 ==== 指定邀约事件
	GIFTDE, //7 ==== 获得卷王事件
	ADD_POWER, //8 ==== 增加实力事件
	GAMBLING, //9 ==== 博彩事件
	;
public static final ETravelEventType[]  ETravelEventType_Values = ETravelEventType.values();
public static final int ETravelEventType_Length = ETravelEventType_Values.length;
public static ETravelEventType ETravelEventType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETravelEventType_Length){ return null; }
	return ETravelEventType_Values[_ivalue];
}
}

