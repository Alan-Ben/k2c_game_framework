package NPEnum;

/*********
 * 英雄标记处理
 **/
public enum ENPHeroTag {
	NONE, //0 ==== 无效属性
	RANGE_ATT, //1 ==== 远程
	NEAR_ATT, //2 ==== 近战
	MAGIC_ATT, //3 ==== 魔法
	;
public static final ENPHeroTag[]  ENPHeroTag_Values = ENPHeroTag.values();
public static final int ENPHeroTag_Length = ENPHeroTag_Values.length;
public static ENPHeroTag ENPHeroTag_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPHeroTag_Length){ return null; }
	return ENPHeroTag_Values[_ivalue];
}
}

