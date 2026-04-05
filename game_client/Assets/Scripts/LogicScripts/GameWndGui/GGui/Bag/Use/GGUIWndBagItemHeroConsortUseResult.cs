using System.Collections.Generic;
using ALPackage;
using Common.BagItemUseEnum;
using Common.BagItemUseObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴家人使用道具完成界面
    /// </summary>
    public class GGUIWndBagItemHeroConsortUseResult : _ANPGGUIBasicWnd<GGUIMonoBagItemHeroConsortUseResult>
    {
        private static GGUIWndBagItemHeroConsortUseResult _g_instance;
        public static GGUIWndBagItemHeroConsortUseResult instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndBagItemHeroConsortUseResult();
                return _g_instance;
            }
        }

        //属性图标
        private NPGGuiWndTexture _m_wAttrIcon;
        //结果列表
        private GGUIWndBagItemHeroConsortUseResultContainer _m_wResultContainer;

        public GGUIWndBagItemHeroConsortUseResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagItemHeroConsortUseResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagItemHeroConsortUseResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wAttrIcon?.hideWnd();
            _m_wResultContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wAttrIcon?.discardTexture();
            _m_wResultContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wAttrIcon?.discard();
            _m_wAttrIcon = null;
            _m_wResultContainer?.discard();
            _m_wResultContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgAttrIcon != null)
                _m_wAttrIcon = new NPGGuiWndTexture(wnd.imgAttrIcon);

            if (wnd.monoResultContainer != null)
                _m_wResultContainer = new GGUIWndBagItemHeroConsortUseResultContainer(wnd.monoResultContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EBagItemUse_ConsortDrawShowType _type, List<BagItemUse_ConsortShowInfo> _infoList)
        {
            if (wnd == null || _infoList == null)
                return;

            string desc = null;
            NPCommonItem item = GCommon.getItemByConsortShowType(_type);
            if (item != null)
            {
                desc = TextTranslate.instance.getLanguage(TransKeyConst.bag_consortAddValueType_str, GCommon.getItemName(item.itemType, item.itemId));
                ALUGUICommon.setLabelTxt(wnd.txtDesc, desc);

                _m_wAttrIcon?.showWnd();
                _m_wAttrIcon?.setTexture(GCommon.getItemTexIcon(item.itemType, item.itemId));
            }

            _m_wResultContainer?.showWnd();
            _m_wResultContainer?.showItemList(_infoList);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EBagItemUse_HeroDrawShowType _type, List<BagItemUse_HeroShowInfo> _infoList)
        {
            if (wnd == null || _infoList == null)
                return;

            string desc = null;
            NPCommonItem item = null;
            switch (_type)
            {

                case EBagItemUse_HeroDrawShowType.POWER:
                    item = GRefdataCoreMgr.instance.npGeneral.power_sys_info;
                    break;
            }

            if (item != null)
            {
                desc = TextTranslate.instance.getLanguage(TransKeyConst.bag_heroAddValueType_str, GCommon.getItemName(item.itemType, item.itemId));
                ALUGUICommon.setLabelTxt(wnd.txtDesc, desc);

                _m_wAttrIcon?.showWnd();
                _m_wAttrIcon?.setTexture(GCommon.getItemTexIcon(item.itemType, item.itemId));
            }

            _m_wResultContainer?.showWnd();
            _m_wResultContainer?.showItemList(_infoList);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BAG_ITEM_HERO_CONSORT_USE_RESULT);
        }
    }
}