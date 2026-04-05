package NPEnum;

/*********
 * 宠物资质品质类型
 **/
public enum ENPAttrGrade {
	NONE, //0 ==== 
	N, //1 ==== 
	R, //2 ==== 
	SR, //3 ==== 
	SSR, //4 ==== 
	UR, //5 ==== 
	SP, //6 ==== 
	;
public static final ENPAttrGrade[]  ENPAttrGrade_Values = ENPAttrGrade.values();
public static final int ENPAttrGrade_Length = ENPAttrGrade_Values.length;
public static ENPAttrGrade ENPAttrGrade_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPAttrGrade_Length){ return null; }
	return ENPAttrGrade_Values[_ivalue];
}
}

