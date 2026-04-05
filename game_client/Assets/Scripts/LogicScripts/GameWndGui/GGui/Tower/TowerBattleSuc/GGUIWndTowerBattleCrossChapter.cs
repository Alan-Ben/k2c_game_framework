using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerBattleCrossChapter : _ATALBasicUIWnd<GGUIMonoTowerBattleCrossChapter>
    {
        private static GGUIWndTowerBattleCrossChapter _g_instance = new GGUIWndTowerBattleCrossChapter();

        public static GGUIWndTowerBattleCrossChapter instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerBattleCrossChapter();
                return _g_instance;
            }
        }

        private NPGGUIWndCommonItemContainer _m_itemContainer;
    
        private TowerChapterRefObj _m_towerChapterRefObj;
        private NPGGuiWndTexture _m_towerBg;
        private Action _m_setDealerDone;

        public GGUIWndTowerBattleCrossChapter() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerBattleCrossChapter.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerBattleCrossChapter.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_towerBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_towerBg?.discardTexture();
            if (_m_itemContainer != null)
                _m_itemContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(null == wnd)
                return;
            _m_towerBg?.discard();
            _m_towerBg = null;
        
            // 奖励列表
            _m_itemContainer?.discard();
            _m_itemContainer = null;
        
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        
            if (wnd.imgChapter != null)
                _m_towerBg = new NPGGuiWndTexture(wnd.imgChapter);
            // 奖励列表
            if (null != wnd.itemContainer)
                _m_itemContainer = new NPGGUIWndCommonItemContainer(wnd.itemContainer);
        
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        public void setInfo(long _chapterId, Action _setDealerDone)
        {
            _m_towerChapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_chapterId);
            _m_setDealerDone = _setDealerDone;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd || _m_towerChapterRefObj == null)
                return;
            _m_towerBg?.showWnd();
            _m_towerBg?.setTexture(_m_towerChapterRefObj.banner_tex);
            if (_m_towerChapterRefObj.gain_item_list != null && _m_towerChapterRefObj.gain_item_list.Count > 0)
            {
                _m_itemContainer?.showWnd();
                _m_itemContainer?.showItemList(_m_towerChapterRefObj.gain_item_list);
            }
            else
            {
                _m_itemContainer?.hideWnd();
            }
        
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_towerChapterRefObj.chapter_unlock_desc, _m_towerChapterRefObj.chapter_unlock_desc_args));
        }
    
        private void _onBtnCloseClick(GameObject _go)
        {
            _m_setDealerDone?.Invoke();
        }
    }
}