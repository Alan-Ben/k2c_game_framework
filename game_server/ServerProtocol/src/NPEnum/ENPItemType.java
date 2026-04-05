package NPEnum;

/*********
 * 物品大类型
 **/
public enum ENPItemType {
	NONE, //0 ==== 
	CURRENCY, //1 ==== 货币，count: 数量
	BAG_ITEM, //2 ==== 背包物品，count: 数量
	TITLE, //3 ==== 普通称号，count: 时间（秒）
	ICON, //4 ==== 头像，count: 时间（秒）
	ICON_BGK, //5 ==== 头像框，count: 时间（秒）
	BUBBLE, //6 ==== 气泡框，count: 时间（秒）
	REMOTE_EFFECT, //7 ==== 远程效果，id为Remote配表ID，count: 执行次数
	MAIL, //8 ==== 邮件，预设邮件
	SYS_INFO, //9 ==== 系统信息（如解锁信息），count: 数量
	BUILDING, //10 ==== 给予建筑信息，count: 数量
	REWARD, //11 ==== 奖励, count: 数量
	CUTE_ACTOR, //12 ==== Q版形象，count: 时间（秒）
	WEEK_CARD, //13 ==== 周卡，count:时间（秒）
	SYS_UNLOCK, //14 ==== 系统解锁类型，对应simple_unlock数据，可以增加系统解锁展示，count: 数量
	ANECDOTE_EVENT, //15 ==== 政务事件 ----- itemId：事件id，count: 位置id
	EQUIP, //16 ==== 藏品，count: 数量
	TITLE_PRE, //17 ==== 组合称号前缀
	TITLE_SFX, //18 ==== 组合称号后缀
	TITLE_BG, //19 ==== 组合称号底色
	PLAYER_SKIN, //20 ==== 玩家形象，count: 时间（秒）
	ACTIVITY_CURRENCY, //21 ==== 活动兑换券，id为活动id, count: 数量
	LAZY_CD, //22 ==== CD，count: 数量
	INN_RECIPE, //23 ==== 旅店菜谱，count: 1
	MUSEUM_ITEM, //24 ==== 博物馆物品，count:1
	PAY, //25 ==== 现金支付，count:无意义
	BUFF, //26 ==== BUFF，count:有效时长（秒）
	FOREVER_ADD, //27 ==== 永久加成，id：player_forever_add配表id，count: 次数
	TOOL_TIP_ITEM, //28 ==== 点击提示，客户端使用
	EXCHANGE_ITEM, //29 ==== 转换道具，id为转换配表id,count:转换次数
	COUNTDOWN_EVENT, //30 ==== 倒计时事件，id为倒计时事件配表id，count：数量
	QUEST, //31 ==== 任务，count无意义
	ITEM_DEF, //32 ==== 自定义道具 id为itemRef表id，count：数量
	FIXED_CD, //33 ==== 固定点增加cd，count: 数量
	RECORD, //34 ==== 玩家记录，id枚举：ENPPlayerRecordParam，count：数量
	TREASURE_MAP, //35 ==== 藏宝图，count:数量
	UNUSE_36, //36 ==== 未使用 ----- 随机宠物，count:数量
	SHARE, //37 ==== 分享物品，id为share表ID，count:分享次数
	CLOTHES_UNIT_RAND_DYE, //38 ==== 随机获得指定单品色盘，id为单品ID，count：1
	CLOTHES_DYE_PALETTE, //39 ==== 色盘, id为配表ID，count：1
	UNUSE_40, //40 ==== 未使用 ----- 精灵，id为elf表id，count为数量
	HERO, //41 ==== 大臣，id为hero表id，count：1
	HERO_SKIN, //42 ==== 大臣皮肤，id为hero_skin表id，count：1
	CONSORT, //43 ==== 情人，id为consort表id，count：1
	CONSORT_SKIN, //44 ==== 情人皮肤，id为consort_skin表id，count：1
	CLOTHES_UNIT, //45 ==== 时装单品，id为配表ID，count：1
	CLOTHES_BG, //46 ==== 时装背景，id为配表ID，count：1
	ACHIEVE_POINT, //47 ==== 成就点，id为EAchieveTyped的id，count：数量
	CLOTHES_SUIT, //48 ==== 时装套装，id为配表ID，count：1
	HERO_RECOMMEND, //49 ==== 大臣推荐事件
	CHAT_EMOTE_GROUP, //50 ==== 聊天表情包，count: 时间（秒）
	CLOTHES_ACT, //51 ==== 时装动作，id为配表ID，count：1
	CLOTHES_POSE, //52 ==== 时装姿势，id为配表ID，count：1
	CLOTHES_HAND_POSE, //53 ==== 时装手势，id为配表ID，count：1
	CONSORT_CG, //54 ==== 情人CG，id为consort_cg表id，count：1
	PRIVILEGE_CARD, //55 ==== 权益卡，id为EPrivilegeCardType枚举，count：数量
	GUILD_BOX, //56 ==== 联盟宝箱
	ROOM_SKIN, //57 ==== 房间皮肤
	;
public static final ENPItemType[]  ENPItemType_Values = ENPItemType.values();
public static final int ENPItemType_Length = ENPItemType_Values.length;
public static ENPItemType ENPItemType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPItemType_Length){ return null; }
	return ENPItemType_Values[_ivalue];
}
}

