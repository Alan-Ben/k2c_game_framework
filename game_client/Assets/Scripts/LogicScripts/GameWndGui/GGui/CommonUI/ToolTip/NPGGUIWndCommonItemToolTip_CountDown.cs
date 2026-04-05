using ALPackage;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用带两个倒计时的自定义跟随窗口
    /// </summary>
    public class NPGGUIWndCommonItemToolTip_CountDown : _ATNPGGUIWndCommonItemToolTip<NPGGUIMonoCommonToolTip_CountDown>
    {
        private NPGGUIWndCommonCountDown _m_countDownOne;

        private NPGGUIWndCommonCountDown _m_countDownTwo;

        public NPGGUIWndCommonItemToolTip_CountDown(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
            
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (null != wnd.countDownOne)
                _m_countDownOne = new NPGGUIWndCommonCountDown(wnd.countDownOne);
            if (null != wnd.countDownTwo)
                _m_countDownTwo = new NPGGUIWndCommonCountDown(wnd.countDownTwo);
        }

        protected override void _onShowWnd()
        {
            if (null != _m_countDownOne)
                _m_countDownOne.showWnd();

            if (null != _m_countDownTwo)
                _m_countDownTwo.showWnd();
        }

        protected override void _onHideWnd()
        {
            if (null != _m_countDownOne)
                _m_countDownOne.hideWnd();

            if (null != _m_countDownTwo)
                _m_countDownTwo.hideWnd();
        }
        protected override void _onDiscard()
        {
            base._onDiscard();

            if (null != _m_countDownOne)
                _m_countDownOne.discard();
            _m_countDownOne = null;

            if (null != _m_countDownTwo)
                _m_countDownTwo.discard();
            _m_countDownTwo = null;

        }

        protected override float getSelfHeight()
        {
            if (null == wnd)
                return 0;

            return rectTransform.rect.height;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(long _oneTimeS,string _oneKeyStr, long _twoTimeS, string _twoKeyStr, RectTransform _targetTransRoot, float _interval,Action _doneOneAction,Action _doneTwoAction)
        {
            if(null == wnd)
                return;

            if (null != _m_countDownOne)
                _m_countDownOne.setInfo(_oneTimeS, _oneKeyStr, _doneOneAction);
            if(null != _m_countDownTwo)
                _m_countDownTwo.setInfo(_twoTimeS, _twoKeyStr, _doneTwoAction);

            //设置位置
            setPos(_targetTransRoot, _interval);

            //刷新
            LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.gameObject.GetComponent<RectTransform>());
        }

        public void setInfo(long _oneTimeS, long _twoTimeS)
        {
            if (null != _m_countDownOne)
                _m_countDownOne.setCountTimeS(_oneTimeS);
            if (null != _m_countDownTwo)
                _m_countDownTwo.setCountTimeS(_twoTimeS);

        }
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGNodeCommonToolTip_CountDown));
        }
    }
}