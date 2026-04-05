package CommonEnum;

/*********
 * 属性加成类型
 **/
public enum EBonusPropertyType {
	NONE, //0 ==== 
	TALENT, //1 ==== 大臣资质加成
	POWER, //2 ==== 大臣实力绝对值加成
	POWER_PER, //3 ==== 大臣实力加成（万分比）
	BONUS, //4 ==== 建筑收益绝对值加成
	EVENING_DUNGEON_POWER_PER, //5 ==== 晚间副本实力加成（万分比）
	FIN_STUDY_BONUS_PER, //6 ==== 毕业收益加成（万分比）
	BUILDING_PROFIT_ADD_PER, //7 ==== 建筑收益加成（万分比）
	INTIMACY, //8 ==== 亲密度
	CHARM, //9 ==== 加护力
	CHARMPOINT_PER, //10 ==== 加护点加成（万分比）
	MIDDAY_DUNGEON_HERO_ATTACK_EXTRA_TIMES, //11 ==== 午间副本大臣额外攻击次数
	ARENA_POWER_ADD_PER, //12 ==== 竞技场实力加成（万分比）
	CHILD_TRAIN_COST_PER, //13 ==== 子嗣上课金币消耗减少加成（万分比）
	CHILD_TRAIN_GAIN_PER, //14 ==== 子嗣上课获得伙伴经验加成（万分比）
	BUILDING_EMPLOYEE_REDUCE_PER, //15 ==== 建筑雇佣成本减少加成（万分比）
	CHARMPOINT, //16 ==== 加护点（绝对值）
	FIN_STUDY_BONUS, //17 ==== 毕业收益加成（绝对值）
	CHILD_TRAIN_GAIN, //18 ==== 子嗣上课获得伙伴经验加成（绝对值）
	ARENA_COINS, //19 ==== 商会谈判战胜伙伴商会币加成（绝对值）
	BUILDING_EMPLOYEE_PROFIT_ADD, //20 ==== 建筑员工基础赚速加成（绝对值）
	GIFT_CHILD_FIN_STUDY_BONUS, //21 ==== 卷王子嗣毕业收益加成（万分比）
	TREASURE_HUNT_TREASURE_OUTPUT_ADD, //22 ==== 太空寻宝奇物产出加成
	ARENA_GAIN_INFLUENCE_PER, //23 ==== 竞技场影响力获得加成（万分比）
	;
public static final EBonusPropertyType[]  EBonusPropertyType_Values = EBonusPropertyType.values();
public static final int EBonusPropertyType_Length = EBonusPropertyType_Values.length;
public static EBonusPropertyType EBonusPropertyType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBonusPropertyType_Length){ return null; }
	return EBonusPropertyType_Values[_ivalue];
}
}

