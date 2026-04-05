using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 限时冲榜列表
    /// </summary>
    public class GGUIWndRankRushContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoRankRushContainerItem, GGUIMonoRankRushContainer, GGUIWndRankRushContainerItem>
    {
        //窗口容器
        protected List<GGUIWndRankRushContainerItem> _m_lItemList;

        public GGUIWndRankRushContainer(GGUIMonoRankRushContainer _containerMono) : base(_containerMono)
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
            _m_lItemList = new List<GGUIWndRankRushContainerItem>();
        }

        protected override GGUIWndRankRushContainerItem _createItemWnd(GGUIMonoRankRushContainerItem _itemMono)
        {
            // 创建对象
            GGUIWndRankRushContainerItem item = new GGUIWndRankRushContainerItem(_itemMono);
            return item;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(List<ActivityRankRushInfo> _rankRushInfoList)
        {
            if (wnd == null || _rankRushInfoList == null)
                return;

            GGUIWndRankRushContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _rankRushInfoList.Count; i++)
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
                itemWnd.setInfo(_rankRushInfoList[i]);
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

            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _rankRushInfoList.Count == 0);
        }
    }
}
