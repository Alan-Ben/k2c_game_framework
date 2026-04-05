using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 晚间副本排行榜奖励列表
    /// </summary>
    public class GGUIWndEveningDungeonRankRewardContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoEveningDungeonRankRewardContainerItem, GGUIMonoEveningDungeonRankRewardContainer, GGUIWndEveningDungeonRankRewardContainerItem>
    {
        //窗口容器
        protected List<GGUIWndEveningDungeonRankRewardContainerItem> _m_lItemList;

        public GGUIWndEveningDungeonRankRewardContainer(GGUIMonoEveningDungeonRankRewardContainer _containerMono) : base(_containerMono)
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
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndEveningDungeonRankRewardContainerItem>();
        }

        protected override GGUIWndEveningDungeonRankRewardContainerItem _createItemWnd(GGUIMonoEveningDungeonRankRewardContainerItem _itemMono)
        {
            // 创建对象
            GGUIWndEveningDungeonRankRewardContainerItem item = new GGUIWndEveningDungeonRankRewardContainerItem(_itemMono);
            return item;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(List<EveningDungeonRankRewardRefObj> rankInfoList)
        {
            if (wnd == null || rankInfoList == null)
                return;

            GGUIWndEveningDungeonRankRewardContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < rankInfoList.Count; i++)
            {
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
                itemWnd.setInfo(rankInfoList[i]);
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

        /// <summary>
        /// 根据自己的排名刷新显隐
        /// </summary>
        /// <param name="_rank"></param>
        public void refreshBySelfRank(long _rank)
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _m_lItemList[i]?.refreshBySelfRank(_rank);
            }
        }
    }
}