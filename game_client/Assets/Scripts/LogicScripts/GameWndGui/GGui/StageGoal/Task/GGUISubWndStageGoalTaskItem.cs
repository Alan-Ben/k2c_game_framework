using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndStageGoalTaskItem : _ATALBasicUISubWnd<GGUIMonoStageGoalTaskItem>
    {
        //目标图片
        private NPGGuiWndTexture _m_goalImgWnd;
        private NPGGuiWndTexture _m_goalImg2Wnd;
        private NPGGuiWndTexture _m_goalImg3Wnd;
        private NPGGuiWndTexture _m_goalImg4Wnd;
        //目标进度
        private NPGGUIWndProgress _m_goalProgressWnd;
        //目标数据
        private StageGoalTaskItem _m_taskItem;
        //引导手指
        private GGUIWndGuideHand _m_guideHand;
        //当点击前往按钮
        private readonly Action _m_onJumpBtnClick;
        //是否正在播放动画
        private bool _m_bIsPlayingAni;
        //显示序列号
        private long _m_lShowSerialize;


        public GGUISubWndStageGoalTaskItem(GGUIMonoStageGoalTaskItem _wnd, Action _onJumpBtnClick) 
            : base(_wnd)
        {
            _m_onJumpBtnClick = _onJumpBtnClick;
            
            initWnd();
        }
        
        
        public StageGoalTaskItem taskItem { get { return _m_taskItem; } }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_STAGE_GOAL_TASK_CHG, _onTaskChg);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_STAGE_GOAL_TASK_CHG, _onTaskChg);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            hideHandGuide();
            _m_bIsPlayingAni = false;
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (null != _m_goalImgWnd)
                _m_goalImgWnd.discard();
            _m_goalImgWnd = null;
            if (null != _m_goalImg2Wnd)
                _m_goalImg2Wnd.discard();
            _m_goalImg2Wnd = null;
            if (null != _m_goalImg3Wnd)
                _m_goalImg3Wnd.discard();
            _m_goalImg3Wnd = null;
            if (null != _m_goalImg4Wnd)
                _m_goalImg4Wnd.discard();
            _m_goalImg4Wnd = null;
            if (null != _m_goalProgressWnd)
                _m_goalProgressWnd.discard();
            _m_goalProgressWnd = null;

            if (null != wnd)
            {
                ALUGUICommon.uncombineBtnClick(wnd.goToBtn, _goToBtnDidClick);
                ALUGUICommon.uncombineBtnClick(wnd.getRewardBtn, _getRewardBtnDidClick);//点击领取、预览奖励
            }
        }
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.goalImg)
                _m_goalImgWnd = new NPGGuiWndTexture(wnd.goalImg);
            
            if (null != wnd.goalImg2)
                _m_goalImg2Wnd = new NPGGuiWndTexture(wnd.goalImg2);

            if (null != wnd.goalImg3)
                _m_goalImg3Wnd = new NPGGuiWndTexture(wnd.goalImg3);

            if (null != wnd.goalImg4)
                _m_goalImg4Wnd = new NPGGuiWndTexture(wnd.goalImg4);

            if (null != wnd.goalProgressMono)
                _m_goalProgressWnd = new NPGGUIWndProgress(wnd.goalProgressMono);

            ALUGUICommon.combineBtnClick(wnd.goToBtn, _goToBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.getRewardBtn, _getRewardBtnDidClick);
        }


        public void setItem(StageGoalTaskItem _taskItem)
        {
            if (null == _taskItem || null == _taskItem.refObj)
                return;

            _m_taskItem = _taskItem;
            _refresh();
        }
        public void showHandGuide()
        {
            if (wnd == null)
                return;

            if (wnd.guideTarget == null)
                return;

            _m_guideHand?.discard();
            _m_guideHand = new GGUIWndGuideHand(wnd.handGuideResId);
            _m_guideHand.load(_m_guideHand.showWnd);
            _m_guideHand.refreshWnd(wnd.guideTarget);
        }
        public void hideHandGuide()
        {
            _m_guideHand?.discard();
            _m_guideHand = null;
        }


        protected virtual void _refresh()
        {
            if (null == wnd || null == _m_taskItem || null == _m_taskItem.refObj)
                return;

            if (null != _m_goalImgWnd)
                _m_goalImgWnd.setTexture(_m_taskItem.refObj.icon);
            
            if (null != _m_goalImg2Wnd)
                _m_goalImg2Wnd.setTexture(_m_taskItem.refObj.icon);
            
            if (null != _m_goalImg3Wnd)
                _m_goalImg3Wnd.setTexture(_m_taskItem.refObj.icon);
            
            if (null != _m_goalImg4Wnd)
                _m_goalImg4Wnd.setTexture(_m_taskItem.refObj.icon);

            if (null != _m_goalProgressWnd)
            {
                _m_goalProgressWnd.setProgress(_m_taskItem.getCurProgress());
                _m_goalProgressWnd.setProgressTxt(_m_taskItem.getCurProgressFormat(wnd.completeCurProcessColor, wnd.notCompleteCurProcessColor), _m_taskItem.isCompleted());
            }

            //设置领奖状态
            EStageGoalTaskItemState rewardState = _m_taskItem.getCurRewardType();
            NPCommonEnumStatInfo<EStageGoalTaskItemState>.setStat(wnd.getRewardStateList, rewardState);

            //设置标题文本
            string titleStr = _m_taskItem.refObj.getTitle;
            titleStr = GCommon.addColorForRichText(titleStr, rewardState == EStageGoalTaskItemState.HAS_GET ? wnd.alreadyGetRewardTitleColor : wnd.notGetRewardTitleColor);
            ALUGUICommon.setLabelTxt(wnd.goalTitleTxt, titleStr);

            //播放连线状态动画
            //加到laterUpdate是为了防止同一帧频繁显隐打断动画
            ALCommonActionMonoTask.addLaterMonoTask(_playLineAni);
        }

        //播放连线状态动画
        private void _playLineAni()
        {
            if (_m_bIsPlayingAni)
                return;

            if (_m_taskItem == null)
                return;

            EStageGoalTaskItemState rewardState = _m_taskItem.getCurRewardType();

            EStageGoalTaskItemCompleteShowState lineShowState = EStageGoalTaskItemCompleteShowState.UNDONE;
            switch (rewardState)
            {
                case EStageGoalTaskItemState.UNLOCK:
                case EStageGoalTaskItemState.CAN_NOT_GET:
                    lineShowState = EStageGoalTaskItemCompleteShowState.UNDONE;
                    break;
                case EStageGoalTaskItemState.CAN_GET:
                case EStageGoalTaskItemState.HAS_GET:
                    StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
                    if (stageRefObj == null)
                    {
                        lineShowState = EStageGoalTaskItemCompleteShowState.DONE;
                        break;
                    }
                    bool isNewCompleteTask = AccountSettingMgr.instance.accountSetting.isNewCompletedStageGoalTask(stageRefObj.step, _m_taskItem.taskId);
                    if (isNewCompleteTask)
                    {
                        lineShowState = EStageGoalTaskItemCompleteShowState.SHOW_DONE;
                        //下一帧记录新完成的任务
                        ALCommonActionMonoTask.addNextFrameLaterTask(() =>
                        {
                            //如果不在展示中，则说明一帧界面就隐藏了，这时不处理
                            if (!isShow)
                                return;

                            AccountSettingMgr.instance.accountSetting.setNewCompletedStageGoalTask(stageRefObj.step, _m_taskItem.taskId);
                        });
                    }
                    else
                        lineShowState = EStageGoalTaskItemCompleteShowState.DONE;
                    break;
            }
            _m_bIsPlayingAni = true;
            _m_lShowSerialize = ALSerializeOpMgr.next();
            long showSerialize = _m_lShowSerialize;
            wnd?.stateAniInfo?.forcePlay(lineShowState, () =>
            {
                if (showSerialize != _m_lShowSerialize)
                    return;

                _m_bIsPlayingAni = false;
            });
        }
        protected virtual void _goToBtnDidClick(GameObject _go)
        {
            if (null == _m_taskItem || null == _m_taskItem.refObj)
                return;

            if (_m_taskItem.getCurRewardType() == EStageGoalTaskItemState.CAN_GET || _m_taskItem.getCurRewardType() == EStageGoalTaskItemState.HAS_GET)
                return;
            
            _m_taskItem.refObj.go_to.dealEffect();
            _m_onJumpBtnClick?.Invoke();
        }
        protected virtual void _getRewardBtnDidClick(GameObject _go)
        {
            if (null == _m_taskItem || null == _m_taskItem.refObj)
                return;

            //隐藏手指引导
            hideHandGuide();

            switch (_m_taskItem.getCurRewardType())
            {
                case EStageGoalTaskItemState.UNLOCK:
                    GCommon.isSimpleUnlock(_m_taskItem.refObj.simple_unlock_id, true);
                    break;
                case EStageGoalTaskItemState.CAN_GET:
                    NPPlayer.instance.stageGoalComp.reqTakeStageGoalSubTaskReward(_m_taskItem.taskId, _msg =>
                    {
                        if (_msg == null)
                            return;

                        GCommon.showGainRewardTip(_msg.getItemList());
                        _refresh();
                    });
                    break;
                case EStageGoalTaskItemState.CAN_NOT_GET:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndStageGoalTaskInfo.instance, () =>
                    {
                        GGUIWndStageGoalTaskInfo.instance.showWnd();
                        GGUIWndStageGoalTaskInfo.instance.setInfo(_m_taskItem);
                    },UINodeTagConst.C_STAGE_GOAL_TASK_INFO);
                    break;
                case EStageGoalTaskItemState.HAS_GET:
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.stage_goal_taskDone_none);
                    break;
            }
        }

        // 任务变化
        private void _onTaskChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long taskId = (long)_objects[0];
            if (null == _m_taskItem || _m_taskItem.taskId != taskId)
                return;

            _refresh();
        }
    }
}