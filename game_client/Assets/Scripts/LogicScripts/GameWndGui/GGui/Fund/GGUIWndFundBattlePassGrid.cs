using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基金战令通行证Grid
    /// </summary>
    public class GGUIWndFundBattlePassGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoFundBattlePassGridItem, GGUIMonoFundBattlePassGrid, GGUIWndFundBattlePassGridItem>
    {
        //基金信息快照
        private FundInfoSnapshot _m_fundSnapshot;
        //阶段列表数据
        private List<ActivityFundStepRefObj> _m_lStepRefList;
        //最后一个可见阶段变化回调
        private Action _m_onLastVisibleStepChg;
        //当前最后可见的阶段索引
        private int _m_nLastVisibleStepIdx = -1;
        

        public GGUIWndFundBattlePassGrid(GGUIMonoFundBattlePassGrid _gridMono, Action _onLastVisibleStepChg) : base(_gridMono)
        {
            _m_onLastVisibleStepChg = _onLastVisibleStepChg;
            
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            if (wnd == null)
                return;

            if (wnd.scrollRect == null)
                return;
            
            wnd.scrollRect.onValueChanged.AddListener(_onScrollRectValueChg);
        }
        protected override void _onHideWnd()
        {
            if (wnd == null)
                return;

            if (wnd.scrollRect == null)
                return;
            
            wnd.scrollRect.onValueChanged.RemoveListener(_onScrollRectValueChg);
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }
        protected override void _onRefreshItemWnd(GGUIWndFundBattlePassGridItem _itemWnd, int _itemIdx)
        {
            if (_m_lStepRefList == null)
                return;

            if (_itemIdx < 0 || _itemIdx >= _m_lStepRefList.Count || _m_fundSnapshot == null)
                return;

            ActivityFundStepRefObj stepRef = _m_lStepRefList[_itemIdx];
            _itemWnd?.setInfo(stepRef, _m_fundSnapshot);
        }
        protected override GGUIWndFundBattlePassGridItem _createItemWnd(GGUIMonoFundBattlePassGridItem _itemMono)
        {
            return new GGUIWndFundBattlePassGridItem(_itemMono);
        }
        

        /// <summary>
        /// 显示item列表
        /// </summary>
        public void showItemList(FundInfoSnapshot _snapshot)
        {
            if (_snapshot == null)
                return;

            _m_fundSnapshot = _snapshot;

            //获取当前等级的所有阶段
            _m_lStepRefList = _snapshot.getStepRefList();
            setItemCount(Mathf.Max(0, _m_lStepRefList.Count));
        }


        /// <summary>
        /// 获取当前最后可见的阶段索引
        /// </summary>
        public int getLastVisibleStepIdx()
        {
            return _m_nLastVisibleStepIdx;
        }


        private void _onScrollRectValueChg(Vector2 _value)
        {
            if (wnd == null || wnd.itemTemplate == null)
                return;

            float itemHeight = wnd.itemTemplate.height;
            float space = wnd.spaceSize.y;
            float paddingTop = wnd.paddingForSide.x;

            RectTransform viewport = wnd.gridAreaMaskObj;
            RectTransform itemArea = wnd.gridAreaUIObj;
            if (viewport == null || itemArea == null)
                return;

            //计算viewport底部在content中的位置
            float contentOffsetY = itemArea.anchoredPosition.y;
            float viewportHeight = viewport.rect.height;
            float bottomPosInContent = contentOffsetY + viewportHeight;

            //计算底部位置对应的item索引
            float itemTotalHeight = itemHeight + space;
            int lastVisibleIdx = Mathf.FloorToInt((bottomPosInContent - paddingTop) / itemTotalHeight);

            //限制范围
            if (_m_lStepRefList != null)
                lastVisibleIdx = Mathf.Clamp(lastVisibleIdx, 0, _m_lStepRefList.Count - 1);

            //检查是否变化
            if (lastVisibleIdx != _m_nLastVisibleStepIdx)
            {
                _m_nLastVisibleStepIdx = lastVisibleIdx;
                _m_onLastVisibleStepChg?.Invoke();
            }
        }
    }
}
