using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using GC2GS.p034_InnOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public partial class InnViewMgr : _AALBasicLoadObj
    {
        [NotNull] private readonly InnCashRegisterView _m_cashRegister;
        [NotNull] private readonly InnGuestDataMgr _m_guestDataMgr;
        [NotNull] private readonly NormalState _m_normalState;
        private int _m_clickSerialize;


        public InnViewMgr()
        {
            _m_cashRegister = new InnCashRegisterView(this);
            _m_guestDataMgr = new InnGuestDataMgr();
            _m_normalState = new NormalState(this);
        }
        

        public long guestNum { get { return _m_guestDataMgr.totalGuestNum; } }
        
        [NotNull] internal InnGuestDataMgr guestDataMgr { get { return _m_guestDataMgr; } }
        [NotNull] internal InnCashRegisterView cashRegister { get { return _m_cashRegister; } }


        protected override void _loadOp()
        {   
            _m_guestDataMgr.init();
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2 + 1);
            stepCounter.regAllDoneDelegate(_onInitDone);
            {
                _m_normalState.enter(stepCounter.addDoneStepCount);
                _m_cashRegister.load(stepCounter.addDoneStepCount);
            }
            stepCounter.addDoneStepCount();
        }
        protected void _onInitDone()
        {
            _setLoadDone();

            GGUIWndInnMain.instance.refreshWnd(this);
            refreshHadSettledGuestNum();
            
            WinMsg.SendMsg(WinMsgType.ON_INN_VIEW_MGR_INIT);
        }
        public void preDiscard()
        {
            _m_normalState.preExit();
        }
        protected override void _discard()
        {
            _m_clickSerialize = ALSerializeOpMgr.next();
            _m_normalState.exit();
            _m_cashRegister.discard();
            _m_guestDataMgr.discard();
        }
        

        public void createGuest(int _guestNum)
        {
            if (!GCommon.isItemEnough(ENPItemType.LAZY_CD, GRefdataCoreMgr.instance.npGeneral.inn_receive_guest_lazy_cd_id, 1, true))
                return;

            long personNum = _m_guestDataMgr.totalGuestNum;
            NPGSClientListener.sendRequestByLog(new GC2GS_034_001_ReqInnReceiveGuest(_guestNum > 1), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    refreshGuestNum();
                    long newPersonNum = _m_guestDataMgr.totalGuestNum;
                    if (newPersonNum > personNum)
                    {
                        GGUIWndInnMain.instance.createGuestWnd?.playPersonAddEffect(newPersonNum - personNum);
                    }
                }));
        }
        public void refreshGuestNum()
        {
            GGUIWndInnMain.instance.createGuestWnd?.refreshPersonNum();
        }
        public long getHadSettleGuestsCount()
        {
            return _m_guestDataMgr.totalSettledGuestNum;
        }
        public void refreshHadSettledGuestNum()
        {
            // 刷新当前收银台积攒的奖励数量
            _m_cashRegister.setRewardNum(_m_guestDataMgr.rewardCount);
            WinMsg.SendMsg(WinMsgType.ON_INN_VIEW_GUEST_SETTLED);
        }


        internal void dealCashRegisterClick()
        {
            InnLevelRefObj oriLevelRef = NPPlayer.instance.innComp.levelRef;
            int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            int clickSerialize = _m_clickSerialize;
            _m_guestDataMgr.getSettleReward((_isSuc, _msg) =>
            {
                if (!_isSuc)
                {
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                    return;
                }

                _m_cashRegister.setRewardNum(_m_guestDataMgr.rewardCount);
                float collectAnimBeginTime = Time.time;
                ALProcess.CreateProcess()
                    .addProcess(() => _m_cashRegister.setRewardCollecting(true))
                    .addProcess(_m_cashRegister.playCollectAnim) // 播放火箭发射
                    .addProcess(() => MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize)) // 允许点击
                    .addDelegateProcess(_complete =>
                    {
                        if (clickSerialize != _m_clickSerialize)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        QueueMgr.instance.AddNode(new GNodeInnGetCashRegisterReward(_msg.getSettleInfo(), () =>
                        {
                            _complete?.Invoke();
                        }));
                    }) // 播放结算弹窗
                    .addDelegateProcess(_complete =>
                    {
                        if (clickSerialize != _m_clickSerialize)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        if (oriLevelRef != null && oriLevelRef != NPPlayer.instance.innComp.levelRef)
                            GCommon.enterDialogueNode(oriLevelRef.upgrade_dialogue_id, _complete);
                        else
                            _complete?.Invoke();
                    }) // 如果升级了，播放升级对话
                    .addDelegateProcess(_complete =>
                    {
                        if (clickSerialize != _m_clickSerialize)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        if (oriLevelRef != NPPlayer.instance.innComp.levelRef)
                        {
                            GGUIWndInnLevelUpSuccess.instance.refreshWnd(NPPlayer.instance.innComp.levelRef);
                            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndInnLevelUpSuccess.instance, _complete, EUIQueueStageType.MAIN, UINodeTagConst.C_INN_LEVEL_UP_SUCCESS, false, false));
                        }
                        else
                            _complete?.Invoke();
                    }) // 如果升级了，播放升级弹窗
                    .addDelegateProcess(_complete =>
                    {
                        if (clickSerialize != _m_clickSerialize)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        if (oriLevelRef?.inn_cash_register_index != null && !oriLevelRef.inn_cash_register_index.Equals(NPPlayer.instance.innComp.levelRef?.inn_cash_register_index))
                        {
                            GGUIWndInnCashRegisterUpgradeTip.instance.refreshWnd(NPPlayer.instance.innComp.levelRef);
                            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndInnCashRegisterUpgradeTip.instance, _complete, EUIQueueStageType.MAIN, UINodeTagConst.C_INN_CASH_REGISTER_UPGRADE_TIP, false, false));
                        }
                        else
                            _complete?.Invoke();
                    }) // 如果要换火箭模型，播放火箭变化提示
                    .addProcess(() =>
                    {
                        if (clickSerialize != _m_clickSerialize)
                            return;
                        
                        WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.INN_CASH_REGISTER_COLLECT_REWARD_DONE);
                    })
                    .addDelegateProcess(_complete =>
                    {
                        if (_m_cashRegister.mono == null || clickSerialize != _m_clickSerialize)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        float delayTime = _m_cashRegister.mono.collectAnimTime - (Time.time - collectAnimBeginTime);
                        delayTime = Mathf.Max(0, delayTime);
                        ALCommonActionMonoTask.addMonoTask(_complete, delayTime);
                    }) // 等待火箭发射完毕
                    .addDelegateProcess(_complete =>
                    {
                        if (clickSerialize != _m_clickSerialize)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        if (oriLevelRef?.inn_cash_register_index != null && !oriLevelRef.inn_cash_register_index.Equals(NPPlayer.instance.innComp.levelRef?.inn_cash_register_index))
                        {
                            _m_cashRegister.discard();
                            _m_cashRegister.load(_complete);
                        }
                        else
                            _complete?.Invoke();
                    }) // 如果要换火箭模型，就替换模型
                    .addDelegateProcess(_complete =>
                    {
                        if (clickSerialize != _m_clickSerialize)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        _m_cashRegister.playRecoverAnim(_complete);
                    }) // 播放火箭恢复的动画
                    .addProcess(() => _m_cashRegister.setRewardCollecting(false))
                    .deal();
            });
        }
    }
}