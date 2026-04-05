using System;
using ALPackage;
using Common.PlayerEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndStageGoalOverviewGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoStageGoalOverviewGridItem>
    {
        private readonly Action _m_jumpToFunc;
        
        private StageGoalBigStepRefObj _m_refObj;
        private bool _m_isFirst;
        private bool _m_isLast;
        private bool _m_isPrivate;
        private long _m_lPrivateNum;
        
        private NPGGuiWndTexture _m_texWnd;
        private GGUISubWndStageGoalOverviewSmallStepPointContainer _m_smallStepPointContainer;
        private GGUIWndCommonRewardContainer _m_rewardContainer;
        private NPGGUIWndCommonItemContainer _m_unlockFunctionContainer;
        
        /// <summary>
        /// 大阶段数据
        /// </summary>
        public StageGoalBigStepRefObj refObj { get { return _m_refObj; } }
        
        public GGUISubWndStageGoalOverviewGridItem(GGUIMonoStageGoalOverviewGridItem _wnd, Action _jumpToFunc) 
            : base(_wnd)
        {
            _m_jumpToFunc = _jumpToFunc;
            
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_GET_STAGE_GOAL_BIG_STEP_REWARD_BY_ID, _onSimulateClickGetReward);
            _m_texWnd?.showWnd();
            _m_smallStepPointContainer?.showWnd();
            _m_rewardContainer?.showWnd();
            _m_unlockFunctionContainer?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_GET_STAGE_GOAL_BIG_STEP_REWARD_BY_ID, _onSimulateClickGetReward);
            _m_texWnd?.hideWnd();
            _m_smallStepPointContainer?.hideWnd();
            _m_rewardContainer?.hideWnd();
            _m_unlockFunctionContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_texWnd?.discardTexture();
            _m_smallStepPointContainer?.resetWnd();
            _m_rewardContainer?.resetWnd();
            _m_unlockFunctionContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_texWnd?.discard();
            _m_smallStepPointContainer?.discard();
            _m_rewardContainer?.discard();
            _m_unlockFunctionContainer?.discard();
            _m_texWnd = null;
            _m_smallStepPointContainer = null;
            _m_rewardContainer = null;
            _m_unlockFunctionContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnPlayDialogue, _onBtnPlayDialogue);
            ALUGUICommon.uncombineBtnClick(wnd.btnJumpTo, _onBtnJumpTo);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onBtnGetRewardClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgTex != null)
                _m_texWnd = new NPGGuiWndTexture(wnd.imgTex);
            if (wnd.monoSmallStepPointContainer != null)
                _m_smallStepPointContainer = new GGUISubWndStageGoalOverviewSmallStepPointContainer(wnd.monoSmallStepPointContainer);
            if (wnd.monoRewardContainer != null)
                _m_rewardContainer = new GGUIWndCommonRewardContainer(wnd.monoRewardContainer);
            if (wnd.monoUnlockContainer != null)
                _m_unlockFunctionContainer = new NPGGUIWndCommonItemContainer(wnd.monoUnlockContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnPlayDialogue, _onBtnPlayDialogue);
            ALUGUICommon.combineBtnClick(wnd.btnJumpTo, _onBtnJumpTo);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onBtnGetRewardClick);
        }
        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="_refObj"></param>
        /// <param name="_isFirst"></param>
        /// <param name="_isLast"></param>
        /// <param name="_isPrivate"></param>
        /// <param name="_privateNum"></param>
        public void refreshWnd(StageGoalBigStepRefObj _refObj, bool _isFirst, bool _isLast, bool _isPrivate, long _privateNum)
        {
            _m_refObj = _refObj;
            _m_isFirst = _isFirst;
            _m_isLast = _isLast;
            _m_isPrivate = _isPrivate;
            _m_lPrivateNum = _privateNum;
            wnd?.aniUnlock?.resetAni();
            wnd?.aniFinish?.resetAni();

            refreshWnd();
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            long itemStep = _m_refObj?.begins_from_small_step ?? 0;
            long currentStep = NPPlayer.instance.stageGoalComp.bigStepRefObj.begins_from_small_step;
            bool isAllDone = NPPlayer.instance.stageGoalComp.isAllDone;
            ECommonRewardType getRewardType = NPPlayer.instance.stageGoalComp.bigStepRewardState(_m_refObj?.big_step ?? 0);
            wnd.setState(itemStep, currentStep, isAllDone, _m_isPrivate, getRewardType);
            wnd.setIsFirst(_m_isFirst, _m_isLast);
            wnd.setDialogue(_m_refObj?.dialogue_id > 0);

            float progress;
            if (_m_isPrivate)
                progress = 0;
            else if (isAllDone || itemStep < currentStep)
                progress = 1;
            else if (itemStep > currentStep)
                progress = 0;
            else
                progress = NPPlayer.instance.stageGoalComp.getStageGoalBigProgress();
            
            if (wnd.sldProgress != null)
            {
                wnd.sldProgress.minValue = (_m_refObj?.begins_from_small_step ?? 0) - 1;
                wnd.sldProgress.maxValue = _m_refObj?.end_small_step ?? 0;
                wnd.sldProgress.normalizedValue = progress;
            }
            if (wnd.sldProgress2 != null)
            {
                wnd.sldProgress2.minValue = (_m_refObj?.begins_from_small_step ?? 0) - 1;
                wnd.sldProgress2.maxValue = _m_refObj?.end_small_step ?? 0;
                wnd.sldProgress2.normalizedValue = progress;
            }
            ALUGUICommon.setLabelTxt(wnd.txtProgress2, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, progress * 100f));
            _m_smallStepPointContainer?.refreshWnd(_m_refObj, NPPlayer.instance.stageGoalComp.curStageStepId);

            ALUGUICommon.setLabelTxt(wnd.txtTaskNum, string.Format("{0:D2}", _m_refObj?.big_step ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _m_refObj?.getTitle ?? string.Empty);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_refObj?.getDesc ?? string.Empty);
            ALUGUICommon.setLabelTxt(wnd.txtPrivateNum, string.Format("{0:D2}", _m_lPrivateNum));

            if (_m_refObj != null)
            {
                long time = NPPlayer.instance.eventRecordComp.getValue(EPlayerEventRecordType.STAGE_GOAL_FINISH_TIME_MS, _m_refObj.begins_from_small_step);
                ALUGUICommon.setLabelTxt(wnd.txtCompletedTime, TimeUtil.Milliseconds2StringYMD(time));
            }
            else
                ALUGUICommon.setLabelTxt(wnd.txtCompletedTime, string.Empty);

            _m_texWnd?.setTexture(_m_refObj?.icon);
            _m_rewardContainer?.setRewardList(_m_refObj?.reward_item_list, NPPlayer.instance.stageGoalComp.bigStepRewardState(_m_refObj?.big_step ?? 0));
            _m_unlockFunctionContainer?.showItemList(_m_refObj?.func_unlock_item_list);

            //需要先领取小阶段奖励的提示
            StageGoalBigStepRefObj curBigStepRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            bool isGetSmallStageRewardFirst = curBigStepRefObj != null && 
                                              curBigStepRefObj.big_step == _m_refObj?.big_step && 
                                              _m_refObj.done_need_draw_all_step_reward && 
                                              NPPlayer.instance.stageGoalComp.subStepCanGetReward();
            ALUGUICommon.setGameObjEnable(wnd.goNeedGetSmallStageRewardFirstTip, isGetSmallStageRewardFirst);
        }

        /// <summary>
        /// 播放解锁动画
        /// </summary>
        /// <param name="_onDone"></param>
        public void playUnlockAni(Action _onDone)
        {
            if (wnd == null || !isShow)
            {
                _onDone?.Invoke();
                return;
            }

            wnd.aniUnlock?.forcePlay(_onDone);
            // 记录已播放
            AccountSettingMgr.instance.accountSetting.setAlreadyShowUnlockAniBigStageGoalId(_m_refObj?.big_step ?? 0);
        }

        /// <summary>
        /// 设置解锁动画进度
        /// </summary>
        /// <param name="_normalizeTime"></param>
        public void sampleUnlockAni(float _normalizeTime)
        {
            if (wnd == null || !isShow)
                return;

            wnd.aniUnlock?.sample(_normalizeTime);
        }

        /// <summary>
        /// 播放完成动画
        /// </summary>
        /// <param name="_onDone"></param>
        public void playFinishAni(Action _onDone)
        {
            if (wnd == null || !isShow)
            {
                _onDone?.Invoke();
                return;
            }

            wnd.aniFinish?.forcePlay(_onDone);
            // 记录已播放
            AccountSettingMgr.instance.accountSetting.setAlreadyShowFinishAniBigStageGoalId(_m_refObj?.big_step ?? 0);
        }

        private void _onBtnPlayDialogue(GameObject _)
        {
            if (_m_refObj == null)
                return;

            GCommon.enterDialogueNode(_m_refObj.dialogue_id, null);
        }
        private void _onBtnJumpTo(GameObject _) 
        {
            _m_jumpToFunc?.Invoke();
        }
        private void _onBtnGetRewardClick(GameObject _)
        {
            if (_m_refObj == null)
                return;

            if (!NPPlayer.instance.stageGoalComp.bigGoalCanGetReward(_m_refObj.big_step))
                return;

            //当前小阶段
            StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            StageGoalBigStepRefObj curBigStepRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            NPPlayer.instance.stageGoalComp.reqTakeBigStepReward(_m_refObj.big_step, _msg =>
            {
                //如果领取的是当前大阶段，走解锁新阶段流程
                if (curBigStepRefObj != null && curBigStepRefObj.big_step == _m_refObj.big_step)
                    GCommon.dealStageGoalGetRewardProcess(_msg.getItemList(), stageRefObj);
                else
                    GCommon.dealGainItem(_msg.getItemList());
            });
        }

        //模拟点击获取奖励
        private void _onSimulateClickGetReward(params object[] _objects)
        {
            if (_m_refObj == null || _objects == null || _objects.Length < 1 || _objects[0] == null)
                return;

            long bigStepId = (long)_objects[0];
            if (_m_refObj.big_step == bigStepId)
                _onBtnGetRewardClick(null);
        }
    }
}