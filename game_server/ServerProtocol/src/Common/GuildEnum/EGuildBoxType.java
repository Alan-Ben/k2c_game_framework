package Common.GuildEnum;

/*********
 * 联盟宝箱类型
 **/
public enum EGuildBoxType {
	NONE, //0 ==== 
	GUILD_ACTIVE_BOX, //1 ==== 联盟辉煌宝箱
	GUILD_FREE_BOX, //2 ==== 免费赠礼宝箱
	GUILD_GIFT_BOX, //3 ==== 盟友等级宝箱
	;
public static final EGuildBoxType[]  EGuildBoxType_Values = EGuildBoxType.values();
public static final int EGuildBoxType_Length = EGuildBoxType_Values.length;
public static EGuildBoxType EGuildBoxType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildBoxType_Length){ return null; }
	return EGuildBoxType_Values[_ivalue];
}
}

