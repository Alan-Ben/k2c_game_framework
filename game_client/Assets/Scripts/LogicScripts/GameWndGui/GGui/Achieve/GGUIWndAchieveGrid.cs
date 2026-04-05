using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 成就信息列表
    /// </summary>
    public class GGUIWndAchieveGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoAchieveGridItem, GGUIMonoAchieveGrid, GGUIWndAchieveGridItem>
    {
        private List<AchieveInfo> _m_showList;
        private Action<GGUIWndAchieveGridItem> _m_aClickGetReward;//点击领取奖励

        /// <summary>
        /// 点击领取奖励
        /// </summary>
        public Action<GGUIWndAchieveGridItem> onClickGetReward
        {
            get { return _m_aClickGetReward; }
            set { _m_aClickGetReward = value; }
        }

        public GGUIWndAchieveGrid(GGUIMonoAchieveGrid _wnd)
            : base(_wnd)
        {
            _m_showList = new List<AchieveInfo>();
            initWnd();
        }

        protected override GGUIWndAchieveGridItem _createItemWnd(GGUIMonoAchieveGridItem _itemMono)
        {
            GGUIWndAchieveGridItem item = new GGUIWndAchieveGridItem(_itemMono);
            item.onClickGetReward += _onClickGetReward;
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndAchieveGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_showList.Count)
                return;

            AchieveInfo info = _m_showList[_itemIdx];

            _itemWnd.setInfo(info);
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
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_list"></param>
        public void setShowData(List<AchieveInfo> _list)
        {
            _m_showList.Clear();
            _m_showList.AddRange(_list);
            _m_showList.Sort(_sortList);
            setItemCount(_m_showList.Count);
        }

        //列表排序：第一优先级：可领取>进行中>已完成；第二优先级：根据任务id顺序排序
        private int _sortList(AchieveInfo _a, AchieveInfo _b)
        {
            if (_a == null || _a.achieveRefObj == null || _b == null || _b.achieveRefObj == null)
                return 0;

            if (_a.getCurStepRewardState().CompareTo(_b.getCurStepRewardState()) != 0)
                return _a.getCurStepRewardState().CompareTo(_b.getCurStepRewardState());
            else
                return _a.achieveRefObj.sort_id.CompareTo(_b.achieveRefObj.sort_id);
        }

        private void _onClickGetReward(GGUIWndAchieveGridItem _item)
        {
            _m_aClickGetReward?.Invoke(_item);
        }
    }
}
