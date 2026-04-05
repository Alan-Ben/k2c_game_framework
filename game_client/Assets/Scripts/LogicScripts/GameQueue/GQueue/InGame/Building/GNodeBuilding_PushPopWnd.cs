using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    public partial class GNodeBuilding
    {
        private bool _m_bIsShowingPushPopWnd;//是否正在显示推送弹窗
        private Action _m_aOnAllPushPopWndDone;//所有推送弹窗处理完成回调
        
        protected internal override void _doNoticeCheckDone(Action _allNodePreDone)
        {
            if(_allNodePreDone != null)
                _m_aOnAllPushPopWndDone += _allNodePreDone;
            
            if(_m_bIsShowingPushPopWnd)
                return;

            _m_bIsShowingPushPopWnd = true;
            
            Action onNoticeShowDone = () =>
            {
                _showPushPopWnd(() =>
                {
                    _m_bIsShowingPushPopWnd = false;
                    Action action = _m_aOnAllPushPopWndDone;
                    _m_aOnAllPushPopWndDone = null;
                    action?.Invoke();
                });
            };

            // if (!NPUINoticeMgr.instance.isOpenOpMasking)
            // {
                onNoticeShowDone();
            // }
            // else
            // {
            //     NPUINoticeMgr.instance.addDoneDelegate(onNoticeShowDone);
            // }
        }

        private void _showPushPopWnd(Action _showDone)
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess(_showFirstRechargePopWnd)
                .addDelegateProcess(_showNationalPowerTargetPopWnd)
                .addDelegateProcess(_showSevenDayLoginPopWnd)
                .addDelegateProcess(_showSevenDayGoalsPopWnd)
                .addProcess(() =>
                {
                    _showDone?.Invoke();
                });
            
            process.deal();
        }

        /// <summary>
        /// 显示七日登录奖励弹窗
        /// </summary>
        /// <param name="_showDone"></param>
        private void _showSevenDayLoginPopWnd(Action _showDone)
        {
            if ((AccountSettingMgr.instance.accountSetting?.isTodayShowedSevenDayLoginMainCityPopWnd() ?? false) || !GCommon.isSimpleUnlock(SevenDayLoginComponent.SYSTEM_SIMPLE_UNLOCK_ID))
            {
                _showDone?.Invoke();
                return;
            }
            
            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndSevenDayLogin.instance, () =>
            {
                AccountSettingMgr.instance.accountSetting?.setSevenDayLoginMainCityPopWndShowDate();
                _showDone?.Invoke();
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_SEVEN_DAY_LOGIN));
        }

        private CustomMonoCommonTargetRewardEntry _m_nationalPowerTargetPopWndEntry;
        private bool _m_bHasTryGetNationalPowerTargetPopWndEntry = false;
        
        /// <summary>
        /// 显示国力目标弹窗
        /// </summary>
        private void _showNationalPowerTargetPopWnd(Action _showDone)
        {
            if((AccountSettingMgr.instance.accountSetting?.isTodayShowedNationalPowerTargetPopWnd() ?? false) || 
               !GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.national_power_target_simple_unlock_id))
            {
                _showDone?.Invoke();
                return;
            }

            // 还未尝试获取CustomMonoCommonTargetRewardEntry, 则尝试获取
            if (!_m_bHasTryGetNationalPowerTargetPopWndEntry)
            {
                _m_bHasTryGetNationalPowerTargetPopWndEntry = true;
                if (GGUIWndBuildingMain.instance.wnd != null)
                {
                    _m_nationalPowerTargetPopWndEntry = GGUIWndBuildingMain.instance.wnd.GetComponentInChildren<CustomMonoCommonTargetRewardEntry>(false);
                }
            }
            
            // 已经有CustomMonoCommonTargetRewardEntry, 直接打开弹窗
            if (_m_nationalPowerTargetPopWndEntry != null)
            {
                _m_nationalPowerTargetPopWndEntry.openTargetRewardWnd(() =>
                {
                    AccountSettingMgr.instance.accountSetting?.setNationalPowerTargetPopWndShowDate();
                    _showDone?.Invoke();
                });
            }
            else
            {
                _showDone?.Invoke();
            }
        }
        
        /// <summary>
        /// 展示首充礼包弹窗
        /// </summary>
        /// <param name="_showDone"></param>
        private void _showFirstRechargePopWnd(Action _showDone)
        {
            // 今天已显示过或未解锁，直接返回
            if ((AccountSettingMgr.instance.accountSetting?.isTodayShowedFirstRechargeMainCityPopWnd() ?? false) || 
                !GCommon.isSimpleUnlock(GNodeFirstRecharge.FIRST_RECHARGE_SIMPLE_UNLOCK_ID))
            {
                _showDone?.Invoke();
                return;
            }
            
            QueueMgr.instance.AddNode(new GNodeFirstRecharge(() =>
            {
                AccountSettingMgr.instance.accountSetting?.setFirstRechargeMainCityPopWndShowDate();
                _showDone?.Invoke();
            }));
        }

        /// <summary>
        /// 展示七日目标推送弹窗
        /// </summary>
        /// <param name="_showDone"></param>
        private void _showSevenDayGoalsPopWnd(Action _showDone)
        {
            if ((!AccountSettingMgr.instance.dailyTagSaver?.isNewDay(DailyTagConst.SEVEN_DAY_GOAL_PUSH_NOTICE) ?? false) || !GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.seven_day_goals_push_notice_simple_unlock_id))
            {
                _showDone?.Invoke();
                return;
            }

            //活动是否打开
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByType(ECommonActivityType.SEVEN_DAY_GOALS);
            if(activityInfo == null || !activityInfo.isEnable )
            {
                _showDone?.Invoke();
                return;
            }

            //打开界面
            QueueMgr.instance.AddNode(new GNodeEffectCustomAddUI(true, 5803, () =>
            {
                //设置今天已打开
                AccountSettingMgr.instance.dailyTagSaver.setSaveToday(DailyTagConst.SEVEN_DAY_GOAL_PUSH_NOTICE);
                _showDone?.Invoke();
            }));
        }
    }
}