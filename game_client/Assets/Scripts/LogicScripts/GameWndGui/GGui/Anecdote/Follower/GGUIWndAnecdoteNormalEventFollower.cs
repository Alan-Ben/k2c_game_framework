
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAnecdoteNormalEventFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoAnecdoteNormalEventFollower, GGUIWndAnecdoteNormalEventFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private Action _m_triggerAction;
        private _AAnecdoteEventInfo _m_iEventInfo;
        
        public GGUIWndAnecdoteNormalEventFollowerController()
        {
            _m_resIndex = new GResPathIndex(3401);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndAnecdoteNormalEventFollower _createItemWnd(GGUIMonoAnecdoteNormalEventFollower _wndMono)
        {
            GGUIWndAnecdoteNormalEventFollower wnd = new GGUIWndAnecdoteNormalEventFollower(_wndMono);
            wnd.refreshWnd(_m_iEventInfo);
            wnd.setTriggerAction(_m_triggerAction);
            wnd.showWnd();
            return wnd;
        }
        
        
        public void setTriggerAction(Action _triggerAction)
        {
            _m_triggerAction = _triggerAction;
            wnd?.setTriggerAction(_m_triggerAction);
        }
        
        public void refreshWnd(_AAnecdoteEventInfo _eventInfo)
        {
            _m_iEventInfo = _eventInfo;
            wnd?.refreshWnd(_m_iEventInfo);
        }
    }
    public class GGUIWndAnecdoteNormalEventFollower : _AAnecdoteEventFollower<GGUIMonoAnecdoteNormalEventFollower>
    {
        private Action _m_triggerAction;
        
        
        public GGUIWndAnecdoteNormalEventFollower(GGUIMonoAnecdoteNormalEventFollower _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
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