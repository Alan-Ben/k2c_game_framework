using System;

namespace GOE
{
    /// <summary>
    /// 倒计时事件重置展示
    /// </summary>
    public class NoticeDealer_CountdownEventResetShow : _AMainCityCanJumpPushNotice
    {
        private long _m_lShowDialogId;
        private Action _m_onDealDone;
        
        public NoticeDealer_CountdownEventResetShow(long _dialogId, Action _onDealDone, EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
            _m_lShowDialogId = _dialogId;
            _m_onDealDone = _onDealDone;
        }

        protected override bool _isEnable { get { return true; } }
        protected override bool _canCurShow { get { return true; } }
        protected override string _noticeTag { get { return NoticeTagConst.COUNT_DOWN_EVENT_RESET_SHOW; } }

        public override void dealShowNotice()
        {
            //展示未完成对话再打开弹窗
            GCommon.enterDialogueNode(_m_lShowDialogId, () =>
            {
                //获取最新数据打开弹窗
                CountdownEventInfo curInfo = NPPlayer.instance.countdownEventComp.countdownEventInfo;
                if (curInfo != null && curInfo.isValid)
                {
                    AccountSettingMgr.instance.accountSetting.setRecordCDEvent(curInfo.dbId, curInfo.finishTimeMs);
                    GGUIWndCountdownEvent cdEventWnd = new GGUIWndCountdownEvent(curInfo.countdownEventRef.ui_res_id);
                    QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_COUNTDOWN_EVENT_WND, true, false, false, null
                        , cdEventWnd, true, false,
                        () =>
                        {
                            cdEventWnd.showWnd();
                            cdEventWnd.setInfo(curInfo);
                            GCommon.triggerTutorial();
                        }, null, null, () =>
                        {
                            // 因为_needControlRes为true, 所以理论上Node内会自动释放资源，这里不需要手动释放
                            // cdEventWnd.discard();
                            // cdEventWnd = null;
                            
                            setDealerDone();
                        }));
                }
                else
                {
                    setDealerDone();
                }
            });
        }

        public override void dealHideNotice()
        {
        }
        
        protected override void __onDealerDone()
        {
            _m_onDealDone?.Invoke();
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