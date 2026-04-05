package CommonEnum;

/*********
 * 商店商品刷新来源类型
 **/
public enum EShopItemRefreshSourceType {
	NONE, //0 ==== 
	PLAYER, //1 ==== 玩家
	SERVER, //2 ==== 服务器
	;
public static final EShopItemRefreshSourceType[]  EShopItemRefreshSourceType_Values = EShopItemRefreshSourceType.values();
public static final int EShopItemRefreshSourceType_Length = EShopItemRefreshSourceType_Values.length;
public static EShopItemRefreshSourceType EShopItemRefreshSourceType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EShopItemRefreshSourceType_Length){ return null; }
	return EShopItemRefreshSourceType_Values[_ivalue];
}
}

