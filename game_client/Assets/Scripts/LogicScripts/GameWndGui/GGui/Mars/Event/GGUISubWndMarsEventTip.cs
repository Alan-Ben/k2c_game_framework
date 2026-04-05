using Common.MarsObj;

namespace GOE
{
    public class GGUISubWndMarsEventTip : _ANPGGUIBasicSubWnd<GGUISubMonoMarsEventTip>
    {
        private NPGGUICommonTipDealerMgr _m_tipDealerMgr;
        private Mars_Event _m_iWaitingShowEvent;
        
        private NPCenterTipsRefObj _m_rCenterTipsRefObj;
        
        public GGUISubWndMarsEventTip(GGUISubMonoMarsEventTip _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_tipDealerMgr = new NPGGUICommonTipDealerMgr(wnd.parentGo);
            _m_rCenterTipsRefObj = GRefdataCoreMgr.instance.tipMap.getRef(wnd.showCenterTipRefObjId);

            _m_iWaitingShowEvent = null;
            
            // 注册事件触发监听
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_EVENT_TRIGGER, _onMarsEventTrigger);
        }
        
        protected override void _onDiscard()
        {
            _m_rCenterTipsRefObj = null;
            
            // 取消注册事件监听
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_EVENT_TRIGGER, _onMarsEventTrigger);
            
            _m_iWaitingShowEvent = null;
            
            _m_tipDealerMgr?.clear();
            _m_tipDealerMgr = null;
        }
        
        protected override void _onShowWnd()
        {
            _showEventTip();
        }

        protected override void _onHideWnd()
        {
            _m_iWaitingShowEvent = null;
            
            _m_tipDealerMgr?.clear();
        }

        protected override void _onReset()
        {
            _m_iWaitingShowEvent = null;
            
            _m_tipDealerMgr?.clear();
        }

        private void _showEventTip()
        {
            if(_m_tipDealerMgr == null || _m_rCenterTipsRefObj == null || _m_iWaitingShowEvent != null)
                return;

            _m_iWaitingShowEvent = NPPlayer.instance.marsComp.peopleSubComponent.triggerEventTipInfoList?.GetFirst();
            if(_m_iWaitingShowEvent == null)
                return;

            MarsEventInfo eventInfo = NPPlayer.instance.marsComp.peopleSubComponent.getEventInfo(_m_iWaitingShowEvent.getEventId());
            MarsEventRefObj eventRefObj = eventInfo?.eventRefObj;
            // 若显示条件不通过
            if (eventRefObj == null || (eventRefObj.need_show_tip_cond != null && !eventRefObj.need_show_tip_cond.isNoConditionOrEnable(null)))
            {
                // 从等待显示列表中移除该事件，继续显示下一个
                NPPlayer.instance.marsComp.peopleSubComponent.removeTriggerEventTip(_m_iWaitingShowEvent);
                _m_iWaitingShowEvent = null;
                _showEventTip();
                
                return;
            }

            NPIconTextTipDealer tipDealer = new NPIconTextTipDealer(eventRefObj.banner_img, eventRefObj.name,
                _m_rCenterTipsRefObj, (_tipWnd) =>
                {
                    // 一个tip展示出来后, 开始准备进行下一个tip的显示
                    NPPlayer.instance.marsComp.peopleSubComponent.removeTriggerEventTip(_m_iWaitingShowEvent);
                    _m_iWaitingShowEvent = null;
                    _showEventTip();
                });

            tipDealer.onClick += () =>
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsEventDetail.instance, () =>
                {
                    GGUIWndMarsEventDetail.instance.setData(eventInfo);
                    GGUIWndMarsEventDetail.instance.showWnd();
                }, UINodeTagConst.C_MARS_EVENT_DETAIL);
            };
            
            _m_tipDealerMgr.addTip(tipDealer);
        }
        
        /// <summary>
        /// 当火星事件触发时的处理
        /// </summary>
        private void _onMarsEventTrigger()
        {
            // 尝试显示事件tip
            _showEventTip();
        }
    }
}