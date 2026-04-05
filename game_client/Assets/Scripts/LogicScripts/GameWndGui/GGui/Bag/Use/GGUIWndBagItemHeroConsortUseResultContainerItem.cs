using ALPackage;
using Common.BagItemUseEnum;
using Common.BagItemUseObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴家人使用道具完成伙伴家人列表item
    /// </summary>
    public class GGUIWndBagItemHeroConsortUseResultContainerItem : _ATALBasicUISubWnd<GGUIMonoBagItemHeroConsortUseResultContainerItem>
    {
        //头像
        private NPGGuiWndTexture _m_wIcon;
        //头像框
        private GGuiWndSprite _m_wIconBgk;
        //属性图标
        private NPGGuiWndTexture _m_wAttrIcon;

        public GGUIWndBagItemHeroConsortUseResultContainerItem(GGUIMonoBagItemHeroConsortUseResultContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wIconBgk?.hideWnd();
            _m_wAttrIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wIconBgk?.discardTexture();
            _m_wAttrIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wIconBgk?.discard();
            _m_wIconBgk = null;
            _m_wAttrIcon?.discard();
            _m_wAttrIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgIconBgk != null)
                _m_wIconBgk = new GGuiWndSprite(wnd.imgIconBgk);

            if (wnd.imgAttrIcon != null)
                _m_wAttrIcon = new NPGGuiWndTexture(wnd.imgAttrIcon);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(BagItemUse_ConsortShowInfo _info)
        {
            if (_info == null)
                return;

            NPGTextureIndex icon = null;
            NPGSpriteIndex iconBgk = null;
            NPGTextureIndex attrIcon = null;
            GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_info.getConsortId());
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_info.getConsortId());
            if (consortInfo != null)
                icon = consortInfo.consortSkinShowInfo?.consortCardImage;
            else
            {
                if (consortRef != null)
                {
                    GConsortSkinRefObj consortSkinRef = GRefdataCoreMgr.instance.consortSkinRefCore.getRef(consortRef.default_skin_id);
                    icon = consortSkinRef?.consort_card_image;
                }
            }
            iconBgk = GCommon.getItemQualitySpIcon(ENPItemType.CONSORT, consortInfo.consortId);

            NPCommonItem item = GCommon.getItemByConsortShowType(_info.getType());
            if(item != null)
                attrIcon = GCommon.getItemTexIcon(item.itemType, item.itemId);

            _refreshWnd(icon, iconBgk, attrIcon, _info.getCount());
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(BagItemUse_HeroShowInfo _info)
        {
            if (_info == null)
                return;

            NPGTextureIndex icon = null;
            NPGSpriteIndex iconBgk = null;
            NPGTextureIndex attrIcon = null;
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_info.getHeroId());
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_info.getHeroId());
            if (heroInfo != null)
                icon = heroInfo.getCardImage();
            else
            {
                if (heroRef != null)
                {
                    HeroSkinRefObj heroSkinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(heroRef.default_skin_id);
                    icon = heroSkinRef?.card_image;
                }
            }
            iconBgk = GCommon.getItemQualitySpIcon(ENPItemType.HERO, _info.getHeroId());

            NPCommonItem item = null;
            switch (_info.getType())
            {

                case EBagItemUse_HeroDrawShowType.POWER:
                    item = GRefdataCoreMgr.instance.npGeneral.power_sys_info;
                    attrIcon = GCommon.getItemTexIcon(item.itemType, item.itemId);
                    break;
            }

            _refreshWnd(icon, iconBgk, attrIcon, _info.getCount());
        }

        //刷新界面
        private void _refreshWnd(NPGTextureIndex _iconIndex, NPGSpriteIndex _iconBgkIndex, NPGTextureIndex _attrIndex, long _value)
        {
            if (wnd == null)
                return;

            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_iconIndex);
            _m_wIconBgk?.showWnd();
            _m_wIconBgk?.setTexture(_iconBgkIndex);
            _m_wAttrIcon?.showWnd();
            _m_wAttrIcon?.setTexture(_attrIndex);
            ALUGUICommon.setLabelTxt(wnd.txtAddValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _value.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }
    }
}