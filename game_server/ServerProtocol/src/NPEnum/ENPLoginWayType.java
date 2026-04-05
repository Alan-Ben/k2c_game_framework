package NPEnum;

/*********
 * 游戏登录方式，与文档账号类型相同 https\:\/\/thoughts.teambition.com\/share\/647e8f5e90a37b003eb87cf4
 **/
public enum ENPLoginWayType {
	NONE, //0 ==== 
	GUEST, //1 ==== 游客
	GAMECENTER, //2 ==== 游戏圈账号登录  -- iOS支持 android不支持
	IOS, //3 ==== Apple账号登录    -- iOS支持 android不支持
	GOOGLE, //4 ==== 谷歌账号登录    -- iOS不支持 android支持
	FACEBOOK, //5 ==== facebook账号登录
	VK, //6 ==== vk账号登录
	LINE, //7 ==== line账号登录
	TWITTER, //8 ==== twitter账号登录
	MJ, //9 ==== 梦加账号登录
	;
public static final ENPLoginWayType[]  ENPLoginWayType_Values = ENPLoginWayType.values();
public static final int ENPLoginWayType_Length = ENPLoginWayType_Values.length;
public static ENPLoginWayType ENPLoginWayType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPLoginWayType_Length){ return null; }
	return ENPLoginWayType_Values[_ivalue];
}
}

