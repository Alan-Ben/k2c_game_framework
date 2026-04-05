//窗口事件类型
public enum WinMsgType
{
    //这边初始100开始，因为跟ALMsgSys是一个管理器，避免跟ALMsgType枚举冲突
    NONE = 100,
    ON_RED_TIP_CHANGE, //红点变化

    APPLICATION_PAUSE,      //程序切换暂停
    APPLICATION_PAUSE_RESUME,      //程序暂停回复

    ON_PLAYER_NAME_CHANGE,//玩家改名
    ON_PLAYER_PARAM_CHANGE,//玩家信息变化
    PLAYER_LVL_UP_SUCC,//玩家升级成功消息
    ON_PLAYER_STATE_CHANGE,//玩家状态变化
    ON_PLAYER_SPEAKER_CHANGE,//玩家扬声器状态发生变化
    ON_PLAYER_MICRO_CHANGE,//玩家麦克风状态发生变化
    ON_PLAYER_FLOURISH_CHANGE,//玩家繁荣度发生变化

    TRIGGER_TUTORIAL,//触发战斗外引导
    TRIGGER_FUNC_UNLOCK_TIP,//触发功能解锁弹窗
    TRIGGER_TUTORIAL_START, //触发引导开始的消息
    SET_TUTORIAL_DONE,//设置引导完成
    QUIT_CURRENT_TUTORIAL,//退出当前引导
    CUR_TUTORIAL_DONE,  //当前引导完成

    CONTROL_TUROTIAL_SETP,//控制引导步骤
    TUTORIAL_DEAL_NEXT_STEP_BUTTON,//执行到下一步
    UI_CLICK,   //UI点击处理
    SCREEN_CLICK,   //屏幕点击事件，参数第一个为点击的NPTouchInfo信息
    SCREEN_PRESS,   //屏幕拖动事件

    ON_BAG_ITEM_ADD,//背包物品 增
    ON_BAG_ITEM_REMOVE,//背包物品 删
    ON_BAG_ITEM_UPDATE,//背包物品 改
    ON_BAG_ITEM_USE,//使用物品
    BAG_ITEM_USE_HERO_GRID_SCROLL_MOVE_TO,// 使用物品-伙伴列表, 滚动到大臣
    BAG_ITEM_USE_HERO_GRID_USE_FOR_HERO,// 使用物品-伙伴列表, 对大臣使用
    BAG_ITEM_GRID_SCROLL_MOVE_TO,// 背包物品列表, 滚动到指定物品
    
    ON_BAG_ITEM_COUNT_ADD,//背包物品 数量增加

    ON_PLAYER_RES_CHANGE,//玩家资源改变
    ON_PLAYER_BUFF_TRIGGER,//玩家buff触发

    ON_ADD_SELECT_MOB_CARD, //新增选中怪物卡牌
    ON_REMOVE_SELECT_MOB_CARD,//移除选择的怪物卡牌
    ON_SELECT_MOB_CARD,//选中怪物卡牌
    ON_DESELECT_MOB_CARD,//取消选中当前卡牌

    BATTLE_RESYNC,      //重新同步数据

    ON_CAMERA_POS_CHANGE, // 相机位置发生变化
    ON_CAMERA_SCALE_CHANGE, // 相机缩放发生变化
    ON_CAMERA_ORTHOGRAPH_SIZE_CHANGE, // 相机正交size发生变化

    ON_CHAT_DISCONNECT, //聊天断开
    ON_CHAT_JOIN_SESSION, //加入聊天会话
    ON_CHAT_QUIT_SESSION, //退出聊天会话
    ON_CHAT_REC_MSG, //收到聊天消息

    PRELAYOUT_CHG,       //阵容更改的消息
    PRELAYOUT_SINGLE_CHG,       //阵容单个对象修改的消息
    BATTLE_START,       //战斗开始的消息
    BATTLE_END,         //战斗结束的消息

    CUSTOM_RELOAD,      //自定义数据的重新加载消息

    MAIN_CAMERA_ENABLE, //主摄像头状态切换消息
    MAIN_CAMERA_DISABLE,

    ENTER_SPACE,//进入大地图
    ENTER_CITY,//进入乐园
    ENTER_PET,//进入宠物界面

    LORD_ATK,//领主出击
    MEET_BOSS,          //遭遇boss

    ON_PREVIEW_CARD_SKILL_DETAIL,//预览卡牌详情
    ENTER_BOSS_BATTLE,      //进入boss战
    QUIT_BOSS_BATTLE,       //退出boss战

    HANGUP_STATE_CHG,       //挂机状态变化枚举

    ON_INIT_MAIL_LIST, //初始化邮件列表
    ON_SET_MAIL_BRIEF, //设置邮件简要信息
    ON_SET_MAIL_DETAIL,//设置邮件详细信息
    ON_ADD_MAIL, //收到邮件
    ON_CHG_MAIL, //修改邮件
    ON_DEL_MAIL, //删除邮件
    GAIN_MAIL_REWARD, //获取邮件奖励
    ON_MAIL_STAT_CHG,     //邮件状态更新
    SIMULATE_GAIN_SHOW_MAIL_REWARD,  //模拟领取当前显示的邮件奖励(只有有邮件详情在显示时有效)
    SIMULATE_CLICK_MAIL_ITEM, //模拟点击邮件item

    ON_TUTORIAL_ALL_FORCE_DONE,//所有强制引导完成


    #region 乐园操作
    
    QUIT_CITY_OP,//退出乐园选中操作
    ENTER_CITY_OP,//开始进入乐园选中操作
    
    #endregion

    #region 玩家信息相关
    PLAYER_INFO_TITLE_CHG, //玩家拥有的称号数据变动
    PLAYER_INFO_ICON_CHG,  //玩家拥有的头像数据变动
    PLAYER_INFO_ICON_BGK_CHG,//玩家拥有的头像框数据变动
    PLAYER_INFO_BUBBLE_CHG,//玩家拥有的气泡框数据变动
    #endregion

    #region 排行榜相关
    RANK_LIKE_IS_ADD,//对别人进行点赞
    #endregion

    #region 任务相关
    QUEST_ADD,//新增任务
    QUEST_UPDATE,//更新任务
    QUEST_REMOVE,//删除任务
    QUEST_FOLLOW,//跟踪任务

    QUEST_START_COUNT_CHG,//任务接受数量变动
    QUEST_DONE_COUNT_CHG,//任务完成数量变动

    QUEST_TARGET_UPDATE,//更新任务目标数量
    QUEST_STEP_STATUS_CHG,//任务步骤状态变化
    #endregion

    #region 任务特殊触发，客户端触发任务进度部分
    QUEST_CT_TEST_BAG,  //打开背包，此消息在每次打开背包的时候触发，用于测试任务客户端触发的进度功能
    #endregion

    #region Avatar相关

    ON_AVATAR_SNAPSHOT_COUNT_CHG,//可用时装预设数量变更
    ON_AVATAR_SNAPSHOT_CHG_NAME_SUC,//预设重命名成功
    ON_AVATAR_SNAPSHOT_IDX_CHG,//客户端 切换预设
    ON_AVATAR_SNAPSHOT_EQUIP,//客户端 真实穿戴Avatar

    #endregion

    #region 列表操作

    ON_TO_SHOW_ALL_ITEM, // 需要显示整个item的RectRransform的时候


    #endregion

    ON_CLICK_LAYOUT_TYPE_SELECT_CHG,//当点击切换阵型
    
    ON_NEW_FUNCTION_UNLOCK_SHOW,//新功能解锁表现

    #region 大地图相关
    ON_SPACE_DIGGINGS_CHG,//大地图矿区数据变动
    ON_SPACE_PLAYER_OBJ_CHG,//大地图玩家对象发生变化的处理
    ON_SPACE_SIDE_CHG,//大地图侧边信息变动
    #endregion

    #region 博物馆相关
    ON_MUSEUM_ITEM_CHG,//博物馆收藏品变更
    ON_Museum_Pour_ResCrit,//注能触发暴击
    #endregion
    
    #region 玩家计数相关
    PLAYER_EVENT_RECORD_CHG,
    PLAYER_RECORD_CHG,
    #endregion

    #region 关卡变动
    PLAYER_MISSON_CHG,
    ON_CHAPTER_STEP_CHG,//章节阶段奖励数据变更
    ON_CHAPTER_CHG,//章节数据变更
    ON_RES_ISLAND_CHG,//资源岛数据变更
    
    ON_CHAPTER_EVENT_ADD,//当新增关卡事件推送
    ON_CHAPTER_EVENT_CHG,//关卡事件信息变化推送
    ON_CHAPTER_EVENT_REMOVE,//收到关卡事件移除推送
    ON_CHAPTER_PLOT_REWARD_DRAW,//关卡剧情奖励领取
    
    ON_CHAPTER_GAME_POS_CHANGED_BY_GM_COMMAND,//因为 GM 命令而改变了玩家的坐标
    #endregion
    
    ON_DIGGINGS_BATTLE_NODE_CLOSE,//矿区战斗窗口关闭
    CLICK_LOW_DIGGING_SHOW_OCCUPIED,//模拟点击低级矿的点击入驻
    
    REFRESH_SPACE_ITEM_SHOW_STATE,//刷新大地图物体显示状态
    REFRESH_SPACE_OPERATION_TRIGGER,//刷新大地图的操作触发器（就是玩家移动后，自动弹出的交互按钮功能，可以用来主动触发一次弹出）
    REFRESH_SPACE_OPERATION_NOT_ADD,//刷新大地图的操作触发器,只刷新，不创建
    
    #region 每日悬赏任务相关

    ON_MINI_GAME_QUEST_CHG,//悬赏任务信息变更
    ON_MINI_GAME_QUEST_REFRESH_COUNT_CHG,//悬赏任务刷新次数变更
    ON_MINI_GAME_QUEST_LVL_CHG,//悬赏等级信息变更
    ON_MINI_GAME_OPEN,//打开悬赏界面的时候触发的消息
    #endregion

    #region 日常任务
    DAILY_QUEST_CHG,//日常任务变动
    DAILY_QUEST_REWARD_CHG,//日常任务奖励变动
	DAILY_QUEST_GROUP_CHG,//任务组变动
    DAILY_QUEST_FINISH,//完成日常任务
    #endregion

    #region 好友相关
    FRIENDS_CHG,//好友数量变动
    FRIENDS_APPLY_CHG,//好友申请数量变动
    FRIENDS_VISIT_REWARD_GET,//好友拜访奖励领取

    #endregion

    #region 商店相关

    SHOP_CHG,//单个商店变动
    SHOP_ITEM_CHG,//单个商品变动
    SHOP_AUTO_REFRESH,//商店定时刷新
    ON_SHOP_RED_TIP_CHG,//商店红点变更

    #endregion

    #region 成就相关
    ON_ACHIEVE_STEP_REWARD_CHG,//成就阶段奖励变动
    ON_ACHIEVE_INFO_CHG,//成就信息变动
    ON_ACHIEVE_POINT_CHG,//成就点变动

    #endregion

    #region 聚会相关
    
    PARTY_CHG,//聚会变化，无聚会->有聚会，有聚会->无聚会
    PARTY_INFO_CHG,//聚会具体信息变化，
    PARTY_END,//聚会结束，会传出结束的消息结构体

    #endregion

    #region 藏宝图相关

    ON_TREASURE_MAP_REMOVE,//藏宝图移除
    ON_TREASURE_MAP_ADD,//藏宝图添加

    #endregion

    #region NPC_PVE挑战


    #endregion

    #region 对话消息
    DIALOG_END,//对话结束时触发的消息
    SIMULATE_CLICK_DIALOG_OPTION,//模拟点击对话选项
    #endregion
    #region 签到变动
    DAILY_CHECK_CHG,//单次签到变动
    DAILY_CHECK_TOTAL_CHG,//签到累计奖励变动
    #endregion

    SMALL_MAP_FITTER_CHG, //mini地图筛选变化
    MINI_MAP_FITTER_CHG, //mini地图筛选变化

    ON_START_GAME_WND_SHOW,//start game窗口show
    ON_US_ENTER_DONE,//进入US完成

    ON_WISH_QUEST_INFO_CHG,//心愿任务信息变更
    ON_WISH_QUEST_REFRESH,//心愿任务刷新

    ON_WISH_QUEST_FINISH,//完成心愿任务消息

    ON_ACCEPT_NEW_QUEST_SHOW,//接受新任务任务栏需要展示
    
    ON_PLAYER_ENTER_PRIVATE_ITEM_INTERACTIVE_RANGE,//玩家进入某个个人物件交互范围
    
    CLOSE_CUSTOM_OPEN_SUB_WND,//关闭自定义展开窗口
    STICK_PRESS_VALUE, // 摇杆值变化

    ON_SCREEN_SFX_SETTING_CHG,//屏幕点击特效开关变化

    ON_CUSTOM_BATTLE_LAYOUT_UP_PET,//自定义效果设置宠物上阵
    ON_CUSTOM_BATTLE_LAYOUT_DOWN_PET,//自定义效果设置宠物下阵
    START_CURRENT_BATTLE,//进入当前关卡或PVE战斗
    ON_SELECT_USE_BATTLE_SKILL,//模拟选择释放战斗技能
    ON_CLICK_REPAIR_BTN,//模拟点击修复按钮
    ON_OCCUPY_DIGGINGS_UP_PET,//自定义效果派遣宠物驻扎到矿区
    ON_OCCUPY_DIGGINGS_PET_CONFIRM,//确定派遣宠物驻扎矿区

    CREATE_PLAYER_SELECT,//创角选择角色，1是男；2是女 ENPGenderType

    #region 子嗣
    CHILD_EDUCATION_PREDICT_FAIL,//子嗣上课客户端预测数据错误
    #endregion
    #region 成年子嗣
    ADULT_UNMARRY_CHG_ADD,//新增未婚成年子嗣
    ADULT_UNMARRY_CHG_DEL,//删除未婚成年子嗣

    ADULT_MARRIED_CHG,//已婚成年子嗣变动
    ADULT_APPLY_CHG,//子嗣申请变动
    ADULT_REFUSE_NUM_CHG,//子嗣被拒绝数量变动
    ADULT_STATUS_CHG,//子嗣状态变动
    #endregion

    #region 情人相关

    ON_CONSORT_ADD,//情人新增
    ON_CONSORT_INTIMACY_CHG,//亲密度变化
    ON_CONSORT_CHARM_CHG,//魅力值变化
    ON_CONSORT_CHARM_POINT_CHG,//加护点数变化
    ON_CONSORT_SKILL_POINT_CHG,
    ON_CONSORT_CUR_SKIN_INFO_CHG,//当前穿戴皮肤变化
    ON_CONSORT_SKIN_INFO_ADD,//皮肤信息新增--解锁
    ON_CONSORT_SKIN_INFO_CHG,//皮肤信息变化
    ON_CONSORT_SKILL_INFO_ADD,//技能信息新增--解锁
    ON_CONSORT_SKILL_INFO_CHG,//技能信息变化
    ON_CONSORT_CHILD_COUNT_CHG,//子嗣数量变化
    ON_CONSORT_CHILD_ATTR_CHG,//子嗣总属性变化
    ON_CONSORT_SHOW_STAT_CHG, //展示状态变化
    ON_CONSORT_TRAVEL_COUNT_CHG, //出游次数变化
    
    ON_CONSORT_UN_GET_CHG, //未获得的妃子变化
    ON_CONSORT_UN_GET_DEL, //未获得的妃子移除
    
    ON_CONSORT_FETTER_LEVEL_CHG,//妃子羁绊等级变化
    ON_CONSORT_BUSINESS_SKILL_INFO_CHG,//妃子经营技能信息变化
    ON_CONSORT_BLESS_SKILL_INFO_CHG,//妃子加护技能信息变化
    ON_CONSORT_ADD_CG,//新增妃子CG
    ON_CONSORT_CG_CHG,//妃子CG信息变化
    ON_CONSORT_HALO_UNLOCK_CHG,//妃子星辉解锁状态变化
    ON_CONSORT_HALO_INFO_CHG,//妃子星辉数据变化(不包含解锁状态变化)
    ON_CONSORT_RELATION_HERO_ATTR_PROPERTY_CHG,//妃子关联英雄属性加成变化
    
    ON_CONSORT_INVITE_SHOW_DONE,//妃子邀约表现完成
    ON_ASSIGN_INVITE_CONSORT_CHG,//指定邀约妃子变化
    
    SHOW_CONSORT_BUSINESS_SKILL_UNLOCK,//展示妃子经营技能解锁弹窗
    SHOW_CONSORT_STORY_UNLOCK,//展示妃子故事解锁弹窗

    RET_CONSORT_SEND_GIFT,//收到赠送礼物回包
    
    SET_CONSORT_TD_SHOW_ANI,//设置妃子TdShow展示动画
    CONSORT_TD_SHOW_ANI_PLAY_COMPLETE,//妃子TdShow展示动画播放完成
    
    ON_CONSORT_ENTRANCE_CONSORT_CHG,//当妃子入口展示的妃子变化时
    
    CONSORT_MAIN_SCROLL_MOVE_TO_CONSORT,//模拟妃子主界面滚动到指定妃子
    SIMULATE_CLICK_CONSORT,//模拟点击妃子item
    
    #endregion

    #region 情人互动相关

    ON_CONSORT_CHAT_PRESET_CHAT_CHG,//妃子预设聊天变化
    ON_CONSORT_CHAT_MOMENTS_CHG,//妃子朋友圈变化
    ON_CONSORT_CHAT_ADD_FRIEND, //妃子添加好友变化
    

    #endregion

    #region 伙伴相关

    ON_HERO_LEVEL_CHG,//伙伴等级变化
    ON_HERO_TALENT_SKILL_CHG,//伙伴资质技能变更
    ON_HERO_BUSINESS_SKILL_CHG,//伙伴经营技能变更
    ON_HERO_GET_NEW_BUSINESS_SKILL,//伙伴新增经营技能
    ON_HERO_GAIN,//获得伙伴
    ON_HERO_SKIN_CHG,//伙伴皮肤变化推送
    ON_HERO_STEP_CHG,//伙伴阶段变化
    ON_HERO_CUR_SKIN_ID_CHG,//伙伴当前皮肤变化
    ON_HERO_HALO_CHG,//伙伴光环变化
    ON_HERO_PLACE_BUILDING_CHG,//伙伴驻扎建筑变化
    ON_HERO_STAR_CHG,//伙伴觉醒星级变化
    ON_HERO_SUIT_CHG,//伙伴套系变更
    HERO_INFO_GUIDE_TAB,//伙伴信息界面引导点击页签
    ON_HERO_POWER_CHG,//伙伴实力变更
    ON_HERO_ADD_TALENT_SKILL,//伙伴资质技能新增
    ON_ALL_HERO_TOTAL_POWER_CHG,//所有伙伴总实力变更

    #endregion

    LEVY_FOOD_CHG,//征收粮食变化
    LEVY_SILVER_CHG,//征收银币变化
    LEVY_SOLIDER_CHG,//征收士兵变化

    #region 大学
    COLLEGE_POS_CHG,//大学位置变动
    #endregion

    #region 活动通用

    ON_COMMON_ACTIVITY_ADD, //新增活动
    ON_COMMON_ACTIVITY_UPDATE, //活动更新
    ON_COMMON_ACTIVITY_START, //当活动开启
    ON_COMMON_ACTIVITY_SETTLING, //当活动结算
    ON_COMMON_ACTIVITY_END, //当活动结束
    ON_COMMON_ACTIVITY_CLOSE, //当活动关闭
    ON_COMMON_ACTIVITY_STATE_CHG, //活动状态变更

    #endregion

    #region 活动阶段奖励

    ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG,//活动阶段奖励变化
    ON_COMMON_ACTIVITY_STEP_REWARD_GET,//活动阶段领取奖励
    ON_COMMON_ACTIVITY_STEP_REWARD_ONE_KEY_GET,//活动阶段一键领取奖励成功

    #endregion

    #region 热更配表

    ON_PATCH_START,//开始进行热更配表(开始下载)
    ON_PATCH_LOADDONE, //热更配表加载完成(补丁完成)

    #endregion


    #region 宴会

    ON_DINNER_CHG,//宴会信息变化
    ON_MY_DINNER_ADD,//我的宴会有人参加推送
    
    ON_REQ_DINNER_IS_PAUSE,//宴会强制请求信息是否暂停
    
    ON_CUR_LOOK_DINNER_CHG,//当前观看的宴会信息变化
    
    #endregion

    #region 爬塔
    ON_TOWER_CHG,//爬塔信息变化
    ON_TOWER_ACTIVE_POS_CHG,//爬塔激活位置信息变化
    #endregion

    #region 杰出者大厅

    ON_GRAVE_NEW_CHANGE,//杰出者大厅新晋杰出者奖励变化

    #endregion

    ON_ANECDOTE_CHG,//政务数据变更
    ON_ANECDOTE_ONE_KEY_REWARD_SHOW,//一键完成政务获取奖励展示
    RFRESH_ANECDOTE_VIEW,

    #region 游历

    ON_SKIP_TRAVEL, //跳过游历过程
    SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_CHANGE,//模拟点击游历交换事件交换
    SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_NOT_CHANGE,//模拟点击游历交换事件不交换
    TRAVEL_FOCUS_TO_POS,//游历聚焦到某个位置
    TRAVEL_SET_PARKING_POS,//游历设置停靠位置
    TRAVEL_POS_UNLOCK,//游历位置解锁

    #endregion
    
    ON_LAZY_CD_CHG,//lazy_cd变化
    CLOTHES_BOTTOM_CLOSE,// 衣橱背景选择窗的关闭
    CLOTHES_BOTTOM_SHOW,// 衣橱背景选择窗的打开
    CLOTHES_SHARE_CLOSE,// 衣橱分享截图界面的关闭
    CLOTHES_SHARE_SHOW,// 衣橱分享截图界面的打开
    CLOTHES_SHARE_BOTTOM_CLOSE,// 衣橱分享的时候背景选择窗的关闭
    CLOTHES_SHARE_BOTTOM_SHOW,// 衣橱分享的时候背景选择窗的打开
    CLOTHES_DYE_SHOW,// 衣橱染色界面的打开
    CLOTHES_DYE_CLOSE,// 衣橱染色界面的关闭
    CLOTHES_MENU_GRID_HEIGHT_CHG,// 修改衣橱菜单列表的高度
    CLOTHES_MENU_GRID_HEIGHT_RESET,// 重置衣橱菜单列表的高度
    CLOTHES_SUB_EDIT_FUNCTION_REFRESH,// 衣橱编辑功能刷新
    CLOTHES_HIDE_SHOW_STATE_CHG,// 衣橱隐藏显示状态变化

    HERO_RECOMMEND_REFRESH_ENTRANCE,//骑士推荐刷新入口
    MAIN_CITY_WND_SHOW,//打开主城界面显示
    #region 引导

    SIMULATE_CLICK_CLOTHES_UNIT,//模拟点击选择衣服部件单位
    SIMULATE_CLICK_CLOTHES_DIRECTORY,//模拟点击选择文件夹
    SIMULATE_CLICK_CLOTHES_BACK_ROOT,//模拟点击返回上级目录
    SIMULATE_CLICK_CLOTHES_SAVE,//模拟点击衣柜保存按钮
    SIMULATE_CLICK_CLOTHES_EDITOR_WND_BTN,//模拟点击衣柜编辑页面按钮
    SIMULATE_CLICK_CHAPTER_MOVE,//模拟点击关卡摇骰子
    SIMULATE_OPEN_HERO_INFO,//模拟打开英雄信息界面
    SIMULATE_CLICK_HERO_LEVEL_UPGRADE,//模拟点击英雄升级
    SIMULATE_RET_MAIN_QUEST_REWARD,//模拟点击主线任务奖励
    SIMULATE_PAGE_RET_MAIN_QUEST_REWARD,//模拟页面内点击主线任务领奖
    SIMULATE_CLICK_CONSORT_RANDOM_GREET,//模拟点击妃子随机宠幸
    SIMULATE_SELECT_HERO_RECOMMEND_INDEX,//模拟选择英雄推荐位置
    SIMULATE_CLICK_HERO_RECOMMEND_GET,//模拟点击英雄推荐获取大臣
    SIMULATE_CLICK_DAILY_QUEST_REWARD,//模拟点击日常任务领奖
    SIMULATE_CLICK_TRAVEL,//模拟点击游历
    SIMULATE_CLICK_SHOW_CHAPTER_EVENT_LIST,//模拟点击打开关卡事件列表
    SIMULATE_CLICK_LEVY_SILVER,//模拟点击征收银币
    SIMULATE_CLICK_ACHIEVE_REWARD,//模拟点击成就领奖
    SIMULATE_CLICK_COLLEGE_POS_ITEM,//模拟点击大学位置
    SIMULATE_CLICK_HOLD_DINNER,//模拟点击举办宴会
    SIMULATE_CLICK_LEVY_SOLDIER,//模拟点击征收士兵
    SIMULATE_CLICK_OPEN_CONSORT_GIVE_GIFT,//模拟点击打开妃子送礼弹窗
    SIMULATE_CLICK_OPEN_MARKET_SHOP,//模拟打开店铺弹窗，market_shop表id
    SIMULATE_CLICK_MARKET_GET_REWARD,//模拟店铺领取奖励
    SIMULATE_CLICK_TRAIN_CHILD,//模拟点击培养子嗣
    SIMULATE_CLICK_OPEN_CHILD_SET_NAME,//模拟点击打开子嗣取名弹窗
    SIMULATE_CLICK_OPEN_CHILD_TRAIN,//模拟点击打开子嗣培养弹窗
    SIMULATE_CLICK_SUIT_MENU,//模拟点击套装菜单
    SIMULATE_CLICK_SUIT_ITEM,//模拟点击套装item试穿，clothes_suit表id
    SIMULATE_CLICK_GIVE_CONSORT_GIFT,//模拟点击赠送知己礼物，默认选择当前选中的
    SIMULATE_CLICK_OPEN_FIXED_RANK,//模拟点击打开常驻排行榜，rank_fixed表id
    SIMULATE_CLICK_FIXED_RANK_LIKE,//模拟点击常驻排行榜点赞，rank_fixed表id
    SIMULATE_CLICK_SHARE_CLOTHES,//模拟点击分享装扮按钮
    SIMULATE_CLICK_SHARE_CLOTHES_CONFIRM,//模拟点击确认分享装扮按钮
    SIMULATE_CLICK_SHOW_CUSTOM_SUIT_LIST,//模拟点击打开自定义套装下拉列表
    SIMULATE_CLICK_CUSTOM_SUIT_LIST_ITEM,//模拟点击自定义套装选项，选项从上到下的下标从0开始
    SIMULATE_CLICK_ONE_DRAW_CARDS,//模拟点击单次抽卡
    SIMULATE_CLICK_TEN_DRAW_CARDS,//模拟点击十连抽卡
    SIMULATE_CLICK_OPEN_HERO_TRAIN,//模拟点击打开培养骑士窗口
    SIMULATE_CLICK_ADULT_START_MARRY,//模拟点击打开发起联姻窗口
    SIMULATE_CLICK_HERO_TALENT_TAB,//模拟点击打开骑士资质页签
    SIMULATE_CLICK_HERO_TALENT_ITEM,//模拟点击骑士资质item，item下标从0开始
    SIMULATE_CLICK_AVATAR_SCENE_NULL_RETURN,//模拟点击avatar场景空白处返回页签
    SIMULATE_CLICK_OPEN_MARKET_UPGRADE,//模拟点击打开码头升级界面
    SIMULATE_CLICK_MARKET_ITEM_UPGRADE,//模拟点击码头item升级，item下标从0开始
    SIMULATE_CLICK_OPEN_CONSORT_TRAVEL,//模拟点击打开妃子出游弹窗
    SIMULATE_CLICK_CONSORT_TRAVEL_USE_ITEM,//模拟点击情人使用道具出游
    SIMULATE_CLICK_CONSORT_TRAVEL_ITEM_BY_INDEX, //模拟点击情人出游列表 item，item下标从0开始
    SIMULATE_CLICK_CHAPTER_START_FIGHT,//模拟点击关卡boss战出战
    SIMULATE_OPEN_HERO_INFO_BY_INDEX, //模拟点击打开骑士信息界面，item下标从0开始
    SIMULATE_CLICK_OPEN_CHAPTER_LOSS_DEGREE,//模拟点击打开关卡面包消耗详情
    SIMULATE_CLICK_CLOTHES_ACTION_INDEX, //47 ==== 模拟点击选择第x个动作,下标从0开始
    SIMULATE_CLICK_CLOTHES_ACTION_BACK, //48 ==== 模拟点击动作选择窗口返回
    SIMULATE_CLICK_CLOTHES_POSE_INDEX, //49 ==== 模拟点击选择第x个站姿,下标从0开始
    SIMULATE_CLICK_CLOTHES_POSE_BACK, //50 ==== 模拟点击站姿选择窗口返回
    SIMULATE_CLICK_CLOTHES_BG_INDEX, //51 ==== 模拟点击选择第x个背景,下标从0开始
    SIMULATE_CLICK_CLOTHES_BG_BACK, //52 ==== 模拟点击背景选择窗口返回
    SIMULATE_CLICK_CLOTHES_MAIN_BACK, //53 ==== 模拟点击衣柜窗口返回
    SIMULATE_OPEN_CONSORT_INFO_BY_INDEX, //模拟点击打开情人信息界面，item下标从0开始
    SIMULATE_CLICK_CONSORT_DETAIL_TAB,//模拟点击妃子详情页面页签
    SIMULATE_SUMMON_ONE_DRAW,//模拟召唤单抽
    SIMULATE_SUMMON_TEN_DRAW,//模拟召唤十抽
    SIMULATE_SUMMON_FREE_DRAW,//模拟召唤免费抽
    SIMULATE_CLICK_FARMING_BUILDING_COLLECTION,//模拟点击农场建筑收获
    SIMULATE_CLICK_BUILDING_BUILD,//模拟点击建筑建造
    SIMULATE_CLICK_PLAYER_LVL_UP,//模拟点击玩家升级
    SIMULATE_CLICK_BUILDING_BUILD_WND_BTN,//模拟点击建筑建造界面按钮
    SIMULATE_CLICK_BUSINESS_BUILDING_TEN_TIMES_HIRE_TOGGLE,//模拟点击经营建筑十次雇佣开关
    SIMULATE_CLICK_BUSINESS_BUILDING_HIRE_BTN,//模拟点击经营建筑雇佣按钮
    SIMULATE_CLICK_FARMING_BUILDING_UPGRADE_BTN,//模拟点击农场建筑升级按钮
    SIMULATE_CLICK_ANECDOTE_POS,//模拟点击经营事件
    SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SLOT,//模拟点击经营建筑英雄槽
    SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_BTN,//模拟点击经营建筑升级按钮
    SIMULATE_CLICK_BUSINESS_BUILDING_SET_HERO_BTN,//模拟点击经营建筑设置英雄按钮
    SIMULATE_CLICK_BUSINESS_BUILDING_RND_BTN,//模拟点击经营建筑研发按钮
    SIMULATE_CLICK_BUSINESS_BUILDING_HERO_CARD,//模拟点击经营建筑英雄卡牌
    SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SELECT_CONFIRM,//模拟点击经营建筑英雄选择确认
    MOVE_FOCUS_TO_FARMING_BUILDING_UPGRADE_BTN_AND_SHOW_GUIDE_HAND,//移动焦点到农场建筑升级按钮并展示引导手指
    SIMULATE_CLICK_CHAPTER_FORWARD,//模拟点击关卡前进
    SIMULATE_CLICK_TREASURE_HUNT_MAIN_GAIN_ENERGY,  //模拟点击领取太空打捞体力
    SIMULATE_CLICK_TREASURE_HUNT_MAIN_PLAY,  //模拟点击前往太空打捞界面
    SIMULATE_CLICK_TREASURE_HUNT_GAME_PLAY,  //模拟点击开始探索
    SIMULATE_CLICK_MIDDAY_DUNGEON_ENTER,  //模拟点击太空维护按钮打开维护界面
    SIMULATE_CLICK_MIDDAY_DUNGEON_BATTLE_ATTACK,  //模拟点击太空站维护派遣按钮
    SIMULATE_CLICK_EVENING_DUNGEON_ENTER,  //模拟点击晚间副本(抵御陨石)按钮
    SIMULATE_CLICK_EVENING_DUNGEON_GAME_FIGHT,  //模拟点击晚间副本派遣按钮
    SIMULATE_CLICK_TREASURE_HUNT_GAME_CHANGE_AREA,  //模拟点击太空寻宝更换地点按钮
    SIMULATE_SELECT_TREASURE_HUNT_SELECT_AREA,// 模拟点击太空寻宝"区域选择"
    SIMULATE_CLICK_TREASURE_HUNT_SELECT_AREA_GO,// 模拟点击太空寻宝"前往"新区域
    SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_SKIP_DIALOG_CONFIRM,// 模拟点击关卡快速前进是否跳过对话"确认"按钮
    SIMULATE_CLICK_BAG_POP_ITEM_USE_SIMPLE_USE_BTN,//模拟点击GGUIWndBagPopItemUseSimple使用按钮
    SIMULATE_CLICK_BAG_ITEM_USE_WND_USE_BTN,//模拟点击背包使用物品弹窗(_AGGUIWndBagItemUse)使用按钮
    SIMULATE_CLICK_BAG_ITEM_GRID_ITEM,//模拟点击背包物品，参数格式: EGGUIMonoBagItemGridTargetItemType:附加参数
    SIMULATE_CLICK_MAIN_QUEST_GO_TO,//模拟点击主线任务前往按钮

    #endregion

    ON_MARKET_ENTRANCE_GET_REWARD_SUC,//商店入口领取奖励成功
    MARKET_SHOP_CHG,//集市店铺变动
    MARKET_SHOP_AKEY_SUC,//一键领取成功
    
    ON_NODE_CHG,//节点变化消息
    
    ON_AVATAR_SCENE_CLICK_NULL,//avatar场景触发点击空白处

    ON_ADD_MARQUEE,//新增跑马灯
    ON_DEL_MARQUEE,//删除跑马灯

    #region 聊天

    ON_CHAT_CHANNEL_SELECT_TOG_ON,//批量选择选中
    ON_CHAT_CHANNEL_SELECT_TOG_OFF,//批量选择取消选中
    
    ON_CHAT_CHANNEL_SELECT_ALL,//批量选择全选
    ON_CHAT_CHANNEL_DIS_SELECT_ALL,//批量选择全取消
    
    ON_CHAT_UP_TO_TOP_CHG,//置顶变化

    #endregion

    ON_COMMON_DIALOG_START,//通用对话开始
    FRIENDS_GROUP_CHG, //好友分组变更
    FRIENDS_SHIELD_CHG, //屏蔽好友变更
    
    MSG_NEXT_STEP_TRIGGER,//下一步触发消息

    ON_FUNC_UNLOCK_GET_REWARD,//领取功能解锁奖励

    TRIGGER_SHOW_LEVEL_UP,//尝试触发升级弹窗

    ON_CLICK_SWITCH_CLOTHES_UNIT_SELECTION,//点击更换时装部件

    ON_CUSTOM_CLOTHES_EDIT_CHG,//自定义套装编辑变更

    SET_SHOW_ENTRY_GUIDE_HAND,//展示入口手指引导
    SHOW_GUIDE_HAND,//展示手指引导
    SET_SHOW_ANECDOTE_GUIDE_HAND,//展示政务入口手指引导
    SET_SHOW_HERO_RECOMMEND_GUIDE_HAND,//展示骑士推荐入口手指引导

    ON_START_TUTORIAL,//开始引导
    ON_REFRESH_ENTRY_STATE,//刷新入口状态
    ON_SHARE_CLOTHES_SUC,//分享时装成功
    ON_DINNER_LIST_WND_SHOW,//打开查看舞会列表弹窗

    TRIGGER_CAT_BUBBLE_SHOW_ANI,//触发猫咪气泡显示动画
    TRIGGER_CAT_BUBBLE_HIDE_ANI,//触发猫咪气泡隐藏动画

    ON_MAKE_FACE_WND_SHOW,//捏脸窗口打开消息

    ON_ADULT_APPLY_SUC,//请求联姻成功
    ON_ADULT_RELEASE_SUC,//学徒远行成功
    SET_HERO_RECOMMEND_BUBBLE_ANI_SAMPLE,//设置骑士推荐气泡动画到某帧
    
    SHOW_PLAYER_RES_VALUE_BIG_TEXT_SFX,//显示玩家资源值大文本特效
    ON_FIXED_CD_COUNT_CHG,//fixedCD数量变化
    ON_CHAT_SENDED, //聊天消息发送的时候
    ON_CHAT_SENDED_PRIVATE_MSG_DONE, //聊天消息发送私聊消息完成
    ON_CHAT_RECEIVE_PRIVATE_MSG, //聊天消息接收私聊消息
    ON_DINNER_LIST_DISCARD, //宴会列表关闭的时候

    MAIN_ROOM_WND_SHOW,//打开卧室界面显示
    DOWNLOAD_CHECK_AVATAR_UNIT_DOWNLOADED,//带有下载检查的 Avatar Unit 下载完成
    ON_GAME_ANNOUNCEMENT_IMAGE_LOAD_SUC,//运营公告图片下载完成

    ON_QUESTIONNAIRE_START,//问卷调查开始
    ON_QUESTIONNAIRE_END,//问卷调查结束
    ON_QUESTIONNAIRE_REWARD_ADD,//可领取奖励推送
    ON_QUESTIONNAIRE_REWARD_GET,//已领取奖励推送
    
    ON_PLAYABLE_AUDIO_CHANGE,//GGUIPlayableAudioPlay脚本中的音频播放变化
    ON_PLAYABLE_SUBTITLE_CHANGE, //GGUIPlayableSubtitle脚本中的字幕变化
    
    COMMON_INPUT_SCENE_START_DRAG,//常规输入开始拖拽

    ON_CLOTHES_MAIN_WND_SHOW,//进入换装界面
    ON_PLAYER_RENAME_WND_SHOW,//进入玩家取名界面
    
    ON_RET_COLLEGE_STUDY_START, // 大学开始学习
    ON_RET_COLLEGE_AKEY_STUDY_START,// 大学一键学习开始

    ON_GUILD_JOIN_REQUEST_ADD,//新增入盟请求
    ON_GUILD_JOIN_REQUEST_REMOVE,//移除入盟请求
    ON_GUILD_SHOW_INFO_CHG,//联盟展示信息变更
    ON_JOIN_GUILD,//加入联盟
    ON_LEAVE_GUILD,//退出联盟
    ON_GUILD_MEMBER_BASE_INFO_CHG,//联盟成员基础信息变更
    ON_GUILD_CONSTRUCT_INFO_CHG,//联盟建设信息变更
    ON_GUILD_CONSTRUCT_PROGRESS_REWARD_POINT_CHG,//联盟建设进度奖励积分变更
    ON_GUILD_WEALTH_CHG,//联盟财富变更
    ON_GUILD_CONSTRUCT_DONE,//联盟建设回包完成
    ON_GUILD_EVENT_ADD,//联盟事件新增
    ON_GUILD_EVENT_CHG,//联盟事件变更
    ON_GUILD_EVENT_REMOVE,//联盟事件移除
    ON_GUILD_MEMBER_ADD,//联盟成员新增
    ON_GUILD_MEMBER_REMOVE,//联盟成员移除
    ON_GUILD_CAN_RECRUIT_TIME_CHG,//联盟可招募时间变更
    
    ON_GUILD_ACTIVE_BOX_EXPIRED,//联盟活跃宝箱过期
    ON_GUILD_GREAT_REWARD_EXPIRED,//联盟大礼过期
    ON_GUILD_ENTRUST_CHG_NEW,//联盟委托变化为新数据
    ON_GUILD_ENTRUST_INFO_CHG,//联盟委托数据变更
    ON_GUILD_MEMBER_DAILY_DATA_CHG,//联盟成员每日数据变更
    ON_GUILD_MEMBER_DAILY_DRAW_CONSTRUCT_REWARD_LIST_CHG,//联盟成员每日领取建设奖励列表变更
    ON_GUILD_MEMBER_WEEK_DATA_CHG,//联盟成员每周数据变更
    ON_GUILD_MEMBER_HAD_DRAW_ACTIVE_BOX_NUM_CHG,//联盟成员已领取活跃宝箱数量变更
    ON_GUILD_MEMBER_GAIN_ACTIVE_STAMP_NUM_CHG,//联盟成员获得活跃印章数量变更
    ON_GUILD_MEMBER_HAD_DRAW_GREAT_REWARD_NUM_CHG,//联盟成员已领取大礼数量变更
    ON_GUILD_WEEK_DATA_CHG,//联盟每周数据变更
    ON_GUILD_GREAT_REWARD_POINT_CHG,//联盟大礼积分变更
    ON_GUILD_DISPATCH_INFO_CHG,//联盟派遣信息变更

    ON_EQUIP_ADD,//藏品新增
    ON_EQUIP_BASE_CHG,//藏品基础信息变更
    ON_EQUIP_Skill_CHG,//藏品技能信息变更
    ON_EQUIP_REMOVE,//藏品移除
    ON_EQUIP_RELATE_HERO_POWER_RECALCULATE,//藏品关联伙伴需重新计算
    ON_EQUIP_LEVEL_CHG,//藏品等级变更
    
    ON_GACHA_POOL_INFO_CHG,//抽卡卡池信息变更
    
    ON_RECRUIT_EXCHANGE_SUCC,//招募兑换成功

    ON_ARENA_BATTLE_INFO_CHG,//竞技场战斗信息变更
    ON_ARENA_BASE_INFO_CHG,//竞技场基础信息变更
    ON_STATION_CHG,//贸易站信息变更
    ON_GET_ARENA_CELEBRITY_LIST,//竞技场名人榜信息获取

    ON_CROSS_DAY,//发生跨天
    ON_BUILDING_LEVEL_CHG,//建筑等级变化
    ON_BUILDING_ADD,//新增建筑
    ON_BUILDING_BUILT,//新增其他建筑
    ON_BUILDING_EMPLOYEE_NUM_CHG,//建筑员工数量变化
    ON_BUILDING_UNLOCK_PRODUCT,//建筑解锁产品
    ON_CHAPTER_POS_CHG,//玩家关卡位置变化
    ON_CHAPTER_POS_CHG_CHEAT,//作弊修改玩家关卡位置变化
    BUILD_BUILD_STATE_REFRESH,//建筑状态刷新

    #region 阶段目标
    ON_STAGE_GOAL_CHG,//阶段变动
    ON_STAGE_GOAL_TASK_CHG,//目标变动
    ON_STAGE_GOAL_BIG_STEP_REWARD_DRAW,//已经领取阶段目标大步奖励
    ON_STAGE_GOAL_FIRST_REACH_ADD,//新增可领取大阶段首达奖励
    ON_STAGE_GOAL_FIRST_REACH_DRAW,//已经领取大阶段首达奖励
    #endregion

    ON_EARNINGS_CHG,//赚速变化

    #region 午间副本
    ON_MIDDAY_DUNGEON_STATE_CHG,//午间副本状态刷新
    ON_MIDDAY_DUNGEON_STATE_END,//午间副本状态变为结束
    #endregion

    #region 晚间副本

    ON_EVENING_DUNGEON_PREVIEW,//晚间副本进入预览状态
    ON_EVENING_DUNGEON_START,//晚间副本进入开始状态
    ON_EVENING_DUNGEON_END,//晚间副本进入结束状态
    ON_EVENING_DUNGEON_CLOSE,//晚间副本进入关闭状态
    ON_EVENING_DUNGEON_SEC_TICK,//晚间副本每秒tick
    ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG,//晚间副本活动状态变化
    ON_EVENING_DUNGEON_USED_HERO_CHG,//晚间副本使用过的英雄变化

    #endregion

    #region 赚速目标
    ON_EARNINGS_GOAL_STATE_CHG,//赚速目标活动状态变化
    ON_EARNINGS_GOAL_SELF_REACH_CHG,//赚速目标达成变化
    ON_EARNINGS_GOAL_HONOR_REACH_CHG,//赚速目标荣耀达成变化
    ON_EARNINGS_GOAL_SELF_DRAW_CHG,//赚速目标领取变化
    ON_EARNINGS_GOAL_HONOR_DRAW_CHG,//赚速目标荣耀领取变化
    

    #endregion

    #region 联盟PVE

    ON_GUILD_DUNGEON_SET_CHG,//联盟PVE信息变化
    ON_GUILD_DUNGEON_RESET,//联盟PVE信息重置
    ON_GUILD_DUNGEON_INSTANCE_CHG,//联盟PVE实例信息变化
    ON_GUILD_DUNGEON_MONSTER_CHG,//联盟PVE怪物信息变化
    ON_GUILD_DUNGEON_REWARD_CHG,//联盟PVE奖励信息变化
    ON_GUILD_DUNGEON_TAG_MONSTER_CHG,//联盟PVE标签怪物信息变化

    #endregion
    
    #region 联盟火星求助

    ON_GUILD_MARS_HELP_MY_CHG,//联盟火星求助-我的求助变更
    ON_GUILD_MARS_HELP_CAN_DEAL_CHG,//联盟火星求助-可处理变更

    #endregion
    
    #region 联盟宝箱

    ON_GUILD_BOX_COUNT_CHG,//联盟宝箱数量变更
    ON_GUILD_BOX_REWARD_SHOW,//联盟宝箱奖励物品列表推送
    ON_GUILD_BOX_DETAIL_ADD,//联盟宝箱新增详情推送
    ON_GUILD_BOX_ACTIVE_POINT_CHG,//联盟宝箱活跃点变化

    #endregion
    
    #region 急速兑换

    ON_RUSH_EXCHANGE_CHG,//急速兑换信息变更

    #endregion
    
    
    ON_COMMON_ITEM_COUNT_CHG,//common_item数量变化
    ON_COMMON_ITEM_COUNT_CHG_TICK_TOTAL,//common_item数量变化在间隔时间内累计变化
    SWITCH_HERO_INFO_TAB,//切换伙伴信息页签
    SIMULATE_CLICK_HERO_BUSINESS_SKILL_UPGRADE,//模拟点击伙伴经营技能升级
    SIMULATE_CLICK_QUIT_CONSORT_GIVE_GIFT,//模拟点击退出家人送礼页面
    SIMULATE_CLICK_CONFIRM_CHILD_SET_NAME,//模拟点击确认子嗣取名
    SIMULATE_CLICK_EQUIP_OPEN_SKILL_PAGE,//模拟点击打开藏品技能重塑页面
    SIMULATE_CLICK_EQUIP_UPGRADE,//模拟点击升级藏品
    SIMULATE_CLICK_EQUIP_SKILL_REBUILD,//模拟点击藏品技能重塑
    SIMULATE_CLICK_HERO_EQUIP_ENTRY_BTN,//模拟点击伙伴藏品入口按钮
    SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_CONFIRM_BTN,//模拟点击经营建筑升级确认按钮
    SIMULATE_CLICK_HERO_WEAR_EQUIP_BY_INDEX,//模拟点击伙伴选择佩戴藏品，item下标从0开始
    SIMULATE_CLICK_HERO_EQUIP_INFO_STRENGTHEN_BTN,//模拟点击伙伴佩戴藏品信息界面强化按钮
    SIMULATE_CLICK_CONSORT_BLESS_SKILL_UPGRADE_BY_INDEX,//模拟点击家人加护技能升级，item下标从0开始
    SIMULATE_CLICK_ARENA_RANDOM_ATTACK,//模拟点击打开谈判界面
    SIMULATE_CLICK_ARENA_OPEN_SELECT_HERO,//模拟点击打开选择伙伴界面
    SIMULATE_SELECT_ARENA_HERO_BY_INDEX,//模拟选择竞技场伙伴，item下标从0开始
    SIMULATE_CLICK_ARENA_OPEN_SELECT_INITIAL_BUFF,//模拟点击打开竞技场初始增益界面
    SIMULATE_CLICK_ARENA_SELECT_NULL_INITIAL_BUFF,//模拟点击选择竞技场空的初始增益
    SIMULATE_ARENA_SELECT_OPPONENT_HERO_BY_INDEX,//模拟选择竞技场想要打的对手伙伴，item下标从0开始
    SIMULATE_CLICK_HOLD_DINNER_INDEX,//模拟点击选择举办宴会，item下标从0开始
    SIMULATE_CLICK_TOWER_CHALLENGE_INDEX,//爬塔模拟点击挑战当前列表第几个boss的效果(序列号0为当前界面最底下一层)
    SIMULATE_CLICK_STAGE_GOAL_MAIN_TASK_GAIN_REWARD,//模拟点击阶段目标主线任务领取奖励
    SIMULATE_CLICK_ARENA_START_FIGHT,//模拟点击竞技场开始战斗
    SIMULATE_CLICK_EQUIP_CONFIRM_REBUILD,//模拟点击藏品技能重塑确认
    SIMULATE_CLICK_EQUIP_ITEM_BY_INDEX,//模拟点击藏品列表打开详情界面, item下标从0开始

    ON_PLAYER_SKIN_LEVEL_CHG,//玩家皮肤等级变化
    ON_PLAYER_SKIN_CHG,//玩家皮肤变化
    ON_PLAYER_SKIN_ADD,//玩家皮肤新增

    ON_ACTIVITY_SHOP_REFRESH,//活动商店刷新
    ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH,//活动钻石礼包刷新
    ON_ACTIVITY_SHOP_BUY_RECORD_CHG,//活动商店商品购买次数变更
    ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG,//活动钻石礼包购买次数变更
    ON_ACTIVITY_CURRENCY_CHG,//活动兑换券变更

    ON_CD_EVENT_CHG,//倒计时事件变更
    ON_CD_EVENT_REMOVE,//倒计时事件移除

    SIMULATE_CLICK_COUNTDOWN_EVENT_GET_REWARD,//模拟点击倒计时事件领取奖励
    ON_QUEST_ENTRY_SHOW,//主线任务入口显示
    ON_QUEST_CLICK_GOTO,//主线任务点击前往
    SIMULATE_CLICK_CONSORT_NORMAL_COMPREHEND,//模拟点击情人普通领悟按钮
    SIMULATE_CLICK_SCHOOL_GET_CHILD,//模拟点击学校招生按钮
    SIMULATE_CLICK_GET_STAGE_GOAL_BIG_STEP_REWARD_BY_ID,//模拟点击领取阶段目标大阶段奖励
    ON_SYSTEM_QUEST_INFO_CHG,//系统任务信息变更
    ON_SYSTEM_QUEST_COUNT_CHG,//系统任务计数变更
    SIMULATE_CLICK_BUILDING_PRODUCT_UNLOCK_BY_INDEX,//模拟点击解锁建筑产品
    SIMULATE_CLICK_INN_STATION_ENTER_BY_INDEX,//模拟点击旅店设施列表进入按钮
    
    ON_INN_DISH_UNLOCK, //旅店菜品解锁
    ON_INN_DISH_LEVEL_CHG, //旅店菜品等级变化
    ON_INN_POPULARITY_CHG, //旅店人气变化
    ON_INN_LEVEL_CHG, //旅店等级变化
    ON_INN_STATION_LEVEL_CHG, //旅店设施等级变化
    ON_INN_VIEW_MGR_INIT, //旅店表现管理器初始化完成
    ON_INN_VIEW_GUEST_SETTLED, //旅店客人接待完成
    ON_INN_GET_SETTLE_REWARD, //旅店获取结算奖励
    ON_INN_GET_CASH_REGISTER_REWARD_NODE_CLOSE,//当旅店结算奖励Node关闭时
    ON_INN_MEDAL_LEVEL_CHG, //旅店勋章等级变化
    
    ON_TREASURE_HUNT_STATION_CHG,//太空寻宝 - 空间站数据变化
    ON_TREASURE_HUNT_UNLOCK_NORMAL_ORE,//太空寻宝 - 解锁普通矿石消息
    ON_TREASURE_HUNT_UNLOCK_ADVANCED_ORE,//太空寻宝 - 解锁高级矿石消息
    ON_TREASURE_HUNT_UNLOCK_TREASURE,//太空寻宝 - 解锁奇物消息
    ON_TREASURE_HUNT_TREASURE_LEVEL_CHG,//太空寻宝 - 奇物等级变化
    ON_TREASURE_HUNT_UNLOCK_NORMAL_COMPOSITE_CATALOG,//太空寻宝 - 解锁普通组合图鉴消息
    ON_TREASURE_HUNT_UNLOCK_ADVANCED_COMPOSITE_CATALOG,//太空寻宝 - 解锁高级组合图鉴消息
    ON_TREASURE_HUNT_STATION_LVL_CHG,//太空寻宝 - 空间站等级变化
    ON_TREASURE_HUNT_TREASURE_OUTPUT_CHG,//太空寻宝 - 奇物产出变化
    ON_TREASURE_HUNT_LAB_REFRESH_SELECTED_TREASURE,//太空寻宝 - 实验室刷新选中奇物
    ON_TREASURE_HUNT_ORE_SKILL_CHG,//太空寻宝 - 矿石技能变化
    ON_TREASURE_HUNT_COMPOSITE_CATALOG_CHG,//太空寻宝 - 组合图鉴变化
    ON_TREASURE_HUNT_ORE_MAX_RECORD_CHG,// 太空寻宝 - 矿石质量最高记录变化
    ON_TREASURE_HUNT_ORE_HAD_DRAW_RECORD_REWARD_CHG,// 太空寻宝 - 矿石质量记录奖励领取变化
    
    #region 火星居民
    ON_MARS_PEOPLE_NUM_CHG,//火星居民 - 人口数量变化(各种不同状态之间人数变化也发这个)
    ON_MARS_TOTAL_PEOPLE_NUM_CHG,//火星居民 - 总人口数量变化
    ON_MARS_IDLE_PEOPLE_NUM_CHG,//火星居民 - 空闲人口数量变化
    ON_MARS_SICK_PEOPLE_NUM_CHG,//火星居民 - 空闲人口数量变化
    ON_MARS_INTELLIGENT_CHG,//火星居民 - 决策数据变化
    ON_MARS_SATISFACTION_DREGREE_CHG,//火星居民 - 满意度变化
    ON_MARS_LETTER_ADD,//火星居民 - 新增信件
    ON_MARS_LETTER_UPDATE,//火星居民 - 更新信件
    ON_MARS_LETTER_DEL,//火星居民 - 删除信件
    ON_MARS_HELP_ADD,//火星居民 - 新增求助
    ON_MARS_HELP_UPDATE,//火星居民 - 更新求助
    ON_MARS_HELP_DEL,//火星居民 - 删除求助
    ON_MARS_EVENT_TRIGGER,//火星居民 - 事件触发
    ON_MARS_PEOPLE_IMMIGRANT_ADD,//火星居民 - 移民数据增加
    ON_MARS_PEOPLE_IMMIGRANT_DEL,//火星居民 - 移民数据删除
    ON_MARS_PEOPLE_IMMIGRANT_COUNT_CHG,//火星居民 - 移民次数变化
    #endregion

    #region 火星基地
    TRIGGER_MARS_ENERGY_COLLECT,
    TRIGGER_MARS_BUILDING_FOCUS,
    TRIGGER_MARS_BUILDING_INTELLIGENT_CONTROL_EFFECT,//触发火星建筑智能控制效果
    ON_MARS_BUILDING_CHG,//火星基地 - 建筑数据变化
    ON_MARS_BUILDING_EQUIPMENT_CHG,//火星基地 - 建筑部件数据变化
    ON_MARS_BUILDING_DISPATCH_PEOPLE_CHG,//火星基地 - 建筑派遣居民变化
    ON_MARS_STORED_ENERGY_CHG,//火星基地 - 存储能量变化
    ON_MARS_COLLECT,//火星基地 - 采集行为
    #endregion

    #region 火星科技

    ON_MARS_TECHNOLOGY_CHG,//火星科技 - 科技数据变化
    ON_MARS_UPGRADING_TECHNOLOGY_CHG,//火星科技 - 正在升级的科技发生变化

    #endregion

    #region 火星探索

    ON_MARS_EXPLORE_DATA_CHG,//火星探索 - 探索数据变化
    ON_MARS_TEAM_SOLDIER_NUM_CHG, // 火星探索 - 探索队伍士兵数量变化

    #endregion

    SIMULATE_CLICK_INN_MAIN_STATION_BUTTON, //模拟点击旅店设施按钮
    SIMULATE_CLICK_INN_STATION_BUILD_BUTTON, //模拟点击旅店设施建造按钮
    SIMULATE_CLICK_INN_STATION_LEVEL_UP_BUTTON, //模拟点击旅店设施升级按钮
    SIMULATE_CLICK_INN_MAIN_MENU_BUTTON, //模拟点击旅店主界面菜单按钮
    SIMULATE_CLICK_INN_DISH_ITEM_BY_DISH_INDEX, //模拟点击旅店菜品item，使用菜品索引
    SIMULATE_CLICK_INN_DISH_UNLOCK_BUTTON, //模拟点击旅店菜品解锁按钮
    SIMULATE_CLICK_INN_DISH_LEVEL_UP_BUTTON, //模拟点击旅店菜品升级按钮
    SIMULATE_CLICK_INN_CASH_REGISTER, //模拟点击旅店收银台
    SIMULATE_CLICK_INN_CREATE_GUEST_BUTTON, //模拟点击旅店创建客人按钮
    SIMULATE_CLICK_INN_MEDAL_UPGRADE_BUTTON, //模拟点击旅店勋章升级按钮
    SIMULATE_CLICK_INN_MEDAL_DETAIL_BUTTON, //模拟点击旅店勋章详情按钮
    SIMULATE_CLICK_INN_SPECIAL_GUEST_SERVE_BUTTON, //模拟点击旅店特殊客人接待按钮
    SIMULATE_CLICK_SYSTEM_QUEST_GET_REWARD, //模拟点击系统任务领取奖励按钮
    SIMULATE_CLICK_SYSTEM_QUEST_DETAIL_BUTTON, //模拟点击系统任务详情按钮
    SIMULATE_CLICK_CONSORT_CG_ITEM_BY_INDEX, //模拟点击妃子CG item，item下标从0开始

    ON_GIFT_PACK_LIST_REFRESH,//礼包列表刷新
    ON_GIFT_PACK_CHG,//礼包变化
    ON_GIFT_PACK_ADD,//新增礼包
    ON_SERVER_GIFT_PACK_LEFT_BUY_COUNT_CHG,//服务器给的礼包剩余购买次数变化
    ON_GIFT_PACK_REMOVE,//移除礼包
    SIMULATE_CLICK_CONSORT_ADD_FRIEND_INDEX, //模拟点击情人聊天添加好友列表 item，item下标从0开始
    SIMULATE_CLICK_CONSORT_CHAT_INDEX, //模拟点击情人聊天列表 item，item下标从0开始

    SWITCH_CONSORT_MAIN_TAB,//切换情人主界面页签
    SIMULATE_CLICK_CITY_MAIN_EXPAND_BAR,//模拟点击主城主界面扩展栏按钮

    ON_MARS_ARRIVED_NEW_STAGE,//火星新阶段到达

    ON_GUILD_COOPERATE_RECOMMEND_CHG,//公会协作推荐据点变化
    ON_GUILD_COOPERATE_PROPERTY_POINT_CHG,//公会协作属性据点变化
    ON_GUILD_COOPERATE_RESET,//公会协作重置
    ON_GUILD_COOPERATE_DRAW_REWARD_CHG,//公会协作已领取奖励变更
    ON_GUILD_COOPERATE_HERO_USE_INFO_CHG,//公会协作伙伴使用信息变更
    ON_GUILD_COOPERATE_REWARD_POINT_UNLOCK,//公会协作奖励据点解锁
    ON_GUILD_COOPERATE_REWARD_POINT_DEFEAT,//公会协作奖励据点击败
    ON_GUILD_COOPERATE_RESET_DEFAULT_AREA_SHOW,//公会协作重置默认区域显示
    
    SIMULATE_TRIGGER_INN_CASH_REGISTER_COLLECT_ANIM, //模拟触发旅店收银台收银动画
    SIMULATE_CLICK_EQUIP_RECYCLE_ITEM_INDEX,//模拟点击藏品回收列表 item，item下标从0开始
    SIMULATE_CLICK_RECRUIT_ENTRANCE, //模拟点击招募顾问、情人入口

    ON_CURRENT_TITLE_CHG,//当前佩戴称号变化

    SIMULATE_CLICK_HERO_TEN_UPGRADE_TOGGLE,//模拟点击伙伴十连升级开关
    SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_TOGGLE,//模拟点击子嗣一键教学开关
    SIMULATE_CLICK_OPEN_CHILD_ONE_KEY_PLUS_WND,//模拟点击打开子嗣进阶一键教学窗口
    SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_PLUS_TOGGLE,//模拟点击子嗣进阶一键教学开关
    SIMULATE_CLICK_CONSORT_ONE_KEY_INVITE_TOGGLE,//模拟点击情人一键邀约开关
    SIMULATE_CLICK_OPEN_ARENA_ONE_KEY_BATTLE_SETTING,//模拟点击打开竞技场一键战斗设置窗口
    SIMULATE_CLICK_TRAVEL_ONE_KEY_TOGGLE,//模拟点击一键游历开关
    SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_TOGGLE,//模拟点击关卡快速前进开关
    SIMULATE_CLICK_OPEN_CHAPTER_AUTO_SETTING,//模拟点击关卡自动设置界面
    IS_BUILDING_UNDER_CONSTRUCTION,//正在建造建筑

    ON_RECHARGE_REBATE_ADD,//新增充值返利
    ON_RECHARGE_REBATE_REMOVE,//移除充值返利
    ON_RECHARGE_REBATE_COUNT_CHG,//充值返利计算变化
    ON_RECHARGE_REBATE_DRAW_CHG,//充值返利领奖变化
    ON_RECHARGE_REBATE_GROUP_CHG,//充值返利组变化

    HOT_REF_PATCH_COMPLETE,//热更配表完成

    ON_PRIVILEGE_CARD_CHG,//权益卡信息变更
    ON_PRIVILEGE_CARD_REMOVE,//权益卡移除
    SIMULATE_CLICK_MARS_BUILDING_BUILD,//模拟点击火星建筑建造按钮
    SIMULATE_CLICK_SYSTEM_QUEST_BAR_REWARD, //模拟点击系统任务栏领取奖励按钮
    SIMULATE_CLICK_CONDITION_DESC_JUMP_BY_INDEX, //模拟点击条件描述跳转按钮
    SIMULATE_CLICK_MARS_BUILDING_UPGRADE, //模拟点击火星建筑升级按钮
    SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_FOLLOWER, //模拟点击火星居民补充按钮
    SIMULATE_CLICK_MARS_RESIDENT_REPLENISH, //模拟点击火星居民补充窗口补充按钮
    SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_RESULT_SURE, //模拟点击火星居民补充结果确认按钮
    SIMULATE_CLICK_MARS_BUILDING_EQUIPMENT_UPGRADE, //模拟点击火星建筑装备升级按钮
    SIMULATE_CLICK_MARS_BUILDING_UPGRADE_WND_BTN, //模拟点击火星建筑升级按钮（建筑详情窗口）
    SIMULATE_CLICK_MARS_BUILDING_SETTLE, //模拟点击火星建筑结算按钮
    SIMULATE_CLICK_MARS_BUILDING_TECHNOLOGY_RESEARCH, //模拟点击火星建筑科技树研究按钮
    SIMULATE_CLICK_MARS_TECHNOLOGY_DETAIL_UPGRADE, //模拟点击火星科技详情升级按钮
    SIMULATE_CLICK_EXPLORE_TEAM_NORMAL_MARS_BUILDING, //模拟点击火星探索队伍建筑
    SIMULATE_CLICK_MARS_BUILDING_TEAM_EXPLORE_REPAIR, //模拟点击火星建筑队伍探索维修按钮
    SIMULATE_CLICK_MARS_EXPLORE_TEAM_EDIT_ITEM_EDIT_BY_INDEX, //模拟点击火星探索队伍编辑item编辑按钮
    SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_SELECT_GRID_ITEM_BY_INDEX, //模拟点击火星探索队伍英雄选择grid item
    SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_EDIT_CONFIRM, //模拟点击火星探索队伍英雄编辑确认按钮
    SIMULATE_CLICK_MARS_EXPLORE_EVENT, //模拟点击火星探索事件
    SIMULATE_CLICK_MARS_EXPLORE_BATTLE_GO_EXPLORE, //模拟点击火星探索战斗前往探索按钮
    SIMULATE_CLICK_MARS_EXPLORE_TEAM_SELECT_ITEM_CONFIRM_BY_INDEX, //模拟点击火星探索队伍选择item确认按钮
    SIMULATE_CLICK_MARS_EXPLORE, //模拟点击火星探索按钮
    SIMULATE_CLICK_MARS_POPULAR_WILL_CENTER,//模拟点击火星民意中心
    SIMULATE_CLICK_MARS_POPULAR_WILL_HELP_ITEM,//模拟点击火星民意中心求助item

    SIMULATE_CLICK_MARS_GO_TO_START_CONFIRM, //模拟点击前往火星确认按钮
    SIMULATE_CLICK_TOWER_RESEARCH, //模拟点击爬塔研究按钮
    SIMULATE_CLICK_MARS_LANDING_CONFIRM, //模拟点击火星着陆确认按钮

    ON_PLAYER_PERMISSION_CHG,//玩家权限信息变更
    ON_PLAYER_PERMISSION_REMOVE,//玩家权限移除

    #region 推送礼包

    PUSH_GIFT_PACK_DISABLE,//推送礼包失效
    TRIGGER_NEW_PUSH_GIFT_PACK,//触发新的推送礼包
    PUSH_GIFT_PACK_INFO_CHG,//推送礼包信息变化

    #endregion
    
    HERO_MAIN_SCROLL_MOVE_TO_HERO,//伙伴主界面滚动到指定伙伴
    SIMULATE_CLICK_HERO,//模拟点击伙伴item
    
    SIMULATE_SWITCH_STEP_TASK_RUSH_RANK_TAB,//模拟切换阶段任务冲榜页签
    SIMULATE_CLICK_RULE_WND_NEXT_BTN,//模拟点击规则窗口下一页按钮
    SIMULATE_CLICK_RULE_WND_PRE_BTN,//模拟点击规则窗口上一页按钮
    
    ON_RANK_RUSH_GET_REWARD,//领取冲榜奖励完成
    ON_RANK_RUSH_GET_SETTLE_INFO,//冲榜获取结算信息完成
    SIMULATE_CLICK_HOLD_CONSORT_DINNER_INDEX,//模拟点击选择举办情人宴会，item下标从0开始

    ON_GET_QUEST_REWARD,//领取完主线任务奖励
    SET_SELECT_FIRST_NAMED_NO_GRADUATE_CHILD,//选择第一个已命名未毕业的子嗣
    
    ON_RANK_GIFT_CHG,//排行榜礼包变化
    SIMULATE_CLICK_MARS_BUILDING_HOME_INFO_SWITCH_ON_TOGGLE, //模拟点击火星家园建筑信息窗口开关按钮
    SIMULATE_CLICK_MARS_HOME_COLLECT, //模拟点击火星家园收集按钮
    ON_SEVEN_DAY_GOAL_RED_TIP_CHG,//七日目标红点变化
    ON_SEVEN_DAY_GOAL_INIT_DONE,//七日目标初始化完成

    #region 房间皮肤

    ON_ROOM_SKIN_INFO_CHG,//房间皮肤数据切换
    ON_ROOM_SKIN_ADD,//房间皮肤添加
    ON_ROOM_SKIN_INVALID, // 房间皮肤无效
    ON_ROOM_SKIN_DEL,//房间皮肤移除
    ON_IN_USE_ROOM_SKIN_CHG,// 当前使用的房间皮肤变更

    #endregion

    ON_ACCESS_BAG_ITEM_USE_ONE,//获取途径道具使用过一次
    SHOP_LIST_SCROLL_MOVE_TO_ITEM,//商店列表滚动到指定物品
    SIMULATE_CLICK_SHOP_ITEM,//模拟点击购买商店商品，下标从0开始
    SIMULATE_CLICK_BATCH_BUY_CONFIRM,//模拟点击批量购买确认按钮
    SET_ARENA_CELEBRITY_ONE_BOT,//设置竞技场名人榜只有一个机器人
    SIMULATE_CLICK_ARENA_CELEBRITY_ATTACK_BY_INDEX,//模拟点击竞技场名人榜攻击按钮
    SIMULATE_CLICK_GUILD_COOPERATE_CAN_GET_REWARD_POS,//模拟点击联盟协作可领取奖励据点
    SIMULATE_CLICK_GUILD_COOPERATE_REWARD_POS_GET_REWARD,//模拟点击联盟协作可领取奖励据点领取奖励
    SIMULATE_CLICK_GUILD_COOPERATE_CAN_CONSTRUCT_ATTR_POS,//模拟点击联盟协作可建造属性据点
    SIMULATE_CLICK_GUILD_COOPERATE_DISPATCH_HERO,//模拟点击联盟协作派遣顾问
    ON_BUILDING_NODE_ENTER_DONE,//建筑节点进入完成
    
    SIMULATE_TRIGGER_WND_SHAKE,//模拟触发窗口受击震动（可传：震动强度、震动持续时间）
    
    SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_SELECT_BY_INDEX,//模拟点击切换入口妃子选中第几个妃子，item下标从0开始
    SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_CONFIRM,//模拟点击切换入口妃子确认按钮
}