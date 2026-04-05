package NPEnum;

/*********
 * 玩家组合具体称号
 **/
public enum ENPPlayerComboTitleType {
	PRE, //0 ==== 前缀
	SFX, //1 ==== 后缀
	BG, //2 ==== 底色
	;
public static final ENPPlayerComboTitleType[]  ENPPlayerComboTitleType_Values = ENPPlayerComboTitleType.values();
public static final int ENPPlayerComboTitleType_Length = ENPPlayerComboTitleType_Values.length;
public static ENPPlayerComboTitleType ENPPlayerComboTitleType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerComboTitleType_Length){ return null; }
	return ENPPlayerComboTitleType_Values[_ivalue];
}
}

