using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;
using System.Text;

namespace GOE
{
    public class NPGGUIWndCommonTextItem : _ANPGGUIBasicGridItemWnd<NPGGUIMonoCommonTextItem>
    {
        private NPGGuiWndTexture _m_iconWnd;

        public NPGGUIWndCommonTextItem(NPGGUIMonoCommonTextItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }

        //重置Grid单个对象
        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        public void setTxt(string _txtOne, string _txtTwo, NPGTextureIndex _texture, bool _needShowGo)
        {
            if(wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtOne, _txtOne);
            ALUGUICommon.setLabelTxt(wnd.txtTwo, _txtTwo);
            ALUGUICommon.setGameObjEnable(wnd.goShow,_needShowGo);
            if (_m_iconWnd != null)
            {
                if (_texture != null && _texture.isValid())
                {
                    _m_iconWnd.showWnd();
                    _m_iconWnd.setTexture(_texture);
                }
                else
                {
                    _m_iconWnd.hideWnd();
                }
            }
        }
    }
}

