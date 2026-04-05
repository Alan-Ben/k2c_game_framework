using ALPackage;

namespace GOE
{
    /// <summary>
    /// 星辉技能item
    /// </summary>
    public class GGUIWndConsortHaloSkillLvlItem : _ATALBasicUISubWnd<GGUIMonoConsortHaloSkillLvlItem>
    {
        private ConsortHaloSkillInfo _m_iHaloSkillInfo;//星辉技能信息
        
        private NPGGuiWndTexture _m_wIcon;//图标
        
        public GGUIWndConsortHaloSkillLvlItem(GGUIMonoConsortHaloSkillLvlItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.skillIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.skillIcon);

        }
        
        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
        }

        public void setData(ConsortHaloSkillInfo _haloSkillInfo)
        {
            _m_iHaloSkillInfo = _haloSkillInfo;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iHaloSkillInfo == null || _m_iHaloSkillInfo.haloSkillRefObj == null || _m_iHaloSkillInfo.haloSkillLvlRefObj == null)
                return;

            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_iHaloSkillInfo.haloSkillRefObj.icon);
            }

            ALUGUICommon.setLabelTxt(wnd.txtSkillName, TextTranslate.instance.getLanguage(_m_iHaloSkillInfo.haloSkillRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtSkillLvl, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_iHaloSkillInfo.lvl));
            // string effectDesc = $"{_m_iHaloSkillInfo.haloSkillRefObj.add_type_desc}{TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_iHaloSkillInfo.haloSkillLvlRefObj.add_value_desc)}";
            // ALUGUICommon.setLabelTxt(wnd.txtSKillAddTypeDesc, TextTranslate.instance.getLanguage(effectDesc));
            
            ALUGUICommon.setLabelTxt(wnd.txtSKillAddTypeDesc, TextTranslate.instance.getLanguage(_m_iHaloSkillInfo.haloSkillRefObj.add_type_desc));
            ALUGUICommon.setLabelTxt(wnd.txtSkillAddValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_iHaloSkillInfo.haloSkillLvlRefObj.add_value_desc));
        }
    }
}