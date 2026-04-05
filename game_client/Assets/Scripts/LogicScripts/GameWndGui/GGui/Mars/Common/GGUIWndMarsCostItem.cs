using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星消耗物品展示
    /// </summary>
    public class GGUIWndMarsCostItem : _ANPGGUIBasicSubWnd<GGUIMonoMarsCostItem>
    {
        private NPCommonCostItem _m_costItemData;
        private Action<GGUIWndMarsCostItem> _m_aOnClickItem;

        private NPGGUIWndCommonItem _m_wCommonItem;
        
        public GGUIWndMarsCostItem(GGUIMonoMarsCostItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public NPCommonCostItem costItemData { get { return _m_costItemData; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化通用物品展示
            if (wnd.monoItem != null)
                _m_wCommonItem = new NPGGUIWndCommonItem(wnd.monoItem);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }
        
        protected override void _onDiscard()
        {
            // 解绑点击事件
            if (wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);

            // 销毁通用物品窗口
            if (_m_wCommonItem != null)
            {
                _m_wCommonItem.discard();
                _m_wCommonItem = null;
            }
        }
        
        protected override void _onShowWnd()
        {
            _m_wCommonItem?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wCommonItem?.hideWnd();

            _m_costItemData = null;
            _m_aOnClickItem = null;
        }

        protected override void _onReset()
        {
            _m_wCommonItem?.resetWnd();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_costItemData">消耗物品数据</param>
        /// <param name="_aOnClickItem">点击回调</param>
        public void setData(NPCommonCostItem _costItemData, Action<GGUIWndMarsCostItem> _aOnClickItem)
        {
            _m_costItemData = _costItemData;
            _m_aOnClickItem = _aOnClickItem;
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            // 刷新物品显示
            if (_m_wCommonItem != null)
            {
                _m_wCommonItem.showWnd();
                _m_wCommonItem.setItem(_m_costItemData);
            }
        }


        /// <summary>
        /// 点击Item回调
        /// </summary>
        /// <param name="_go">点击的GameObject</param>
        private void _onClickItem(GameObject _go)
        {
            if (_m_aOnClickItem != null)
                _m_aOnClickItem.Invoke(this);
            else if(_m_costItemData != null)
            {
                GCommon.popItemAccessWays(_m_costItemData.getItemType(), _m_costItemData.subId);
            }
        }
    }
}