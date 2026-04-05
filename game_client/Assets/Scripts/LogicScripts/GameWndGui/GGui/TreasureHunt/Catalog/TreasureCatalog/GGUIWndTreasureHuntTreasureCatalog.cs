using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物图鉴
    /// </summary>
    public class GGUIWndTreasureHuntTreasureCatalog : _AGGUIWndTreasureHuntCatalogTabWnd<GGUIMonoTreasureHuntTreasureCatalog>
    {
        private static GGUIWndTreasureHuntTreasureCatalog _g_instance;
        public static GGUIWndTreasureHuntTreasureCatalog instance { get { return _g_instance ??= new GGUIWndTreasureHuntTreasureCatalog(); } }

        private GGUIWndTreasureHuntTreasureCatalogFilter _m_wFilterWnd;
        private GGUIWndTreasureHuntTreasureItemGrid _m_wTreasureGrid;

        private List<_ITreasureHuntTreasureInfo> _m_lAllTreasureInfoList;//全部奇物信息列表
        private List<_ITreasureHuntTreasureInfo> _m_lAfterFilterTreasureInfoList;//过滤后的奇物信息列表

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntTreasureCatalog.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntTreasureCatalog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override List<TreasureHuntCatalogTabRefObj> tabRefObjList { get { return GRefdataCoreMgr.instance.treasureHuntCatalogTabRefCore.refList; } }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.filterWnd != null)
                _m_wFilterWnd = new GGUIWndTreasureHuntTreasureCatalogFilter(wnd.filterWnd, _onFilterTypeChg);

            if (wnd.treasureGrid != null)
            {
                _m_wTreasureGrid = new GGUIWndTreasureHuntTreasureItemGrid(wnd.treasureGrid, _checkNeedShowTreasureRedTip);
                _m_wTreasureGrid.onTreasureClick += _onClickTreasure;
            }
        }

        protected override void _onDiscardSub()
        {
            _m_wFilterWnd?.discard();
            _m_wFilterWnd = null;

            if (_m_wTreasureGrid != null)
            {
                _m_wTreasureGrid.onTreasureClick -= _onClickTreasure;
                _m_wTreasureGrid.discard();
                _m_wTreasureGrid = null;    
            }
            
            _m_lAllTreasureInfoList?.Clear();
            _m_lAllTreasureInfoList = null;
            _m_lAfterFilterTreasureInfoList?.Clear();
            _m_lAfterFilterTreasureInfoList = null;
        }

        protected override void _onShowWndSub()
        {
            _m_wFilterWnd?.showWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_LEVEL_CHG, _onTreasureSkillChg);
        }

        protected override void _onHideWndSub()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_LEVEL_CHG, _onTreasureSkillChg);
            
            _m_lAllTreasureInfoList?.Clear();
            _m_lAfterFilterTreasureInfoList?.Clear();

            _m_wFilterWnd?.hideWnd();
            _m_wTreasureGrid?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_lAllTreasureInfoList?.Clear();
            _m_lAfterFilterTreasureInfoList?.Clear();
            
            _m_wFilterWnd?.resetWnd();
            _m_wTreasureGrid?.resetWnd();
        }

        public override bool needDiscardOnSwitch { get { return true; } }

        /// <summary>
        /// 设置过滤类型
        /// </summary>
        /// <param name="_filterType"></param>
        public void setFilterType(ETreasureHuntTreasureCatalogFilterType _filterType, bool _needCallChgCallBack = false)
        {
            _m_wFilterWnd?.setSelectType(_filterType, _needCallChgCallBack);
        }

        /// <summary>
        /// 选择默认过滤类型
        /// </summary>
        /// <param name="_needCallChgCallBack"></param>
        public void selectDefaultFilterType(bool _needCallChgCallBack = false)
        {
            _m_wFilterWnd?.selectDefaultType(_needCallChgCallBack);
        }
        
        protected override void _refreshWndSub()
        {
            if(wnd == null)
                return;

            if (_m_lAllTreasureInfoList == null)
                _m_lAllTreasureInfoList = new List<_ITreasureHuntTreasureInfo>();
            _m_lAllTreasureInfoList.Clear();
            TreasureHuntUtil.getAllTreasureInfo(_m_lAllTreasureInfoList);
            _m_lAllTreasureInfoList.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if (object.ReferenceEquals(_a, _b)) return 0;
                
                // 已拥有>未拥有
                bool gotA = !(_a.treasureState is ETreasureHuntTreasureState.NOT_GET or ETreasureHuntTreasureState.NONE);//是否获取A奇物
                bool gotB = !(_b.treasureState is ETreasureHuntTreasureState.NOT_GET or ETreasureHuntTreasureState.NONE);//是否获取B奇物
                if (gotA != gotB)
                    return -gotA.CompareTo(gotB);

                // 高品质>低品质
                EQuality qualityA = _a.treasureRefObj?.quality ?? EQuality.NONE;
                EQuality qualityB = _b.treasureRefObj?.quality ?? EQuality.NONE;
                if (qualityA != qualityB)
                    return -qualityA.CompareTo(qualityB);

                return _a.treasureId.CompareTo(_b.treasureId);
            });

            // 刷新过滤后的奇物图鉴信息
            _refreshAfterFilterTreasureCatalogInfo();
        }

        /// <summary>
        /// 刷新过滤后的奇物图鉴信息
        /// </summary>
        private void _refreshAfterFilterTreasureCatalogInfo()
        {
            if(wnd == null)
                return;
            
            TreasureHuntCatalogTabRefObj tabRefObj = this.selectedTabRefObj;//选中的页签
            ETreasureHuntTreasureCatalogFilterType filterType = _m_wFilterWnd?.nowSelectType ?? ETreasureHuntTreasureCatalogFilterType.ALL;
            
            if (_m_lAfterFilterTreasureInfoList == null)
                _m_lAfterFilterTreasureInfoList = new List<_ITreasureHuntTreasureInfo>();
            _m_lAfterFilterTreasureInfoList.Clear();
            
            int hadGotNum = 0;//已经获取的数量
            if (_m_lAllTreasureInfoList != null && tabRefObj != null)
            {
                foreach (var treasureInfo in _m_lAllTreasureInfoList)
                {
                    if (treasureInfo == null || treasureInfo.treasureRefObj == null || treasureInfo.treasureRefObj.catalog_tab_id_list == null
                        || !treasureInfo.treasureRefObj.catalog_tab_id_list.Contains(tabRefObj.tab_id))
                    {
                        continue;
                    }

                    ETreasureHuntTreasureState treasureState = treasureInfo.treasureState;
                    bool needAddToFilterList = false;//是否需要添加到过滤列表
                    switch (filterType)
                    {
                        case ETreasureHuntTreasureCatalogFilterType.ALL:
                            needAddToFilterList = true;
                            break;
                        
                        case ETreasureHuntTreasureCatalogFilterType.HAD:
                            if(treasureState is ETreasureHuntTreasureState.GOT_ACTIVATED or ETreasureHuntTreasureState.GOT_NOT_ACTIVATE)
                                needAddToFilterList = true;
                            break;                                
                        
                        case ETreasureHuntTreasureCatalogFilterType.NOT_HAD:
                            if(treasureState == ETreasureHuntTreasureState.NOT_GET)
                                needAddToFilterList = true;
                            break;
                    }

                    if (needAddToFilterList)
                    {
                        _m_lAfterFilterTreasureInfoList.Add(treasureInfo);
                        if (treasureState is ETreasureHuntTreasureState.GOT_ACTIVATED or ETreasureHuntTreasureState.GOT_NOT_ACTIVATE)
                        {
                            hadGotNum++;
                        }
                    }
                }
            }

            int afterFilterTreasureCount = _m_lAfterFilterTreasureInfoList.Count;
            
            // 刷新奇物收集进度
            string progressKey = string.IsNullOrEmpty(wnd.txtTreasureCollectProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtTreasureCollectProgressKey;
            ALUGUICommon.setLabelTxt(wnd.txtTreasureCollectProgress, TextTranslate.instance.getLanguage(progressKey, hadGotNum, afterFilterTreasureCount));

            if (_m_wTreasureGrid != null)
            {
                _m_wTreasureGrid.showWnd();
                _m_wTreasureGrid.setData(_m_lAfterFilterTreasureInfoList);
            }
        }

        /// <summary>
        /// 当过滤类型变化时
        /// </summary>
        private void _onFilterTypeChg(NPGGUICommonFitterMono<ETreasureHuntTreasureCatalogFilterType> _filterTypeMono)
        {
            _refreshAfterFilterTreasureCatalogInfo();
        }

        /// <summary>
        /// 检查是否需要显示奇物红点
        /// </summary>
        /// <param name="_treasureInfo"></param>
        private bool _checkNeedShowTreasureRedTip(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            return TreasureHuntUtil.treasureCatalogNeedShowRed(_treasureInfo);
        }
        
        /// <summary>
        /// 点击奇物
        /// </summary>
        private void _onClickTreasure(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            if(_treasureInfo == null)
                return;
            
            GGUIWndTreasureHuntTreasureDetailInfo.instace.setData(_m_lAfterFilterTreasureInfoList, _treasureInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndTreasureHuntTreasureDetailInfo.instace, () =>
            {
                GGUIWndTreasureHuntTreasureDetailInfo.instace.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_TREASURE_DETAIL_INFO);
        }
        
        protected override void _onSelectTab(TreasureHuntCatalogTabRefObj _refObj)
        {
            _refreshAfterFilterTreasureCatalogInfo();
        }

        protected override void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_TREASURE_CATALOG);
        }
        
        /// <summary>
        /// 监听奇物技能变化的回调
        /// </summary>
        private void _onTreasureSkillChg(params object[] _objs)
        {
            // 参数：_objs[0] 为 TreasureHuntGotTreasureInfo
            if (_objs == null || _objs.Length < 1 || 
                !(_objs[0] is TreasureHuntGotTreasureInfo _treasureInfo))
                return;

            // 刷新全部奇物信息(因为奇物升级需要道具可能相同, 所以这里有奇物技能升级成功, 可能会影响到其他奇物是否可以升级, 所以只能刷新全部)
            _m_wTreasureGrid?.forceRefreshAllItem();
        }
    }
}