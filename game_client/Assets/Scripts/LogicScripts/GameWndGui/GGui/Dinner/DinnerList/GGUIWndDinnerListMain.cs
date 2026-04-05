using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会列表界面
    /// </summary>
    public class GGUIWndDinnerListMain : _ATALBasicUIWnd<GGUIMonoDinnerListMain>
    {
    
        private static GGUIWndDinnerListMain _g_instance = new GGUIWndDinnerListMain();

        public static GGUIWndDinnerListMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerListMain();
                return _g_instance;
            }
        }

        private GGUIWndDinnerItemGrid _m_dinnerGrid;

        private NPGGUIWndCommonTab _m_tabFriend;
        private NPGGUIWndCommonTab _m_tabGuild;
        private NPGGUIWndCommonTab _m_tabServer;
        private EDinnerListStatType _m_listStatType;
    
        private int _m_page = 1;
        private int _m_nextPage = 1;
    
        private bool _m_isReqPageListIng = false;
        private int _m_refreshSerialize = -1;
        private List<DinnerIndex> _m_itemDataList = new List<DinnerIndex>();

        public GGUIWndDinnerListMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerListMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerListMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _m_itemDataList.Clear();
            _m_page = 1;
            _m_listStatType = EDinnerListStatType.NORMAL;
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            _m_dinnerGrid?.discard();
            _m_dinnerGrid = null;
            _m_tabFriend?.discard();
            _m_tabFriend = null;
            _m_tabGuild?.discard();
            _m_tabGuild = null;
            _m_tabServer?.discard();
            _m_tabServer = null;
        
            _m_page = 1;
            _m_nextPage = 1;
            _m_refreshSerialize = ALSerializeOpMgr.next();
            WinMsg.SendMsg(WinMsgType.ON_DINNER_LIST_DISCARD);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
            if (null != wnd.dinnerItemGrid)
            {
                _m_dinnerGrid = new GGUIWndDinnerItemGrid(wnd.dinnerItemGrid);
                wnd.dinnerItemGrid.scrollRect.onValueChanged.AddListener(_onScrollRectValueChg);
            }

            if (wnd.tabFriend != null)
            {
                _m_tabFriend = new NPGGUIWndCommonTab(wnd.tabFriend);
                _m_tabFriend.clickDelegate += _onTabClickFriend;
            }
            if (wnd.tabGuild != null)
            {
                _m_tabGuild = new NPGGUIWndCommonTab(wnd.tabGuild);
                _m_tabGuild.clickDelegate += _onTabClickGuild;
            }
            if (wnd.tabServer != null)
            {
                _m_tabServer = new NPGGUIWndCommonTab(wnd.tabServer);
                _m_tabServer.clickDelegate += _onTabClickServer;
            }
        }

        private void _clickClose(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_LIST);
        }
    
        private void _onTabClickFriend(bool tab)
        {
            if (null == wnd)
                return;
            _m_listStatType = EDinnerListStatType.FRIEND;
            _m_page = 1;
            _m_itemDataList.Clear();
            refreshWnd();
        }
    
        private void _onTabClickGuild(bool tab)
        {
            if (null == wnd)
                return;
            _m_listStatType = EDinnerListStatType.GUILD;
            _m_page = 1;
            _m_itemDataList.Clear();
            refreshWnd();
        }
    
        private void _onTabClickServer(bool tab)
        {
            if (null == wnd)
                return;
            _m_listStatType = EDinnerListStatType.NORMAL;
            _m_page = 1;
            _m_itemDataList.Clear();
            refreshWnd();
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
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if(null == wnd)
                return;
      
            _m_tabFriend?.setSelected(_m_listStatType == EDinnerListStatType.FRIEND);
            _m_tabGuild?.setSelected(_m_listStatType == EDinnerListStatType.GUILD);
            _m_tabServer?.setSelected(_m_listStatType == EDinnerListStatType.NORMAL);
        
            NPCommonEnumStatInfo<EDinnerListStatType>.setStat(wnd.statInfos, _m_listStatType);
        
            _refreshList(_m_page);
        }
    
        /// <summary>
        /// 刷新宴会列表
        /// </summary>
        /// <param name="_pageStartSerial"></param>
        private void _refreshList(int page)
        {
            if (_m_isReqPageListIng)
                return;
            int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_refreshSerialize = ALSerializeOpMgr.next();
            int refreshSerialize = _m_refreshSerialize;
            _m_isReqPageListIng = true;
            NPPlayer.instance.dinnerComp.reqGetDinnerList(page, wnd.perPageItemNum, (_info) =>
            {
                if (refreshSerialize != _m_refreshSerialize || _info == null)
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                    return;
                }
            
                _m_page = page;
                foreach (var idx in _info.getIdxLIst())
                {
                    _m_itemDataList?.Add(new DinnerIndex(idx));
                }

                if (!_info.getHasNext())//没有下一页了，就重新从第一页开始
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
            _m_dinnerGrid?.showWnd();
            _m_dinnerGrid?.showItemList(_m_itemDataList);
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.emptyShowGos, _m_itemDataList?.Count == 0);
        }
    }
}