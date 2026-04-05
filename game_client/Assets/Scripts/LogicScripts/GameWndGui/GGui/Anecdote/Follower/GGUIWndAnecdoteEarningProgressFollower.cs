
using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAnecdoteEarningProgressFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoAnecdoteEarningProgressFollower, GGUIWndAnecdoteEarningProgressFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private long _m_currentEarnings;
        private long _m_targetEarnings;
        private Action _m_triggerAction;
        private _AAnecdoteEventInfo _m_eventInfo;
        
        public GGUIWndAnecdoteEarningProgressFollowerController()
        {
            _m_resIndex = new GResPathIndex(3402);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndAnecdoteEarningProgressFollower _createItemWnd(GGUIMonoAnecdoteEarningProgressFollower _wndMono)
        {
            GGUIWndAnecdoteEarningProgressFollower wnd = new GGUIWndAnecdoteEarningProgressFollower(_wndMono);
            wnd.refreshWnd(_m_currentEarnings, _m_targetEarnings, _m_eventInfo);
            wnd.setTriggerAction(_m_triggerAction);
            wnd.showWnd();
            return wnd;
        }
        
        
        public void refreshWnd(long _currentEarnings, long _targetEarnings, _AAnecdoteEventInfo _eventInfo)
        {
            _m_currentEarnings = _currentEarnings;
            _m_targetEarnings = _targetEarnings;
            _m_eventInfo = _eventInfo;
            wnd?.refreshWnd(_currentEarnings, _targetEarnings, _eventInfo);
        }
        public void setEarnings(long _currentEarnings, long _targetEarnings)
        {
            _m_currentEarnings = _currentEarnings;
            _m_targetEarnings = _targetEarnings;
            wnd?.setEarnings(_currentEarnings, _targetEarnings);
        }
        public void setTriggerAction(Action _triggerAction)
        {
            _m_triggerAction = _triggerAction;
            wnd?.setTriggerAction(_m_triggerAction);
        }
    }
    public class GGUIWndAnecdoteEarningProgressFollower : _AAnecdoteEventFollower<GGUIMonoAnecdoteEarningProgressFollower>
    {
        private long _m_currentEarnings;
        private long _m_targetEarnings;
        private Action _m_triggerAction;
        
        public GGUIWndAnecdoteEarningProgressFollower(GGUIMonoAnecdoteEarningProgressFollower _wnd) 
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
        
        public void refreshWnd(long _currentEarnings, long _targetEarnings, _AAnecdoteEventInfo _eventInfo)
        {
            _m_currentEarnings = _currentEarnings;
            _m_targetEarnings = _targetEarnings;
            
            refreshWnd(_eventInfo);
        }

        protected override void _onRefreshWnd()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.anecdote_earnings_num, _m_currentEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD), _m_targetEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
        }

        public void setTriggerAction(Action _triggerAction)
        {
            _m_triggerAction = _triggerAction;
        }


        private void _onBtnTriggerClick(GameObject _)
        {
            _m_triggerAction?.Invoke();
        }

        public void setEarnings(long _currentEarnings, long _targetEarnings)
        {
            _m_currentEarnings = _currentEarnings;
            _m_targetEarnings = _targetEarnings;
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.anecdote_earnings_num, _m_currentEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD), _m_targetEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
        }
    }
}