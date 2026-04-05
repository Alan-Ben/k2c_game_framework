using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合图鉴
    /// </summary>
    public class GGUIWndTreasureHuntCompositeCatalog : _ANPGGUIBasicResBarWnd<GGUIMonoTreasureHuntCompositeCatalog>
    {
        private static GGUIWndTreasureHuntCompositeCatalog _g_instance;
        public static GGUIWndTreasureHuntCompositeCatalog instance { get { return _g_instance ??= new GGUIWndTreasureHuntCompositeCatalog(); } }

        private _ATNPGGUIWndCommonMutexFiiterWithFoldToggleWnd<ETreasureHuntCompositeCatalogFilterType, _ATNPGGUIMonoCommonMutexFiiterWithFoldToggleWnd<ETreasureHuntCompositeCatalogFilterType>> _m_wFilterWnd;
        private GGUIWndTreasureHuntCompositeCatalogItemGrid _m_wCompositeCatalogGrid;

        private List<TreasureHuntCompositeCatalogInfo> _m_lAllCompositeCatalogInfoList;//全部组合图鉴数据列表
        private List<TreasureHuntCompositeCatalogInfo> _m_lAfterFilterCompositeCatalogInfoList;//过滤后的组合图鉴数据列表

        public GGUIWndTreasureHuntCompositeCatalog() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntCompositeCatalog.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntCompositeCatalog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }
        public override bool showResBarBySelf { get { return true; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoFilter != null)
                _m_wFilterWnd = new _ATNPGGUIWndCommonMutexFiiterWithFoldToggleWnd<ETreasureHuntCompositeCatalogFilterType, _ATNPGGUIMonoCommonMutexFiiterWithFoldToggleWnd<ETreasureHuntCompositeCatalogFilterType>>(wnd.monoFilter, _onFilterTypeChg);

            if (wnd.monoCompositeCatalogItemGrid != null)
            {
                _m_wCompositeCatalogGrid = new GGUIWndTreasureHuntCompositeCatalogItemGrid(wnd.monoCompositeCatalogItemGrid, _checkNeedShowCompositeCatalogRedTip);
                _m_wCompositeCatalogGrid.onItemClick += _onClickCompositeCatalogItem;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _m_wFilterWnd?.discard();
            _m_wFilterWnd = null;

            if (_m_wCompositeCatalogGrid != null)
            {
                _m_wCompositeCatalogGrid.onItemClick -= _onClickCompositeCatalogItem;
                _m_wCompositeCatalogGrid.discard();
                _m_wCompositeCatalogGrid = null;    
            }
            
            _m_lAllCompositeCatalogInfoList?.Clear();
            _m_lAllCompositeCatalogInfoList = null;
            _m_lAfterFilterCompositeCatalogInfoList?.Clear();
            _m_lAfterFilterCompositeCatalogInfoList = null;
        }

        protected override void _onShowWnd()
        {
            _m_wFilterWnd?.showWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_COMPOSITE_CATALOG_CHG, _onCompositeCatalogChg);
            
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_COMPOSITE_CATALOG_CHG, _onCompositeCatalogChg);
            
            _m_lAllCompositeCatalogInfoList?.Clear();
            _m_lAfterFilterCompositeCatalogInfoList?.Clear();

            _m_wFilterWnd?.hideWnd();
            _m_wCompositeCatalogGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_lAllCompositeCatalogInfoList?.Clear();
            _m_lAfterFilterCompositeCatalogInfoList?.Clear();
            
            _m_wFilterWnd?.resetWnd();
            _m_wCompositeCatalogGrid?.resetWnd();
        }

        /// <summary>
        /// 设置过滤类型
        /// </summary>
        /// <param name="_filterType"></param>
        public void setFilterType(ETreasureHuntCompositeCatalogFilterType _filterType, bool _needCallChgCallBack = false)
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
        
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_lAllCompositeCatalogInfoList == null)
                _m_lAllCompositeCatalogInfoList = new List<TreasureHuntCompositeCatalogInfo>();
            _m_lAllCompositeCatalogInfoList.Clear();
            NPPlayer.instance.treasureHuntComponent.getCompositeCatalogInfoList(_m_lAllCompositeCatalogInfoList);
            _m_lAllCompositeCatalogInfoList.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if (object.ReferenceEquals(_a, _b)) return 0;
                
                // 已拥有>未拥有
                bool gotA = !(_a.state is ETreasureHuntCompositeCatalogState.NOT_GET or ETreasureHuntCompositeCatalogState.NONE);//是否拥有A图鉴
                bool gotB = !(_b.state is ETreasureHuntCompositeCatalogState.NOT_GET or ETreasureHuntCompositeCatalogState.NONE);//是否获取B图鉴
                if (gotA != gotB)
                    return -gotA.CompareTo(gotB);

                // 高品质>低品质
                EQuality qualityA = _a.compositeCatalogRefObj?.quality ?? EQuality.NONE;
                EQuality qualityB = _b.compositeCatalogRefObj?.quality ?? EQuality.NONE;
                if (qualityA != qualityB)
                    return -qualityA.CompareTo(qualityB);

                return _a.compositeCatalogId.CompareTo(_b.compositeCatalogId);
            });
            
            // 刷新过滤后的组合图鉴信息
            _refreshAfterFilterCompositeCatalogInfo();
        }

        /// <summary>
        /// 刷新过滤后的组合图鉴信息
        /// </summary>
        private void _refreshAfterFilterCompositeCatalogInfo()
        {
            if(wnd == null)
                return;
            
            ETreasureHuntCompositeCatalogFilterType filterType = _m_wFilterWnd?.nowSelectType ?? ETreasureHuntCompositeCatalogFilterType.ALL;
            
            if (_m_lAfterFilterCompositeCatalogInfoList == null)
                _m_lAfterFilterCompositeCatalogInfoList = new List<TreasureHuntCompositeCatalogInfo>();
            _m_lAfterFilterCompositeCatalogInfoList.Clear();
            
            int normalHadNum = 0;//已经拥有的普通图鉴数量
            int advancedHadNum = 0;//已经拥有的高级图鉴数量
            
            if (_m_lAllCompositeCatalogInfoList != null)
            {
                foreach (var compositeCatalogInfo in _m_lAllCompositeCatalogInfoList)
                {
                    if (compositeCatalogInfo == null)
                        continue;

                    ETreasureHuntCompositeCatalogState catalogState = compositeCatalogInfo.state;
                    
                    bool needAddToFilterList = false;//是否需要添加到过滤列表
                    switch (filterType)
                    {
                        case ETreasureHuntCompositeCatalogFilterType.ALL:
                            needAddToFilterList = true;
                            break;
                        
                        case ETreasureHuntCompositeCatalogFilterType.HAD:
                            if(compositeCatalogInfo.isCollected)
                                needAddToFilterList = true;
                            break;                                
                        
                        case ETreasureHuntCompositeCatalogFilterType.NOT_HAD:
                            if(catalogState is ETreasureHuntCompositeCatalogState.NONE or ETreasureHuntCompositeCatalogState.NOT_GET)
                                needAddToFilterList = true;
                            break;
                        
                        case ETreasureHuntCompositeCatalogFilterType.NORMAL_CATALOG:
                            if(catalogState is ETreasureHuntCompositeCatalogState.GOT_NORMAL)
                                needAddToFilterList = true;
                            break;
                        
                        case ETreasureHuntCompositeCatalogFilterType.ADVANCE_CATALOG:
                            if(catalogState is ETreasureHuntCompositeCatalogState.GOT_ADVANCED)
                                needAddToFilterList = true;
                            break;
                    }

                    if (needAddToFilterList)
                    {
                        _m_lAfterFilterCompositeCatalogInfoList.Add(compositeCatalogInfo);
                        if (catalogState is ETreasureHuntCompositeCatalogState.GOT_NORMAL or ETreasureHuntCompositeCatalogState.GOT_ADVANCED)
                        {
                            normalHadNum++;
                            if (catalogState is ETreasureHuntCompositeCatalogState.GOT_ADVANCED)
                                advancedHadNum++;
                        }
                    }
                }
            }
            
            // 过滤筛选后的组合图鉴数量
            int afterFilterCompositeCatalogCount = _m_lAfterFilterCompositeCatalogInfoList.Count;
            
            // 刷新普通图鉴收集进度
            string normalProgressKey = string.IsNullOrEmpty(wnd.txtNormalCompositeCatalogCollectProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtNormalCompositeCatalogCollectProgressKey;
            ALUGUICommon.setLabelTxt(wnd.txtNormalCompositeCatalogCollectProgress, TextTranslate.instance.getLanguage(normalProgressKey, normalHadNum, afterFilterCompositeCatalogCount));

            // 刷新高级图鉴收集进度
            string advancedProgressKey = string.IsNullOrEmpty(wnd.txtAdvancedCompositeCatalogCollectProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtAdvancedCompositeCatalogCollectProgressKey;
            ALUGUICommon.setLabelTxt(wnd.txtAdvancedCompositeCatalogCollectProgress, TextTranslate.instance.getLanguage(advancedProgressKey, advancedHadNum, afterFilterCompositeCatalogCount));

            if (_m_wCompositeCatalogGrid != null)
            {
                _m_wCompositeCatalogGrid.showWnd();
                _m_wCompositeCatalogGrid.setData(_m_lAfterFilterCompositeCatalogInfoList);
            }
        }

        /// <summary>
        /// 当过滤类型变化时
        /// </summary>
        private void _onFilterTypeChg(NPGGUICommonFitterMono<ETreasureHuntCompositeCatalogFilterType> _filterTypeMono)
        {
            _refreshAfterFilterCompositeCatalogInfo();
        }

        /// <summary>
        /// 检查是否需要显示组合图鉴红点
        /// </summary>
        /// <param name="_compositeCatalogInfo"></param>
        private bool _checkNeedShowCompositeCatalogRedTip(TreasureHuntCompositeCatalogInfo _compositeCatalogInfo)
        {
            return TreasureHuntUtil.compositeCatalogNeedShowRed(_compositeCatalogInfo);
        }
        
        /// <summary>
        /// 点击组合图鉴item
        /// </summary>
        private void _onClickCompositeCatalogItem(GGUIWndTreasureHuntCompositeCatalogItem _itemWnd)
        {
            if(_itemWnd == null)
                return;
            
            GGUIWndTreasureHuntCompositeCatalogDetail.instance.setData(_m_lAfterFilterCompositeCatalogInfoList, _itemWnd.compositeCatalogInfo, _itemWnd.oreInfoList);
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndTreasureHuntCompositeCatalogDetail.instance, () =>
            {
                GGUIWndTreasureHuntCompositeCatalogDetail.instance.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_COMPOSITE_CATALOG_DETAIL);
        }
        
        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_COMPOSITE_CATALOG);
        }
        
        /// <summary>
        /// 监听组合图鉴变化的回调
        /// </summary>
        private void _onCompositeCatalogChg(params object[] _objs)
        {
            // 参数：_objs[0] 为 TreasureHuntCompositeCatalogInfo
            if (_objs == null || _objs.Length < 1 || 
                !(_objs[0] is TreasureHuntCompositeCatalogInfo _compositeCatalogInfo))
                return;

            _m_wCompositeCatalogGrid?.refreshItem(_compositeCatalogInfo.compositeCatalogId);
        }
    }
}