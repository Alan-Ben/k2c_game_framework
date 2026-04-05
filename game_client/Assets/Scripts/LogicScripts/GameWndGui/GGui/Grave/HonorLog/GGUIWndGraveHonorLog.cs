using System.Collections.Generic;
using ALPackage;
using Common.GraveObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 荣誉列表界面
    /// </summary>
    public class GGUIWndGraveHonorLog : _ATALBasicUIWnd<GGUIMonoGraveHonorLog>
    {
        private static GGUIWndGraveHonorLog _g_instance = new GGUIWndGraveHonorLog();
    
        public static GGUIWndGraveHonorLog instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGraveHonorLog();
                return _g_instance;
            }
        }
        
        private int _m_page = 1;
        private int _m_nextPage = 1;
    
        private bool _m_isReqPageListIng = false;
        private int _m_refreshSerialize = -1;
        
        private GGUIWndGraveHonorLogGrid _m_itemGridWnd;
        private List<GraveObj_Record> _m_itemDataList = new List<GraveObj_Record>();
        private int _m_graveTypeId;
        private int _m_totalCount = 0; // 总条数

        // <AutoGen:WndDeclaration>
        
        // </AutoGen:WndDeclaration>

        public GGUIWndGraveHonorLog() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGraveHonorLog.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGraveHonorLog.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            // <AutoGen:_onShowWnd>
            
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
            _m_itemDataList?.Clear();
            // <AutoGen:_onHideWnd>
            
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            _m_itemDataList?.Clear();
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            _m_refreshSerialize = ALSerializeOpMgr.next();
            _m_itemDataList?.Clear();
            // <AutoGen:_onDiscard>
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
            // </AutoGen:_onDiscard>
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.itemGrid != null)
            {
                _m_itemGridWnd = new GGUIWndGraveHonorLogGrid(wnd.itemGrid);
                wnd.itemGrid.scrollRect.onValueChanged.AddListener(_onScrollRectValueChg);
            }
                
            // <AutoGen:_onWndInitDone>
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
            // </AutoGen:_onWndInitDone>
        }

        public void setInfo(int _graveTypeId)
        {
            _m_graveTypeId = _graveTypeId;
            _m_page = 1;
            _m_itemDataList?.Clear();
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            _refreshList(_m_page);
            
            // <AutoGen:_refreshWnd>
            
            // </AutoGen:_refreshWnd>
        }
        
        private void _onScrollRectValueChg(Vector2 arg0)
        {
            if (_m_page == _m_nextPage)//没有下一页了
                return;
            if (arg0.y <= 0f)
            {
                _refreshList(_m_nextPage);
            }
        }
        /// <summary>
        /// 刷列表
        /// </summary>
        /// <param name="_pageStartSerial"></param>
        private void _refreshList(int page)
        {
            if(wnd == null)
                return;
            if (_m_isReqPageListIng)
                return;
            int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_refreshSerialize = ALSerializeOpMgr.next();
            int refreshSerialize = _m_refreshSerialize;
            _m_isReqPageListIng = true;
            NPPlayer.instance.graveComp.reqGraveRecordList(_m_graveTypeId, page, wnd.perPageItemNum, (_totalCount, _records) =>
            {
                if (refreshSerialize != _m_refreshSerialize || _records == null)
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                    return;
                }
            
                _m_page = page;
                if (_m_itemDataList != null) 
                    _m_itemDataList.AddRange(_records);
                _m_totalCount = _totalCount;
                if (_records.Count < wnd.perPageItemNum)// 如果小于每页数量，则没有下一页
                    _m_nextPage = _m_page;
                else
                    _m_nextPage = page + 1;
                _refreshGrid();
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                }, 0.2f);
            });
        }
        private void _refreshGrid()
        {
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(_m_totalCount, _m_itemDataList);
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.emptyShowGos, _m_itemDataList?.Count == 0);
        }
        // <AutoGen:Method>
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            // <UserCode name="btnClose">
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GRAVE_HONOR_LOG);
            // </UserCode>
        }
        // </AutoGen:Method>
    }
}