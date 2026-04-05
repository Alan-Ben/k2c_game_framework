using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerChapter : _ATALBasicUIWnd<GGUIMonoTowerChapter>
    {
        private static GGUIWndTowerChapter _g_instance = new GGUIWndTowerChapter();

        public static GGUIWndTowerChapter instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerChapter();
                return _g_instance;
            }
        }

        public GGUIWndTowerChapter() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerChapter.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerChapter.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

    
        private GGUIWndTowerChapterItemGrid _m_levelGrid;
        private TowerChapterRefObj _m_towerChapterRefObj;
        private Action _m_onChapterHide;
        private bool _m_isMyCurChapter = false;
        private int _m_curItemIndex = 0;

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            _m_levelGrid?.discard();
            _m_levelGrid = null;
            _m_towerChapterRefObj = null; 
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnResearch, _onBtnResearchClick);
            if (wnd.btnCloseList != null)
                foreach (GameObject btn in wnd.btnCloseList)
                    ALUGUICommon.uncombineBtnClick(btn, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        
            if (null != wnd.towerChapterItemGrid)
            {
                _m_levelGrid = new GGUIWndTowerChapterItemGrid(wnd.towerChapterItemGrid);
            } 
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnResearch, _onBtnResearchClick);
            if (wnd.btnCloseList != null)
                foreach (GameObject btn in wnd.btnCloseList)
                    ALUGUICommon.combineBtnClick(btn, _onBtnCloseClick);
        }

        public void setInfo(long _chapter, Action _onChapterHide)
        {
            _m_towerChapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_chapter);
            _m_onChapterHide = _onChapterHide;
            if (_chapter == NPPlayer.instance.towerComp.curChapterId) 
            {
                _m_isMyCurChapter = true;
                int num = 0;
                // 计算当前关卡在当前章节的位置，反向排序，所以index要取反
                if (_m_towerChapterRefObj != null)
                // _m_curItemIndex = _m_towerChapterRefObj.level_count - (NPPlayer.instance.towerComp.curLevel - 1);
                    _m_curItemIndex = (NPPlayer.instance.towerComp.curLevel - 1);
            }
            else
            {
                _m_isMyCurChapter = false;
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _m_levelGrid?.showWnd();
            _m_levelGrid?.showItemList(_m_towerChapterRefObj);
            ALUGUICommon.setLabelTxt(wnd.txtChapterTitle, TextTranslate.instance.getLanguage(_m_towerChapterRefObj?.name));
            if (_m_isMyCurChapter)
            {
                _m_levelGrid?.moveToTargetTop(_m_curItemIndex);
            }
            else
            {
                _m_levelGrid?.moveToTopNextFrame();
            }
        }
        
        private void _onBtnCloseClick(GameObject _obj)
        {
            _m_onChapterHide?.Invoke();
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_CHAPTER);
        }
        private void _onBtnResearchClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndTowerResearch.instance, UINodeTagConst.C_TOWER_RESEARCH);
        }
    }
}