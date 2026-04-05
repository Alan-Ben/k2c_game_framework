package NPEnum;

/*********
 * 通用跨服分组监听类型
 **/
public enum ENPCommCrossGroupEventEnum {
	CREATE, //0 ==== 分组创建触发监听
	DISCARD, //1 ==== 分组销毁触发监听
	;
public static final ENPCommCrossGroupEventEnum[]  ENPCommCrossGroupEventEnum_Values = ENPCommCrossGroupEventEnum.values();
public static final int ENPCommCrossGroupEventEnum_Length = ENPCommCrossGroupEventEnum_Values.length;
public static ENPCommCrossGroupEventEnum ENPCommCrossGroupEventEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCommCrossGroupEventEnum_Length){ return null; }
	return ENPCommCrossGroupEventEnum_Values[_ivalue];
}
}

