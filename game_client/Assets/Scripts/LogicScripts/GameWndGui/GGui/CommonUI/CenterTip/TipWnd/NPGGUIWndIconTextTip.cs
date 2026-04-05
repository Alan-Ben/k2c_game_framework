
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 图片+文本tip模板
    /// </summary>
    public class NPGGUIWndIconTextTip : _ATNPGGUIWndTip<NPGGUIMonoIconTextTip>
    {
        public NPGGUIWndIconTextTip(NPGGUIMonoIconTextTip _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        private Action _m_aOnClick;//点击回调
        private NPGGuiWndTexture _m_wIconWnd;//图片

        protected override void _onShowWndEx()
        {
            if (_m_wIconWnd != null)
                _m_wIconWnd.showWnd();
        }

        protected override void _onResetEx()
        {
            if (_m_wIconWnd != null)
                _m_wIconWnd.discardTexture();
        }

        protected override void _onHideWndEx()
        {
            if (_m_wIconWnd != null)
                _m_wIconWnd.hideWnd();
        }

        protected override void _onDiscardEx()
        {
            _m_aOnClick = null;
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
            }
            
            if (_m_wIconWnd != null)
                _m_wIconWnd.discard();
            _m_wIconWnd = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;

            if (wnd.icon != null)
            {
                _m_wIconWnd = new NPGGuiWndTexture(wnd.icon);
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }

        /// <summary>
        /// 设置tip数据
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_str"></param>
        public void setTipData(NPGTextureIndex _icon, List<string> _strList, Action _onClick)
        {
            if (wnd == null)
                return;

            _m_aOnClick = _onClick;
            
            setTipStr(_strList);

            if (_m_wIconWnd != null)
            {
                _m_wIconWnd.setTexture(_icon);
                _m_wIconWnd.showWnd();
            }
        }
        
        /// <summary>
        /// 设置tip数据
        /// </summary>
        /// <param name="_strList"></param>
        private void setTipStr(List<string> _strList)
        {
            if (_strList == null || wnd == null || wnd.txtStrList == null)
                return;

            int count = 0;//记录已赋值的text控件数量

            for (; count < _strList.Count; count++)
            {
                if (count >= wnd.txtStrList.Count)
                    break;

                ALUGUICommon.setLabelTxt(wnd.txtStrList[count], _strList[count]);
            }

            //多余的text控件设置文本为空
            for (; count < wnd.txtStrList.Count; count++)
            {
                ALUGUICommon.setLabelTxt(wnd.txtStrList[count], string.Empty);
            }
        }

        private void _onClick(GameObject _go)
        {
            _m_aOnClick?.Invoke();
        }
    }
}