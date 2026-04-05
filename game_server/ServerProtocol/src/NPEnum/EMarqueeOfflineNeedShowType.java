package NPEnum;

/*********
 * 跑马灯-玩家离线期间展示
 **/
public enum EMarqueeOfflineNeedShowType {
	READ_REF, //0 ==== 读表
	TRUE, //1 ==== 展示
	FALSE, //2 ==== 不展示
	;
public static final EMarqueeOfflineNeedShowType[]  EMarqueeOfflineNeedShowType_Values = EMarqueeOfflineNeedShowType.values();
public static final int EMarqueeOfflineNeedShowType_Length = EMarqueeOfflineNeedShowType_Values.length;
public static EMarqueeOfflineNeedShowType EMarqueeOfflineNeedShowType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarqueeOfflineNeedShowType_Length){ return null; }
	return EMarqueeOfflineNeedShowType_Values[_ivalue];
}
}

