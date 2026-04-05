using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterMainDialogSliderTipItem : _ATALBasicUISubWnd<GGUIMonoChapterMainDialogSliderTipItem>
    {
        private long _m_point;

        public long point
        {
            get { return _m_point; }
        }

        public GGUIWndChapterMainDialogSliderTipItem(GGUIMonoChapterMainDialogSliderTipItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
           
        }
        
        protected override void _onDiscard()
        {
           
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            _m_point = 0;
        }

        public void setInfo(long _point)
        {
            _m_point = _point;

            _refreshState();
        }

        public void playShowAni(Action _doneAction)
        {
            if (null == wnd || null == wnd.targetAnimation)
            {
                if (_doneAction != null) 
                    _doneAction();
                return;
            }
            
            wnd.targetAnimation.ForcePlay(wnd.targetAnimationName, 0, () =>
            {
                _refreshState();
                
                if (_doneAction != null) 
                    _doneAction();
            });
        }
        
        private void _refreshState()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.completeShowGoList, NPPlayer.instance.chapterComp.curPointId >= _m_point);
        }
    }
}