
using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用提示抽象模板基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ATNPGGUIWndTip<T> : _ATALBasicUISubWnd<T>
        where T : NPGGUIMonoCommonTip // 继承用的泛型类
    {
        private Vector2 _m_vRefPos;//原始位置

        public _ATNPGGUIWndTip(T _wnd)
            : base(_wnd)
        {

        }

        protected override void _onShowWnd()
        {
            _onShowWndEx();
        }

        protected override void _onHideWnd()
        {
            _onHideWndEx();
            _resetTipPos();
            _resetAnim();
        }

        protected override void _onReset()
        {
            _onResetEx();
            _resetTipPos();
            _resetAnim();
        }

        protected override void _onDiscard()
        {
            _onDiscardEx();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            _onWndInitDoneEx();

            if (wnd.rectTransform != null)
                _m_vRefPos = wnd.rectTransform.anchoredPosition;
        }


        #region 子类窗体相关
        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
        #endregion


        #region 窗体方法

        /// <summary>
        /// 播放显示动画
        /// </summary>
        public void playShowAnim()
        {
            if (wnd == null)
                return;

            if (wnd.anim != null)
            {
                if (string.IsNullOrEmpty(wnd.animName))
                    wnd.anim.Play(PlayMode.StopAll);
                else
                    wnd.anim.Play(wnd.animName, PlayMode.StopAll);
            }
        }

        /// <summary>
        /// 设置动画播放速度
        /// </summary>
        /// <param name="_speed"></param>
        public void setAnimSpeed(float _speed)
        {
            if (wnd == null)
                return;

            if (wnd.anim != null)
            {
                wnd.anim.SetAnimSpeed(wnd.animName, _speed);
            }
        }

        /// <summary>
        /// 设置窗体位置
        /// </summary>
        /// <param name="_screenPos"></param>
        public void setTipPos(Vector2 _screenPos)
        {
            if (null == wnd)
                return;

            //屏幕坐标转换到UGUI坐标
            Vector2 uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                wnd.rectTransform.parent.transform as RectTransform,
                _screenPos,
                Game.instance.mainCamera.uiCamera,
                out uiPos);

            ALUGUICommon.setUIPos(wnd.rectTransform, uiPos);
        }

        //重置动画
        private void _resetAnim()
        {
            if (wnd == null || string.IsNullOrEmpty(wnd.animName))
                return;

            wnd.anim.Sample(wnd.animName, 0);
        }

        /// <summary>
        /// 重置窗体位置
        /// </summary>
        protected virtual void _resetTipPos()
        {
            ALUGUICommon.setUIPos(wnd.rectTransform, _m_vRefPos);
            if (wnd.rectTransform != null)
                wnd.rectTransform.localScale = Vector3.one;
        }

        /// <summary>
        /// 显示并播放动画
        /// </summary>
        public void showAndPlayAnim()
        {
            showWnd();
            rectTransform.SetAsLastSibling();
            playShowAnim();
        }

        #endregion

    }
}