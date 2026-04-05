using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    public class _ATNPGGUIWndCommonResItem<T> : _ANPGGUIBasicSubWnd<T> where T : NPGGUIMonoCommonResItem
    {
        /** 图片对象 */
        private NPGGuiWndTexture _m_wIconSubWnd;
        private Color _m_cOrigColor;
        private Action _m_dClickAddBtnDelegate;

        public _ATNPGGUIWndCommonResItem(T _wnd)
            : base(_wnd)
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
            if(null != _m_wIconSubWnd)
                _m_wIconSubWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            if(null != _m_wIconSubWnd)
                _m_wIconSubWnd.discard();
            _m_wIconSubWnd = null;

            _m_dClickAddBtnDelegate = null;
        }

        protected override void _onWndInitDone()
        {
            //绑定进入按钮操作
            if(null == wnd)
                return;

            if(null != wnd.texIcon)
                _m_wIconSubWnd = new NPGGuiWndTexture(wnd.texIcon);

            if(wnd.txtNum != null)
                _m_cOrigColor = wnd.txtNum.color;

            ALUGUICommon.combineBtnClick(wnd.btnAddRes, _onClickAddBtn);

            //初始化时把数值设置为0
            ALUGUICommon.setLabelTxt(wnd.txtNum, 0);
        }

        /**************
         * 点击添加资源按钮
         **/
        protected virtual void _onClickAddBtn(GameObject _go)
        {
            if(null != _m_dClickAddBtnDelegate)
                _m_dClickAddBtnDelegate();
        }

        /**************
        * 设置本窗口相关信息
        **/
        //之前显示的数字
        private int _m_iPreShowCount;
        public void setResItem(NPGSpriteIndex _icon, string _value, string _name = "")
        {
            setResItemIcon(_icon);

            if(wnd == null)
                return;

            _m_iPreShowCount = 0;
            setResText(_value, _name);
        }

        public void setResItem(NPGSpriteIndex _icon, int _value, string _name = "")
        {
            setResItemIcon(_icon);
            setResText(_value, _name);
        }
        public void setResText(int _value, string _name = "")
        {
            if(_m_iPreShowCount == _value)
                return;

            _m_iPreShowCount = _value;
            setResText(_value.ToString(), _name);
        }
        public void setResText(string _value, string _name = "")
        {
            if(wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtNum, _value);
            ALUGUICommon.setLabelTxt(wnd.txtResName, _name);
        }

        public void setResItemNum(long _value, bool _isShake = false)
        {
            if(wnd == null || _m_iPreShowCount == _value)
                return;

            _m_iPreShowCount = (int)_value;
            ALUGUICommon.setLabelTxt(wnd.txtNum, _value);

            //是否需要播放资源摇动的动画
            if(_isShake)
            {
                playShakeAnim();
            }
        }

        public void setResItemIcon(NPGSpriteIndex _icon)
        {
            if(_m_wIconSubWnd != null)
            {
                if(_icon == null)
                {
                    _m_wIconSubWnd.hideWnd();
                }
                else
                {
                    _m_wIconSubWnd.showWnd();
                    _m_wIconSubWnd.setTexture(_icon);
                }
            }
        }

        public void setResItemNum(string _valueStr)
        {
            if(wnd == null)
                return;

            _m_iPreShowCount = 0;
            ALUGUICommon.setLabelTxt(wnd.txtNum, _valueStr);
        }

        public void setUIColor(Color _color)
        {
            ALUGUICommon.setUIObjColor(wnd.txtNum, _color);
        }

        public void refreshTextColor(bool _enough)
        {
            ALUGUICommon.setUIObjColor(wnd.txtNum, _enough ? _m_cOrigColor : Color.red);
        }

        //播放资源摇动的动画
        public void playShakeAnim()
        {
            playAnim("res_shake");
        }

        //播放动画
        public void playAnim(string _animName)
        {
            if(wnd == null || wnd.anim == null || string.IsNullOrEmpty(_animName))
                return;

            wnd.anim.Play(_animName);
        }
    }
}
