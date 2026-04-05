using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴套系技能列表item
    /// </summary>
    public class GGUIWndHeroSuitSkillContainerItem : _ATALBasicUISubWnd<GGUIMonoHeroSuitSkillContainerItem>
    {
        //套系数据
        private HeroSuitInfo _m_heroSuitInfo;
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //套系技能图标
        private NPGGuiWndTexture _m_wIconWnd;
        //套系技能配置
        private HeroSuitSkillRefObj _m_suitSkillRef;

        public GGUIWndHeroSuitSkillContainerItem(GGUIMonoHeroSuitSkillContainerItem _mono) : base(_mono)
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

            if (wnd == null)
                return;

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
        /// <param name="_suitSkillId"></param>
        /// <param name="_suitInfo"></param>
        public void setInfo(HeroSuitInfo _suitInfo, HeroInfo  _heroInfo, long _suitSkillId)
        {
            if (wnd == null)
                return;

            _m_heroSuitInfo = _suitInfo;
            _m_heroInfo = _heroInfo;
            _m_suitSkillRef = GRefdataCoreMgr.instance.heroHaloSuitSkillRefCore.getRef(_suitSkillId);
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_suitSkillRef == null || _m_heroSuitInfo == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            HeroSuitSkillInfo suitSkillInfo = _m_heroSuitInfo.getSuitSkillInfo(_m_suitSkillRef.id);
            long suitSkillLevel = suitSkillInfo != null ? suitSkillInfo.level : 0;
            long skillMaxLevel = GRefdataCoreMgr.instance.getSuitSkillMaxLevelInHero(_m_heroInfo.heroRefObj.halo_id, _m_suitSkillRef.id);
            long defaultLevel = GRefdataCoreMgr.instance.getSuitSkillDefaultLevel(_m_heroInfo.heroRefObj.halo_id, _m_suitSkillRef.id);
            long totalLevel = (skillMaxLevel + defaultLevel) * _m_heroSuitInfo.suitTotalHeroCount;

            //技能图标
            if (_m_wIconWnd != null)
            {
                _m_wIconWnd.showWnd();
                _m_wIconWnd.setTexture(_m_suitSkillRef.icon);
            }

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.hero_suitSkillNameLevel_name_num_num, _m_suitSkillRef.name, suitSkillLevel, totalLevel));

            //设置描述
            PlayerAttrPropertyContainer curAttrContainer = null;
            if(suitSkillInfo != null)
                curAttrContainer = GRefdataCoreMgr.instance.getHeroSuitSkillLevelAttrContainer(_m_suitSkillRef.id, suitSkillInfo.level);
            long curPowerPer = curAttrContainer != null ? curAttrContainer.getValue(EBasicAttrType.POWER_PER) : 0;
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_suitSkillRef.desc, 
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, curPowerPer / 100f)));
        }
    }
}