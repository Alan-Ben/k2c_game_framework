using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜详情奖励页面列表
    /// </summary>
    public class GGUIWndGuildRankRushDetailRewardContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoGuildRankRushDetailRewardContainerItem, GGUIMonoGuildRankRushDetailRewardContainer, GGUIWndGuildRankRushDetailRewardContainerItem>
    {
        //窗口容器
        protected List<GGUIWndGuildRankRushDetailRewardContainerItem> _m_lItemList;

        public GGUIWndGuildRankRushDetailRewardContainer(GGUIMonoGuildRankRushDetailRewardContainer _containerMono) : base(_containerMono)
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
            _m_lItemList = new List<GGUIWndGuildRankRushDetailRewardContainerItem>();
        }

        protected override GGUIWndGuildRankRushDetailRewardContainerItem _createItemWnd(GGUIMonoGuildRankRushDetailRewardContainerItem _itemMono)
        {
            // 创建对象
            GGUIWndGuildRankRushDetailRewardContainerItem item = new GGUIWndGuildRankRushDetailRewardContainerItem(_itemMono);
            return item;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(List<GActivityRankRewardRefObj> rankRushInfoList)
        {
            if (wnd == null || rankRushInfoList == null)
                return;

            GGUIWndGuildRankRushDetailRewardContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < rankRushInfoList.Count; i++)
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
                itemWnd.setInfo(rankRushInfoList[i]);
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
