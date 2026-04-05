using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技树窗口
    /// </summary>
    public class GGUIWndMarsTechnologyTree : _ANPGGUIBasicWnd<GGUIMonoMarsTechnologyTree>
    {
        private static GGUIWndMarsTechnologyTree _g_instance;
        public static GGUIWndMarsTechnologyTree instance { get { return _g_instance ??= new GGUIWndMarsTechnologyTree(); } }

        // 科技类型页签列表
        [NotNull] private Dictionary<EMarsTechnologyType, GGUIWndMarsTechnologyTypeTab> _m_dTabWndDic = new Dictionary<EMarsTechnologyType, GGUIWndMarsTechnologyTypeTab>();
        /// <summary>
        /// 当前显示科技类型
        /// </summary>
        private EMarsTechnologyType _m_eCurSelectTabType = EMarsTechnologyType.NONE;
        
        // 科技树层级Grid
        private GGUIWndMarsTechnologyTreeLayerGrid _m_wTreeLayerGrid;
        private bool _m_bTreeLayerGridNeedScrollMove;
        
        /// <summary>
        /// 升级中的科技子窗口
        /// </summary>
        private GGUIWndMarsUpgradingTechnologyInfo _m_wUpgradingTechnologyWnd;

        public GGUIWndMarsTechnologyTree() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsTechnologyTree.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsTechnologyTree.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            // 构建科技类型页签列表
            _m_dTabWndDic.Clear();
            if (wnd.tabMonoList != null)
            {
                foreach (var tabMono in wnd.tabMonoList)
                {
                    if (tabMono == null)
                        continue;

                    if (_m_dTabWndDic.ContainsKey(tabMono.technologyType))
                    {
                        Debug.LogError_EditorOnly($"[GGUIWndMarsTechnologyTree _onWndInitDone] wnd.tabMonoList列表中页签类型:{tabMono.technologyType}重复配置", wnd);
                        continue;
                    }
                    
                    GGUIWndMarsTechnologyTypeTab tabWnd = new GGUIWndMarsTechnologyTypeTab(tabMono);
                    
                    // 初始化为未选中状态
                    tabWnd.setSelected(false);
                
                    // 绑定点击事件
                    tabWnd.onClickTab += _onTabClick;
                
                    _m_dTabWndDic.Add(tabMono.technologyType, tabWnd);
                }
            }
            
            // 构建科技树层级Grid
            if (wnd.monoTechnologyTreeLayerGrid != null)
                _m_wTreeLayerGrid = new GGUIWndMarsTechnologyTreeLayerGrid(wnd.monoTechnologyTreeLayerGrid);

            if (wnd.monoUpgradingTechnology != null)
                _m_wUpgradingTechnologyWnd = new GGUIWndMarsUpgradingTechnologyInfo(wnd.monoUpgradingTechnology);
            
            ALUGUICommon.combineBtnClick(wnd.addOverviewBtn, _onClickAddOverview);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturn);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.addOverviewBtn, _onClickAddOverview);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturn);
            }
            
            // 清理页签列表
            foreach (var tabWnd in _m_dTabWndDic.Values)
            {
                tabWnd?.discard();
            }
            _m_dTabWndDic.Clear();
            
            _m_eCurSelectTabType = EMarsTechnologyType.NONE;
            
            // 清理子窗口
            _m_wTreeLayerGrid?.discard();
            _m_wTreeLayerGrid = null;
            
            _m_wUpgradingTechnologyWnd?.discard();
            _m_wUpgradingTechnologyWnd = null;
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _onTechnologyChg);
            
            foreach (var tabWnd in _m_dTabWndDic.Values)
            {
                tabWnd?.showWnd();
            }
            _m_wUpgradingTechnologyWnd?.showWnd();
            
            setSelectTab(_m_eCurSelectTabType, true, _m_bTreeLayerGridNeedScrollMove);
        }


        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _onTechnologyChg);
            
            // 隐藏页签
            foreach (var tabWnd in _m_dTabWndDic.Values)
            {
                tabWnd?.hideWnd();
            }
            
            // 隐藏子窗口
            _m_wTreeLayerGrid?.hideWnd();
            
            _m_wUpgradingTechnologyWnd?.hideWnd();
        }


        protected override void _onReset()
        {
            // 重置页签
            foreach (var tabWnd in _m_dTabWndDic.Values)
            {
                tabWnd?.resetWnd();
            }
            
            // 重置子窗口
            _m_wTreeLayerGrid?.resetWnd();
            
            _m_wUpgradingTechnologyWnd?.resetWnd();
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        public void setSelectTab(EMarsTechnologyType _technologyType, bool _forceRefresh, bool _needMoveScroll)
        {
            if(_m_eCurSelectTabType == _technologyType && !_forceRefresh)
                return;

            _m_bTreeLayerGridNeedScrollMove = _needMoveScroll;

            if (_technologyType == EMarsTechnologyType.NONE && wnd != null)
            {
                MarsTechnologyInfo nowTechnologyInfo = NPPlayer.instance.marsComp.technologySubComponent.nowdUpgradingOrUpgradedTechnologyInfo;
                _technologyType = nowTechnologyInfo?.technologyRefObj?.type ?? wnd.defaultSelectType;
            }

            if (_m_dTabWndDic.TryGetValue(_m_eCurSelectTabType, out GGUIWndMarsTechnologyTypeTab _tabWnd))
            {
                _tabWnd?.setSelected(false);
            }

            _m_eCurSelectTabType = _technologyType;
            if (_m_dTabWndDic.TryGetValue(_m_eCurSelectTabType, out _tabWnd))
            {
                _tabWnd?.setSelected(true);
            }

            _refreshTreeLayerGrid();
        }
        
        /// <summary>
        /// 聚焦到某一个科技
        /// </summary>
        /// <param name="_technologyId"></param>
        public void focusTechnology(long _technologyId)
        {
            if(_m_wTreeLayerGrid == null)
                return;
            
            MarsTechnologyRefObj technologyRefObj = GRefdataCoreMgr.instance.marsTechnologyRefCore.getRef(_technologyId);
            if (technologyRefObj == null)
                return;

            setSelectTab(technologyRefObj.type, false, false);
            _m_wTreeLayerGrid.moveToLayer(technologyRefObj.layer);
        }
        
        public void focusTechnologyBySkillType(int _skillType)
        {
            if(_m_wTreeLayerGrid == null)
                return;
            
            MarsTechnologyRefObj technologyRefObj = GRefdataCoreMgr.instance.getFirstUpgradingOrCanUpgradeTechnologyBySkillType(_skillType, true);
            if (technologyRefObj == null)
                return;

            setSelectTab(technologyRefObj.type, false, false);
            _m_wTreeLayerGrid.moveToLayer(technologyRefObj.layer);
        }
        
        /// <summary>
        /// 刷新科技树层级Grid
        /// </summary>
        private void _refreshTreeLayerGrid()
        {
            if (_m_wTreeLayerGrid == null || !isShow)
                return;
            
            // 获取当前选中科技类型的配表对象
            GRefdataCoreMgr.MarsTechnologyTypeLayerGroup typeLayerGroup = GRefdataCoreMgr.instance.getMarsTechnologyTypeLayerGroup(_m_eCurSelectTabType);
            
            // 刷新Grid数据
            _m_wTreeLayerGrid.showWnd();
            _m_wTreeLayerGrid.setData(typeLayerGroup);

            bool treeLayerGridNeedScrollMove = _m_bTreeLayerGridNeedScrollMove;
            _m_bTreeLayerGridNeedScrollMove = false;
            if (treeLayerGridNeedScrollMove)
            {
                MarsTechnologyInfo nowTechnologyInfo = NPPlayer.instance.marsComp.technologySubComponent.nowdUpgradingOrUpgradedTechnologyInfo;
                if (nowTechnologyInfo != null && nowTechnologyInfo.technologyRefObj != null && nowTechnologyInfo.technologyRefObj.type == _m_eCurSelectTabType)
                {
                    _m_wTreeLayerGrid.moveToLayer(nowTechnologyInfo.technologyRefObj.layer);
                }
                else
                {
                    MarsTechnologyRefObj firstUpgradingOrCanUpgradeTechnology = typeLayerGroup.getFirstUpgradingOrCanUpgradeTechnology();
                    if (firstUpgradingOrCanUpgradeTechnology != null)
                    {
                        _m_wTreeLayerGrid.moveToLayer(firstUpgradingOrCanUpgradeTechnology.layer);
                    }
                    else
                    {
                        _m_wTreeLayerGrid.moveToTop();
                    }
                }
            }
        }

        /// <summary>
        /// 点击页签
        /// </summary>
        private void _onTabClick(GGUIWndMarsTechnologyTypeTab _tabWnd)
        {
            if (_tabWnd == null)
                return;
            
            setSelectTab(_tabWnd.tabType, false, true);
        }

        /// <summary>
        /// 点击加成总览按钮
        /// </summary>
        private void _onClickAddOverview(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndMasrTechnologyAddOverview.instance, () =>
            {
                GGUIWndMasrTechnologyAddOverview.instance.showWnd();
            }, UINodeTagConst.C_MARS_TECHNOLOGY_ADD_OVERVIEW);
        }

        private void _onClickReturn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_TECHNOLOGY_TREE);
        }
        
        /// <summary>
        /// 科技变化消息回调
        /// </summary>
        private void _onTechnologyChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is MarsTechnologyInfo _technologyInfo))
                return;
            
            if(_technologyInfo.technologyRefObj != null && _technologyInfo.technologyRefObj.type == _m_eCurSelectTabType)
            {
                // 刷新科技树层级Grid
                _refreshTreeLayerGrid();
            }
        }
    }
}
