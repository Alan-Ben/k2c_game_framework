using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励任务列表
    /// </summary>
    public class GGUIWndActivityStepRewardGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoActivityStepRewardGridItem, GGUIMonoActivityStepRewardGrid, GGUIWndActivityStepRewardGridItem>
    {
        private GActivityMainRefObj _m_activityMainRef;
        private List<ActivityStepRewardInfo> _m_showList;
        private Action<GGUIWndActivityStepRewardGridItem> _m_aClickGetReward;//点击领取奖励

        /// <summary>
        /// 点击领取奖励
        /// </summary>
        public Action<GGUIWndActivityStepRewardGridItem> onClickGetReward { get { return _m_aClickGetReward; } set { _m_aClickGetReward = value; } }

        public GGUIWndActivityStepRewardGrid(GGUIMonoActivityStepRewardGrid _wnd) : base(_wnd)
        {
            _m_showList = new List<ActivityStepRewardInfo>();
            initWnd();
        }

        protected override GGUIWndActivityStepRewardGridItem _createItemWnd(GGUIMonoActivityStepRewardGridItem _itemMono)
        {
            GGUIWndActivityStepRewardGridItem item = new GGUIWndActivityStepRewardGridItem(_itemMono);
            item.onClickGetReward += _onClickGetReward;
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndActivityStepRewardGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_showList.Count)
                return;

            ActivityStepRewardInfo info = _m_showList[_itemIdx];

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
        public void setInfo(List<ActivityStepRewardInfo> _list, GActivityMainRefObj _activityMainRef)
        {
            _m_activityMainRef = _activityMainRef;
            _m_showList.Clear();
            _m_showList.AddRange(_list);
            _m_showList.Sort(_sortList);
            setItemCount(_m_showList.Count);
        }

        //列表排序：第一优先级：可领取>进行中>已完成；第二优先级：根据任务id顺序排序
        private int _sortList(ActivityStepRewardInfo _a, ActivityStepRewardInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            // 获取奖励状态
            EStepRewardState stateA = _a.getStepRewardState(_a.getFirstNotGetRewardStep());
            EStepRewardState stateB = _b.getStepRewardState(_b.getFirstNotGetRewardStep());

            // 可领取的排在最前面
            if (stateA == EStepRewardState.CanGet && stateB != EStepRewardState.CanGet)
                return -1;
            if (stateB == EStepRewardState.CanGet && stateA != EStepRewardState.CanGet)
                return 1;

            // 已领取的排在最后面
            if (stateA == EStepRewardState.AlreadyGet && stateB != EStepRewardState.AlreadyGet)
                return 1;
            if (stateB == EStepRewardState.AlreadyGet && stateA != EStepRewardState.AlreadyGet)
                return -1;

            if (_m_activityMainRef == null || _m_activityMainRef.step_reward_set_id_list == null)
                return 0;

            // 按照配置顺序排序
            return _m_activityMainRef.step_reward_set_id_list.IndexOf(_a.stepRewardSetId)
                .CompareTo(_m_activityMainRef.step_reward_set_id_list.IndexOf(_b.stepRewardSetId));
        }

        private void _onClickGetReward(GGUIWndActivityStepRewardGridItem _item)
        {
            _m_aClickGetReward?.Invoke(_item);
        }
    }
}
