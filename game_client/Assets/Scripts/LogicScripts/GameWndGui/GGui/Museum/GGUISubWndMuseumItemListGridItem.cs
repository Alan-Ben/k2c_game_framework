using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMuseumItemListGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoMuseumItemListGridItem>
    {
        private MuseumItemInfo _m_itemInfo;
        private List<MuseumItemInfo> _m_itemList;

        private NPGGuiWndTexture _m_iconTexture;
        private NPGGuiWndTexture _m_qualityIconTexture;
        private NPGGuiWndTexture _m_quilityBgTexture;
        private GGUISubWndQualityShowGo _m_qualityShowGoWnd;


        public GGUISubWndMuseumItemListGridItem(GGUIMonoMuseumItemListGridItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_iconTexture?.showWnd();
            _m_qualityIconTexture?.showWnd();
            _m_quilityBgTexture?.showWnd();
            _m_qualityShowGoWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconTexture?.hideWnd();
            _m_qualityIconTexture?.hideWnd();
            _m_quilityBgTexture?.hideWnd();
            _m_qualityShowGoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconTexture?.discardTexture();
            _m_qualityIconTexture?.discardTexture();
            _m_quilityBgTexture?.discardTexture();
            _m_qualityShowGoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_iconTexture?.discard();
            _m_iconTexture = null;
            _m_qualityIconTexture?.discard();
            _m_qualityIconTexture = null;
            _m_quilityBgTexture?.discard();
            _m_quilityBgTexture = null;
            _m_qualityShowGoWnd?.discard();
            _m_qualityShowGoWnd = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onItemClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconTexture = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.imgQualityIcon != null)
                _m_qualityIconTexture = new NPGGuiWndTexture(wnd.imgQualityIcon);
            if (wnd.imgQualityBg != null)
                _m_quilityBgTexture = new NPGGuiWndTexture(wnd.imgQualityBg);
            if (wnd.monoQualityShowGo != null)
                _m_qualityShowGoWnd = new GGUISubWndQualityShowGo(wnd.monoQualityShowGo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onItemClick);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(MuseumItemInfo _itemInfo, List<MuseumItemInfo> _itemList) 
        {
            _m_itemInfo = _itemInfo;
            _m_itemList = _itemList;
            refreshWnd();
        }
        public void refreshWnd() 
        {
            if (wnd == null || !_m_bIsShow || _m_itemInfo == null)
                return;

            wnd.setState(_m_itemInfo.isObtain, _m_itemInfo.isActive, _m_itemInfo.levelProperty != null && _m_itemInfo.levelProperty.canLevelUp());

            _m_iconTexture?.setTexture(_m_itemInfo.refObj.icon);
            _m_qualityIconTexture?.setTexture(_m_itemInfo.refObj.quality_ref?.icon);
            _m_quilityBgTexture?.setTexture(_m_itemInfo.refObj.quality_ext_ref?.museum_item_bg);
            _m_qualityShowGoWnd?.setData(_m_itemInfo.refObj.quality_ext_ref);
            ALUGUICommon.setLabelTxt(wnd.txtQuality, TextTranslate.instance.getLanguage(_m_itemInfo.refObj.quality_ref?.name));
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_itemInfo.refObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_itemInfo.levelProperty?.level ?? 1));
        }
        private void _onItemClick(GameObject _)
        {
            if (_m_itemInfo == null)
                return;

            GGUIWndMuseumItemInfo.instance.refreshWnd(_m_itemInfo, _m_itemList);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMuseumItemInfo.instance, GGUIWndMuseumItemInfo.instance.showWnd, UINodeTagConst.C_MUSEUM_ITEM_INFO);
        }
    }
}