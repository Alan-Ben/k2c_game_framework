using System;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星时间加速背包物品显示数据
    /// </summary>
    public class MarsTimeSpeedUpBagItemShowData
    {
        public static ENPItemType SpeedUpItemType = ENPItemType.BAG_ITEM;
        
        [NotNull] private MarsBagItemTimeReduceRefObj _m_rTimeReduceRef; // 时间加速物品配表数据
        [NotNull] private BagItemRefObj _m_rBagItemRef; // 物品配表数据
        private BagItem _m_bagItemInfo; // 玩家实际拥有的物品信息
        private NPCommonCostItem _m_iCommonItemInfo;// 通用物品信息
        
        [NotNull] public MarsBagItemTimeReduceRefObj timeReduceRef { get { return _m_rTimeReduceRef; } }
        public long bagItemId { get { return _m_rTimeReduceRef.id; } }
        [NotNull] public BagItemRefObj bagItemRef { get { return _m_rBagItemRef; } }
        public BagItem bagItemInfo { get { return _m_bagItemInfo; } }
        public long itemCount { get { return _m_bagItemInfo?.count ?? 0; } }
        [NotNull] public NPCommonCostItem commonItemInfo
        {
            get
            {
                if (_m_iCommonItemInfo == null || _m_iCommonItemInfo.getItemType() != SpeedUpItemType || _m_iCommonItemInfo.subId != bagItemId)
                {
                    _m_iCommonItemInfo = new NPCommonCostItem(SpeedUpItemType, bagItemId, itemCount);
                }
                else
                {
                    _m_iCommonItemInfo.setCount(itemCount);
                }

                return _m_iCommonItemInfo;
            }
        }

        public MarsTimeSpeedUpBagItemShowData([NotNull] MarsBagItemTimeReduceRefObj _timeReduceRefObj, [NotNull] BagItemRefObj _bagItemRef, BagItem _itemInfo)
        {
            _m_rTimeReduceRef = _timeReduceRefObj;
            _m_rBagItemRef = _bagItemRef;
            if (_m_rTimeReduceRef.id != _m_rBagItemRef.id)
            {
                Debug.LogError($"创建火星时间加速背包物品显示数据错误，时间加速物品ID与背包物品ID不匹配，时间加速物品ID:{_m_rTimeReduceRef.id}，背包物品ID:{_m_rBagItemRef.id}");
                
                // 以时间加速物品ID为准，重新获取背包物品配表数据
                _m_rBagItemRef = GRefdataCoreMgr.instance.bagItemCore.getRef(_m_rTimeReduceRef.id);
            }
            
            updateBagInfo(_itemInfo);
        }
        
        public MarsTimeSpeedUpBagItemShowData([NotNull] MarsBagItemTimeReduceRefObj _timeReduceRefObj, [NotNull] BagItemRefObj _bagItemRef)
        {
            _m_rTimeReduceRef = _timeReduceRefObj;
            _m_rBagItemRef = _bagItemRef;
            if (_m_rTimeReduceRef.id != _m_rBagItemRef.id)
            {
                Debug.LogError($"创建火星时间加速背包物品显示数据错误，时间加速物品ID与背包物品ID不匹配，时间加速物品ID:{_m_rTimeReduceRef.id}，背包物品ID:{_m_rBagItemRef.id}");
                
                // 以时间加速物品ID为准，重新获取背包物品配表数据
                _m_rBagItemRef = GRefdataCoreMgr.instance.bagItemCore.getRef(_m_rTimeReduceRef.id);
            }
            
            updateBagInfo();
        }

        public void updateBagInfo()
        {
            _m_bagItemInfo = NPPlayer.instance.bagComp.getItem(_m_rBagItemRef.id);
        }
        
        public void updateBagInfo(BagItem _itemInfo)
        {
            if (_itemInfo == null)
            {
                _m_bagItemInfo = null;
                return;
            }
            
            if (_itemInfo.itemId != bagItemId)
            {
                Debug.LogError($"更新背包物品信息错误，物品ID不匹配，期待ID:{bagItemId}，实际ID:{_itemInfo.itemId}");
                return;
            }
            
            _m_bagItemInfo = _itemInfo;
        }
    }
    
    /// <summary>
    /// 火星时间加速背包物品item
    /// </summary>
    public class GGUIWndMarsTimeSpeedUpBagItem : _ATALBasicUISubWnd<GGUIMonoMarsTimeSpeedUpBagItem>
    {
        private MarsTimeSpeedUpBagItemShowData _m_iBagItemShowInfo; // 物品显示数据
        private int _m_iInListIndex;//在列表中的索引
        private bool _m_isSelected; // 是否选中状态
        
        private NPGGUIWndCommonItem _m_wItem; // 物品item
        
        public GGUIWndMarsTimeSpeedUpBagItem(GGUIMonoMarsTimeSpeedUpBagItem _mono) : base(_mono)
        {
            initWnd();
        }

        public int inListIndex { get { return _m_iInListIndex; } }
        
        public event Action<GGUIWndMarsTimeSpeedUpBagItem> onItemClick;
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            if (_m_wItem != null)
                _m_wItem.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wItem != null)
                _m_wItem.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.clickGo, _onClickItem);
            }

            if (_m_wItem != null)
                _m_wItem.discard();
            _m_wItem = null;
            
            onItemClick = null;
            _m_iBagItemShowInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.item != null)
                _m_wItem = new NPGGUIWndCommonItem(wnd.item);

            ALUGUICommon.combineBtnClick(wnd.clickGo, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_showInfo">展示信息</param>
        public void setInfo(MarsTimeSpeedUpBagItemShowData _showInfo, int _inListIndex)
        {
            _m_iBagItemShowInfo = _showInfo;
            _m_iInListIndex = _inListIndex;

            _refreshWnd();
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelected">是否选中</param>
        public void setSelected(bool _isSelected)
        {
            _m_isSelected = _isSelected;
            _refreshSelectState();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_iBagItemShowInfo == null)
                return;

            // 设置物品item
            if (_m_wItem != null)
            {
                _m_wItem.showWnd();
                _m_wItem.setItem(_m_iBagItemShowInfo.commonItemInfo);
            }

            ALUGUICommon.setLabelTxt(wnd.txtReduceTime, TimeUtil.millisecondsToTime_Two(_m_iBagItemShowInfo.timeReduceRef.reduce_sec * 1000));
            
            ALUGUICommon.setGameObjEnable(wnd.zeroNumShow, _m_iBagItemShowInfo.itemCount <= 0);
            
            // 刷新选中状态
            _refreshSelectState();
        }
        
        
        /// <summary>
        /// 刷新选中状态
        /// </summary>
        private void _refreshSelectState()
        {
            if (wnd == null)
                return;

            // 设置选中时显示的对象
            ALUGUICommon.setGameObjEnable(wnd.selectShowGoList, _m_isSelected);
        }

        /// <summary>
        /// 点击物品
        /// </summary>
        private void _onClickItem(GameObject _go)
        {
            onItemClick?.Invoke(this);
        }
    }
}
