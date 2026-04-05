using System;
using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndEveningDungeonLossBloodTip : _ANPGGUIBasicSubWnd<GGUIMonoEveningDungeonLossBloodTip>
    {
        private NPGGUICommonTipDealerMgr _m_tipMgr;//tip管理器
        
        private NPCenterTipsRefObj _m_rDefaultLossHpCenterTipsRefObj;//掉血tip配表数据
        
        public GGUIWndEveningDungeonLossBloodTip(GGUIMonoEveningDungeonLossBloodTip _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.tipParent != null)
                _m_tipMgr = new NPGGUICommonTipDealerMgr(wnd.tipParent);
            
            _m_rDefaultLossHpCenterTipsRefObj = GRefdataCoreMgr.instance.tipMap.getRef(wnd.defaultLossBloodCenterTipId);
        }
        
        protected override void _onDiscard()
        {
            _m_tipMgr?.clear();
            _m_tipMgr = null;
            
            _m_rDefaultLossHpCenterTipsRefObj = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_tipMgr?.start();
        }

        protected override void _onHideWnd()
        {
            _m_tipMgr?.clear();
        }

        protected override void _onReset()
        {
            _m_tipMgr?.clear();
        }
        
        /// <summary>
        /// 显示掉血tip
        /// </summary>
        public void showLossBloodTip(long _lossHp, Action _onPop = null)
        {
            if(_m_tipMgr == null || wnd == null)
                return;

            NPCenterTipsRefObj tipsRef = wnd.getLossBloodTipRefObj(_lossHp);
            if(tipsRef == null)
                tipsRef = _m_rDefaultLossHpCenterTipsRefObj;
            if(tipsRef == null)
                return;
            
            _m_tipMgr.addTip(new NPTextTipDealer(new List<string>() { TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, _lossHp) }, tipsRef, (_tipWnd)=>
            {
                _onPop?.Invoke();
            }));            
        }
    }
}