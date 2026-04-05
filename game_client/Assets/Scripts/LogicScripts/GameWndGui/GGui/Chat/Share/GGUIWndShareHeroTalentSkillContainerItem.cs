using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴分享资质列表item
    /// </summary>
    public class GGUIWndShareHeroTalentSkillContainerItem : _ATALBasicUISubWnd<GGUIMonoShareHeroTalentSkillContainerItem>
    {
        //资质图标
        private NPGGuiWndTexture _m_wIconWnd;
        //是否解锁
        private bool _m_bIsUnlock;
        //资质技能配置
        private HeroTalentSkillRefObj _m_talentSkillRef;
        //等级
        private long _m_lLevel;

        public GGUIWndShareHeroTalentSkillContainerItem(GGUIMonoShareHeroTalentSkillContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIconWnd?.discard();
            _m_wIconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.texIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.texIcon);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HeroShareTalentSkillInfo _talentSkillInfo)
        {
            if (wnd == null)
                return;

            _m_lLevel = _talentSkillInfo.level;
            _m_talentSkillRef = GRefdataCoreMgr.instance.heroTalentSkillRefCore.getRef(_talentSkillInfo.talentSkillId);
            _m_bIsUnlock = _talentSkillInfo.isUnlock;

            //刷新界面
            _refreshWnd();
        }

        //刷新界面
        private void _refreshWnd()
        {
            if (wnd == null || _m_talentSkillRef == null)
                return;

            //设置图标
            if (_m_wIconWnd != null)
            {
                _m_wIconWnd.showWnd();
                _m_wIconWnd.setTexture(_m_talentSkillRef.icon);
            }

            //设置资质
            if (_m_bIsUnlock)
            {
                //当前资质
                HeroTalentSkillLevelRefObj curTalentSkillBaseLevelRef = _m_talentSkillRef?.getBaseTalentSkillLevelRefByLevel(_m_lLevel);
                PlayerAttrPropertyContainer curModifier = new PlayerAttrPropertyContainer();
                if (curTalentSkillBaseLevelRef != null)
                {
                    curModifier.addModifier(curTalentSkillBaseLevelRef.self_attr_prop_modifier);
                    curModifier.addModifier(curTalentSkillBaseLevelRef.self_attr_prop_modifier_per_level, _m_lLevel - curTalentSkillBaseLevelRef.level);
                }
                long curTalent = curModifier.getValue(EBasicAttrType.TALENT);

                ALUGUICommon.setLabelTxt(wnd.txtAddTalent, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentAddValue_num, curTalent));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtAddTalent, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentAddValue_num, 0));
            }

            //是否解锁
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !_m_bIsUnlock);
            if (_m_bIsUnlock)
                GGameCommonInfo.disgrayImage(wnd.lockGrayList);
            else
                GGameCommonInfo.grayImage(wnd.lockGrayList);
        }
    }
}