package NPEnum;

/*********
 * 博物馆藏品数量,筛选类型
 **/
public enum ENPMusuemCountType {
	NONE, //0 ==== 
	QUALITY, //1 ==== 品质
	;
public static final ENPMusuemCountType[]  ENPMusuemCountType_Values = ENPMusuemCountType.values();
public static final int ENPMusuemCountType_Length = ENPMusuemCountType_Values.length;
public static ENPMusuemCountType ENPMusuemCountType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPMusuemCountType_Length){ return null; }
	return ENPMusuemCountType_Values[_ivalue];
}
}

