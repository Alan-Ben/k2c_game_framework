using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 规则二级列表item
    /// </summary>
    public class NPGGUIWndRuleListSubItem : _ATALBasicUISubWnd<NPGGUIMonoRuleListSubItem>
    {
        private NPRuleSubRefObj _m_ruleSubRefObj;

        public NPGGUIWndRuleListSubItem(NPGGUIMonoRuleListSubItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
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
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _clickGoto);
        }
        
        public void setInfo(NPRuleSubRefObj _ruleSubRefObj)
        {
            _m_ruleSubRefObj = _ruleSubRefObj;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_ruleSubRefObj)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_ruleSubRefObj.name));
        }
        

        private void _clickGoto(GameObject _obj)
        {
            if (null == _m_ruleSubRefObj || null == _m_ruleSubRefObj.client_effect)
                return;
            _m_ruleSubRefObj.client_effect.dealEffect();
        }

    }
}
