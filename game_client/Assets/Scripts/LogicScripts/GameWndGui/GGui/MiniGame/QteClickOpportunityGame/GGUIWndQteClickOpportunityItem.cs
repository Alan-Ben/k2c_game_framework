using System;
using ALPackage;
using GOE.MiniGame;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// QTE点击item
    /// </summary>
    public class GGUIWndQteClickOpportunityItem : _ANPGGUIBasicSubWnd<GGUIMonoQteClickOpportunityItem>, _IQteClickOpportunityGameItemShow
    {
        // // 动画机的各种参数
        // private static readonly int animatorParameter_trigger = Animator.StringToHash("trigger"); // 是否触发
        // private static readonly int animatorParameter_triggerTime = Animator.StringToHash("triggerTime"); // 触发时间
        
        private QteClickOpportunityGameController _m_gameController;
        private Action _m_aOnItemTrigger;
        
        public GGUIWndQteClickOpportunityItem(GGUIMonoQteClickOpportunityItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        public QteClickOpportunityItemConfig itemConfig { get { return wnd == null ? null : wnd.itemConfig; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onItemClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onItemClick);
            }
        }
        
        protected override void _onShowWnd()
        {
            if(wnd == null)
                return;
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        public void setGameController(QteClickOpportunityGameController _gameController, Action _onItemTrigger)
        {
            _m_gameController = _gameController;
            _m_aOnItemTrigger = _onItemTrigger;
        }

        public void showItem()
        {
            showWnd();
            
            if(wnd != null && wnd.animation != null && !string.IsNullOrEmpty(wnd.showAnimationName))
                wnd.animation.ForcePlay(wnd.showAnimationName);
        }

        public void hideItem()
        {
            hideWnd();
        }

        public void setTriggerTime(float _triggerTime, OpportunityTimeRange _inTimeRange)
        {
            if(wnd == null || _inTimeRange == null)
                return;

            if(wnd.animation != null && !string.IsNullOrEmpty(_inTimeRange.animationName))
                wnd.animation.CrossFade(_inTimeRange.animationName, _inTimeRange.crossFadeLength);
        }

        private void _onItemClick(GameObject _go)
        {
            _m_aOnItemTrigger?.Invoke();
        }
    }
}