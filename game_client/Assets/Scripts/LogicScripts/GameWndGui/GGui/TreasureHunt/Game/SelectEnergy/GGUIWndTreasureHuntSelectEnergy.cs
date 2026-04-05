using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 选择能源窗口
    /// </summary>
    public class GGUIWndTreasureHuntSelectEnergy : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntSelectEnergy>
    {
        private static GGUIWndTreasureHuntSelectEnergy _g_instance;
        public static GGUIWndTreasureHuntSelectEnergy instance { get { return _g_instance ??= new GGUIWndTreasureHuntSelectEnergy(); } }

        private List<NPCommonItem> _m_lEnergyItemList;//能源道具列表
        private NPCommonItem _m_selectedEnergyItem;//选中的能源道具
        private NPCommonItem _m_usingEnergyItem;//正在使用的能源道具
        private Action<NPCommonItem> _m_aChgUsingEnergyItem;// 变化使用的能源道具时回调
        
        private GGUIWndTreasureHuntSelectEnergyItemContainer _m_wEnergyItemContainer;
        private GGUISubWndCommonItemDetail _m_wSelectedItemDetail;

        public GGUIWndTreasureHuntSelectEnergy() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntSelectEnergy.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntSelectEnergy.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoEnergyItemContainer != null)
            {
                _m_wEnergyItemContainer = new GGUIWndTreasureHuntSelectEnergyItemContainer(wnd.monoEnergyItemContainer);
                _m_wEnergyItemContainer.onClickEnergyItem += _onClickEnergyItem;
            }

            if (wnd.selectedItemDetail != null)
                _m_wSelectedItemDetail = new GGUISubWndCommonItemDetail(wnd.selectedItemDetail);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnUse, _onClickUse);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onClickUse);
            }

            if (_m_wEnergyItemContainer != null)
            {
                _m_wEnergyItemContainer.onClickEnergyItem -= _onClickEnergyItem;
                _m_wEnergyItemContainer.discard();
                _m_wEnergyItemContainer = null;
            }

            _m_wSelectedItemDetail?.discard();
            _m_wSelectedItemDetail = null;

            _m_lEnergyItemList?.Clear();
            _m_lEnergyItemList = null;
            
            _m_aChgUsingEnergyItem = null;
        }
        
        protected override void _onShowWnd()
        {
            if (_m_lEnergyItemList == null)
                _m_lEnergyItemList = new List<NPCommonItem>();
            _m_lEnergyItemList.Clear();
            _m_lEnergyItemList.Add(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_premium_energy_common_item);
            _m_lEnergyItemList.Add(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_advanced_energy_common_item);
            
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lEnergyItemList?.Clear();
            
            _m_wEnergyItemContainer?.hideWnd();
            _m_wSelectedItemDetail?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_lEnergyItemList?.Clear();

            _m_wEnergyItemContainer?.resetWnd();
            _m_wSelectedItemDetail?.resetWnd();
        }

        public void setData(NPCommonItem _selectedEnergyItem, NPCommonItem _usingEnergyItem, Action<NPCommonItem> _onChgUsingEnergyItem)
        {
            _m_selectedEnergyItem = _selectedEnergyItem;
            _m_usingEnergyItem = _usingEnergyItem;
            _m_aChgUsingEnergyItem = _onChgUsingEnergyItem;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            // 刷新能源列表
            if (_m_wEnergyItemContainer != null)
            {
                _m_wEnergyItemContainer.showWnd();
                _m_wEnergyItemContainer.setData(_m_lEnergyItemList, _m_selectedEnergyItem, _m_usingEnergyItem);
            }

            // 刷新选中道具
            _refreshSelectedItem();
        }

        /// <summary>
        /// 刷新选中道具详情
        /// </summary>
        private void _refreshSelectedItem()
        {
            if(wnd == null)
                return;
            
            if (_m_selectedEnergyItem != null && _m_wSelectedItemDetail != null)
            {
                _m_wSelectedItemDetail.showWnd();
                _m_wSelectedItemDetail.setShowData(_m_selectedEnergyItem);
            }
            else
            {
                _m_wSelectedItemDetail?.hideWnd();
            }

            if (_m_selectedEnergyItem == null || GCommon.getItemCount(_m_selectedEnergyItem) <= 0)
            {
                GGameCommonInfo.grayImage(wnd.cannotUseEnergyGrayList);
            }
            else
            {
                GGameCommonInfo.disgrayImage(wnd.cannotUseEnergyGrayList);
            }
        }

        /// <summary>
        /// 当点击某个能源道具时
        /// </summary>
        /// <param name="_energyItem"></param>
        private void _onClickEnergyItem(NPCommonItem _energyItem)
        {
            if(_m_selectedEnergyItem == _energyItem)
                return;
                
            _m_selectedEnergyItem = _energyItem;
            
            // 更新容器中的选中状态
            _m_wEnergyItemContainer?.setSelectedItem(_m_selectedEnergyItem);
            
            // 刷新选中道具
            _refreshSelectedItem();
        }

        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_SELECT_ENERGY);
        }

        /// <summary>
        /// 点击使用按钮时
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickUse(GameObject _go)
        {
            if(_m_selectedEnergyItem == null)
                return;

            // 【优化-0】太空寻宝-体力道具为0时，依然可以切换道具 https://www.teambition.com/task/689dcf61bd59ff9920aba98e
            // // 检查道具数量是否足够
            // long itemCount = GCommon.getItemCount(_m_selectedEnergyItem);
            // if(itemCount <= 0)
            // {
            //     GCommon.dealItemNotEnough(_m_selectedEnergyItem);
            //     return;
            // }

            _m_aChgUsingEnergyItem?.Invoke(_m_selectedEnergyItem);
            
            // 关闭窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_SELECT_ENERGY);
        }
    }
}