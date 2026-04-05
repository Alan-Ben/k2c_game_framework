using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUISubWndInnGuestUnlockTipContainerItem : _ATALBasicUISubWnd<GGUIMonoInnGuestUnlockTipContainerItem>
    {
        private _NPPlayerConditionSerializeInfo _m_condition;
        private string _m_tipKey;
        private CommonStringList _m_tipParams;
        
        
        public GGUISubWndInnGuestUnlockTipContainerItem(GGUIMonoInnGuestUnlockTipContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        
        protected override void _onShowWnd()
        {
            refreshWnd();
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
        
        
        public void refreshWnd(_NPPlayerConditionSerializeInfo _condition, string _tipKey, CommonStringList _tipParams)
        {
            _m_condition = _condition;
            _m_tipKey = _tipKey;
            _m_tipParams = _tipParams;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            List<string> paramList = _m_tipParams?.string_list ?? new List<string>(0);
            ALUGUICommon.setLabelTxt(wnd.txtTip, TextTranslate.instance.getLanguage(_m_tipKey, paramList));
            wnd.setComplete(_m_condition?.IsEnable(null) ?? true);
        }
    }
}