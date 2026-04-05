using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    //上浮提示
    public class NPGGUIWndCenterTipItem : _ANPGGUIBasicSubWnd<NPGGUIMonoCenterTipItem>
    {
        public NPGGUIWndCenterTipItem(NPGGUIMonoCenterTipItem _wnd)
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

        protected override void _onDiscard()
        {

        }

        protected override void _onWndInitDone()
        {
        }

        /**************
         * 设置文本
         **/
        public void showInfo(string _text)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setLabelTxt(wnd.text, _text);
            if(wnd.anim != null)
            {
                //如果已经处在播放状态，那么需要先重置再播放
                if(wnd.anim.isPlaying)
                    wnd.anim.Stop();

                wnd.anim.Play();
            }
            //显示窗口，并设置动画
            showWnd();
            ALUGUICommon.setUIObjScale(wnd, 1f);
        }

        public void hideTip()
        {
            if(null == wnd)
                return;

            ALUGUICommon.setUIObjScale(wnd, 0f);
        }
    }
}
