using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 冲榜详情奖励页面列表
    /// </summary>
    public class GGUIWndRankRushAccessContainer : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoAccessWayItem, GGUIMonoRankRushAccessContainer, NPGGUIWndAccessWayItem>
    {
        //窗口容器
        protected List<NPGGUIWndAccessWayItem> _m_lItemList;

        public GGUIWndRankRushAccessContainer(GGUIMonoRankRushAccessContainer _containerMono) : base(_containerMono)
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
            _m_lItemList = new List<NPGGUIWndAccessWayItem>();
        }

        protected override NPGGUIWndAccessWayItem _createItemWnd(NPGGUIMonoAccessWayItem _itemMono)
        {
            // 创建对象
            NPGGUIWndAccessWayItem item = new NPGGUIWndAccessWayItem(_itemMono);
            return item;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(List<long> _accessIdList)
        {
            if (wnd == null || _accessIdList == null)
                return;

            NPGGUIWndAccessWayItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _accessIdList.Count; i++)
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
                itemWnd.setInfo(new NPAccessInfo(_accessIdList[i], true));
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

            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _accessIdList.Count == 0);
        }
    }
}
