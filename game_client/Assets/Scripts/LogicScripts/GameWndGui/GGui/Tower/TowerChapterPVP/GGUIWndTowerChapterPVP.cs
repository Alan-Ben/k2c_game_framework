using System;
using System.Collections.Generic;
using ALPackage;
using Common.TowerObj;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerChapterPVP : _ATALBasicUIWnd<GGUIMonoTowerChapter>
    {
        private static GGUIWndTowerChapterPVP _g_instance = new GGUIWndTowerChapterPVP();

        public static GGUIWndTowerChapterPVP instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerChapterPVP();
                return _g_instance;
            }
        }
        [NotNull]private List<TowerLevelInfo> _m_itemDataList = new List<TowerLevelInfo>();

        public GGUIWndTowerChapterPVP() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerChapter.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerChapter.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;


        private GGUIWndTowerChapterItemGridPVP _m_levelGrid;
        private TowerChapterRefObj _m_towerChapterRefObj;
        private Action _m_onChapterHide;
        private bool _m_isMyCurChapter = false;
        private int _m_curItemIndex = 0;
        
        private int _m_minPage = 1;
        private int _m_maxPage = 1;
        private bool _m_isReqPageListIng = false;
        private int _m_refreshSerialize = -1;
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_onChapterHide?.Invoke();
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            _m_levelGrid?.discard();
            _m_levelGrid = null;
            _m_towerChapterRefObj = null;
            _m_onChapterHide = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
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
                _m_levelGrid = new GGUIWndTowerChapterItemGridPVP(wnd.towerChapterItemGrid);
                _m_levelGrid.hideWnd();
                wnd.towerChapterItemGrid.scrollRect.onValueChanged.AddListener(_onScrollRectValueChg);

            } 
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            if (wnd.btnCloseList != null)
                foreach (GameObject btn in wnd.btnCloseList)
                    ALUGUICommon.combineBtnClick(btn, _onBtnCloseClick);
        }

        public void setInfo(long _chapter, Action _onChapterHide)
        {
            _m_towerChapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_chapter);
            _m_onChapterHide = _onChapterHide;
            
            if(_m_towerChapterRefObj == null || wnd == null)
                return;

            _m_itemDataList.Clear();
            if (_chapter == NPPlayer.instance.towerComp.curChapterId) 
            {
                _m_isMyCurChapter = true;

                int level = NPPlayer.instance.towerComp.curLevel;
                int levelPage = (level - 1) / wnd.perPageItemNum;
                _m_minPage = levelPage;
                _m_maxPage = levelPage;

                _refreshList(_m_minPage, false, moveToMyLevelPos);
            }
            else
            {
                _m_isMyCurChapter = false;
                _m_minPage = 0;
                _m_maxPage = 0;
                if (_m_levelGrid != null) 
                    _refreshList(_m_minPage, false, _m_levelGrid.moveToTopNextFrame);
            }
        }

        /// <summary>
        /// 刷新宴会列表
        /// </summary>
        /// <param name="page"></param>
        private void _refreshList(int page, bool _isInsertMin = false, Action _onComplete = null)
        {
            if (_m_isReqPageListIng || _m_towerChapterRefObj == null || wnd == null)
                return;
            int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_refreshSerialize = ALSerializeOpMgr.next();
            int refreshSerialize = _m_refreshSerialize;
            _m_isReqPageListIng = true;
            int num = wnd.perPageItemNum;
            
            int startLevel = page * num + 1; // 关卡level是从1开始
            NPPlayer.instance.towerComp.reqPVPLevelInfo(_m_towerChapterRefObj.id,  startLevel, num, (_infos) =>
            {
                if (refreshSerialize != _m_refreshSerialize)
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                    return;
                }
                if(_infos == null || _m_itemDataList == null)
                    return;

                int addCount = 0;
                if (_isInsertMin)
                {
                    List<TowerLevelInfo> tmpList = new List<TowerLevelInfo>();
                    for (int i = page * num + 1; i <= page * num + num; i++)
                    {
                        tmpList.Add(new TowerLevelInfo(_m_towerChapterRefObj.id, i));
                    }
                    _m_itemDataList.InsertRange(0,tmpList);
                    addCount = tmpList.Count;
                }
                else
                {
                    // 限制不超最大关卡数
                    for (int i = page * num + 1; i <= Mathf.Min(page * num + num, _m_towerChapterRefObj.level_count); i++)
                    {
                        _m_itemDataList.Add(new TowerLevelInfo(_m_towerChapterRefObj.id, i));
                        addCount++;
                    }
                }
              
                foreach (Tower_OpponentInfo info in _infos)
                {
                    if (info == null) continue;
                    Tower_PosInfo posInfo = info.getPosInfo();
                    if (posInfo == null) continue;
                    int level = posInfo.getLevel();
                    foreach (TowerLevelInfo item in _m_itemDataList)
                    {
                        if(item != null && item.level == level)
                        {
                            item.updatePlayerInfo(info.getPlayerId());
                            break;
                        }
                    }
                }
                
                if (_m_levelGrid != null)
                {
                    if(!_m_levelGrid.isShow)
                        _m_levelGrid.showWnd();
                    _m_levelGrid.showItemList(_m_itemDataList);
                }
                
                _onComplete?.Invoke();
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                }, 0.2f);
            });
        }
        
        private void _onScrollRectValueChg(Vector2 arg0)
        {
            if (arg0.y <= 0f)
            {
                if(wnd != null && _m_towerChapterRefObj != null && _m_maxPage * wnd.perPageItemNum > _m_towerChapterRefObj.level_count)
                    return;
                // 如果已经在请求了，则不增加page，避免一次添加多次page导致数据错乱
                if(_m_isReqPageListIng)
                    return;
                // 触发更新后停止移动，避免因为没有停止一直拖动导致一直请求刷新
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                if (_m_levelGrid != null && _m_levelGrid.wnd != null &&  _m_levelGrid.wnd.scrollRect != null)
                {
                    _m_levelGrid.wnd.scrollRect.OnEndDrag(pointerEventData);
                    _m_levelGrid.wnd.scrollRect.StopMovement();
                }
                _m_maxPage++;
                _refreshList(_m_maxPage);
            }

            if (arg0.y >= 1f)
            {
                if (_m_minPage <= 0)
                    return;
                if(_m_isReqPageListIng)
                    return;
                    
                // 触发更新后停止移动，避免因为没有停止一直拖动导致一直请求刷新
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                if (_m_levelGrid != null && _m_levelGrid.wnd != null && _m_levelGrid.wnd.scrollRect != null)
                {
                    _m_levelGrid.wnd.scrollRect.OnEndDrag(pointerEventData);
                    _m_levelGrid.wnd.scrollRect.verticalNormalizedPosition = 1f;
                    _m_levelGrid.wnd.scrollRect.StopMovement();
                }

                _m_minPage--;
                int lastCount = _m_itemDataList.Count;
                _refreshList(_m_minPage, true, () =>
                {
                    int addCount = _m_itemDataList.Count - lastCount;
                    _m_levelGrid.moveToTargetTopNextFrame(addCount);
                });
            }
        }

        private void moveToMyLevelPos()
        {
            for (var i = 0; i < _m_itemDataList.Count; i++)
            {
                TowerLevelInfo info = _m_itemDataList[i];
                if (info != null && info.level == NPPlayer.instance.towerComp.curLevel)
                    _m_curItemIndex = i;
            }
            _m_levelGrid?.moveToTargetTopNextFrame(_m_curItemIndex);
        }
        
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_CHAPTER_PVP);
        }
    }
}