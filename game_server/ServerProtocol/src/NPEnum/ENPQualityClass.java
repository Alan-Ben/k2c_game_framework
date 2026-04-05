package NPEnum;

/*********
 * 品质类型，用于客户端和策划定义客户端显示部分数据
 **/
public enum ENPQualityClass {
	NONE, //0 ==== 无
	EQUIP, //1 ==== 藏品
	BAG_ITEM, //2 ==== 道具
	RECIPE, //3 ==== 烹饪食谱
	MUSEUM_ITEM, //4 ==== 博物馆藏品
	AVATAR, //5 ==== 服装
	MUSEUM_ITEM_CHIP, //6 ==== 博物馆藏品碎片
	ICON, //7 ==== 玩家头像
	SPACE_CAPTURE_ITEM, //8 ==== 宠物捕捉道具
	MINI_GAME_QUEST, //9 ==== 悬赏任务
	PET_RAND, //10 ==== 宠物随机资质
	SEED, //11 ==== 种子
	HERO_SKIN, //12 ==== 骑士皮肤
	CONSORT_SKIN, //13 ==== 妃子皮肤
	CLOTHES_SUIT, //14 ==== 套装
	HERO, //15 ==== 伙伴
	CONSORT, //16 ==== 家人
	;
public static final ENPQualityClass[]  ENPQualityClass_Values = ENPQualityClass.values();
public static final int ENPQualityClass_Length = ENPQualityClass_Values.length;
public static ENPQualityClass ENPQualityClass_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPQualityClass_Length){ return null; }
	return ENPQualityClass_Values[_ivalue];
}
}

