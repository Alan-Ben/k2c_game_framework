using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 简易的带消耗道具的按钮，只支持显示单个消耗道具或免费(无消耗)
    /// </summary>
    public class GGUIWndSimpleCostButton : _ANPGGUIBasicSubWnd<GGUIMonoSimpleCostButton>
    {
        // 消耗道具数据
        private NPCommonCostItem _m_costItem;
        private ESimpleCostState _m_eCostState;
        
        // 消耗道具显示窗口
        private NPGGUIWndCommonItem _m_wCostItem;


        public GGUIWndSimpleCostButton(GGUIMonoSimpleCostButton _wnd) : base(_wnd)
        {
            initWnd();
        } 

        public NPCommonCostItem costItem { get { return _m_costItem; } }
        public ESimpleCostState costState { get { return _m_eCostState; } }
        
        public event Action<GGUIWndSimpleCostButton> onClickButton;

        protected override void _onShowWnd()
        {
            // 注册通用道具数量变化消息
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
        }

        protected override void _onHideWnd()
        {
            // 注销通用道具数量变化消息
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
            
            if (_m_wCostItem != null)
                _m_wCostItem.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wCostItem != null)
                _m_wCostItem.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_costItem = null;
            onClickButton = null;

            if (_m_wCostItem != null)
            {
                _m_wCostItem.discard();
                _m_wCostItem = null;
            }

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建消耗道具显示窗口
            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }


        /// <summary>
        /// 设置按钮信息
        /// </summary>
        /// <param name="_costItem">消耗道具数据，传null表示免费</param>
        public void setInfo(NPCommonCostItem _costItem)
        {
            _m_costItem = _costItem;
            _updateCostState();
            
            _refreshWnd();
        }

        /// <summary>
        /// 更新消耗状态
        /// </summary>
        private void _updateCostState()
        {
            if (_m_costItem == null)
            {
                _m_eCostState = ESimpleCostState.FREE;
                return;
            }
            
            if(GCommon.isItemEnough(_m_costItem, false))
                _m_eCostState = ESimpleCostState.COST_ENOUGH;
            else
                _m_eCostState = ESimpleCostState.COST_NOT_ENOUGH;
        }
        
        // 刷新窗口显示
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            // 显示消耗道具
            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(_m_costItem);
            }
            
            NPCommonEnumStatMutexShowInfo<ESimpleCostState>.setStat(wnd.stateShowInfoList, _m_eCostState);
        }

        // 按钮点击事件
        private void _onClick(GameObject _go)
        {
            onClickButton?.Invoke(this);
        }

        /// <summary>
        /// 通用道具数量变化回调
        /// 参数：_objs[0] = ENPItemType, _objs[1] = long subId, _objs[2] = long count
        /// </summary>
        private void _onCommonItemCountChg(params object[] _objs)
        {
            // 检查参数有效性
            if (_objs == null || _objs.Length < 3 || !(_objs[0] is ENPItemType _itemType) || 
                !(_objs[1] is long _subId) || !(_objs[2] is long _count))
                return;

            // 检查是否与当前消耗道具相关
            if (_m_costItem == null || _m_costItem.getItemType() != _itemType || _m_costItem.subId != _subId)
                return;

            // 更新消耗状态并刷新显示
            _updateCostState();
            _refreshWnd();
        }
    }
}
