package CommonEnum;

/*********
 * 跑马灯是否可删除类型
 **/
public enum EMarqueeCanDelType {
	READ_REF, //0 ==== 读表, 即不覆盖
	TRUE, //1 ==== 可删除
	FALSE, //2 ==== 不可删除
	;
public static final EMarqueeCanDelType[]  EMarqueeCanDelType_Values = EMarqueeCanDelType.values();
public static final int EMarqueeCanDelType_Length = EMarqueeCanDelType_Values.length;
public static EMarqueeCanDelType EMarqueeCanDelType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarqueeCanDelType_Length){ return null; }
	return EMarqueeCanDelType_Values[_ivalue];
}
}

