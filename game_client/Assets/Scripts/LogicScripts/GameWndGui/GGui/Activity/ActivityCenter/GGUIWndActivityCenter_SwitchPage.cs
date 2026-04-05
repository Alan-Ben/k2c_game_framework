using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 活动中心界面
    /// </summary>
    public partial class GGUIWndActivityCenter
    {
        //不同页面字典
        [NotNull] private Dictionary<long, _AALBasicLoadUIWndBasicClass> _m_dActivityCenterWndDic = new Dictionary<long, _AALBasicLoadUIWndBasicClass>();

        /// <summary>
        /// 处理切换加载不同页面
        /// </summary>
        private void _dealSwitchPage(bool _needHidePage)
        {
            if (wnd == null || _m_curSelectRef == null)
                return;

            if(_needHidePage)
                _hideAllPage();

            _m_dActivityCenterWndDic.TryGetValue(_m_curSelectRef.id, out _AALBasicLoadUIWndBasicClass targetPage);
            switch (_m_curSelectRef.type)
            {
                //========限时冲榜========
                case EActivityCenterTabType.RANK_RUSH:
                    if (targetPage != null)
                        targetPage?.showWnd();
                    else
                    {
                        GGUIWndRankRushPage rankRushPage = new GGUIWndRankRushPage(UIResPathAssistant.getAssetInfo(3901), wnd.pageParent);
                        rankRushPage.load(() =>
                        {
                            if (rankRushPage == null)
                                return;

                            rankRushPage.showWnd();
                        });
                        _m_dActivityCenterWndDic[_m_curSelectRef.id] = rankRushPage;
                    }
                    break;
                //========阶段奖励========
                case EActivityCenterTabType.STEP_REWARD:
                    if (targetPage != null)
                        targetPage?.showWnd();
                    else
                    {
                        GGUIWndStepRewardPage stepRewardPage = new GGUIWndStepRewardPage(UIResPathAssistant.getAssetInfo(3906), wnd.pageParent);
                        stepRewardPage.load(() =>
                        {
                            if (stepRewardPage == null)
                                return;

                            stepRewardPage.showWnd();
                        });
                        _m_dActivityCenterWndDic[_m_curSelectRef.id] = stepRewardPage;
                    }
                    break;
                //========千万目标========
                case EActivityCenterTabType.EARNING_GOAL:
                    if (NPPlayer.instance.earningGoalComp.isEarningGoalActivityOpen())
                    {
                        if (targetPage is GGUIWndEarningGoalMain)
                            targetPage?.showWnd();
                        else
                        {
                            targetPage?.discard();
                            targetPage = new GGUIWndEarningGoalMain(UIResPathAssistant.getAssetInfo(5700), wnd.pageParent);
                            targetPage.load(() =>
                            {
                                if (targetPage == null)
                                    return;

                                targetPage.showWnd();
                            });
                            _m_dActivityCenterWndDic[_m_curSelectRef.id] = targetPage;
                        }
                    }
                    else
                    {
                        if (targetPage is GGUIWndEarningGoalSelfPage)
                            targetPage?.showWnd();
                        else
                        {
                            targetPage?.discard();
                            targetPage = new GGUIWndEarningGoalSelfPage(UIResPathAssistant.getAssetInfo(5705), wnd.pageParent);
                            targetPage.load(() =>
                            {
                                if (targetPage == null)
                                    return;

                                targetPage.showWnd();
                            });
                            _m_dActivityCenterWndDic[_m_curSelectRef.id] = targetPage;
                        }
                    }
                    break;
                //========七日目标========
                case EActivityCenterTabType.SEVEN_DAY_GOALS:
                    GGUIWndSevenDayGoals sevenDayGoalsPage;
                    if (targetPage != null)
                    {
                        sevenDayGoalsPage = targetPage as GGUIWndSevenDayGoals;
                        sevenDayGoalsPage?.refreshWnd(NPPlayer.instance.sevenDayGoalsComp.data.getCanGetRewardDay());
                        sevenDayGoalsPage?.showWnd();
                    }
                    else
                    {
                        sevenDayGoalsPage = new GGUIWndSevenDayGoals(UIResPathAssistant.getAssetInfo(5800), wnd.pageParent);
                        sevenDayGoalsPage.load(() =>
                        {
                            if (sevenDayGoalsPage == null)
                                return;

                            sevenDayGoalsPage.refreshWnd(NPPlayer.instance.sevenDayGoalsComp.data.getCanGetRewardDay());
                            sevenDayGoalsPage.showWnd();
                        });
                        _m_dActivityCenterWndDic[_m_curSelectRef.id] = sevenDayGoalsPage;
                    }
                    break;
                //========热更活动========
                case EActivityCenterTabType.HOTFIX_ACTIVITY:
                    if (targetPage != null)
                        targetPage?.showWnd();
                    else
                    {
                        targetPage = HotfixStaticFunc.dealLoadHotfixActivityCenterPage(_m_curSelectRef.id, wnd.pageParent);
                        if (targetPage != null)
                            _m_dActivityCenterWndDic[_m_curSelectRef.id] = targetPage;
                        else
                            Debug.LogError($"【活动中心】未正确加载对应HOTFIX_ACTIVITY热更活动窗口，activityCenterId：{_m_curSelectRef.id}");
                    }
                    break;
                case EActivityCenterTabType.STEP_TASK_RUSH_RANK_ACTIVITY:
                    if (targetPage != null)
                        targetPage?.showWnd();
                    else
                    {
                        long uiResId = GRefdataCoreMgr.instance.getActivityPrefabSkinUIResId(_m_curSelectRef.activity_id, UINodeTagConst.C_STEP_TASK_RUSH_RANK_ACTIVITY_MAIN);
                        NPCommonAssetPathInfo assetPathInfo = UIResPathAssistant.getAssetInfo(uiResId);
                        if(assetPathInfo == null)
                        {
                            Debug.LogError($"【活动中心】未正确获取对应阶段任务冲榜活动UI资源，activityCenterId：{_m_curSelectRef.id}, activityId：{_m_curSelectRef.activity_id}。" +
                                           $"请检查活动换皮表 或 ui_res_path表 中是否有对应资源配置");
                        }
                        else
                        {
                            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_curSelectRef.activity_id);
                            ActivityRankRushInfo rankRushInfo = activityInfo?.rankRushInfoList?.SafeGet(0);
                            if (rankRushInfo == null || rankRushInfo.rankRefObj == null)
                            {
                                Debug.LogError($"【活动中心】阶段任务冲榜活动数据异常，找不到活动对应的冲榜信息，activityCenterId：{_m_curSelectRef.id}, activityId：{_m_curSelectRef.activity_id}");
                                return;
                            }

                            switch (rankRushInfo.rankRefObj.rank_type)
                            {
                                case ERankType.PLAYER:
                                    targetPage = new GGUIWndPersonalStepTaskRushRankActivity(_m_curSelectRef.activity_id, assetPathInfo, wnd.pageParent);
                                    break;
                                
                                case ERankType.GUILD:
                                    targetPage = new GGUIWndGuildStepTaskRushRankActivity(_m_curSelectRef.activity_id, assetPathInfo, wnd.pageParent);
                                    break;
                                
                                default:
                                    targetPage = new GGUIWndPersonalStepTaskRushRankActivity(_m_curSelectRef.activity_id, assetPathInfo, wnd.pageParent);
                                    Debug.LogError($"【活动中心】阶段任务冲榜活动数据异常，未处理的排行类型，默认处理为个人排行榜，activityCenterId：{_m_curSelectRef.id}, activityId：{_m_curSelectRef.activity_id}, rankType:{rankRushInfo.rankRefObj.rank_type}");
                                    return;
                            }
                            
                            _m_dActivityCenterWndDic[_m_curSelectRef.id] = targetPage;
                            targetPage.load(() =>
                            {
                                if (targetPage == null)
                                    return;

                                targetPage.showWnd();
                            });
                        }
                    }
                    break;
                //========首次组队活动========
                case EActivityCenterTabType.FIRST_TEAM:
                    if (targetPage != null)
                        targetPage?.showWnd();
                    else
                    {
                        GGUIWndFirstTeamActivity firstTeamPage = new GGUIWndFirstTeamActivity(UIResPathAssistant.getAssetInfo(9200), wnd.pageParent);
                        firstTeamPage.load(() =>
                        {
                            if (firstTeamPage == null)
                                return;

                            firstTeamPage.showWnd();
                        });
                        _m_dActivityCenterWndDic[_m_curSelectRef.id] = firstTeamPage;
                    }
                    break;
                default:
                    Debug.LogError($"【活动中心】未正确加载对应活动窗口，activityCenterId：{_m_curSelectRef.id}");
                    break;
            }
        }

        /// <summary>
        /// 隐藏所有页面
        /// </summary>
        private void _hideAllPage()
        {
            foreach (KeyValuePair<long, _AALBasicLoadUIWndBasicClass> pair in _m_dActivityCenterWndDic)
            {
                _AALBasicLoadUIWndBasicClass curWnd = pair.Value;
                curWnd?.regLoadDoneDelegate(()=>
                {
                    curWnd?.hideWnd();
                });
            }
        }
    }
}