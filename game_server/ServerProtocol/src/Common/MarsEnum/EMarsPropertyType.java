package Common.MarsEnum;

/*********
 * 火星属性
 **/
public enum EMarsPropertyType {
	NONE, //0 ==== 
	HEALTH_ADD_PER, //1 ==== 健康指数万分比加成
	HAPPY_ADD_PER, //2 ==== 幸福指数万分比加成
	OXYGEN_ADD_PER, //3 ==== 氧气指数万分比加成
	SATIETY_ADD_PER, //4 ==== 饱腹指数万分比加成
	SLEEP_ADD_PER, //5 ==== 睡眠指数万分比加成
	COMFORT_ADD_PER, //6 ==== 舒适指数万分比加成
	MOOD_ADD_PER, //7 ==== 心情指数万分比加成
	;
public static final EMarsPropertyType[]  EMarsPropertyType_Values = EMarsPropertyType.values();
public static final int EMarsPropertyType_Length = EMarsPropertyType_Values.length;
public static EMarsPropertyType EMarsPropertyType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarsPropertyType_Length){ return null; }
	return EMarsPropertyType_Values[_ivalue];
}
}

