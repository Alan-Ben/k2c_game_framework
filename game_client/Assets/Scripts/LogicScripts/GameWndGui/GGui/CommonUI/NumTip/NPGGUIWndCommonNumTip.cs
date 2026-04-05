using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;


namespace GOE
{
    public class NPGGUIWndCommonNumTip : _ANPGGUIBasicSubWnd<NPGGUIMonoCommonNumTip>
    {

        private Vector2 refPos;
        public NPGGUIWndCommonNumTip(NPGGUIMonoCommonNumTip _wnd)
           : base(_wnd)
        {
        }


        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _reset();
        }

        protected override void _onReset()
        {
            _reset();
        }

        protected override void _onDiscard()
        {

        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            refPos = wnd.rectTransform.rect.position;
        }

        public void setTip(string _str, string _animName, Vector2 _screenPos)
        {
            if(wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtNum, _str);

            //屏幕坐标转换到UGUI坐标
            Vector2 uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                wnd.rectTransform,
                _screenPos,
                Game.instance.mainCamera.fullCanvas.worldCamera,
                out uiPos);

            ALUGUICommon.setUIPos(wnd.rectTransform, uiPos);

            if(wnd != null && wnd.anim != null)
                wnd.anim.Play(_animName, PlayMode.StopAll);
        }

        private void _reset()
        {
            ALUGUICommon.setUIPos(wnd.rectTransform, refPos);
            wnd.rectTransform.localScale = Vector3.one;
        }
    }
}
