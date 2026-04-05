using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 竞技场伙伴觉醒技能实力加成列表item
    /// </summary>
    public class GGUIWndArenaStarSkillAddPowerContainerItem : _ATALBasicUISubWnd<GGUIMonoArenaStarSkillAddPowerContainerItem>
    {
        //觉醒技能信息
        private HeroStarSkillInfo _m_starSkillInfo;
        //头像
        private NPGGuiWndTexture _m_wIconWnd;
        //头像背景
        private GGuiWndSprite _m_wIconBg;

        public GGUIWndArenaStarSkillAddPowerContainerItem(GGUIMonoArenaStarSkillAddPowerContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIconWnd?.hideWnd();
            _m_wIconBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIconWnd?.discardTexture();
            _m_wIconBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIconWnd?.discard();
            _m_wIconWnd = null;

            _m_wIconBg?.discard();
            _m_wIconBg = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgIconBg != null)
                _m_wIconBg = new GGuiWndSprite(wnd.imgIconBg);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HeroStarSkillInfo _starSkillInfo)
        {
            _m_starSkillInfo = _starSkillInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_starSkillInfo == null || _m_starSkillInfo.starSkillRefObj == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_starSkillInfo.heroId);
            if (heroInfo == null)
                return;

            //设置觉醒技能名称描述
            ALUGUICommon.setLabelTxt(wnd.txtStarSkillName, TextTranslate.instance.getLanguage(_m_starSkillInfo.starSkillRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtStarSkillDesc, TextTranslate.instance.getLanguage(_m_starSkillInfo.starSkillRefObj.desc, _m_starSkillInfo.curStarSkillLevelRefObj?.skill_desc_args));


            //设置头像
            if (_m_wIconWnd != null)
            {
                _m_wIconWnd.showWnd();
                _m_wIconWnd.setTexture(heroInfo.getIcon());
            }

            //设置品质框
            if (_m_wIconBg != null)
            {
                _m_wIconBg.showWnd();
                _m_wIconBg.setTexture(GCommon.getQualityExtRefObj(ENPItemType.HERO, heroInfo.id)?.hero_head_bg);
            }
        }
    }
}