package CommonEnum;

/*********
 * 活动物品过期时间类型
 **/
public enum EActivityItemExpireTimeType {
	NONE, //0 ==== 
	REWARDING, //1 ==== 活动进入领奖期
	CLOSED, //2 ==== 活动关闭
	;
public static final EActivityItemExpireTimeType[]  EActivityItemExpireTimeType_Values = EActivityItemExpireTimeType.values();
public static final int EActivityItemExpireTimeType_Length = EActivityItemExpireTimeType_Values.length;
public static EActivityItemExpireTimeType EActivityItemExpireTimeType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EActivityItemExpireTimeType_Length){ return null; }
	return EActivityItemExpireTimeType_Values[_ivalue];
}
}

