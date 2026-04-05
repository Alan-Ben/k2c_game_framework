using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMuseumItemList : _ATALBasicUIWnd<GGUIMonoMuseumItemList>
    {
        [NotNull] public static GGUIWndMuseumItemList instance { get { return _g_instance ??= new GGUIWndMuseumItemList(); } }
        private static GGUIWndMuseumItemList _g_instance;
        

        private GGUISubWndMuseumItemListGrid _m_itemGrid;


        public GGUIWndMuseumItemList() 
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMuseumItemList.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMuseumItemList.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_itemGrid?.showWnd();
            
            NPPlayer.instance.museumComp.onItemChg += _onMuseumItemChg;
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_itemGrid?.hideWnd();
            
            NPPlayer.instance.museumComp.onItemChg -= _onMuseumItemChg;
        }
        protected override void _onReset()
        {
            _m_itemGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            
            _m_itemGrid?.discard();
            _m_itemGrid = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            
            if (wnd.itemGrid != null)
                _m_itemGrid = new GGUISubWndMuseumItemListGrid(wnd.itemGrid);
        }


        public void refreshWnd() 
        {
            if (wnd == null || !_m_bIsShow)
                return;

            int obtainedCount = NPPlayer.instance.museumComp.getObtainedItemCount();
            int totalCount = NPPlayer.instance.museumComp.allItemCount;
            
            ALUGUICommon.setLabelTxt(wnd.txtCollectProgress, TextTranslate.instance.getLanguage(TransKeyConst.museum_collectProgress_num_num, obtainedCount, totalCount));
            _m_itemGrid?.refreshWnd();
            wnd.setEmpty(obtainedCount <= 0);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MUSEUM_ITEM_LIST);
        }
        private void _onMuseumItemChg(MuseumItemInfo _itemInfo)
        {
            refreshWnd();
        }
    }
}