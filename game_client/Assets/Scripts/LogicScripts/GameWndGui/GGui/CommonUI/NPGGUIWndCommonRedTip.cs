using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using System;

namespace GOE
{
    //public class WCGGGUIWndCommonRedTip : _AWCCGGGUIBasicSubWnd<WCGGGUIMonoCommonRedTip> {
    public class _ATWCGGGUIWndRedTip<T> : _ANPGGUIBasicSubWnd<T> where T : NPGGUIMonoCommonRedTip
    {
        public _ATWCGGGUIWndRedTip(T _wnd)
            : base(_wnd)
        {
        }

        protected override void _onShowWnd() { }

        protected override void _onHideWnd() { }

        protected override void _onReset() { }

        protected override void _onDiscard() { }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        //显示隐藏小红点
        public void showRedTipNum(int _num)
        {
            if (null == wnd)
                return;

            ALUGUICommon.setUIObjScale(wnd.rectRedTip, _num > 0 ? 1.0f : 0f);
            ALUGUICommon.setLabelTxt(wnd.textRedTipNum, _num);
        }

        /// <summary>
        /// 显示小红点，如果超过一定值就显示类似 99+
        /// </summary>
        public void showRedTipNumByMore(int _num,int _max)
        {
            if (null == wnd)
                return;

            ALUGUICommon.setUIObjScale(wnd.rectRedTip, _num > 0 ? 1.0f : 0f);
            ALUGUICommon.setLabelTxt(wnd.textRedTipNum, (_num > _max) ? (_max + "+"): _num + "");
        }
    }

    /// <summary>
    /// 动态红点显示wnd
    /// </summary>
    public class NPGGUIWndCommonRedTip : _ATWCGGGUIWndRedTip<NPGGUIMonoCommonRedTip>
    {
        public NPGGUIWndCommonRedTip(NPGGUIMonoCommonRedTip _wnd)
           : base(_wnd)
        {
            initWnd();
        }

    }
}
