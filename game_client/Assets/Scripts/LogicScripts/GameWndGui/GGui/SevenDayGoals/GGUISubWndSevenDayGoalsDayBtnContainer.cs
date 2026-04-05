using System;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsDayBtnContainer : _AGGUISubWndCommonContainer<GGUIMonoSevenDayGoalsDayBtnContainerItem, GGUIMonoSevenDayGoalsDayBtnContainer, GGUISubWndSevenDayGoalsDayBtnContainerItem>
    {
        // 点击按钮后的回调
        private readonly Action _m_onDayBtnClick;
        
        // 当前选中的天数
        private int _m_selectedDay;
        // 当前服务器是第几天
        private int _m_serverDayNow;
        
        
        public GGUISubWndSevenDayGoalsDayBtnContainer(GGUIMonoSevenDayGoalsDayBtnContainer _containerMono, Action _onDayBtnClick) 
            : base(_containerMono)
        {
            _m_onDayBtnClick = _onDayBtnClick;
            
            initWnd();
        }
        

        /// <summary>
        /// 当前选择的是第几天
        /// </summary>
        public int selectedDay { get { return _m_selectedDay; } }


        protected override GGUISubWndSevenDayGoalsDayBtnContainerItem _createItemWnd(GGUIMonoSevenDayGoalsDayBtnContainerItem _itemMono)
        {
            return new GGUISubWndSevenDayGoalsDayBtnContainerItem(_itemMono, _onDayBtnClick);
        }
        protected override void _refreshItemWnd(GGUISubWndSevenDayGoalsDayBtnContainerItem _itemWnd, int _index)
        {
            int day = _index + 1;
            if (day < 1)
                return;
            
            _itemWnd.refreshWnd(day, _m_serverDayNow, _m_selectedDay);
        }


        public new void refreshWnd(int _selectDay)
        {
            _m_serverDayNow = NPPlayer.instance.sevenDayGoalsComp.data.getNowDay();
            int maxDay = GRefdataCoreMgr.instance.getSevenDayGoalsMaxDay();
            _m_selectedDay = Mathf.Clamp(_selectDay, 1, maxDay);
            base.refreshWnd(maxDay);
            setRedTipRead();
        }


        private void _onDayBtnClick(int _day)
        {
            _m_selectedDay = _day;
            refreshAllItem();
            
            _m_onDayBtnClick?.Invoke();
            setRedTipRead();
        }

        /// <summary>
        /// 设置红点已读
        /// </summary>
        private void setRedTipRead()
        {
            //设置新解锁已读
            NPPlayer.instance.sevenDayGoalsComp.setReadRedTip(_m_selectedDay, ESevenDayGoalsRedTipType.NEW);
        }
    }
}