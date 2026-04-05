using ALPackage;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能等级详情列表item
    /// </summary>
    public class GGUIWndHeroStarSkillLevelDetailContainerItem : _ATALBasicUISubWnd<GGUIMonoHeroStarSkillLevelDetailContainerItem>
    {

        public GGUIWndHeroStarSkillLevelDetailContainerItem(GGUIMonoHeroStarSkillLevelDetailContainerItem _mono) : base(_mono)
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
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_starSkillLevelRef"></param>
        public void setInfo(HeroInfo _heroInfo, HeroStarSkillLevelRefObj _starSkillLevelRef)
        {
            if (wnd == null || _heroInfo == null || _starSkillLevelRef == null)
                return;

            HeroStarSkillInfo heroStarSkillInfo = _heroInfo.starSkillInfoMgr.getStarSkillInfo(_starSkillLevelRef.skill_id);
            if (heroStarSkillInfo == null || heroStarSkillInfo.starSkillRefObj == null)
                return;

            long curLevel = heroStarSkillInfo.level;
            string levelDescStr = TextTranslate.instance.getLanguage(TransKeyConst.hero_starSkillLevelDesc_num_str,
                _starSkillLevelRef.skill_level,
                TextTranslate.instance.getLanguage(heroStarSkillInfo.starSkillRefObj.desc, _starSkillLevelRef.skill_desc_args));

            //根据是否是当前等级设置颜色
            if (curLevel == _starSkillLevelRef.skill_level)
                levelDescStr = GCommon.addColorForRichText(levelDescStr, wnd.curLevelColor);
            else
                levelDescStr = GCommon.addColorForRichText(levelDescStr, wnd.notCurLevelColor);

            ALUGUICommon.setLabelTxt(wnd.txtDesc, levelDescStr);
        }

    }
}