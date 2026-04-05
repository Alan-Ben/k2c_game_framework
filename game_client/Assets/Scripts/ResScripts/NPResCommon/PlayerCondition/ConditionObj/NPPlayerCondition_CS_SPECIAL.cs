using ALPackage;
using Common.ActivityEnum;
using NPEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GOE
{
	public class NPPlayerCondition_CS_SPECIAL : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_SPECIAL; } }

	    private ENPPlayer_CS_SpecialCondition _m_ePlayerCSSpecialType;

	    public ENPPlayer_CS_SpecialCondition specialType { get { return _m_ePlayerCSSpecialType; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_SPECIAL readStr(ALStringReader _reader)
	    {
	        string specialS = _reader.readItem(':');
	        if (null == specialS)
	        {
	            UnityEngine.Debug.LogError("Can not read str for NPPlayerCondition_CS_SPECIAL[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_SPECIAL cond = new NPPlayerCondition_CS_SPECIAL();
        
	        cond._m_ePlayerCSSpecialType = (ENPPlayer_CS_SpecialCondition)ALPackage.ALCommon.EnumParse(typeof(ENPPlayer_CS_SpecialCondition), specialS, true);

	        return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        //根据不同类型进行处理
	        switch(_m_ePlayerCSSpecialType)
	        {
		        //是否在引导中
                case ENPPlayer_CS_SpecialCondition.C_IS_IN_TUTORIAL:
	                return Game.instance.isInTutorial;
		        case ENPPlayer_CS_SpecialCondition.C_ROOM_SCENE_IS_SHOW:
			        return MainAdditionRoomTDScene.instance.isShow;
                case ENPPlayer_CS_SpecialCondition.C_CHILD_HAS_UNMARRIED_ADULT:
	                return NPPlayer.instance.childComp.getUnmarriedChildCount() > 0;
                case ENPPlayer_CS_SpecialCondition.C_CHILD_CAN_TRAIN:
	                return NPPlayer.instance.childComp.getCanTrainChildCount() > 0;
                case ENPPlayer_CS_SpecialCondition.C_HAVE_ANNOUNCEMENT:
                    return AnnouncementMgr.instance.haveValidAnnouncement();
                case ENPPlayer_CS_SpecialCondition.C_HAVE_QUESTIONNAIRE:
                    return NPPlayer.instance.questionnaireComp.canShowEntrance();
                case ENPPlayer_CS_SpecialCondition.C_HAVE_RANK_RUSH_ACTIVITY:
                    return NPPlayer.instance.commonActivityComp.haveRankRushActivity();
                case ENPPlayer_CS_SpecialCondition.C_ANECDOTE_IN_EVENT_PROCESS:
					return NPPlayer.instance.anecdoteComp.inEventProcess;
                case ENPPlayer_CS_SpecialCondition.CS_QUEST_ALL_DONE:
                    QuestItem curQuestItem = null;
                    if (QuestFollowMgr.instance.curFollow != null && QuestFollowMgr.instance.curFollow.questType == ENPFollowQuestType.MAIN)
                        curQuestItem = (QuestItem)QuestFollowMgr.instance.curFollow;
                    return curQuestItem == null;
				case ENPPlayer_CS_SpecialCondition.C_FUNCTION_REWARD_ALL_GET:
                    return NPPlayer.instance.funcUnlockComp.getFirstShowInfo() == null;
				case ENPPlayer_CS_SpecialCondition.C_HAVE_STEP_REWARD_ACTIVITY:
                    List<ActivityStepRewardInfo> stepRewardList = new List<ActivityStepRewardInfo>();
                    NPPlayer.instance.commonActivityComp.getActivityStepRewardInfoList(stepRewardList, (_info) =>
                    {
                        return _info != null && _info.isShowInStepRewardWnd;
                    });
                    return stepRewardList.Count > 0;
				case ENPPlayer_CS_SpecialCondition.C_CONSORT_HAS_UNCLAIMED_CG_REWARD:
			        return NPPlayer.instance.consortComp.hasUnclaimedCgReward();
				case ENPPlayer_CS_SpecialCondition.CS_IS_ARRIVE_MARS:
			        return NPPlayer.instance.marsComp.goToSubComponent.isArriveMars;
				case ENPPlayer_CS_SpecialCondition.C_FIRST_RECHARGE_REWARD_IS_ALL_GET:
                    //首充奖励是否已经全部领取
                    bool firstRechargeIsAllGet = true;
                    //是否购买过首充礼包
                    long firstRechargeBuyCount = NPPlayer.instance.giftPackComp.getGiftPackHadBuyCount(GRefdataCoreMgr.instance.npGeneral.first_recharge_gift_pack_id);
                    if (firstRechargeBuyCount > 0)
                    {
                        //购买过首充礼包，检查首充奖励是否全部领取
                        GRefdataCoreMgr.instance.firstRechargeDayRefCore.dealAllRef(_ref =>
                        {
                            if (_ref != null && NPPlayer.instance.playerBuffComp.lookup(_ref.buff_id) != null)
                                firstRechargeIsAllGet = false;
                        });
                    }
                    else
                        firstRechargeIsAllGet = false;
                    return firstRechargeIsAllGet;
				case ENPPlayer_CS_SpecialCondition.C_HAVE_AVAIABLE_GEM_GIFT_PACK:
                    List<_ABaseActivityInfo>  activityInfoList = NPPlayer.instance.commonActivityComp.getValidActivityListByFunc((_info) =>
                    {
                        return _info != null &&
                               _info.crystalGiftPackInfo != null &&
                               _info.crystalGiftPackInfo.giftPackGroupRef != null &&
                               _info.crystalGiftPackInfo.giftPackGroupRef.page_ui_res_id > 0 &&
                               _info.isPlaying;
                    });
                    return activityInfoList != null && activityInfoList.Count > 0;
                case ENPPlayer_CS_SpecialCondition.C_CAN_USE_WEB_RECHARGE:
                    return GCommon.canUseWebRecharge();
                case ENPPlayer_CS_SpecialCondition.C_STAGE_GOAL_IS_ALL_DONE:
                    return NPPlayer.instance.stageGoalComp.isAllDone;
                case ENPPlayer_CS_SpecialCondition.C_IS_LANDING_MARS:
                    return NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.isSelectLandingMarsArea();
                case ENPPlayer_CS_SpecialCondition.C_CUR_WND_HERO_CAN_UPGRADE:
                    return GGUIWndHeroInfo.instance.curShowHeroCanUpgrade;
                case ENPPlayer_CS_SpecialCondition.C_TRAVEL_HAS_EVENT_UNTREATED:
			        return NPPlayer.instance.travelComp.eventList.Count > 0;
                case ENPPlayer_CS_SpecialCondition.C_HAVE_COUNT_DOWN_EVENT:
                    CountdownEventInfo info = NPPlayer.instance.countdownEventComp.countdownEventInfo;
                    bool isValid = info != null && info.isValid;
                    return isValid;
                case ENPPlayer_CS_SpecialCondition.C_COUNT_DOWN_EVENT_CAN_GET_REWARD:
                    return NPPlayer.instance.countdownEventComp.curCanGetReward();
		        case ENPPlayer_CS_SpecialCondition.C_CUR_IS_IN_CHAPTER_BOSS:
			        return NPPlayer.instance.chapterComp.curIsBossPoint();
		        case ENPPlayer_CS_SpecialCondition.C_CUR_CHAPTER_BOSS_POWER_IS_ENOUGH:
			        return NPPlayer.instance.chapterComp.getTotalPower() > NPPlayer.instance.chapterComp.chapterRefObj?.boss_power;
                default:
	                return false;
	        }
#else
	        return false;
#endif
	    }
	}
}