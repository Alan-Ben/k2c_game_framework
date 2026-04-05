using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIWndCustomCurrencyWnd : _ANPGGUIBasicSubWnd<NPGGUICustomMonoCurrencyWnd>
    {
        private NPCurrencyResObj _m_prPlayerResObj;

        public NPGGUIWndCustomCurrencyWnd(NPGGUICustomMonoCurrencyWnd _wnd)
           : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            NPPlayer.instance.rescourceComp.onResourceCountChg += _onCurrencyChg;
        }

        protected override void _onHideWnd()
        {
            NPPlayer.instance.rescourceComp.onResourceCountChg -= _onCurrencyChg;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            _m_prPlayerResObj = GRefdataCoreMgr.instance.currencyResCore.getRef((int)wnd.currencyType);
        }

        //点击领取奖励按钮，发送请求设置完成step协议
        private void _onCurrencyChg(CommonEnum.ECurrency _currencyType, long _srcV, long _tarV)
        {
            //刷新显示
            if(null == wnd || wnd.currencyType != _currencyType)
                return;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if(null == _m_prPlayerResObj || _m_prPlayerResObj.multiple == 0)
            {
                if(wnd.isNeedLargeString)
                    ALUGUICommon.setLabelTxt(wnd.currencyCount, NPPlayer.instance.rescourceComp.getValue(wnd.currencyType).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                else
                    ALUGUICommon.setLabelTxt(wnd.currencyCount, NPPlayer.instance.rescourceComp.getValue(wnd.currencyType));
            }
            else
            {
                if(wnd.isNeedLargeString)
                    ALUGUICommon.setLabelTxt(wnd.currencyCount, (NPPlayer.instance.rescourceComp.getValue(wnd.currencyType) / _m_prPlayerResObj.multiple).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                else
                    ALUGUICommon.setLabelTxt(wnd.currencyCount, (NPPlayer.instance.rescourceComp.getValue(wnd.currencyType) / _m_prPlayerResObj.multiple));
            }
        }
    }
}


