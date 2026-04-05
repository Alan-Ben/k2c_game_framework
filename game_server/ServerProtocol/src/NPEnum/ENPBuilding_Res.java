package NPEnum;

/*********
 * 资源建筑分类枚举
 **/
public enum ENPBuilding_Res {
	NONE, //0 ==== 
	DEFAULT, //1 ==== 默认产出，获取不到其他类型的时候获取本类型
	GOLD, //2 ==== 金币产出
	FOOD, //3 ==== 食物产出
	P_EXP, //4 ==== 玩家经验
	DUST, //5 ==== 粉尘
	ENERGY, //6 ==== 注能道具
	CEREALS, //7 ==== 粮食
	FIGHT_REWARD, //8 ==== 关卡征收
	WOOD, //9 ==== 建筑材料
	;
public static final ENPBuilding_Res[]  ENPBuilding_Res_Values = ENPBuilding_Res.values();
public static final int ENPBuilding_Res_Length = ENPBuilding_Res_Values.length;
public static ENPBuilding_Res ENPBuilding_Res_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPBuilding_Res_Length){ return null; }
	return ENPBuilding_Res_Values[_ivalue];
}
}

