using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能等级详情界面
    /// </summary>
    public class GGUIWndHeroStarSkillLevelDetailToolTip : _ATNPGGUIWndCommonItemToolTip<GGUIMonoHeroStarSkillLevelDetailToolTip>
    {
        //等级详情列表
        private GGUIWndHeroStarSkillLevelDetailContainer _m_wLevelDetailContainer;

        public GGUIWndHeroStarSkillLevelDetailToolTip() : base(GGUIMonoHeroStarSkillLevelDetailToolTip.assetPath, GGUIMonoHeroStarSkillLevelDetailToolTip.objName)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroStarSkillLevelDetailToolTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroStarSkillLevelDetailToolTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            _m_wLevelDetailContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            _m_wLevelDetailContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_wLevelDetailContainer?.discard();
            _m_wLevelDetailContainer = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            if (wnd.monoLevelDescContainer != null)
                _m_wLevelDetailContainer = new GGUIWndHeroStarSkillLevelDetailContainer(wnd.monoLevelDescContainer);
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_STAR_SKILL_LEVEL_DETAIL);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_starSkillId"></param>
        public void setInfo(HeroInfo _heroInfo, long _starSkillId, RectTransform _targetTransRoot, float _intervalX, float _intervalY)
        {
            if (wnd == null || _heroInfo == null)
                return;

            HeroStarSkillInfo heroStarSkillInfo = _heroInfo.starSkillInfoMgr.getStarSkillInfo(_starSkillId);
            if (heroStarSkillInfo == null || heroStarSkillInfo.starSkillRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtLevelName, TextTranslate.instance.getLanguage(TransKeyConst.hero_starSkillLevelName_level_name, heroStarSkillInfo.level, heroStarSkillInfo.starSkillRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(heroStarSkillInfo.starSkillRefObj.desc, heroStarSkillInfo.curStarSkillLevelRefObj?.skill_desc_args));

            if (_m_wLevelDetailContainer != null)
            {
                _m_wLevelDetailContainer.showWnd();
                _m_wLevelDetailContainer.showItemList(_heroInfo, _starSkillId);
            }

            setPos(_targetTransRoot, _intervalX, _intervalY);
        }
    }
}