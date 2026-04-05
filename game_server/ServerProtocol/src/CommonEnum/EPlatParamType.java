package CommonEnum;

/*********
 * 平台参数类型
 **/
public enum EPlatParamType {
	CLIENT_VERSION, //0 ==== 客户端资源版本
	HOT_REF_URL_BASE, //1 ==== 热更配表的CDN地址
	;
public static final EPlatParamType[]  EPlatParamType_Values = EPlatParamType.values();
public static final int EPlatParamType_Length = EPlatParamType_Values.length;
public static EPlatParamType EPlatParamType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EPlatParamType_Length){ return null; }
	return EPlatParamType_Values[_ivalue];
}
}

