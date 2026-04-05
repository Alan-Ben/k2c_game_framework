using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 选择太空区域窗口
    /// </summary>
    public class GGUIWndTreasureHuntSelectArea : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntSelectArea>
    {
        private static GGUIWndTreasureHuntSelectArea _g_instance;
        public static GGUIWndTreasureHuntSelectArea instance { get { return _g_instance ??= new GGUIWndTreasureHuntSelectArea(); } }

        private List<GGUISubWndTreasureHuntArea> _m_lAreaWndList;
        private GGUIWndTreasureHuntOreItemContainer _m_wCanGainOreContainer;
        
        private TreasureHuntAreaRefObj _m_selectedAreaRefObj;//选中的区域配表数据
        private List<_ITreasureHuntOreInfo> _m_lSelectedAreaOreInfoList;//选中区域可获取的矿石信息列表
        private int _m_iGetNormalOreNum;//选中区域已经获取的普通矿石数量
        private int _m_iGetAdvancedOreNum;//选中区域已经获取的高级矿石数量
        private List<_ITreasureHuntTreasureInfo> _m_lSelectedAreaTreasureInfoList;//选中区域可获取的奇物信息列表
        private int _m_iGetTreasureNum;//选中区域已经获取的奇物数量

        public GGUIWndTreasureHuntSelectArea() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntSelectArea.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntSelectArea.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            // 初始化区域子窗口列表
            if (wnd.areaMonoList != null)
            {
                _m_lAreaWndList = new List<GGUISubWndTreasureHuntArea>();
                foreach (var areaMono in wnd.areaMonoList)
                {
                    if(areaMono == null)
                        continue;
                        
                    GGUISubWndTreasureHuntArea areaWnd = new GGUISubWndTreasureHuntArea(areaMono);
                    areaWnd.onAreaClick += _onAreaClick;
                    _m_lAreaWndList.Add(areaWnd);
                }
            }

            if (wnd.monoCanGainOreContainer != null)
                _m_wCanGainOreContainer = new GGUIWndTreasureHuntOreItemContainer(wnd.monoCanGainOreContainer);

            ALUGUICommon.combineBtnClick(wnd.btnShowAllCanGain, _onClickShowAllCanGain);
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onClickGoto);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturn);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnShowAllCanGain, _onClickShowAllCanGain);
                ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onClickGoto);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturn);
            }
            _m_selectedAreaRefObj = null;
            _m_lSelectedAreaOreInfoList?.Clear();
            _m_lSelectedAreaOreInfoList = null;
            _m_lSelectedAreaTreasureInfoList?.Clear();
            _m_lSelectedAreaTreasureInfoList = null;

            if (_m_lAreaWndList != null)
            {
                foreach (var areaWnd in _m_lAreaWndList)
                {
                    if(areaWnd != null)
                    {
                        areaWnd.onAreaClick -= _onAreaClick;
                        areaWnd.discard();
                    }
                }
                _m_lAreaWndList.Clear();
                _m_lAreaWndList = null;
            }

            _m_wCanGainOreContainer?.discard();
            _m_wCanGainOreContainer = null;
        }

        protected override void _onShowWnd()
        {
            _updateSelectedAreaInfo();
            _refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_SELECT_AREA_GO, _onBtnGoTo);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_SELECT_TREASURE_HUNT_SELECT_AREA, _onSelectArea);
        }

        protected override void _onHideWnd()
        {
            _m_lSelectedAreaOreInfoList?.Clear();
            _m_lSelectedAreaTreasureInfoList?.Clear();

            if (_m_lAreaWndList != null)
            {
                foreach (var areaWnd in _m_lAreaWndList)
                {
                    areaWnd?.hideWnd();
                }
            }

            _m_wCanGainOreContainer?.hideWnd();
            
            // 隐藏窗口时, 设置所有解锁新区域红点为已读
            NPPlayer.instance.treasureHuntComponent.redTipDealer.setReadAllUnlockNewAreaRedTip();
            
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_SELECT_AREA_GO, _onBtnGoTo);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_SELECT_TREASURE_HUNT_SELECT_AREA, _onSelectArea);
        }

        protected override void _onReset()
        {
            _m_lSelectedAreaOreInfoList?.Clear();
            _m_lSelectedAreaTreasureInfoList?.Clear();
            
            if (_m_lAreaWndList != null)
            {
                foreach (var areaWnd in _m_lAreaWndList)
                {
                    areaWnd?.resetWnd();
                }
            }

            _m_wCanGainOreContainer?.resetWnd();
        }
        
        /// <summary>
        /// 设置选中的区域
        /// </summary>
        /// <param name="_areaRefObj"></param>
        public void setSelectedArea(TreasureHuntAreaRefObj _areaRefObj)
        {
            _m_selectedAreaRefObj = _areaRefObj;
            _updateSelectedAreaInfo();
            
            // 刷新区域选中状态
            _refreshAreaList();
            
            // 刷新选中区域信息
            _refreshSelectedAreaInfo();
            
            // 设置解锁新区域红点为已读
            if(_m_selectedAreaRefObj != null)
                NPPlayer.instance.treasureHuntComponent.redTipDealer.setReadUnlockNewAreaRedTip(_m_selectedAreaRefObj.area_id);
        }

        private void _updateSelectedAreaInfo()
        {
            if (_m_lSelectedAreaOreInfoList == null)
                _m_lSelectedAreaOreInfoList = new List<_ITreasureHuntOreInfo>();
            _m_lSelectedAreaOreInfoList.Clear();
            _m_iGetNormalOreNum = 0;
            _m_iGetAdvancedOreNum = 0;
            if (_m_lSelectedAreaTreasureInfoList == null)
                _m_lSelectedAreaTreasureInfoList = new List<_ITreasureHuntTreasureInfo>();
            _m_lSelectedAreaTreasureInfoList.Clear();
            _m_iGetTreasureNum = 0;
            
            // 根据选中区域获取可获取的矿石信息列表
            if (_m_selectedAreaRefObj != null && _m_selectedAreaRefObj.ore_list != null)
            {
                _ITreasureHuntOreInfo oreInfo = null;
                foreach (var oreId in _m_selectedAreaRefObj.ore_list)
                {
                    oreInfo = TreasureHuntUtil.getOreInfo(oreId);
                    if (oreInfo != null)
                    {
                        _m_lSelectedAreaOreInfoList.Add(oreInfo);

                        if (!(oreInfo.oreState is ETreasureHuntOreState.NONE or ETreasureHuntOreState.NOT_GET))
                            _m_iGetNormalOreNum++;
                        if (oreInfo.oreState is ETreasureHuntOreState.ACTIVATED_ADVANCED or ETreasureHuntOreState.NOT_ACTIVATE_ADVANCED)
                            _m_iGetAdvancedOreNum++;
                    }
                }
            }
            _m_lSelectedAreaOreInfoList.Sort((_a, _b) =>
            {
                if (_b == null || _b.oreRefObj == null) return -1;
                if (_a == null || _a.oreRefObj == null) return 1;
                if(ReferenceEquals(_a, _b)) return 0;
                
                // 按照品质排序
                int qualityCompare = _b.oreRefObj.quality.CompareTo(_a.oreRefObj.quality);
                if (qualityCompare != 0)
                    return qualityCompare;

                return _a.oreId.CompareTo(_b.oreId);
            });

            // 根据选中区域获取可获取的奇物信息列表
            if (_m_selectedAreaRefObj != null && _m_selectedAreaRefObj.treasure_list != null)
            {
                _ITreasureHuntTreasureInfo treasureInfo = null;
                foreach (var treasureId in _m_selectedAreaRefObj.treasure_list)
                {
                    treasureInfo = TreasureHuntUtil.getTreasureInfo(treasureId);
                    if (treasureInfo != null)
                    {
                        _m_lSelectedAreaTreasureInfoList.Add(treasureInfo);

                        if (treasureInfo.treasureState is ETreasureHuntTreasureState.GOT_NOT_ACTIVATE or ETreasureHuntTreasureState.GOT_ACTIVATED)
                            _m_iGetTreasureNum++;
                    }
                }
            }
            _m_lSelectedAreaTreasureInfoList.Sort((_a, _b) =>
            {
                if (_b == null || _b.treasureRefObj == null) return -1;
                if (_a == null || _a.treasureRefObj == null) return 1;
                if(ReferenceEquals(_a, _b)) return 0;
                
                // 按照品质排序
                int qualityCompare = _b.treasureRefObj.quality.CompareTo(_a.treasureRefObj.quality);
                if (qualityCompare != 0)
                    return qualityCompare;

                return _a.treasureId.CompareTo(_b.treasureId);
            });
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            // 刷新区域列表
            _refreshAreaList();
            
            // 刷新选中区域信息
            _refreshSelectedAreaInfo();
        }

        /// <summary>
        /// 刷新区域列表
        /// </summary>
        private void _refreshAreaList()
        {
            if (_m_lAreaWndList == null || !isShow)
                return;

            foreach (var areaWnd in _m_lAreaWndList)
            {
                if(areaWnd == null)
                    continue;

                areaWnd.showWnd();
                areaWnd.setSelected(areaWnd.areaRefObj == _m_selectedAreaRefObj);
            }
        }

        /// <summary>
        /// 刷新选中区域信息
        /// </summary>
        private void _refreshSelectedAreaInfo()
        {
            if(wnd == null|| !isShow)
                return;

            if (_m_selectedAreaRefObj != null)
            {
                // 设置选中区域名称
                ALUGUICommon.setLabelTxt(wnd.txtSelectedAreaName, TextTranslate.instance.getLanguage(_m_selectedAreaRefObj.name));
                if (_m_wCanGainOreContainer != null)
                {
                    _m_wCanGainOreContainer.showWnd();
                    _m_wCanGainOreContainer.setData(_m_lSelectedAreaOreInfoList);
                }
                
                int selectedAreaOreCount = _m_lSelectedAreaOreInfoList?.Count ?? 0;//选中区域可获取的矿石数量
                string txtNormalOreCollectProgressKey = string.IsNullOrEmpty(wnd.txtNormalOreCollectProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtNormalOreCollectProgressKey;
                ALUGUICommon.setLabelTxt(wnd.txtNormalOreCollectProgress, TextTranslate.instance.getLanguage(txtNormalOreCollectProgressKey, _m_iGetNormalOreNum, selectedAreaOreCount));
                string txtAdvancedOreCollectProgressKey = string.IsNullOrEmpty(wnd.txtAdvancedOreCollectProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtAdvancedOreCollectProgressKey;
                ALUGUICommon.setLabelTxt(wnd.txtAdvancedOreCollectProgress, TextTranslate.instance.getLanguage(txtAdvancedOreCollectProgressKey, _m_iGetAdvancedOreNum, selectedAreaOreCount));
                
                int selectedAreaTreasureCount = _m_lSelectedAreaTreasureInfoList?.Count ?? 0;//选中区域可获取的奇物数量
                string txtTreasureCollectProgressKey = string.IsNullOrEmpty(wnd.txtTreasureCollectProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtTreasureCollectProgressKey;
                ALUGUICommon.setLabelTxt(wnd.txtTreasureCollectProgress, TextTranslate.instance.getLanguage(txtTreasureCollectProgressKey, _m_iGetTreasureNum, selectedAreaTreasureCount));
             
                ECommonLockState areaState = _m_selectedAreaRefObj.isUnlock() ? ECommonLockState.UNLOCKED : ECommonLockState.LOCKED;
                NPCommonEnumStatInfo<ECommonLockState>.getStat(wnd.selectedAreaStateShowList, areaState);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtSelectedAreaName, string.Empty);
                _m_wCanGainOreContainer?.hideWnd();
                ALUGUICommon.setLabelTxt(wnd.txtNormalOreCollectProgress, string.Empty);
                ALUGUICommon.setLabelTxt(wnd.txtAdvancedOreCollectProgress, string.Empty);
                ALUGUICommon.setLabelTxt(wnd.txtTreasureCollectProgress, string.Empty);
                NPCommonEnumStatInfo<ECommonLockState>.getStat(wnd.selectedAreaStateShowList, ECommonLockState.LOCKED);
            }
        }

        private void _onSelectArea(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] is not long areaId)
                return;

            if (_m_lAreaWndList != null)
                foreach (var areaWnd in _m_lAreaWndList)
                {
                    if (areaWnd != null && areaWnd.areaId == areaId)
                    {
                        _onAreaClick(areaWnd);
                        return;
                    }
                }
        }
        /// <summary>
        /// 点击区域子窗口处理
        /// </summary>
        /// <param name="_areaWnd"></param>
        private void _onAreaClick(GGUISubWndTreasureHuntArea _areaWnd)
        {
            if(_areaWnd == null || _areaWnd.areaRefObj == null)
                return;

            // 设置解锁新区域红点为已读
            NPPlayer.instance.treasureHuntComponent.redTipDealer.setReadUnlockNewAreaRedTip(_areaWnd.areaId);
            
            // 如果点击的是当前选中的区域，则不做处理
            if(_areaWnd.areaRefObj == _m_selectedAreaRefObj)
                return;

            if (!_areaWnd.areaRefObj.isUnlock())
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_areaWnd.areaRefObj.unlock_condition_desc, _areaWnd.areaRefObj.unlock_condition_desc_args_list));
                return;
            }
            
            setSelectedArea(_areaWnd.areaRefObj);
        }

        /// <summary>
        /// 点击展示选中区域可获取所有按钮处理
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickShowAllCanGain(GameObject _go)
        {
            if(_m_selectedAreaRefObj == null)
                return;

            GGUIWndTreasureAreaOreTreasureDetail.instance.setData(_m_lSelectedAreaOreInfoList, _m_lSelectedAreaTreasureInfoList);
            GGUIWndTreasureAreaOreTreasureDetail.instance.setSelectTab(ETreasureHuntAreaOreTreasureDetailTabType.ORE);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTreasureAreaOreTreasureDetail.instance, () =>
            {
                GGUIWndTreasureAreaOreTreasureDetail.instance.showWnd();   
            }, UINodeTagConst.C_TREASURE_HUNT_AREA_ORE_TREASURE_DETAIL);
        }

        private void _onBtnGoTo()
        {
            if (wnd != null) _onClickGoto(wnd.btnGoto);
        }
        /// <summary>
        /// 点击前往按钮处理
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGoto(GameObject _go)
        {
            if(_m_selectedAreaRefObj == null)
                return;

            // 检查区域是否解锁
            if(!_m_selectedAreaRefObj.isUnlock())
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_m_selectedAreaRefObj.unlock_condition_desc, _m_selectedAreaRefObj.unlock_condition_desc_args_list));
                return;
            }

            GNodeTreasureHuntGameMain.addNode(_m_selectedAreaRefObj);
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickReturn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_SELECT_AREA);
        }
    }
}