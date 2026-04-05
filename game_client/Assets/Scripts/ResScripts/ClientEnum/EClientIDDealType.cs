namespace GOE
{
    /// <summary>
    /// 客户端ID处理效果枚举
    /// </summary>
    public enum EClientIDDealType
    {
	    NONE, //0 ==== 
	    REMOTE_EFF, //1 ==== 使用remote effect
	    TUTORIAL_DONE, //2 ==== 设置引导完成
	    QUEST_DONE, //3 ==== 提交任务完成请求
	    OPEN_MAIL, //4 ==== 打开对应邮件
	    ENTER_SIMPLE_BATTLE, //5 ==== 进入一个纯客户端的战斗表现，布阵位置是固定的玩家初始阵型，宠物是玩家当前按战力顺序前5只，一般用于引导missionId
	    SHOW_TUTORIAL, //6 ==== 纯展示教程窗口UI，id = 对应资源resPatchId
	    SIMULATE_CLICK_CLOTHES_UNIT, //7 ==== 衣橱模拟点击衣橱单位：部件表的id
	    SIMULATE_CLICK_CLOTHES_DIRECTORY, //8 ==== 衣橱模拟点击衣橱文件夹：菜单表的id
	    MAIN_CITY_FOCUS_POINT, //9 ==== 主城相机移动到功能入口点，entry_point表id
	    SIMULATE_OPEN_HERO_INFO, //10 ==== 模拟打开英雄信息界面 heroid
	    SIMULATE_SELECT_HERO_RECOMMEND_INDEX, //11 ==== 模拟选择英雄推荐位置 heroid
	    SIMULATE_CLICK_DAILY_QUEST_REWARD, //12 ==== 模拟点击日常任务领奖 位置索引01234
	    SIMULATE_CLICK_ACHIEVE_REWARD, //13 ==== 模拟点击成就领奖 位置索引01234
	    SIMULATE_CLICK_COLLEGE_POS_ITEM, //14 ==== 模拟点击大学位置 位置索引从1开始
	    MOVE_FOCUS_TO_HERO_RECOMMEND, //15 ==== 主城移动镜头到骑士推荐入口居中，id为队列id
	    SIMULATE_CLICK_OPEN_MARKET_SHOP, //16 ==== 模拟打开店铺弹窗，market_shop表id
	    SIMULATE_CLICK_SUIT_ITEM, //17 ==== 模拟点击套装item试穿，clothes_suit表id
	    ROOM_FOCUS_POINT, //18 ==== 卧室相机移动到功能入口点，entry_point表id
	    SIMULATE_CLICK_GIVE_CONSORT_GIFT, //19 ==== 模拟点击赠送知己礼物，道具下标从0开始
	    SIMULATE_CLICK_OPEN_FIXED_RANK, //20 ==== 模拟点击打开常驻排行榜，rank_fixed表id
	    SIMULATE_CLICK_FIXED_RANK_LIKE, //21 ==== 模拟点击常驻排行榜点赞，rank_fixed表id
	    SIMULATE_CLICK_CUSTOM_SUIT_LIST_ITEM, //22 ==== 模拟点击自定义套装选项，选项从上到下的下标从0开始
	    SHOW_SIMPLE_TUTORIAL,//23 ==== 展示简易引导，id = 对应简易引导id,要自己通过node逻辑触发
	    DIALOGUE_PLAY_AUDIO, //24 ==== 对话播放音效，音效id
        SET_HERO_RECOMMEND_BUBBLE_ANI_SAMPLE, //25 ==== 设置骑士推荐气泡动画到某帧，id从0到100，0第一帧，100最后一帧
		SIMULATE_CLICK_HERO_TALENT_ITEM, //26 ==== 模拟点击骑士资质item，item下标从0开始
		SIMULATE_CLICK_MARKET_ITEM_UPGRADE, //27 ==== 模拟点击码头item升级，item下标从0开始
        SIMULATE_OPEN_HERO_INFO_BY_INDEX, //28 ==== 模拟点击打开骑士信息界面，item下标从0开始
        SIMULATE_CLICK_MAIL_ITEM,// 29 ==== 模拟点击邮件item(只在邮件页面执行时有效), item下标从0开始
        SIMULATE_CLICK_CLOTHES_ACTION_INDEX, //30 ==== 模拟点击选择第x个动作, item下标从0开始
        SIMULATE_CLICK_CLOTHES_POSE_INDEX, //31 ==== 模拟点击选择第x个站姿, item下标从0开始
        SIMULATE_CLICK_CLOTHES_BG_INDEX, //32 ==== 模拟点击选择第x个背景, item下标从0开始
        SIMULATE_OPEN_CONSORT_INFO_BY_INDEX, //33 ==== 模拟点击打开情人信息界面，item下标从0开始
        SIMULATE_CLICK_FARMING_BUILDING_COLLECTION, //34 ==== 模拟点击农场建筑收获，id为建筑id
        SIMULATE_CLICK_BUILDING_BUILD, //35 ==== 模拟点击建筑建造，id为建筑id
        SIMULATE_CLICK_ANECDOTE_POS, //36 ==== 模拟点击经营事件，id为事件位置id
        SIMULATE_CLICK_BUSINESS_BUILDING_HERO_CARD, //37 ==== 模拟点击经营建筑英雄卡片, item下标从0开始
        MOVE_FOCUS_TO_FARMING_BUILDING_UPGRADE_BTN_AND_SHOW_GUIDE_HAND, //38 ==== 移动镜头到农场建筑升级按钮并显示引导手指，id为建筑id
        SIMULATE_CLICK_HERO_WEAR_EQUIP_BY_INDEX, //39 ==== 模拟点击伙伴选择佩戴藏品，item下标从0开始
        SIMULATE_CLICK_CONSORT_BLESS_SKILL_UPGRADE_BY_INDEX, //40 ==== 模拟点击家人加护技能升级，item下标从0开始
        SIMULATE_SELECT_ARENA_HERO_BY_INDEX, //41 ==== 模拟选择竞技场伙伴，item下标从0开始
        SIMULATE_ARENA_SELECT_OPPONENT_HERO_BY_INDEX, //42 ==== 模拟选择竞技场想要打的对手伙伴，item下标从0开始
        SIMULATE_CLICK_HOLD_DINNER_INDEX, //43 ==== 模拟点击选择举办宴会，item下标从0开始
        SIMULATE_CLICK_TOWER_CHALLENGE_INDEX, //44 ==== 爬塔模拟点击挑战当前列表第几个boss的效果(序列号0为当前界面最底下一层)
        SIMULATE_CLICK_EQUIP_ITEM_BY_INDEX, //45 ==== 模拟点击藏品列表打开详情界面, item下标从0开始
        SIMULATE_CLICK_GET_STAGE_GOAL_BIG_STEP_REWARD_BY_ID, //46 ==== 模拟点击领取阶段目标大阶段奖励，参数为大阶段id
        SIMULATE_CLICK_BUILDING_PRODUCT_UNLOCK_BY_INDEX, //47 ==== 模拟点击解锁建筑产品，item下标从0开始
        SHOW_SIMPLE_TUTORIAL_AUTO_TRIGGER, //48 ==== 触发简易引导，id = 对应简易引导id,自动触发
        SIMULATE_CLICK_INN_STATION_ENTER_BY_INDEX, //49 ==== 模拟点击旅店设施列表进入按钮，item下标从0开始
        SIMULATE_CLICK_INN_DISH_ITEM_BY_DISH_INDEX, //50 ==== 模拟点击旅店菜品item，使用菜品索引
        SIMULATE_CLICK_CONSORT_CG_ITEM_BY_INDEX, //51 ==== 模拟点击妃子CG item，item下标从0开始
        SIMULATE_CLICK_CONSORT_ADD_FRIEND_INDEX, //52 ==== 模拟点击情人聊天添加好友列表 item，item下标从0开始
        SIMULATE_CLICK_CONSORT_CHAT_INDEX, //53 ==== 模拟点击情人聊天列表 item，item下标从0开始
        SIMULATE_CLICK_EQUIP_RECYCLE_ITEM_INDEX, //54 ==== 模拟点击藏品回收列表 item，item下标从0开始
        SIMULATE_CLICK_RECRUIT_ENTRANCE, //55 ==== 模拟点击招募顾问、情人入口，id为招募商店id
        SIMULATE_SELECT_TREASURE_HUNT_SELECT_AREA, //56 ==== 模拟点击选择区域选择，id为区域id
        SIMULATE_CLICK_CONDITION_DESC_JUMP_BY_INDEX, //57 ==== 模拟点击条件描述跳转按钮，item下标从0开始
        SIMULATE_CLICK_MARS_EXPLORE_TEAM_EDIT_ITEM_EDIT_BY_INDEX, //58 ==== 模拟点击火星探索队伍编辑item编辑按钮，item下标从0开始
        SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_SELECT_GRID_ITEM_BY_INDEX, //59 ==== 模拟点击火星探索队伍英雄选择grid item，item下标从0开始
        SIMULATE_CLICK_MARS_EXPLORE_EVENT, //60 ==== 模拟点击火星探索事件，id为事件位置posId
        SIMULATE_CLICK_MARS_EXPLORE_TEAM_SELECT_ITEM_CONFIRM_BY_INDEX, //61 ==== 模拟点击火星探索队伍选择item确认按钮，item下标从0开始
        SIMULATE_CLICK_MARS_EXPLORE, //62 ==== 模拟点击火星探索按钮，id为posId
        SIMULATE_CLICK_HOLD_CONSORT_DINNER_INDEX, //63 ==== 模拟点击选择举办情人宴会，item下标从0开始
        SIMULATE_CLICK_SHOP_ITEM, //64 ==== 模拟点击购买商店商品，id为商品下标
        SIMULATE_CLICK_CONSORT_TRAVEL_ITEM_BY_INDEX, //65 ==== 模拟点击情人出游列表 item，item下标从0开始
        SIMULATE_CLICK_ARENA_CELEBRITY_ATTACK_BY_INDEX, //66 ==== 模拟点击竞技场名人榜攻击，id为名人榜下标
    }
}

