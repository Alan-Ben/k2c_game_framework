using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndStageGoalPageTask : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoStageGoalPageTask>
    {
        //阶段图片
        private NPGGuiWndTexture _m_stageImgWnd;
        //背景图
        private NPGGuiWndTexture _m_stageBgWnd;
        //这个阶段完成后解锁的内容列表
        private NPGGUIWndCommonItemContainer _m_unlockContainerWnd;
        private NPGGUIWndCommonItemContainer _m_rewardItemContainerWnd;
        private GGUIPrefabSubWndStageGoalTask _m_taskWnd;
        //任务进度
        private NPGGUIWndProgress _m_wTaskProgress;
        //操作序列号
        private long _m_lOpSerialize;
        //是否播放过首次显示动画
        private bool _m_bIsShowFirstAni;
        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;


        public GGUIPrefabSubWndStageGoalPageTask(Transform _parent) : base(_parent)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoStageGoalPageTask.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalPageTask.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        /// <summary>
        /// show 动画是否只播放一次
        /// </summary>
        protected override bool isShowAniPlayOnlyOne { get { return true; } }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_STAGE_GOAL_MAIN_TASK_GAIN_REWARD, _onBtnGetRewardClick);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onParamChg);
            _m_lOpSerialize = ALSerializeOpMgr.next();
            //刷新任务进度值
            _refreshTaskProgress();

            _m_stageImgWnd?.showWnd();
            _m_unlockContainerWnd?.showWnd();
            _m_rewardItemContainerWnd?.showWnd();
            _m_taskWnd?.showWnd();
            _m_stageBgWnd?.showWnd();
            _m_wTaskProgress?.showWnd();

            //刷新界面
            _refreshWnd();
            //设置子任务红点已读
            _setReadTaskRedTip();
            //检查是否需要播放首次显示动画
            _checkPlayFirstShowAni();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_STAGE_GOAL_MAIN_TASK_GAIN_REWARD, _onBtnGetRewardClick);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onParamChg);
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _m_stageImgWnd?.hideWnd();
            _m_unlockContainerWnd?.hideWnd();
            _m_rewardItemContainerWnd?.hideWnd();
            _m_taskWnd?.hideWnd();
            _m_stageBgWnd?.hideWnd();
            _m_wTaskProgress?.hideWnd();
            _m_iTickTask.setDisable();
        }
        protected override void _onReset()
        {
            _m_stageImgWnd?.discardTexture();
            _m_unlockContainerWnd?.resetWnd();
            _m_rewardItemContainerWnd?.resetWnd();
            _m_taskWnd?.resetWnd();
            _m_stageBgWnd?.discardTexture();
            _m_wTaskProgress?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_stageImgWnd?.discard();
            _m_stageImgWnd = null;
            _m_unlockContainerWnd?.discard();
            _m_unlockContainerWnd = null;
            _m_rewardItemContainerWnd?.discard();
            _m_rewardItemContainerWnd = null;
            _m_taskWnd?.discard();
            _m_taskWnd = null;
            _m_stageBgWnd?.discard();
            _m_stageBgWnd = null;
            _m_wTaskProgress?.discard();
            _m_wTaskProgress = null;
            _m_bIsShowFirstAni = false;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onBtnGetRewardClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgStageIcon != null)
                _m_stageImgWnd = new NPGGuiWndTexture(wnd.imgStageIcon);
            if (wnd.unlockItemContainer != null)
                _m_unlockContainerWnd = new NPGGUIWndCommonItemContainer(wnd.unlockItemContainer);
            if (wnd.rewardItemContainer != null)
                _m_rewardItemContainerWnd = new NPGGUIWndCommonItemContainer(wnd.rewardItemContainer);
            if (wnd.imgStageGoalBg != null)
                _m_stageBgWnd = new NPGGuiWndTexture(wnd.imgStageGoalBg);
            if (wnd.monoTaskProgress != null)
                _m_wTaskProgress = new NPGGUIWndProgress(wnd.monoTaskProgress);

            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onBtnGetRewardClick);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _refreshStageGoalShow();
            _refreshGetRewardState();
            _refreshTaskSubWnd();
            _checkNeedUnlockCDTask();
        }

        /// <summary>
        /// 刷新当前阶段目标显示
        /// </summary>
        private void _refreshStageGoalShow()
        {
            if (wnd == null)
                return;

            //阶段相关显示
            StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            StageGoalRefObj nextStageRefObj = stageRefObj == null ? null : GRefdataCoreMgr.instance.stageGoalRefCore.getRef(stageRefObj.step + 1);
            StageGoalBigStepRefObj bigStepRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            int bigStepNum = 0;
            int bigStepFromSmallStep = 0;
            if (null != bigStepRefObj)
            {
                bigStepNum = bigStepRefObj.big_step;
                bigStepFromSmallStep = bigStepRefObj.begins_from_small_step;
                ALUGUICommon.setLabelTxt(wnd.stageBigNumAndTitleTxt, TextTranslate.instance.getLanguage(TransKeyConst.stage_goal_taskNumAndName_num_name, bigStepRefObj.big_step, bigStepRefObj.getTitle));
            }

            //当前阶段
            if (null != stageRefObj)
            {
                int smallStepNum = (int)stageRefObj.step - bigStepFromSmallStep + 1;
                ALUGUICommon.setLabelTxt(wnd.stageNumTxt, TextTranslate.instance.getLanguage(TransKeyConst.common_interval2_num_num, bigStepNum, smallStepNum));
                ALUGUICommon.setLabelTxt(wnd.stageTitleTxt, stageRefObj.getTitle);
                _m_stageImgWnd?.setTexture(stageRefObj.icon);
                ALUGUICommon.setGameObjEnable(wnd.listUnlockNothingHide, (stageRefObj.show_item_list?.Count ?? 0) > 0);
                _m_unlockContainerWnd?.showItemList(stageRefObj.show_item_list);
                _m_rewardItemContainerWnd?.showItemList(stageRefObj.reward_item_list);
                ALUGUICommon.setLabelTxt(wnd.btnGetRewardTxt, TextTranslate.instance.getLanguage(TransKeyConst.stage_goal_taskGetReward_num_num, bigStepNum, smallStepNum));
                ALUGUICommon.setLabelTxt(wnd.btnGetRewardTxt2, TextTranslate.instance.getLanguage(TransKeyConst.stage_goal_taskGetReward_num_num, bigStepNum, smallStepNum));
                //背景图
                _m_stageBgWnd?.setTexture(stageRefObj?.task_bg);
            }

            //下一阶段
            ALUGUICommon.setGameObjEnable(wnd.noNextStageHideList, nextStageRefObj != null);
            ALUGUICommon.setGameObjEnable(wnd.noNextStageShowList, nextStageRefObj == null);
            if (null != nextStageRefObj)
            {
                StageGoalBigStepRefObj nextBigStepRef = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj((int)nextStageRefObj.step);
                if (nextBigStepRef != null)
                {
                    int smallStepNum = (int)nextStageRefObj.step - nextBigStepRef.begins_from_small_step + 1;
                    ALUGUICommon.setLabelTxt(wnd.nextStageNumTxt, TextTranslate.instance.getLanguage(TransKeyConst.common_interval2_num_num, nextBigStepRef.big_step, smallStepNum));
                    ALUGUICommon.setLabelTxt(wnd.nextStageTitleTxt, nextStageRefObj.getTitle);
                }
            }
        }

        /// <summary>
        /// 刷新是否可领取奖励状态
        /// </summary>
        private void _refreshGetRewardState()
        {
            if (wnd == null)
                return;

            //重置显示
            ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, "");
            ALUGUICommon.setGameObjEnable(wnd.goNoServerDayCDHideList, false);
            ALUGUICommon.setGameObjEnable(wnd.goNoServerDayCDShowList, true);

            EStageGoalTaskRewardBtnState btnState = EStageGoalTaskRewardBtnState.CAN_NOT_GET_REWARD;
            StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            if (stageRefObj != null)
            {
                StageGoalRefObj nextStageRefObj = GRefdataCoreMgr.instance.stageGoalRefCore.getRef(stageRefObj.step + 1);
                bool isDayLock = stageRefObj.next_step_need_server_start_day > NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS);
                bool isConditionLock = stageRefObj.next_step_simple_unlock_id > 0 && !GCommon.isSimpleUnlock(stageRefObj.next_step_simple_unlock_id);
                bool isLock = isDayLock || isConditionLock;
                if (isLock)
                    btnState = EStageGoalTaskRewardBtnState.LOCK;
                else if(NPPlayer.instance.stageGoalComp.subStepCanGetReward())
                    btnState = EStageGoalTaskRewardBtnState.CAN_GET_REWARD;
                else
                    btnState = EStageGoalTaskRewardBtnState.CAN_NOT_GET_REWARD;

                //显示解锁条件描述
                if (isDayLock)
                {
                    ALUGUICommon.setGameObjEnable(wnd.goNoServerDayCDHideList, true);
                    ALUGUICommon.setGameObjEnable(wnd.goNoServerDayCDShowList, false);
                    ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, TextTranslate.instance.getLanguage(TransKeyConst.stage_goal_nextStegeNeedServerDayDesc_str_num, nextStageRefObj?.getTitle, stageRefObj.next_step_need_server_start_day));
                }
                else if(isConditionLock)
                {
                    ALUGUICommon.setGameObjEnable(wnd.goNoServerDayCDHideList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goNoServerDayCDShowList, true);
                    NPSimpleUnlockRef simpleUnlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(stageRefObj.next_step_simple_unlock_id);
                    ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, simpleUnlockRef?.getUnlockTip());
                    ALUGUICommon.setLabelTxt(wnd.txtUnclockCDDesc, "");
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.goNoServerDayCDHideList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goNoServerDayCDShowList, true);
                    ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, "");
                    ALUGUICommon.setLabelTxt(wnd.txtUnclockCDDesc, "");
                }
            }
            NPCommonEnumStatInfo<EStageGoalTaskRewardBtnState>.setStat(wnd.getBtnRewardState, btnState);
            wnd.setAllDone(NPPlayer.instance.stageGoalComp.isAllDone);
        }

        /// <summary>
        /// 刷新任务列表界面
        /// </summary>
        private void _refreshTaskSubWnd()
        {
            StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            if (stageRefObj == null)
                return;

            long curUIResId = stageRefObj?.task_ui_res_id ?? 0;
            if (_m_taskWnd == null || _m_taskWnd.uiResId != curUIResId)
            {
                _m_taskWnd?.discard();
                _m_taskWnd = new GGUIPrefabSubWndStageGoalTask(wnd.transTaskRoot, curUIResId);
                _m_taskWnd.load(() =>
                {
                    if (_m_bIsShow)
                        _m_taskWnd.showWnd();
                });
            }
            _m_taskWnd.refreshWnd();
        }

        /// <summary>
        /// 刷新任务进度值
        /// </summary>
        private void _refreshTaskProgress()
        {
            if (wnd == null)
                return;

            StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            if (stageRefObj == null)
                return;

            long lastValue = AccountSettingMgr.instance.accountSetting.getStageGoalRecordTaskCount(stageRefObj.step) * 100 / NPPlayer.instance.stageGoalComp.getSubStageGoalAvailableTaskCount() ;
            long curValue = NPPlayer.instance.stageGoalComp.getSubStageGoalTaskProgressPercent();

            //进度条
            _m_wTaskProgress?.setProgress(curValue / 100f);
            //任务完成进度
            ALUGUICommon.setLabelTxt(wnd.txtTaskProgress, TextTranslate.instance.getLanguage(TransKeyConst.stage_goal_subTaskProgressPercent_num, lastValue));
            //如果进度值不一致，需要有变化过程
            if(lastValue != curValue)
                _showTaskProgressValueLerp(lastValue, curValue);
        }

        /// <summary>
        /// 检查是否需要播放首次显示动画
        /// </summary>
        private void _checkPlayFirstShowAni()
        {
            if (_m_bIsShowFirstAni)
                wnd?.aniFirstShow?.sample(1);
            else
            {
                _m_bIsShowFirstAni = true;
                wnd?.aniFirstShow?.forcePlay();
            }
        }

        /// <summary>
        /// 一段时间内，任务进度值从开始到结束的变化过程
        /// </summary>
        /// <param name="_start"></param>
        /// <param name="_end"></param>
        private void _showTaskProgressValueLerp(long _start, long _end)
        {
            if (null == wnd || _start == _end)
                return;

            _m_lOpSerialize = ALSerializeOpMgr.next();
            NPMonoTaskLerpStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }
                , _m_lOpSerialize
                , _start
                , _end
                , Math.Abs(wnd.taskProgressChgTime) < 0.01f ? 0.5f : wnd.taskProgressChgTime
                , value =>
                {
                    ALUGUICommon.setLabelTxt(wnd.txtTaskProgress, TextTranslate.instance.getLanguage(TransKeyConst.stage_goal_subTaskProgressPercent_num, value));
                }
                , () => { return _m_lOpSerialize; }
                , null);
        }

        //设置任务非强制红点已读
        private void _setReadTaskRedTip()
        {
            //用下一帧避免节点在回退时同一帧显隐该窗口导致提前设置红点已读
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (!isShow)
                    return;

                //清空红点
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_STAGE_GOAL_NOW_TASK, 0);

                //设置子任务红点已读
                List<StageGoalTaskItem> taskList = new List<StageGoalTaskItem>();
                NPPlayer.instance.stageGoalComp.getSubStageGoalTaskList(taskList);
                foreach (StageGoalTaskItem taskItem in taskList)
                {
                    if (taskItem == null || taskItem.isReadRedTip)
                        continue;
                    //如果任务未完成，则不设置已读
                    if (!taskItem.isCompleted())
                        continue;
                    //设置已读
                    taskItem.isReadRedTip = true;
                }
            });
        }


        #region 解锁倒计时

        /// <summary>
        /// 检查是否需要开启解锁倒计时任务
        /// </summary>
        private void _checkNeedUnlockCDTask()
        {
            _m_iTickTask.setDisable();
            StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            if (stageRefObj == null)
                return;

            //当前阶段配置了需要服务器开服天数才能领取奖励，并且当前服务器开服天数不满足条件，则开启定时任务
            if (stageRefObj.next_step_need_server_start_day > 0 && stageRefObj.next_step_need_server_start_day > NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS))
                _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_cdTick, 0.2f);
        }

        /// <summary>
        /// 倒计时任务
        /// </summary>
        private void _cdTick()
        {
            if (wnd == null)
                return;

            long leftTimeMs = 0;
            StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            if (stageRefObj == null || 
                stageRefObj.next_step_need_server_start_day <= 0 || 
                stageRefObj.next_step_need_server_start_day <= NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS))
            {
                _m_iTickTask.setDisable();
                _refreshGetRewardState();
                return;
            }

            //间隔的天数
            long dayInterval = stageRefObj.next_step_need_server_start_day - NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS);
            //距离24点的毫秒数
            leftTimeMs = TimeUtil.getNextAssignTimeRemainMs(FpsAndPingMgr.instance.serverTimeTag, 24, 0);
            if (dayInterval > 1)
                leftTimeMs = leftTimeMs + (dayInterval - 1) * 24 * 60 * 60 * 1000;

            if (leftTimeMs < 0)
            {
                _m_iTickTask.setDisable();
                _refreshGetRewardState();
            }
            else
            {
                //刷新倒计时显示
                ALUGUICommon.setLabelTxt(wnd.txtUnclockCDDesc, TimeUtil.millisecondsToTime_hms(leftTimeMs, TransKeyConst.stage_goal_nextStageGoalUnlockCD_num_num_num));
            }
        }

        #endregion


        #region 点击事件

        /// <summary>
        /// 点击领取奖励
        /// </summary>
        private void _onBtnGetRewardClick()
        {
            if (wnd == null)
                return;
            
            _onBtnGetRewardClick(wnd.btnGetReward);
        }

        /// <summary>
        /// 点击领取奖励
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnGetRewardClick(GameObject _)
        {
            StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            if (!NPPlayer.instance.stageGoalComp.subStepCanGetReward())
            {
                bool showGuideHand = _m_taskWnd != null && _m_taskWnd.showGuideHand();
                bool needShowTip = !showGuideHand;

                //如果有配置simpleUnlockId且没有解锁，则提示解锁
                if (needShowTip && stageRefObj != null && stageRefObj.next_step_simple_unlock_id > 0)
                {
                    GCommon.isSimpleUnlock(stageRefObj.next_step_simple_unlock_id, true);
                }
                return;
            }
            
            _m_taskWnd?.hideHandGuide();

            NPPlayer.instance.stageGoalComp.reqTakeStageGoalTaskReward((_msg) =>
            {
                if(_m_taskWnd != null && _m_taskWnd.isShow)
                {
                    // 开启全屏遮罩
                    int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                    // 先播放任务完成动画
                    _m_taskWnd.playAllFinishAni(() =>
                    {
                        // 关闭全屏遮罩
                        MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                        GCommon.dealStageGoalGetRewardProcess(_msg.getItemList(), stageRefObj, () =>
                        {
                            if(wnd != null && isShow)
                                _refreshWnd();
                        });
                    });
                }
                else
                {
                    GCommon.dealStageGoalGetRewardProcess(_msg.getItemList(), stageRefObj, () =>
                    {
                        if (wnd != null && isShow)
                            _refreshWnd();
                    });
                }
            });
        }

        #endregion

        #region 消息事件

        //玩家参数变化
        private void _onParamChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            int paramIndex = (int)_objects[0];
            //如果是服务器开启天数变化，刷新领奖按钮展示
            if ((ENPPlayerParam) paramIndex == ENPPlayerParam.SERVER_START_DAYS)
            {
                _refreshGetRewardState();
                _checkNeedUnlockCDTask();
            }
        }

        #endregion
    }
}