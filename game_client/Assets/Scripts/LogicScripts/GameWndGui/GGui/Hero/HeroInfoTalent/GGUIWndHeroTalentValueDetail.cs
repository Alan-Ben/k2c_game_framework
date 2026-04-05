using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴资质详情界面
    /// </summary>
    public class GGUIWndHeroTalentValueDetail : _ANPGGUIBasicWnd<GGUIMonoHeroTalentValueDetail>
    {
        private static GGUIWndHeroTalentValueDetail _g_instance;
        public static GGUIWndHeroTalentValueDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroTalentValueDetail();
                return _g_instance;
            }
        }

        public GGUIWndHeroTalentValueDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroTalentValueDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroTalentValueDetail.objName; } }
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
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        public void setInfo(HeroInfo _heroInfo)
        {
            if (wnd == null || _heroInfo == null)
                return;

            //家人加成
            PlayerAttrPropertyContainer consortAttrContainer = HeroCommon.calConsortAddAttrProperty(_heroInfo);
            //藏品信息
            EquipInfo equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_heroInfo.id);

            //总资质
            ALUGUICommon.setLabelTxt(wnd.txtTotalValue, _heroInfo.getTotalTalent());
            //技能加成
            ALUGUICommon.setLabelTxt(wnd.txtSkillAdd, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, HeroCommon.calAllSkillTalent(_heroInfo)));
            //藏品加成
            ALUGUICommon.setLabelTxt(wnd.txtCollectionAdd, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, equipInfo != null ? equipInfo.talentValue : 0));
            //家人加成
            ALUGUICommon.setLabelTxt(wnd.txtFamilyAdd, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, consortAttrContainer.getValue(EBasicAttrType.TALENT)));
            //服装加成
            long skinTalent = NPPlayer.instance.heroComponent.heroSkinUnionBonusMgr.getTotalPropertyBonus(EBonusPropertyType.TALENT, _heroInfo.toJudgeUnionBonus());
            ALUGUICommon.setLabelTxt(wnd.txtSkinAdd, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, skinTalent));
        }
    }
}