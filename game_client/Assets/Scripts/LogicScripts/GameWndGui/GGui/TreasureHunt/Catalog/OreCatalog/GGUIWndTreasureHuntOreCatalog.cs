using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石图鉴
    /// </summary>
    public class GGUIWndTreasureHuntOreCatalog : _AGGUIWndTreasureHuntCatalogTabWnd<GGUIMonoTreasureHuntOreCatalog>
    {
        private static GGUIWndTreasureHuntOreCatalog _g_instance;
        public static GGUIWndTreasureHuntOreCatalog instance { get { return _g_instance ??= new GGUIWndTreasureHuntOreCatalog(); } }
        
        private GGUIWndTreasureHuntOreCatalogFilter _m_wFilterWnd;
        private GGUIWndTreasureHuntOreItemGrid _m_wOreGrid;

        private List<_ITreasureHuntOreInfo> _m_lAllOreInfoList;//全部矿石信息列表
        private List<_ITreasureHuntOreInfo> _m_lAfterFilterOreInfoList;//过滤后的矿石信息列表
        
        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntOreCatalog.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntOreCatalog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }
        
        protected override List<TreasureHuntCatalogTabRefObj> tabRefObjList { get { return GRefdataCoreMgr.instance.treasureHuntCatalogTabRefCore.refList; } }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.filterWnd != null)
                _m_wFilterWnd = new GGUIWndTreasureHuntOreCatalogFilter(wnd.filterWnd, _onFilterTypeChg);

            if (wnd.oreGrid != null)
            {
                _m_wOreGrid = new GGUIWndTreasureHuntOreItemGrid(wnd.oreGrid, _checkNeedShowOreRedTip);
                _m_wOreGrid.onOreClick += _onClickOre;
            }
        }

        protected override void _onDiscardSub()
        {
            _m_wFilterWnd?.discard();
            _m_wFilterWnd = null;

            if (_m_wOreGrid != null)
            {
                _m_wOreGrid.onOreClick -= _onClickOre;
                _m_wOreGrid.discard();
                _m_wOreGrid = null;    
            }
            
            _m_lAllOreInfoList?.Clear();
            _m_lAllOreInfoList = null;
            _m_lAfterFilterOreInfoList?.Clear();
            _m_lAfterFilterOreInfoList = null;
        }

        protected override void _onShowWndSub()
        {
            _m_wFilterWnd?.showWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_SKILL_CHG, _onOreSkillChg);
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_HAD_DRAW_RECORD_REWARD_CHG, _onOreHadDrawRecordRewardChg);
        }

        protected override void _onHideWndSub()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_SKILL_CHG, _onOreSkillChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_HAD_DRAW_RECORD_REWARD_CHG, _onOreHadDrawRecordRewardChg);
            
            _m_lAllOreInfoList?.Clear();
            _m_lAfterFilterOreInfoList?.Clear();

            _m_wFilterWnd?.hideWnd();
            _m_wOreGrid?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_lAllOreInfoList?.Clear();
            _m_lAfterFilterOreInfoList?.Clear();

            _m_wFilterWnd?.resetWnd();
            _m_wOreGrid?.resetWnd();
        }
        
        /// <summary>
        /// 设置过滤类型
        /// </summary>
        /// <param name="_filterType"></param>
        public void setFilterType(ETreasureHuntOreCatalogFilterType _filterType,  bool _needCallChgCallBack = false)
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

            if (_m_lAllOreInfoList == null)
                _m_lAllOreInfoList = new List<_ITreasureHuntOreInfo>();
            _m_lAllOreInfoList.Clear();
            TreasureHuntUtil.getAllOreInfo(_m_lAllOreInfoList);
            _m_lAllOreInfoList.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if (object.ReferenceEquals(_a, _b)) return 0;
                
                // 已拥有>未拥有
                bool gotA = !(_a.oreState is ETreasureHuntOreState.NOT_GET or ETreasureHuntOreState.NONE);//是否获取A矿石
                bool gotB = !(_b.oreState is ETreasureHuntOreState.NOT_GET or ETreasureHuntOreState.NONE);//是否获取B矿石
                if (gotA != gotB)
                    return -gotA.CompareTo(gotB);

                // 高品质>低品质
                EQuality qualityA = _a.oreRefObj?.quality ?? EQuality.NONE;
                EQuality qualityB = _b.oreRefObj?.quality ?? EQuality.NONE;
                if (qualityA != qualityB)
                    return -qualityA.CompareTo(qualityB);

                return _a.oreId.CompareTo(_b.oreId);
            });

            // 刷新过滤后的矿石图鉴信息
            _refreshAfterFilterOreCatalogInfo();
        }

        /// <summary>
        /// 刷新过滤后的矿石图鉴信息
        /// </summary>
        private void _refreshAfterFilterOreCatalogInfo()
        {
            if(wnd == null)
                return;
            
            TreasureHuntCatalogTabRefObj tabRefObj = this.selectedTabRefObj;//选中的页签
            ETreasureHuntOreCatalogFilterType filterType = _m_wFilterWnd?.nowSelectType ?? ETreasureHuntOreCatalogFilterType.ALL;
            
            if (_m_lAfterFilterOreInfoList == null)
                _m_lAfterFilterOreInfoList = new List<_ITreasureHuntOreInfo>();
            _m_lAfterFilterOreInfoList.Clear();
            int hadGotNum = 0;//已经获取的数量
            int advanceNum = 0;//高级矿石数量
            if (_m_lAllOreInfoList != null && tabRefObj != null)
            {
                foreach (var oreInfo in _m_lAllOreInfoList)
                {
                    if (oreInfo == null || oreInfo.oreRefObj == null || oreInfo.oreRefObj.catalog_tab_id_list == null
                        || !oreInfo.oreRefObj.catalog_tab_id_list.Contains(tabRefObj.tab_id))
                    {
                        continue;
                    }

                    ETreasureHuntOreState oreState = oreInfo.oreState;
                    bool needAddToFilterList = false;//是否需要添加到过滤列表
                    switch (filterType)
                    {
                        case ETreasureHuntOreCatalogFilterType.ALL:
                            needAddToFilterList = true;
                            break;
                        
                        case ETreasureHuntOreCatalogFilterType.HAD:
                            if(oreState is ETreasureHuntOreState.ACTIVATED_NORMAL or ETreasureHuntOreState.NOT_ACTIVATE_NORMAL 
                               or ETreasureHuntOreState.NOT_ACTIVATE_NORMAL or ETreasureHuntOreState.NOT_ACTIVATE_ADVANCED)
                                needAddToFilterList = true;
                            break;                                
                        
                        case ETreasureHuntOreCatalogFilterType.NOT_HAD:
                            if(oreState is ETreasureHuntOreState.NOT_GET)
                                needAddToFilterList = true;
                            break;
                        
                        case ETreasureHuntOreCatalogFilterType.NORMAL_ORE:
                            if(oreState is ETreasureHuntOreState.NOT_ACTIVATE_NORMAL or ETreasureHuntOreState.ACTIVATED_NORMAL)
                                needAddToFilterList = true;
                            break;
                        
                        case ETreasureHuntOreCatalogFilterType.ADVANCE_ORE:
                            if(oreState is ETreasureHuntOreState.NOT_ACTIVATE_ADVANCED or ETreasureHuntOreState.ACTIVATED_ADVANCED)
                                needAddToFilterList = true;
                            break;
                    }

                    if (needAddToFilterList)
                    {
                        _m_lAfterFilterOreInfoList.Add(oreInfo);
                        if (!(oreState is ETreasureHuntOreState.NOT_GET or ETreasureHuntOreState.NONE))
                        {
                            hadGotNum++;
                            if (oreState is ETreasureHuntOreState.ACTIVATED_ADVANCED or ETreasureHuntOreState.NOT_ACTIVATE_ADVANCED)
                                advanceNum++;
                        }
                    }
                }
            }

            int afterFilterOreCount = _m_lAfterFilterOreInfoList.Count;
            
            // 刷新普通矿石收集进度
            string normalProgressKey = string.IsNullOrEmpty(wnd.txtNormalOreCollectProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtNormalOreCollectProgressKey;
            ALUGUICommon.setLabelTxt(wnd.txtNormalOreCollectProgress, TextTranslate.instance.getLanguage(normalProgressKey, hadGotNum, afterFilterOreCount));

            // 刷新高级矿石收集进度
            string advancedProgressKey = string.IsNullOrEmpty(wnd.txtAdvancedOreCollectProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtAdvancedOreCollectProgressKey;
            ALUGUICommon.setLabelTxt(wnd.txtAdvancedOreCollectProgress, TextTranslate.instance.getLanguage(advancedProgressKey, advanceNum, afterFilterOreCount));

            if (_m_wOreGrid != null)
            {
                _m_wOreGrid.showWnd();
                _m_wOreGrid.setData(_m_lAfterFilterOreInfoList);
            }
        }

        /// <summary>
        /// 当过滤类型变化时
        /// </summary>
        private void _onFilterTypeChg(NPGGUICommonFitterMono<ETreasureHuntOreCatalogFilterType> _filterTypeMono)
        {
            _refreshAfterFilterOreCatalogInfo();
        }

        /// <summary>
        /// 检查是否需要显示矿石红点
        /// </summary>
        /// <param name="_oreInfo"></param>
        private bool _checkNeedShowOreRedTip(_ITreasureHuntOreInfo _oreInfo)
        {
            return TreasureHuntUtil.oreCatalogNeedShowRed(_oreInfo);
        }
        
        /// <summary>
        /// 点击矿石
        /// </summary>
        private void _onClickOre(_ITreasureHuntOreInfo _oreInfo)
        {
            if(_oreInfo == null)
                return;

            GGUIWndTreasureHuntOreDetailInfo.instance.setData(_m_lAfterFilterOreInfoList, _oreInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndTreasureHuntOreDetailInfo.instance, () =>
            {
                GGUIWndTreasureHuntOreDetailInfo.instance.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_ORE_DETAIL_INFO);
        }
        
        /// <summary>
        /// 切换选中页签回调
        /// </summary>
        /// <param name="_refObj"></param>
        protected override void _onSelectTab(TreasureHuntCatalogTabRefObj _refObj)
        {
            _refreshAfterFilterOreCatalogInfo();
        }

        protected override void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_ORE_CATALOG);
        }
        
        /// <summary>
        /// 监听矿石技能变化的回调
        /// </summary>
        private void _onOreSkillChg(params object[] _objs)
        {
            // 参数：_objs[0] 为 TreasureHuntGotOreInfo, _objs[1] 为 bool isNormal
            if (_objs == null || _objs.Length < 2 || 
                !(_objs[0] is TreasureHuntGotOreInfo _oreInfo) ||
                !(_objs[1] is bool))
                return;

            _m_wOreGrid?.refreshItem(_oreInfo.oreId);
        }
        
        /// <summary>
        /// 监听矿石质量记录奖励领取变化的回调
        /// </summary>
        private void _onOreHadDrawRecordRewardChg(params object[] _objs)
        {
            // 参数：_objs[0] 为 TreasureHuntGotOreInfo
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntGotOreInfo _oreInfo))
                return;

            _m_wOreGrid?.refreshItem(_oreInfo.oreId);
        }
    }
}