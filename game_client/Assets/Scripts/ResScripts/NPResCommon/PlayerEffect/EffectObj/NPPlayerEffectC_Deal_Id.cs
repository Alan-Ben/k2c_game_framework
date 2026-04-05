using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using NPCommon;
using UnityEngine;


namespace GOE
{
	public class NPPlayerEffectC_Deal_Id : _ANPPlayerEffectInfo
	{
	    private EClientIDDealType _m_eDealType;       //处理枚举
	    private long _m_lId;

	    public NPPlayerEffectC_Deal_Id()
	    {
	        _m_eDealType = EClientIDDealType.NONE;
	        _m_lId = 0;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_DEAL_ID; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        //使用QueueNode进行系统界面跳转
	        switch(_m_eDealType)
	        {
		        case EClientIDDealType.REMOTE_EFF:
	                //发送消息
	                NPGSClientListener.sendMsgByLog(GSWriter_007_CommOp.make_001_ReqDealRemoteEffect(0, _m_lId));
	                break;
	            case EClientIDDealType.TUTORIAL_DONE:
	                //设置引导完成
	                NPPlayer.instance.tutorialComp.setTutorialDone(_m_lId);
	                break;
                // case EClientIDDealType.QUEST_GET:
                //     NPGQuestWndCommon.showGetQuestBranch(_m_lId);
                    break;
                case EClientIDDealType.QUEST_DONE:
                    NPPlayer.instance.questComp.reqFinishQuest(_m_lId);
                    break;
                case EClientIDDealType.OPEN_MAIL:
			        NPPlayer.instance.mailComp.reqMailBreifList(() =>
			        {
				        GMailDataInfo mailDataInfo = NPPlayer.instance.mailComp.getFirstMailByMailRefId(_m_lId);
				        if (null == mailDataInfo)
				        {
#if UNITY_EDITOR
					        ALLog.Error($"Can not find mail for mail refId: {_m_lId}");
#endif
				        }
				        else
				        {
					        mailDataInfo.reqTitleInfo(null);
					        mailDataInfo.showDetailWnd();
				        }
			        });
                    break;
		        case EClientIDDealType.SHOW_TUTORIAL:
			        NPGTutorialController.instance.showTutorial(UIResPathAssistant.getAssetInfo(_m_lId), null);
			        break;
				case EClientIDDealType.SIMULATE_CLICK_CLOTHES_UNIT:
					//模拟点击选择衣服部件单位
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_UNIT, _m_lId);//模拟点击选择衣服部件单位
					break;
				case EClientIDDealType.SIMULATE_CLICK_CLOTHES_DIRECTORY:
					//模拟点击选择文件夹
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_DIRECTORY, _m_lId);//模拟点击选择文件夹
					break;
                case EClientIDDealType.MAIN_CITY_FOCUS_POINT:
                    break;
		        case EClientIDDealType.ROOM_FOCUS_POINT:
			        if (QueueMgr.instance._lastNode is not GNodeRoom)
			        {
				        QueueMgr.instance.AddNode(new GNodeRoom(() =>
				        {
					        MainAdditionRoomTDScene.instance.focusToEntryPoint(_m_lId);
				        }));
			        }
			        else
				        MainAdditionRoomTDScene.instance.focusToEntryPoint(_m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_OPEN_HERO_INFO:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_OPEN_HERO_INFO, _m_lId);//模拟点击选择文件夹
			        break;
		        case EClientIDDealType.SIMULATE_SELECT_HERO_RECOMMEND_INDEX:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_SELECT_HERO_RECOMMEND_INDEX, _m_lId);//模拟选择英雄推荐位置
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_DAILY_QUEST_REWARD:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_DAILY_QUEST_REWARD, _m_lId);//模拟点击日常任务领奖
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_ACHIEVE_REWARD:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ACHIEVE_REWARD, _m_lId);//模拟点击成就领奖
					break;
		        case EClientIDDealType.SIMULATE_CLICK_COLLEGE_POS_ITEM:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_COLLEGE_POS_ITEM, _m_lId);//模拟点击大学位置
					break;
				case EClientIDDealType.MOVE_FOCUS_TO_HERO_RECOMMEND:
					break;
				case EClientIDDealType.SIMULATE_CLICK_OPEN_MARKET_SHOP:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_MARKET_SHOP, _m_lId);//模拟打开店铺弹窗
                    break;
				case EClientIDDealType.SIMULATE_CLICK_SUIT_ITEM:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SUIT_ITEM, _m_lId);//模拟点击套装item试穿
					break;
				case EClientIDDealType.SIMULATE_CLICK_GIVE_CONSORT_GIFT:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_GIVE_CONSORT_GIFT, _m_lId);//模拟点击赠送知己礼物，道具下标从0开始
					break;
				case EClientIDDealType.SIMULATE_CLICK_OPEN_FIXED_RANK:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_OPEN_FIXED_RANK, _m_lId);//模拟点击打开常驻排行榜，rank_fixed表id
					break;
				case EClientIDDealType.SIMULATE_CLICK_FIXED_RANK_LIKE:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_FIXED_RANK_LIKE, _m_lId);//模拟点击常驻排行榜点赞，rank_fixed表id
					break;
				case EClientIDDealType.SIMULATE_CLICK_CUSTOM_SUIT_LIST_ITEM:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CUSTOM_SUIT_LIST_ITEM, _m_lId);//模拟点击自定义套装选项，选项从上到下的下标从0开始
					break;
		        case EClientIDDealType.SHOW_SIMPLE_TUTORIAL:
			        NPSimpleTutorialRefObj simpleTutorialRefObj = GRefdataCoreMgr.instance.simpleTutorialRefCore.getRef(_m_lId);
			        if (null == simpleTutorialRefObj)
			        {
				        UnityEngine.Debug.LogError($"SHOW_SIMPLE_TUTORIAL 找不到简易引导id：{_m_lId}");
				        return;
			        }
			        SimpleTutorialController.instance.setCurSimpleGuide(simpleTutorialRefObj);
			        break;
		        case EClientIDDealType.SHOW_SIMPLE_TUTORIAL_AUTO_TRIGGER:
			        NPSimpleTutorialRefObj itemTutorialRefObj = GRefdataCoreMgr.instance.simpleTutorialRefCore.getRef(_m_lId);
			        if (null == itemTutorialRefObj)
			        {
				        UnityEngine.Debug.LogError($"SHOW_SIMPLE_TUTORIAL 找不到简易引导id：{_m_lId}");
				        return;
			        }
			        SimpleTutorialController.instance.setCurSimpleGuide(itemTutorialRefObj);
			        //如果还不在引导中，触发简易引导
			        if(!Game.instance.isInTutorial)
			        {
				        //尝试触发简易引导
				        SimpleTutorialController.instance.checkStartSimpleTutorial(QueueMgr.instance._lastNode.nodeTag);
			        }
			        break;
				case EClientIDDealType.DIALOGUE_PLAY_AUDIO:
                    //对话播放音效，音效id
					if(NPGGUIWndDialogue.instance.isShow)
                        NPGGUIWndDialogue.instance.playAudio(_m_lId);

					if(GGUIWndPlotDialogue.instance.isShow)
                        GGUIWndPlotDialogue.instance.playAudio(_m_lId);
					break;
				case EClientIDDealType.SET_HERO_RECOMMEND_BUBBLE_ANI_SAMPLE:
					//设置骑士推荐气泡动画到某帧，id从0到100，0第一帧，100最后一帧
					WinMsg.SendMsg(WinMsgType.SET_HERO_RECOMMEND_BUBBLE_ANI_SAMPLE, _m_lId);
					break;
				case EClientIDDealType.SIMULATE_CLICK_HERO_TALENT_ITEM:
					//模拟点击骑士资质item，item下标从0开始
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_TALENT_ITEM, _m_lId);
					break;
				case EClientIDDealType.SIMULATE_CLICK_MARKET_ITEM_UPGRADE:
					//模拟点击码头item升级，item下标从0开始
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARKET_ITEM_UPGRADE, _m_lId);
					break;
				case EClientIDDealType.SIMULATE_OPEN_HERO_INFO_BY_INDEX:
					//模拟点击打开骑士信息界面，item下标从0开始
					WinMsg.SendMsg(WinMsgType.SIMULATE_OPEN_HERO_INFO_BY_INDEX, _m_lId);
					break;
				case EClientIDDealType.SIMULATE_CLICK_MAIL_ITEM://模拟点击邮件item(只在邮件页面执行时有效), item下标从0开始
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MAIL_ITEM, _m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_CLOTHES_ACTION_INDEX:
			        //模拟点击选择第x个动作, item下标从0开始
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_ACTION_INDEX,_m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_CLOTHES_POSE_INDEX:
			        //模拟点击选择第x个站姿, item下标从0开始
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_POSE_INDEX,_m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_CLOTHES_BG_INDEX:
			        //模拟点击选择第x个背景, item下标从0开始
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_BG_INDEX,_m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_OPEN_CONSORT_INFO_BY_INDEX:
					//模拟点击打开情人信息界面，item下标从0开始
					WinMsg.SendMsg(WinMsgType.SIMULATE_OPEN_CONSORT_INFO_BY_INDEX, _m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_FARMING_BUILDING_COLLECTION:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_FARMING_BUILDING_COLLECTION, _m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_BUILDING_BUILD:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUILDING_BUILD, _m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_ANECDOTE_POS:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ANECDOTE_POS, _m_lId);
			        break;
		        case EClientIDDealType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_CARD:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_CARD, _m_lId);
			        break;
		        case EClientIDDealType.MOVE_FOCUS_TO_FARMING_BUILDING_UPGRADE_BTN_AND_SHOW_GUIDE_HAND:
			        void FocusToTarget()
			        {
				        WinMsg.SendMsg(WinMsgType.MOVE_FOCUS_TO_FARMING_BUILDING_UPGRADE_BTN_AND_SHOW_GUIDE_HAND, _m_lId);
			        }
			        
			        if (QueueMgr.instance._lastNode is GNodeBuilding)
				        FocusToTarget();
			        else
				        QueueMgr.instance.AddNode(new GNodeBuilding(FocusToTarget));
			        break;
                case EClientIDDealType.SIMULATE_CLICK_HERO_WEAR_EQUIP_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO_WEAR_EQUIP_BY_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_CONSORT_BLESS_SKILL_UPGRADE_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_BLESS_SKILL_UPGRADE_BY_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_SELECT_ARENA_HERO_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_SELECT_ARENA_HERO_BY_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_ARENA_SELECT_OPPONENT_HERO_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_ARENA_SELECT_OPPONENT_HERO_BY_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_HOLD_DINNER_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HOLD_DINNER_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_TOWER_CHALLENGE_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_TOWER_CHALLENGE_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_EQUIP_ITEM_BY_INDEX:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EQUIP_ITEM_BY_INDEX, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_GET_STAGE_GOAL_BIG_STEP_REWARD_BY_ID:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_GET_STAGE_GOAL_BIG_STEP_REWARD_BY_ID, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_BUILDING_PRODUCT_UNLOCK_BY_INDEX:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BUILDING_PRODUCT_UNLOCK_BY_INDEX, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_INN_STATION_ENTER_BY_INDEX:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_STATION_ENTER_BY_INDEX, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_INN_DISH_ITEM_BY_DISH_INDEX:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_INN_DISH_ITEM_BY_DISH_INDEX, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_CONSORT_CG_ITEM_BY_INDEX:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_CG_ITEM_BY_INDEX, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_CONSORT_ADD_FRIEND_INDEX:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_ADD_FRIEND_INDEX, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_CONSORT_CHAT_INDEX:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_CHAT_INDEX, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_EQUIP_RECYCLE_ITEM_INDEX:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_EQUIP_RECYCLE_ITEM_INDEX, _m_lId);
	                break;
                case EClientIDDealType.SIMULATE_CLICK_RECRUIT_ENTRANCE:
	                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_RECRUIT_ENTRANCE, _m_lId);
	                break;
		        case EClientIDDealType.SIMULATE_SELECT_TREASURE_HUNT_SELECT_AREA:
			        WinMsg.SendMsg(WinMsgType.SIMULATE_SELECT_TREASURE_HUNT_SELECT_AREA, _m_lId);
			        break;
                case EClientIDDealType.SIMULATE_CLICK_CONDITION_DESC_JUMP_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONDITION_DESC_JUMP_BY_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_EDIT_ITEM_EDIT_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_EDIT_ITEM_EDIT_BY_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_SELECT_GRID_ITEM_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_SELECT_GRID_ITEM_BY_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_MARS_EXPLORE_EVENT:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_EVENT, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_SELECT_ITEM_CONFIRM_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_SELECT_ITEM_CONFIRM_BY_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_MARS_EXPLORE:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_HOLD_CONSORT_DINNER_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HOLD_CONSORT_DINNER_INDEX, _m_lId);
                    break;
                case EClientIDDealType.SIMULATE_CLICK_SHOP_ITEM:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_SHOP_ITEM, _m_lId);
                    break;
				case EClientIDDealType.SIMULATE_CLICK_CONSORT_TRAVEL_ITEM_BY_INDEX:
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_TRAVEL_ITEM_BY_INDEX, _m_lId);
					break;
                case EClientIDDealType.SIMULATE_CLICK_ARENA_CELEBRITY_ATTACK_BY_INDEX:
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_ARENA_CELEBRITY_ATTACK_BY_INDEX, _m_lId);
                    break;
                default:
	                break;
	        }
#endif
	    }

	    public static NPPlayerEffectC_Deal_Id readEffect(string _str)
	    {
	        string[] strs = _str.Split(':');
	        if(strs.Length < 2)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_DEAL_ID   example: enum:type:id Error Str: " + _str);
	            return null;
	        }

	        NPPlayerEffectC_Deal_Id effectObj = new NPPlayerEffectC_Deal_Id();

	        try
	        {
	            effectObj._m_eDealType = (EClientIDDealType)ALCommon.EnumParse(typeof(EClientIDDealType), strs[0], true);
	            effectObj._m_lId = long.Parse(strs[1]);

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_DEAL_ID   example: enum:type:id Error Str: " + _str);
	            return null;
	        }
	    }
	}
}