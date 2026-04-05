package Common.MailEnum;

/*********
 * 邮件额外数据类型
 **/
public enum EMailExtType {
	NONE, //0 ==== 
	TEST_ITEM_LIST, //1 ==== 测试-物品列表
	HERO_GAIN, //2 ==== 骑士-骑士获得
	HERO_UP_STEP, //3 ==== 骑士-骑士升阶
	;
public static final EMailExtType[]  EMailExtType_Values = EMailExtType.values();
public static final int EMailExtType_Length = EMailExtType_Values.length;
public static EMailExtType EMailExtType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMailExtType_Length){ return null; }
	return EMailExtType_Values[_ivalue];
}
}

