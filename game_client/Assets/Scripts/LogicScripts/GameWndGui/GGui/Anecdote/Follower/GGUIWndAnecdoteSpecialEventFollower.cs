
using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAnecdoteSpecialEventFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoAnecdoteSpecialEventFollower, GGUIWndAnecdoteSpecialEventFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private Action _m_triggerAction;
        private _AAnecdoteEventInfo _m_eventInfo;
        
        public GGUIWndAnecdoteSpecialEventFollowerController()
        {
            _m_resIndex = new GResPathIndex(3400);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndAnecdoteSpecialEventFollower _createItemWnd(GGUIMonoAnecdoteSpecialEventFollower _wndMono)
        {
            GGUIWndAnecdoteSpecialEventFollower wnd = new GGUIWndAnecdoteSpecialEventFollower(_wndMono);
            wnd.refreshWnd(_m_eventInfo);
            wnd.setTriggerAction(_m_triggerAction);
            wnd.showWnd();
            return wnd;
        }
        
        public void refreshWnd(_AAnecdoteEventInfo _eventInfo)
        {
            _m_eventInfo = _eventInfo;
            wnd?.refreshWnd(_eventInfo);
        }
        
        public void setTriggerAction(Action _triggerAction)
        {
            _m_triggerAction = _triggerAction;
            wnd?.setTriggerAction(_m_triggerAction);
        }
    }
    public class GGUIWndAnecdoteSpecialEventFollower : _AAnecdoteEventFollower<GGUIMonoAnecdoteSpecialEventFollower>
    {
        private Action _m_triggerAction;
        private _AAnecdoteEventInfo _m_iEventInfo;
        
        public GGUIWndAnecdoteSpecialEventFollower(GGUIMonoAnecdoteSpecialEventFollower _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onWndInitDoneSub()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnTrigger, _onBtnTriggerClick);
        }

        protected override void _onDiscardSub()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnTrigger, _onBtnTriggerClick);
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
        }

        protected override void _onResetSub()
        {
        }

        protected override void _onRefreshWnd()
        {
        }

        public void setTriggerAction(Action _triggerAction)
        {
            _m_triggerAction = _triggerAction;
        }
        private void _onBtnTriggerClick(GameObject _)
        {
            _m_triggerAction?.Invoke();
        }
    }
}