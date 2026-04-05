using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 新倒计时事件展示
    /// </summary>
    public class NoticeDealer_NewCountdownEventShow : _AMainCityCanJumpPushNotice
    {
        private CountdownEventInfo _m_countdownEventInfo;
        private Action _m_aOnDealDone;
        
        public NoticeDealer_NewCountdownEventShow(CountdownEventInfo _countdownEventInfo, Action _onDealDone, EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
            _m_countdownEventInfo = _countdownEventInfo;
            _m_aOnDealDone = _onDealDone;
        }

        protected override bool _isEnable { get { return true; } }

        protected override bool _canCurShow { get { return true; } }
        protected override string _noticeTag { get { return NoticeTagConst.NEW_COUNT_DOWN_EVENT_SHOW; } }

        public override void dealShowNotice()
        {
            if (_m_countdownEventInfo == null || !_m_countdownEventInfo.isValid || _m_countdownEventInfo.countdownEventRef == null)
            {
                setDealerDone();
                return;
            }
                
            bool isNew = AccountSettingMgr.instance.accountSetting.getCDEventIsNew(_m_countdownEventInfo.dbId);
            if(!isNew)
            {
                setDealerDone();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                setDealerDone();
            });
            
            //打开弹窗，展示新事件对话
            GGUIWndCountdownEvent cdEventWnd = new GGUIWndCountdownEvent(_m_countdownEventInfo.countdownEventRef.ui_res_id);
            QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_COUNTDOWN_EVENT_WND, true, false, false, null
                , cdEventWnd, true, false,
                () =>
                {
                    cdEventWnd.showWnd();
                    cdEventWnd.setInfo(_m_countdownEventInfo);
                }, null, null, () =>
                {
                    // 因为_needControlRes为true, 所以理论上Node内会自动释放资源，这里不需要手动释放
                    // cdEventWnd.discard();
                    // cdEventWnd = null;
                    
                    stepCounter.addDoneStepCount();
                }));

            GCommon.enterDialogueNode(_m_countdownEventInfo.countdownEventRef.trigger_dialogue_id, () =>
            {
                CountdownEventInfo curInfo = NPPlayer.instance.countdownEventComp.countdownEventInfo;
                if (curInfo != null && curInfo.isValid)
                    AccountSettingMgr.instance.accountSetting.setRecordCDEvent(_m_countdownEventInfo.dbId, _m_countdownEventInfo.finishTimeMs);
                
                stepCounter.addDoneStepCount();
                GCommon.triggerTutorial();
            });
        }

        public override void dealHideNotice()
        {
        }
        
        protected override void __onDealerDone()
        {
            _m_aOnDealDone?.Invoke();
        }
        
        // 临时加上, 防止不能IF修改
        public override void showNotice()
        {
            base.showNotice();
        }

        // 临时加上, 防止不能IF修改
        protected override void _onGotoOtherMainViewNode()
        {
        }
    }
}