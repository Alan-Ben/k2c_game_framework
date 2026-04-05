using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
	public class NPPlayerEffectCSpecial : _ANPPlayerEffectInfo
	{
	    private EClientSpecialDealType _m_eSpecialDealType;       //特殊处理效果枚举

	    public NPPlayerEffectCSpecial()
	    {
	        _m_eSpecialDealType = EClientSpecialDealType.NONE;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SPECIAL; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        //使用QueueNode进行系统界面跳转
	        switch(_m_eSpecialDealType)
	        {
	            case EClientSpecialDealType.QUIT_ACC:
	                //注销
	                Game.instance.relogin();
	                break;
                case EClientSpecialDealType.ENTER_EMPTY_SCENE:
                    //进入一个空的UI视图
                    QueueMgr.instance.addNode_InGame_MainUIMainScene(NPGMainGUIAddSceneEmpty.instance, UINodeTagConst.C_Empty, false, 0);
                    break;
                case EClientSpecialDealType.QUIT_EMPTY_SCENE:
                    //进入一个空的UI视图
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Empty);
                    break;
	            case EClientSpecialDealType.ADD_EMPTY_ONL_OP_NODE:
		            QueueMgr.instance.addNode_OnlyOp(null, null);
		            break;
	            case EClientSpecialDealType.DO_ROLLBACK_BY_ESC:
		            QueueMgr.instance.DoUIRollBackByEsc();
		            break;
				case EClientSpecialDealType.IGNORE_ALL_INPUT:
					//使用key关闭所有输入，一般引导使用
					Game.instance.mainCamera.openAllInputMask();
					break;
				case EClientSpecialDealType.OPEN_ALL_INPUT:
					//使用key开启所有输入，一般引导使用
					Game.instance.mainCamera.forceCloseAllInputMask();
					break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CLOTHES_BACK_ROOT:
		            //衣柜模拟点击返回根目录
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_BACK_ROOT);//衣柜模拟点击返回根目录
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CLOTHES_SAVE:
		            //衣柜模拟点击保存
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_SAVE);//衣柜模拟点击保存
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CHAPTER_MOVE:
		            //衣柜模拟点击保存
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CHAPTER_MOVE);//模拟点击关卡摇骰子
		            break;
	            case EClientSpecialDealType.OPEN_SET_NAME_WND:
		            //打开设置名字窗口
					GCommon.showPlayerCreatName();
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_HERO_LEVEL_UPGRADE:
		            //模拟点击英雄升级
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_LEVEL_UPGRADE);//模拟点击英雄升级
		            break;
	            case EClientSpecialDealType.SIMULATE_RET_MAIN_QUEST_REWARD:
		            //模拟点击主线任务奖励
		            WinMsg.SendMsg(WinMsgType.SIMULATE_RET_MAIN_QUEST_REWARD);//模拟点击主线任务奖励
		            break;
	            case EClientSpecialDealType.SIMULATE_PAGE_RET_MAIN_QUEST_REWARD:
		            //模拟页面内点击主线任务领奖
		            WinMsg.SendMsg(WinMsgType.SIMULATE_PAGE_RET_MAIN_QUEST_REWARD);//模拟页面内点击主线任务领奖
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CONSORT_RANDOM_GREET:
		            //模拟点击妃子随机宠幸
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_RANDOM_GREET);//模拟点击妃子随机宠幸
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_HERO_RECOMMEND_GET:
		            //模拟点击英雄推荐获取大臣
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_RECOMMEND_GET);//模拟点击英雄推荐获取大臣
		            break;
                case EClientSpecialDealType.SIMULATE_CLICK_TRAVEL:
                    //模拟点击游历
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TRAVEL);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_SHOW_CHAPTER_EVENT_LIST:
                    //模拟点击打开关卡事件列表
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SHOW_CHAPTER_EVENT_LIST);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_LEVY_SILVER:
					//模拟点击征收粮食
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_LEVY_SILVER);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_HOLD_DINNER:
					//模拟点击举办宴会
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HOLD_DINNER);
					break;
	            case EClientSpecialDealType.SIMULATE_CLICK_LEVY_SOLDIER:
		            //模拟点击征收士兵
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_LEVY_SOLDIER);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_OPEN_CONSORT_GIVE_GIFT:
					//模拟点击打开妃子送礼弹窗
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_CONSORT_GIVE_GIFT);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_MARKET_GET_REWARD:
					//模拟店铺领取奖励
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARKET_GET_REWARD);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_TRAIN_CHILD:
					//模拟点击培养子嗣
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TRAIN_CHILD);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_OPEN_CHILD_SET_NAME:
					//模拟点击打开子嗣取名弹窗
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_CHILD_SET_NAME);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_OPEN_CHILD_TRAIN:
					//模拟点击打开子嗣培养弹窗
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_CHILD_TRAIN);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_SUIT_MENU:
					//模拟点击套装菜单
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SUIT_MENU);
		            break;
	            case EClientSpecialDealType.MOVE_FOCUS_TO_HERO_RECOMMEND:
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_SHARE_CLOTHES:
					//模拟点击分享装扮按钮
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SHARE_CLOTHES);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_SHARE_CLOTHES_CONFIRM:
					//模拟点击确认分享装扮按钮
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SHARE_CLOTHES_CONFIRM);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_SHOW_CUSTOM_SUIT_LIST:
					//模拟点击打开自定义套装下拉列表
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SHOW_CUSTOM_SUIT_LIST);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_ONE_DRAW_CARDS:
					//模拟点击单次抽卡
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ONE_DRAW_CARDS);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_OPEN_HERO_TRAIN:
					//模拟点击打开培养骑士窗口
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_HERO_TRAIN);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_ADULT_START_MARRY:
					//模拟点击打开发起联姻窗口
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ADULT_START_MARRY);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_HERO_TALENT_TAB:
					//模拟点击打开骑士资质页签
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_TALENT_TAB);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_AVATAR_SCENE_NULL_RETURN:
					//模拟点击avatar场景空白处返回页签
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_AVATAR_SCENE_NULL_RETURN);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_OPEN_MARKET_UPGRADE:
					//模拟点击打开码头升级界面
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_MARKET_UPGRADE);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_OPEN_CONSORT_TRAVEL:
					//模拟点击打开情人出游弹窗
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_CONSORT_TRAVEL);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_CONSORT_TRAVEL_USE_ITEM:
					//模拟点击情人使用道具出游
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_TRAVEL_USE_ITEM);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_TEN_DRAW_CARDS:
					//模拟点击十连抽卡
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TEN_DRAW_CARDS);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_CHAPTER_START_FIGHT:
					//模拟点击关卡boss战出战
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CHAPTER_START_FIGHT);
					break;
				case EClientSpecialDealType.SIMULATE_GAIN_SHOW_MAIL_REWARD:
					WinMsg.SendMsg(WinMsgType.SIMULATE_GAIN_SHOW_MAIL_REWARD);
					break;
	            case EClientSpecialDealType.SIMULATE_CLICK_OPEN_CHAPTER_LOSS_DEGREE:
		            //模拟点击打开关卡面包消耗详情
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_CHAPTER_LOSS_DEGREE);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CLOTHES_ACTION_BACK:
		            //模拟点击动作选择窗口返回
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_ACTION_BACK);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CLOTHES_POSE_BACK:
		            //模拟点击站姿选择窗口返回
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_POSE_BACK);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CLOTHES_BG_BACK:
		            //模拟点击背景选择窗口返回
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_BG_BACK);
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CLOTHES_MAIN_BACK:
		            //模拟点击衣柜窗口返回
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_MAIN_BACK);//模拟点击衣柜窗口返回
		            break;
	            case EClientSpecialDealType.SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_CHANGE:
		            // 模拟点击游历交换事件交换
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_CHANGE);
		            break;
                case EClientSpecialDealType.SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_NOT_CHANGE:
	                // 模拟点击游历交换事件不交换
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_NOT_CHANGE);
	                break;
	            case EClientSpecialDealType.SIMULATE_CLICK_PLAYER_LVL_UP:
		            // 模拟点击玩家升级
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_PLAYER_LVL_UP);
		            break;
                case EClientSpecialDealType.SIMULATE_CLICK_HERO_BUSINESS_SKILL_UPGRADE:
                    // 模拟点击伙伴经营技能升级
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_BUSINESS_SKILL_UPGRADE);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUILDING_BUILD_WND_BTN:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUILDING_BUILD_WND_BTN);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUSINESS_BUILDING_TEN_TIMES_HIRE_TOGGLE:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_TEN_TIMES_HIRE_TOGGLE);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUSINESS_BUILDING_HIRE_BTN:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HIRE_BTN);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_FARMING_BUILDING_UPGRADE_BTN:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_FARMING_BUILDING_UPGRADE_BTN);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SLOT:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SLOT);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_BTN:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_BTN);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUSINESS_BUILDING_SET_HERO_BTN:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_SET_HERO_BTN);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUSINESS_BUILDING_RND_BTN:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_RND_BTN);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SELECT_CONFIRM:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SELECT_CONFIRM);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_QUIT_CONSORT_GIVE_GIFT:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_QUIT_CONSORT_GIVE_GIFT);
                    break;
	            case EClientSpecialDealType.SIMULATE_CLICK_CHAPTER_FORWARD:
		            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CHAPTER_FORWARD);
		            break;
                case EClientSpecialDealType.TRIGGERS_DEVICE_VIBRATION:
#if UNITY_ANDROID || unity_IOS
					Handheld.Vibrate();
#endif
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_CONFIRM_CHILD_SET_NAME:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONFIRM_CHILD_SET_NAME);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_EQUIP_OPEN_SKILL_PAGE:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EQUIP_OPEN_SKILL_PAGE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_EQUIP_UPGRADE:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EQUIP_UPGRADE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_EQUIP_SKILL_REBUILD:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EQUIP_SKILL_REBUILD);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_HERO_EQUIP_ENTRY_BTN:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_EQUIP_ENTRY_BTN);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_CONFIRM_BTN:
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_CONFIRM_BTN);
					break;
                case EClientSpecialDealType.SIMULATE_CLICK_HERO_EQUIP_INFO_STRENGTHEN_BTN:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_EQUIP_INFO_STRENGTHEN_BTN);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_ARENA_RANDOM_ATTACK:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ARENA_RANDOM_ATTACK);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_ARENA_OPEN_SELECT_HERO:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ARENA_OPEN_SELECT_HERO);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_ARENA_OPEN_SELECT_INITIAL_BUFF:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ARENA_OPEN_SELECT_INITIAL_BUFF);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_ARENA_SELECT_NULL_INITIAL_BUFF:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ARENA_SELECT_NULL_INITIAL_BUFF);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_STAGE_GOAL_MAIN_TASK_GAIN_REWARD:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_STAGE_GOAL_MAIN_TASK_GAIN_REWARD);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_ARENA_START_FIGHT:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ARENA_START_FIGHT);
	                break;
                case EClientSpecialDealType.SIMULATE_SUMMON_ONE_DRAW:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_SUMMON_ONE_DRAW);
	                break;
                case EClientSpecialDealType.SIMULATE_SUMMON_TEN_DRAW:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_SUMMON_TEN_DRAW);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_EQUIP_CONFIRM_REBUILD:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EQUIP_CONFIRM_REBUILD);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_COUNTDOWN_EVENT_GET_REWARD:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_COUNTDOWN_EVENT_GET_REWARD);
	                break;
	            case EClientSpecialDealType.TRY_CHECK_SIMPLE_GUIDE:
		            //如果还不在引导中，触发简易引导
		            if(!Game.instance.isInTutorial)
		            {
			            //尝试触发简易引导
			            SimpleTutorialController.instance.checkStartSimpleTutorial(QueueMgr.instance._lastNode.nodeTag);
		            }
		            break;
                case EClientSpecialDealType.SIMULATE_CLICK_CONSORT_NORMAL_COMPREHEND:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_NORMAL_COMPREHEND);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_SCHOOL_GET_CHILD:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SCHOOL_GET_CHILD);
	                break;
                case EClientSpecialDealType.FORCE_RESET_SIMPLE_GUIDE:
	                //强制重置简单引导
	                SimpleTutorialController.instance.forceResetGuide();
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_MAIN_STATION_BUTTON:
	                //模拟点击旅店设施按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_MAIN_STATION_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_STATION_BUILD_BUTTON:
	                //模拟点击旅店设施建造按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_STATION_BUILD_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_STATION_LEVEL_UP_BUTTON:
	                //模拟点击旅店设施升级按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_STATION_LEVEL_UP_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_MAIN_MENU_BUTTON:
	                //模拟点击旅店主界面菜单按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_MAIN_MENU_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_DISH_UNLOCK_BUTTON:
	                //模拟点击旅店菜品解锁按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_DISH_UNLOCK_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_DISH_LEVEL_UP_BUTTON:
	                //模拟点击旅店菜品升级按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_DISH_LEVEL_UP_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_CASH_REGISTER:
	                //模拟点击旅店收银台
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_CASH_REGISTER);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_CREATE_GUEST_BUTTON:
	                //模拟点击旅店创建客人按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_CREATE_GUEST_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_INN_SPECIAL_GUEST_SERVE_BUTTON:
	                //模拟点击旅店特殊客人接待按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_SPECIAL_GUEST_SERVE_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_SYSTEM_QUEST_GET_REWARD:
	                //模拟点击系统任务领取奖励按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_GET_REWARD);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_SYSTEM_QUEST_DETAIL_BUTTON:
	                //模拟点击系统任务详情按钮
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_DETAIL_BUTTON);
	                break;
                case EClientSpecialDealType.SIMULATE_TRIGGER_INN_CASH_REGISTER_COLLECT_ANIM:
	                //模拟触发旅店收银台收获动画
	                WinMsg.SendMsg(WinMsgType.SIMULATE_TRIGGER_INN_CASH_REGISTER_COLLECT_ANIM);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_HERO_TEN_UPGRADE_TOGGLE:
                    //模拟点击伙伴十连升级开关
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_TEN_UPGRADE_TOGGLE);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_TOGGLE:
                    //模拟点击子嗣一键教学开关
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_TOGGLE);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_OPEN_CHILD_ONE_KEY_PLUS_WND:
                    //模拟点击打开子嗣进阶一键教学窗口
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_CHILD_ONE_KEY_PLUS_WND);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_PLUS_TOGGLE:
                    //模拟点击子嗣进阶一键教学开关
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_PLUS_TOGGLE);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_CONSORT_ONE_KEY_INVITE_TOGGLE:
                    //模拟点击情人一键邀约开关
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_ONE_KEY_INVITE_TOGGLE);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_OPEN_ARENA_ONE_KEY_BATTLE_SETTING:
                    //模拟点击打开竞技场一键战斗设置窗口
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_ARENA_ONE_KEY_BATTLE_SETTING);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_TRAVEL_ONE_KEY_TOGGLE:
                    //模拟点击一键游历开关
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TRAVEL_ONE_KEY_TOGGLE);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_TOGGLE:
                    //模拟点击关卡快速前进开关
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_TOGGLE);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_OPEN_CHAPTER_AUTO_SETTING:
                    //模拟点击关卡自动设置界面
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_CHAPTER_AUTO_SETTING);
	                break;
                case EClientSpecialDealType.SHOW_AIHELP:
                    //打开AIHelp界面
                    GCommon.showAIHelp();
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_TREASURE_HUNT_MAIN_GAIN_ENERGY:
                    //模拟点击领取太空打捞体力
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_MAIN_GAIN_ENERGY);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_TREASURE_HUNT_MAIN_PLAY:
                    //模拟点击前往太空打捞界面
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_MAIN_PLAY);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_TREASURE_HUNT_GAME_PLAY:
                    //模拟点击开始探索
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_GAME_PLAY);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_MIDDAY_DUNGEON_ENTER:
                    //模拟点击太空维护按钮打开维护界面
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MIDDAY_DUNGEON_ENTER);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_MIDDAY_DUNGEON_BATTLE_ATTACK:
                    //模拟点击太空站维护派遣按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MIDDAY_DUNGEON_BATTLE_ATTACK);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_EVENING_DUNGEON_ENTER:
                    //模拟点击晚间副本(抵御陨石)按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EVENING_DUNGEON_ENTER);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_EVENING_DUNGEON_GAME_FIGHT:
                    //模拟点击晚间副本派遣按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EVENING_DUNGEON_GAME_FIGHT);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_TREASURE_HUNT_GAME_CHANGE_AREA:
                    //模拟点击太空寻宝更换地点按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_GAME_CHANGE_AREA);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_TREASURE_HUNT_SELECT_AREA_GO:
                    //模拟点击太空寻宝"前往"新区域
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_SELECT_AREA_GO);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_SKIP_DIALOG_CONFIRM:
                    //模拟点击关卡快速前进是否跳过对话"确认"按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CHAPTER_QUICK_FORWARD_SKIP_DIALOG_CONFIRM);
	                break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_BUILDING_BUILD:
                    //模拟点击火星建筑建造按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_BUILD);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_SYSTEM_QUEST_BAR_REWARD:
                    //模拟点击系统任务栏领取奖励按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SYSTEM_QUEST_BAR_REWARD);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_BUILDING_UPGRADE:
                    //模拟点击火星建筑升级按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_UPGRADE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_FOLLOWER:
                    //模拟点击火星居民补充按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_FOLLOWER);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH:
                    //模拟点击火星居民补充窗口补充按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_RESULT_SURE:
                    //模拟点击火星居民补充结果确认按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_RESULT_SURE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_BUILDING_EQUIPMENT_UPGRADE:
                    //模拟点击火星建筑装备升级按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_EQUIPMENT_UPGRADE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_BUILDING_UPGRADE_WND_BTN:
                    //模拟点击火星建筑升级按钮（建筑详情窗口）
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_UPGRADE_WND_BTN);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_BUILDING_SETTLE:
                    //模拟点击火星建筑结算按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_SETTLE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_BUILDING_TECHNOLOGY_RESEARCH:
                    //模拟点击火星建筑科技树研究按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_TECHNOLOGY_RESEARCH);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_TECHNOLOGY_DETAIL_UPGRADE:
                    //模拟点击火星科技详情升级按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_TECHNOLOGY_DETAIL_UPGRADE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_EXPLORE_TEAM_NORMAL_MARS_BUILDING:
                    //模拟点击火星探索队伍建筑
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EXPLORE_TEAM_NORMAL_MARS_BUILDING);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_BUILDING_TEAM_EXPLORE_REPAIR:
                    //模拟点击火星建筑队伍探索维修按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_TEAM_EXPLORE_REPAIR);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_EDIT_CONFIRM:
                    //模拟点击火星探索队伍英雄编辑确认按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_EDIT_CONFIRM);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_EXPLORE_BATTLE_GO_EXPLORE:
                    //模拟点击火星探索战斗前往探索按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_BATTLE_GO_EXPLORE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_GO_TO_START_CONFIRM:
                    //模拟点击前往火星确认按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_GO_TO_START_CONFIRM);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_TOWER_RESEARCH:
                    //模拟点击爬塔研究按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TOWER_RESEARCH);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_LANDING_CONFIRM:
                    //模拟点击火星着陆确认按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_LANDING_CONFIRM);
                    break;
				case EClientSpecialDealType.MARS_HOME_COLLECT:
					NPPlayer.instance.marsComp.buildingSubComponent.collectHome();
					break;
				case EClientSpecialDealType.SET_SELECT_FIRST_NAMED_NO_GRADUATE_CHILD:
                    //选择第一个已命名的子嗣
                    WinMsg.SendMsg(WinMsgType.SET_SELECT_FIRST_NAMED_NO_GRADUATE_CHILD);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_BUILDING_HOME_INFO_SWITCH_ON_TOGGLE:
                    //模拟点击火星家园建筑信息窗口开关按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_HOME_INFO_SWITCH_ON_TOGGLE);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_MARS_HOME_COLLECT:
                    //模拟点击火星家园收集按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_HOME_COLLECT);
                    break;
                case EClientSpecialDealType.SIMULATE_CLICK_BATCH_BUY_CONFIRM:
                    //模拟点击批量购买确认
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BATCH_BUY_CONFIRM);
                    break;
                case EClientSpecialDealType.SET_ARENA_CELEBRITY_ONE_BOT:
                    //设置竞技场名人榜只有一个机器人
                    WinMsg.SendMsg(WinMsgType.SET_ARENA_CELEBRITY_ONE_BOT);
                    break;
				case EClientSpecialDealType.SIMULATE_CLICK_MAIN_QUEST_GO_TO:
					//模拟点击主线任务前往按钮
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MAIN_QUEST_GO_TO);
					break;
				case EClientSpecialDealType.GUILD_COOPERATE_RESET_DEFAULT_AREA_SHOW:
					//联盟协作重置默认区域显示
					WinMsg.SendMsg(WinMsgType.ON_GUILD_COOPERATE_RESET_DEFAULT_AREA_SHOW);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_GUILD_COOPERATE_CAN_GET_REWARD_POS:
                    //模拟点击联盟协作可领取奖励据点
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_CAN_GET_REWARD_POS);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_GUILD_COOPERATE_REWARD_POS_GET_REWARD:
                    //模拟点击联盟协作可领取奖励据点领取奖励
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_REWARD_POS_GET_REWARD);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_GUILD_COOPERATE_CAN_CONSTRUCT_ATTR_POS:
                    //模拟点击联盟协作可建造属性据点
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_CAN_CONSTRUCT_ATTR_POS);
					break;
				case EClientSpecialDealType.SIMULATE_CLICK_GUILD_COOPERATE_DISPATCH_HERO:
                    //模拟点击联盟协作派遣顾问
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_DISPATCH_HERO);
					break;
				default:
	                break;
	        }
#endif
	    }

	    public static NPPlayerEffectCSpecial readEffect(string _str)
	    {
	        NPPlayerEffectCSpecial effectObj = new NPPlayerEffectCSpecial();

	        try
	        {
	            effectObj._m_eSpecialDealType = (EClientSpecialDealType)ALCommon.EnumParse(typeof(EClientSpecialDealType), _str, true);

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("玩家效果——特殊处理效果——配置错误 - C_SPECIAL   example: enum:specialDealType Error Str: " + _str);
	            return null;
	        }
	    }
	}
}