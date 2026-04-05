using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _AGGUIWndConsortInviteBuffItem<T> : _ANPGGUIBasicSubWnd<T> where T : _AGGUIMonoConsortInviteBuffItem
    {
        private long _m_iBuffCount;//buff次数
        
        public _AGGUIWndConsortInviteBuffItem(T _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _onWndInitDoneSub();
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
            }

            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _onResetSub();
        }

        public void setBuffCount(long _buffCount)
        {
            _m_iBuffCount = _buffCount;
            if(wnd == null)
                return;

            string buffCountStr = _m_iBuffCount <= 0
                ? GCommon.addColorForRichText(_m_iBuffCount.ToString(), wnd.noCountColor)
                : GCommon.addColorForRichText(_m_iBuffCount.ToString(), wnd.hasCountColor);
            ALUGUICommon.setLabelTxt(wnd.buffCount, TextTranslate.instance.getLanguage(TransKeyConst.common_multiple_num, buffCountStr));
            
            ALUGUICommon.setGameObjEnable(wnd.hasCountShowList, _m_iBuffCount > 0);
            ALUGUICommon.setGameObjEnable(wnd.noCountShowList, _m_iBuffCount <= 0);
            
            GGameCommonInfo.grayImage(wnd.noCountGrayList, _m_iBuffCount <= 0);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClick(GameObject _go)
        {
            if(wnd == null || _go == null)
                return;
            
            _showToolTip(wnd.showToolTipUIResId, (RectTransform)_go.transform, wnd.showToolTipIntervalX, wnd.showToolTipIntervalY);
        }

        protected abstract void _onWndInitDoneSub();

        protected abstract void _onDiscardSub();

        protected abstract void _onShowWndSub();
        
        protected abstract void _onHideWndSub();

        protected abstract void _onResetSub();
        
        protected abstract void _showToolTip(long _assetPathId, RectTransform _targetTransRoot, float _intervalX, float _intervalY);
    }
}