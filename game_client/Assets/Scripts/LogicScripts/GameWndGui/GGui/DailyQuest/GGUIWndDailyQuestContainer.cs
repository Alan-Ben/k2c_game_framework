using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 每日任务信息列表
    /// </summary>
    public class GGUIWndDailyQuestContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoDailyQuestContainerItem, GGUIMonoDailyQuestContainer, GGUIWndDailyQuestContainerItem>
    {
        private List<GGUIWndDailyQuestContainerItem> _m_lItemList;//item列表
        private List<DailyQuestItem> _m_showList;//信息列表
        private Action<GGUIWndDailyQuestContainerItem, RectTransform> _m_aOnClickGetReward;//点击领取奖励事件
        private Action<List<GGUIWndDailyQuestContainerItem>> _m_aOnClickOnceGetReward;//点击一键领取奖励事件

        /// <summary>
        /// 点击领取奖励事件
        /// </summary>
        public Action<GGUIWndDailyQuestContainerItem, RectTransform> onClickGetReward
        {
            get { return _m_aOnClickGetReward; }
            set { _m_aOnClickGetReward = value; }
        }

        /// <summary>
        /// 点击一键领取奖励事件
        /// </summary>
        public Action<List<GGUIWndDailyQuestContainerItem>> onClickOnceGetReward
        {
            get { return _m_aOnClickOnceGetReward; }
            set { _m_aOnClickOnceGetReward = value; }
        }

        public GGUIWndDailyQuestContainer(GGUIMonoDailyQuestContainer _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override GGUIWndDailyQuestContainerItem _createItemWnd(GGUIMonoDailyQuestContainerItem _itemMono)
        {
            GGUIWndDailyQuestContainerItem item = new GGUIWndDailyQuestContainerItem(_itemMono);
            item.onClickGetReward += _onClickGetReward;
            item.onClickOnceGetReward += _onClickOnceGetReward;

            return item;
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
            _m_showList = new List<DailyQuestItem>();
            _m_lItemList = new List<GGUIWndDailyQuestContainerItem>();
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_list"></param>
        public void setShowData(List<DailyQuestItem> _list,bool _isShowOnceGet)
        {
            _m_showList.Clear();
            _m_showList.AddRange(_list);
            _m_showList.Sort(_sortList);

            int count = 0;
            GGUIWndDailyQuestContainerItem itemWnd; 
            //遍历玩家数据
            for (int i = 0; i < _m_showList.Count; i++)
            {
                DailyQuestItem info = _m_showList[i];
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];

                itemWnd.showWnd();
                itemWnd.setInfo(info, _isShowOnceGet);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }

        //列表排序：1、可领取奖励的任务item>未完成任务item>未解锁的任务item>已领取已完成的任务item  2、根据日常任务表格ID从小到大，从上到下进行排列
        private int _sortList(DailyQuestItem _a, DailyQuestItem _b)
        {
            if (_a == null || _b == null)
                return 0;

            if (_a.getDailyQuestState().CompareTo(_b.getDailyQuestState()) != 0)
                return _a.getDailyQuestState().CompareTo(_b.getDailyQuestState());
            else
                return _a.dailyQuestId.CompareTo(_b.dailyQuestId);
        }

        //点击领取奖励
        private void _onClickGetReward(GGUIWndDailyQuestContainerItem _item, RectTransform _starTransform)
        {
            if (_m_aOnClickGetReward != null)
                _m_aOnClickGetReward(_item, _starTransform);
        }

        //点击一键领取奖励
        private void _onClickOnceGetReward()
        {
            List<GGUIWndDailyQuestContainerItem> itemList = new List<GGUIWndDailyQuestContainerItem>();
            
            //获取当前可领取的位置
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                GGUIWndDailyQuestContainerItem item = _m_lItemList[i];
                if (null == item || !item.isShow || null == item.dailyQuestInfo || null == item.wnd)
                    return;

                EDailyQuestState curState = item.dailyQuestInfo.getDailyQuestState();
                if (curState == EDailyQuestState.CAN_GET)
                {
                    itemList.Add(item);
                }
            }

            if (_m_aOnClickOnceGetReward != null)
                _m_aOnClickOnceGetReward(itemList);
        }
        
    }
}
