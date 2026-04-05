using System;
using ALPackage;
using Random = UnityEngine.Random;

namespace GOE
{
    public class GGUIWndInnCashRegisterRewardTipFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoInnCashRegisterRewardTip, GGUIWndInnCashRegisterRewardTip>
    {
        private readonly GResPathIndex _m_resIndex;
        private long _m_num;
        private NPGTextureIndex _m_iconIndex;
        
        
        public GGUIWndInnCashRegisterRewardTipFollowItemController()
        {
            _m_resIndex = new GResPathIndex(6439);
        }
        public GGUIWndInnCashRegisterRewardTipFollowItemController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndInnCashRegisterRewardTip _createItemWnd(GGUIMonoInnCashRegisterRewardTip _wndMono)
        {
            GGUIWndInnCashRegisterRewardTip wnd = new GGUIWndInnCashRegisterRewardTip(_wndMono, discard);
            wnd.refreshWnd(_m_num, _m_iconIndex);
            wnd.showWnd();
            return wnd;
        }

        
        public void setReward(long _num, NPGTextureIndex _iconIndex)
        {
            _m_num = _num;
            _m_iconIndex = _iconIndex;
            wnd?.refreshWnd(_m_num, _m_iconIndex);
        }
    }
    public class GGUIWndInnCashRegisterRewardTip : _ATALGGUIWndCommonFollowItem<GGUIMonoInnCashRegisterRewardTip>
    {
        private long _m_num;
        private NPGTextureIndex _m_iconIndex;
        private Action _m_deleteFunc;
        private NPGGuiWndTexture _m_iconWnd;
        
        
        public GGUIWndInnCashRegisterRewardTip(GGUIMonoInnCashRegisterRewardTip _wnd, Action _deleteFunc) : base(_wnd)
        {
            _m_deleteFunc = _deleteFunc;
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgRewardIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgRewardIcon);
        }
        
        
        public void refreshWnd(long _num, NPGTextureIndex _iconIndex)
        {
            _m_num = _num;
            _m_iconIndex = _iconIndex;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtRewardNum, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_num.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            _m_iconWnd?.setTexture(_m_iconIndex);
            
            if (wnd.showAnimation != null)
                wnd.showAnimation.Play(wnd.showAnimationName);
            ALCommonActionMonoTask.addMonoTask(_delete, wnd.deleteDelay);
        }


        private void _delete()
        {
            _m_deleteFunc?.Invoke();
        }
    }
}