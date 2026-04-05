using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndEarningGoalGlobalItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoEarningGoalGlobalItem,GGUIMonoEarningGoalGlobalItemGrid,GGUIWndEarningGoalGlobalItem>
    {
        private List<EarningGoalRewardRefObj> _m_itemDataList = new List<EarningGoalRewardRefObj>();

        public GGUIWndEarningGoalGlobalItemGrid(GGUIMonoEarningGoalGlobalItemGrid gridMono) : base(gridMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndEarningGoalGlobalItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemMono.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndEarningGoalGlobalItem _createItemWnd(GGUIMonoEarningGoalGlobalItem _itemMono)
        {
            GGUIWndEarningGoalGlobalItem itemWnd = new GGUIWndEarningGoalGlobalItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<EarningGoalRewardRefObj> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            _m_itemDataList.Sort(NPPlayer.instance.earningGoalComp.sortEarningGoalGlobalReward);
            setItemCount(_m_itemDataList.Count);
        }
    }
}
