package Common.TreasureHuntEnum;

/*********
 * 太空寻宝获得类型
 **/
public enum ETreasureHuntGainType {
	NONE, //0 ==== 
	ORE, //1 ==== 矿石
	TREASURE, //2 ==== 奇物
	REWARD, //3 ==== 奖励
	;
public static final ETreasureHuntGainType[]  ETreasureHuntGainType_Values = ETreasureHuntGainType.values();
public static final int ETreasureHuntGainType_Length = ETreasureHuntGainType_Values.length;
public static ETreasureHuntGainType ETreasureHuntGainType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETreasureHuntGainType_Length){ return null; }
	return ETreasureHuntGainType_Values[_ivalue];
}
}

