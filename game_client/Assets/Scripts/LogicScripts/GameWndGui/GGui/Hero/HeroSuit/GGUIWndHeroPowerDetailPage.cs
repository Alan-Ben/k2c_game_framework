using ALPackage;
using CommonEnum;
using GOE.BonusSpace;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴战力详情页签
    /// </summary>
    public class GGUIWndHeroPowerDetailPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroPowerDetailPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴信息
        private HeroInfo _m_heroInfo;

        public GGUIWndHeroPowerDetailPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

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

            if (wnd == null)
                return;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        public void setInfo(HeroInfo _heroInfo)
        {
            _m_heroInfo = _heroInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            //星级数据
            HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, _m_heroInfo.star);
            PlayerAttrPropertyModifier starRefAttrModifier = starRef != null ? starRef.self_attr_prop_modifier : null;
            //等级上限
            long levelLimit = 0;
            GRefdataCoreMgr.instance.heroStepRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && levelLimit < _ref.level_limit)
                    levelLimit = _ref.level_limit;
            });
            //星辉加成
            PlayerAttrPropertyContainer haloAttrContainer = HeroCommon.calHaloAddAttrProperty(_m_heroInfo);
            //家人加成
            PlayerAttrPropertyContainer consortAttrContainer = HeroCommon.calConsortAddAttrProperty(_m_heroInfo);
            //藏品信息
            EquipInfo equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_m_heroInfo.id);


            //简介
            ALUGUICommon.setLabelTxt(wnd.txtIntroduction, TextTranslate.instance.getLanguage(_m_heroInfo.heroRefObj.introduction_desc));

            //============实力============
            //总实力
            ALUGUICommon.setLabelTxt(wnd.txtTotalPower, _m_heroInfo.power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //基础实力
            ALUGUICommon.setLabelTxt(wnd.txtBasePower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, HeroCommon.calBasePower(_m_heroInfo).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            //星级实力
            long starPower = starRefAttrModifier != null ? starRefAttrModifier.getPropValue(EBasicAttrType.POWER) : 0;
            long starPowerPer = starRefAttrModifier != null ? starRefAttrModifier.getPropValue(EBasicAttrType.POWER_PER) : 0;
            starPower += NPPlayer.instance.heroComponent.commonUnionBonusMgr.getTotalPropertyBonus(EBonusPropertyType.POWER, _m_heroInfo.toJudgeUnionBonus());
            starPowerPer += NPPlayer.instance.heroComponent.commonUnionBonusMgr.getTotalPropertyBonus(EBonusPropertyType.POWER_PER, _m_heroInfo.toJudgeUnionBonus());
            ALUGUICommon.setLabelTxt(wnd.txtStarPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, starPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtStarPowerPer, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, starPowerPer / 100f));
            //家人实力
            ALUGUICommon.setLabelTxt(wnd.txtConsortPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, consortAttrContainer.getValue(EBasicAttrType.POWER).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtConsortPowerPer, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, consortAttrContainer.getValue(EBasicAttrType.POWER_PER) / 100f));
            //藏品实力百分比
            long equipValue = equipInfo != null ? equipInfo.skillAddValue : 0;
            ALUGUICommon.setLabelTxt(wnd.txtEquipPowerPer, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, equipValue / 100f));
            //星辉实力
            ALUGUICommon.setLabelTxt(wnd.txtHaloPowerPer, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num,  haloAttrContainer.getValue(EBasicAttrType.POWER_PER) / 100f));
            ALUGUICommon.setLabelTxt(wnd.txtHaloPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, haloAttrContainer.getValue(EBasicAttrType.POWER).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            //道具实力
            ALUGUICommon.setLabelTxt(wnd.txtBagItemPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_heroInfo.itemAddPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            //游历实力
            ALUGUICommon.setLabelTxt(wnd.txtTravelPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_heroInfo.travelAddPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            //竞技场实力
            ALUGUICommon.setLabelTxt(wnd.txtArenaPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_heroInfo.arenaAddPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            //太空寻宝实力
            _AUnionBonusMgr treasureHuntBonusMgr = NPPlayer.instance.getUnionBonusMgrByTag(EUnionBonusMgrTag.TREASURE_HUNT);
            long treasureHuntPower = treasureHuntBonusMgr != null ? treasureHuntBonusMgr.getTotalPropertyBonus(EBonusPropertyType.POWER, _m_heroInfo.toJudgeUnionBonus()) : 0;
            long treasureHuntPowerPer = treasureHuntBonusMgr != null ? treasureHuntBonusMgr.getTotalPropertyBonus(EBonusPropertyType.POWER_PER, _m_heroInfo.toJudgeUnionBonus()) : 0;
            ALUGUICommon.setLabelTxt(wnd.txtTreasureHuntPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, treasureHuntPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtTreasureHuntPowerPer, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, treasureHuntPowerPer / 100f));

            //============资质============
            //总资质
            ALUGUICommon.setLabelTxt(wnd.txtTotalTalent, HeroCommon.calTalent(_m_heroInfo));
            //基础资质
            ALUGUICommon.setLabelTxt(wnd.txtBaseTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, HeroCommon.calInitTalent(_m_heroInfo.id)));
            //技能资质
            ALUGUICommon.setLabelTxt(wnd.txtSkillTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, HeroCommon.calBaseSkillTalent(_m_heroInfo)));
            //进阶资质
            ALUGUICommon.setLabelTxt(wnd.txtStepUpTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, HeroCommon.calStepUpAddTalent(_m_heroInfo)));
            //藏品资质
            ALUGUICommon.setLabelTxt(wnd.txtEquipTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, equipInfo != null ? equipInfo.talentValue : 0));
            //家人资质
            ALUGUICommon.setLabelTxt(wnd.txtConsortTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, consortAttrContainer.getValue(EBasicAttrType.TALENT)));
            //服装资质
            long skinTalent = NPPlayer.instance.heroComponent.heroSkinUnionBonusMgr.getTotalPropertyBonus(EBonusPropertyType.TALENT, _m_heroInfo.toJudgeUnionBonus());
            ALUGUICommon.setLabelTxt(wnd.txtSkinTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, skinTalent));
            //太空寻宝资质
            long treasureHuntTalent = treasureHuntBonusMgr != null ? treasureHuntBonusMgr.getTotalPropertyBonus(EBonusPropertyType.TALENT, _m_heroInfo.toJudgeUnionBonus()) : 0;
            ALUGUICommon.setLabelTxt(wnd.txtTreasureHuntTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, treasureHuntTalent));

            //============等级上限============
            //总等级上限
            ALUGUICommon.setLabelTxt(wnd.txtTotalLevelLimit, levelLimit);
            //基础等级上限
            ALUGUICommon.setLabelTxt(wnd.txtBaseLevelLimit, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, levelLimit));
        }
    }
}