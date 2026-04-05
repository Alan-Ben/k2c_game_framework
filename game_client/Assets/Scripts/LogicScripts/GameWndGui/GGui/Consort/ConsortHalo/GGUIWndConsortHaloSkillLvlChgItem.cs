using ALPackage;

namespace GOE
{
    /// <summary>
    /// 星辉技能等级变更Item
    /// </summary>
    public class GGUIWndConsortHaloSkillLvlChgItem  : _ATALBasicUISubWnd<GGUIMonoConsortHaloSkillLvlChgItem>
    {
        private NPGGuiWndTexture _m_wSkillIcon;//技能图标

        private ConsortHaloSkillLvlChgInfo _m_iSkillLvlChgInfo;//技能等级变化信息
        
        private ConsortHaloSkillRefObj _m_rSkillRefObj;//技能配表数据
        private ConsortHaloSkillLvlRefObj _m_rPreLvlRefObj;//上一等级配表数据
        private ConsortHaloSkillLvlRefObj _m_rNextLvlRefObj;//下一等级配表数据
        
        public GGUIWndConsortHaloSkillLvlChgItem(GGUIMonoConsortHaloSkillLvlChgItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.skillIcon != null)
                _m_wSkillIcon = new NPGGuiWndTexture(wnd.skillIcon);
        }
        
        protected override void _onDiscard()
        {
            _m_wSkillIcon?.discard();
            _m_wSkillIcon = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wSkillIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSkillIcon?.discardTexture();
        }

        public void setData(ConsortHaloSkillLvlChgInfo _haloSKillLvlChgInfo)
        {
            _m_iSkillLvlChgInfo = _haloSKillLvlChgInfo;
            _m_rSkillRefObj = GRefdataCoreMgr.instance.consortHaloSkillRefCore.getRef(_m_iSkillLvlChgInfo?.halo_skill_id ?? 0);
            _m_rPreLvlRefObj = _m_rSkillRefObj?.getHaloSkillLvlRefObj(_m_iSkillLvlChgInfo?.pre_lvl ?? 0);
            _m_rNextLvlRefObj = _m_rSkillRefObj?.getHaloSkillLvlRefObj(_m_iSkillLvlChgInfo?.next_lvl ?? 0);
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iSkillLvlChgInfo == null || _m_rSkillRefObj == null)
                return;

            if (_m_wSkillIcon != null)
            {
                _m_wSkillIcon.showWnd();
                _m_wSkillIcon.setTexture(_m_rSkillRefObj.icon);
            }

            ALUGUICommon.setLabelTxt(wnd.txtSkillName, TextTranslate.instance.getLanguage(_m_rSkillRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtPreLvl, TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_rPreLvlRefObj?.halo_skill_lvl ?? 0)));
            ALUGUICommon.setLabelTxt(wnd.txtNextLvl, TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_rNextLvlRefObj?.halo_skill_lvl ?? 0)));

            ALUGUICommon.setLabelTxt(wnd.txtAddTypeDesc, TextTranslate.instance.getLanguage(_m_rSkillRefObj.add_type_desc));

            ALUGUICommon.setLabelTxt(wnd.txtPreLvlAddValue, TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_rPreLvlRefObj?.add_value_desc)));
            ALUGUICommon.setLabelTxt(wnd.txtNextLvlAddValue, TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_rNextLvlRefObj?.add_value_desc)));
        }
    }
}