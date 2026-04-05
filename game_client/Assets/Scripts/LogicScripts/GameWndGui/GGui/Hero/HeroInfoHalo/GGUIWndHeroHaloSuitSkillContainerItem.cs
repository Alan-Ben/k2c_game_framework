using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴套系技能列表item
    /// </summary>
    public class GGUIWndHeroHaloSuitSkillContainerItem : _ATALBasicUISubWnd<GGUIMonoHeroHaloSuitSkillContainerItem>
    {
        //套系技能图标
        private NPGGuiWndTexture _m_wIconWnd;
        //套系技能配置
        private HeroSuitSkillRefObj _m_suitSkillRef;
        //等级
        private long _m_lLevel;
        //仅显示当前加成值
        private bool _m_bOnlyShowCurValue;


        public GGUIWndHeroHaloSuitSkillContainerItem(GGUIMonoHeroHaloSuitSkillContainerItem _mono) : base(_mono)
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
        public void setInfo(long _suitSkillId, long _suitSkillLevel, bool _onlyShowCurValue)
        {
            if (wnd == null)
                return;

            _m_suitSkillRef = GRefdataCoreMgr.instance.heroHaloSuitSkillRefCore.getRef(_suitSkillId);
            _m_lLevel = _suitSkillLevel;
            _m_bOnlyShowCurValue = _onlyShowCurValue;
            wnd?.aniUpgrade?.resetAni();
            _refreshWnd();
        }

        /// <summary>
        /// 播放升级激活动画
        /// </summary>
        public void playUpgradeAni()
        {
            wnd?.aniUpgrade?.forcePlay();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_suitSkillRef == null)
                return;

            //技能图标
            if (_m_wIconWnd != null)
            {
                _m_wIconWnd.showWnd();
                _m_wIconWnd.setTexture(_m_suitSkillRef.icon);
            }

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_suitSkillRef.name));
            //设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLastLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_lLevel - 1));
            ALUGUICommon.setLabelTxt(wnd.txtCurLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_lLevel));
            //设置描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_suitSkillRef.simple_desc));
            //设置加成
            PlayerAttrPropertyContainer lastAttrContainer = GRefdataCoreMgr.instance.getHeroSuitSkillLevelAttrContainer(_m_suitSkillRef.id, _m_lLevel-1);
            PlayerAttrPropertyContainer curAttrContainer = GRefdataCoreMgr.instance.getHeroSuitSkillLevelAttrContainer(_m_suitSkillRef.id, _m_lLevel);
            long lastPower = lastAttrContainer.getValue(EBasicAttrType.POWER);
            long curPower = curAttrContainer.getValue(EBasicAttrType.POWER);
            long lastPowerPer = lastAttrContainer.getValue(EBasicAttrType.POWER_PER);
            long curPowerPer = curAttrContainer.getValue(EBasicAttrType.POWER_PER);

            ALUGUICommon.setLabelTxt(wnd.txtLastAddValue, lastPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            ALUGUICommon.setLabelTxt(wnd.txtCurAddValue, curPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            ALUGUICommon.setLabelTxt(wnd.txtLastAddPerValue, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, lastPowerPer/100f));
            ALUGUICommon.setLabelTxt(wnd.txtCurAddPerValue, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, curPowerPer/100f));

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goHaveAddValueShowList, _m_bOnlyShowCurValue ? curPower > 0 : (lastPower > 0 || curPower > 0));
            ALUGUICommon.setGameObjEnable(wnd.goHaveAddPerValueShowList, _m_bOnlyShowCurValue ? curPowerPer > 0 : (lastPowerPer > 0 || curPowerPer > 0));
        }
    }
}