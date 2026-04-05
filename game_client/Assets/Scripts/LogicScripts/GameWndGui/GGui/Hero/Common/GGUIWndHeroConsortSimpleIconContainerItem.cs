using System.Text;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴家人通用展示头像列表item
    /// </summary>
    public class GGUIWndHeroConsortSimpleIconContainerItem : _ATALBasicUISubWnd<GGUIMonoHeroConsortSimpleIconContainerItem>
    {
        //伙伴、家人id
        public long _m_lId;
        //显示类型
        public EHeroConsortSimpleIconShowType _m_eShowType;
        //头像
        private NPGGuiWndTexture _m_wIconWnd;
        //头像背景
        private GGuiWndSprite _m_wIconBg;

        public GGUIWndHeroConsortSimpleIconContainerItem(GGUIMonoHeroConsortSimpleIconContainerItem _mono) : base(_mono)
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

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgIconBg != null)
                _m_wIconBg = new GGuiWndSprite(wnd.imgIconBg);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_id"></param>
        public void setInfo(long _id, EHeroConsortSimpleIconShowType _type)
        {
            if (wnd == null)
                return;

            _m_lId = _id;
            _m_eShowType = _type;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            ENPItemType itemType = ENPItemType.NONE;
            ENPItemType skinItemType = ENPItemType.NONE;
            bool isUnlock = false;
            long skinId = 0;
            NPGSpriteIndex qualityBgIndex = null;
            switch (_m_eShowType)
            {
                case EHeroConsortSimpleIconShowType.HERO:
                    HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_lId);
                    HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lId);
                    skinId = heroRef != null ? heroRef.default_skin_id : 0;
                    if (heroInfo != null)
                        skinId = heroInfo.curSkinId; 
                    isUnlock = heroInfo != null;
                    itemType = ENPItemType.HERO;
                    skinItemType = ENPItemType.HERO_SKIN;
                    qualityBgIndex = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_lId)?.hero_head_bg;
                    break;
                case EHeroConsortSimpleIconShowType.CONSORT:
                    GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lId);
                    GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_lId);
                    skinId = consortRef != null ? consortRef.default_skin_id : 0;
                    if (consortInfo != null && consortInfo.consortSkinShowInfo != null)
                        skinId = consortInfo.consortSkinShowInfo.skinId;
                    isUnlock = consortInfo != null;
                    itemType = ENPItemType.CONSORT;
                    skinItemType = ENPItemType.CONSORT_SKIN;
                    qualityBgIndex = GCommon.getQualityExtRefObj(ENPItemType.CONSORT, _m_lId)?.consort_head_bg;
                    break;
            }

            _m_wIconWnd?.showWnd();
            _m_wIconWnd?.setTexture(GCommon.getItemTexIcon(skinItemType, skinId));

            _m_wIconBg?.showWnd();
            _m_wIconBg?.setTexture(qualityBgIndex);

            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.addColorForRichText(GCommon.getItemName(itemType, _m_lId), isUnlock ? wnd.unlockNameColor : wnd.lockNameColor));

            NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.lockStatInfos, isUnlock ? EGameCommonUnlockType.UNLOCK : EGameCommonUnlockType.LOCK);
        }

        //点击详情按钮
        private void _onClickItem(GameObject _go)
        {
            if (wnd == null)
                return;

            string nameString = string.Empty;
            //称号文本
            string titleString = null;
            ENPItemType itemType = ENPItemType.NONE;
            switch (_m_eShowType)
            {
                case EHeroConsortSimpleIconShowType.HERO:
                    itemType = ENPItemType.HERO;
                    HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lId);
                    nameString = TextTranslate.instance.getLanguage(TransKeyConst.hero_heroName_str, GCommon.getItemName(ENPItemType.HERO, _m_lId));
                    titleString = TextTranslate.instance.getLanguage(TransKeyConst.hero_heroTitle_str, GCommon.getItemName(ENPItemType.HERO_SKIN, heroRef != null ? heroRef.default_skin_id : 0));
                    break;
                case EHeroConsortSimpleIconShowType.CONSORT:
                    itemType = ENPItemType.CONSORT;
                    GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_lId);
                    titleString = TextTranslate.instance.getLanguage(TransKeyConst.hero_consortTitle_str, consortRef?.consort_title);
                    break;
            }

            //品质文本
            NPQualityRefObj qualityRef = GCommon.getItemQualityRef(itemType, _m_lId);
            string qualityString = TextTranslate.instance.getLanguage(TransKeyConst.common_quality_str, qualityRef?.name);
            //获取途径文本
            string accessString = TextTranslate.instance.getLanguage(TransKeyConst.hero_accessWayDesc_str, GCommon.getItemSource(itemType,_m_lId));

            //目标展示文本
            StringBuilder targetStringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(nameString))
                targetStringBuilder.Append($"{nameString}\n");
            if (!string.IsNullOrEmpty(qualityString))
                targetStringBuilder.Append($"{qualityString}\n");
            if (!string.IsNullOrEmpty(titleString))
                targetStringBuilder.Append($"{titleString}\n");
            if (!string.IsNullOrEmpty(accessString))
                targetStringBuilder.Append($"{accessString}");

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                targetStringBuilder.ToString(),
                (RectTransform)_go.transform, wnd.toolTipIntervalX, wnd.toolTipIntervalY));
        }
    }
}