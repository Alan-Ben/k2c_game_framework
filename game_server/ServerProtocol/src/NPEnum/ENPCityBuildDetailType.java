package NPEnum;

/*********
 * 建筑细分类型
 **/
public enum ENPCityBuildDetailType {
	NONE, //0 ==== 
	NORMAL, //1 ==== 未分类
	HATCH, //2 ==== 孵化场
	MUSEUM, //3 ==== 博物馆
	COOKING, //4 ==== 烹饪
	FIGHT, //5 ==== 关卡
	MINI_GAME_QUEST, //6 ==== 悬赏任务
	PLAYER_CARVING, //7 ==== 玩家雕像
	FIGHT_REWARD, //8 ==== 关卡奖励
	STELLA, //9 ==== 家族星座
	SIGN_IN, //10 ==== 签到建筑
	ELF, //11 ==== 精灵爱心工坊建筑
	;
public static final ENPCityBuildDetailType[]  ENPCityBuildDetailType_Values = ENPCityBuildDetailType.values();
public static final int ENPCityBuildDetailType_Length = ENPCityBuildDetailType_Values.length;
public static ENPCityBuildDetailType ENPCityBuildDetailType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCityBuildDetailType_Length){ return null; }
	return ENPCityBuildDetailType_Values[_ivalue];
}
}

