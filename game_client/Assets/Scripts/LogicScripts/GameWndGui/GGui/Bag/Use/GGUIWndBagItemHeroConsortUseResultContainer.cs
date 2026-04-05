using Common.BagItemUseObj;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴家人使用道具完成伙伴家人列表
    /// </summary>
    public class GGUIWndBagItemHeroConsortUseResultContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoBagItemHeroConsortUseResultContainerItem, GGUIMonoBagItemHeroConsortUseResultContainer, GGUIWndBagItemHeroConsortUseResultContainerItem>
    {
        //item列表
        protected List<GGUIWndBagItemHeroConsortUseResultContainerItem> _m_lItemList;

        public GGUIWndBagItemHeroConsortUseResultContainer(GGUIMonoBagItemHeroConsortUseResultContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndBagItemHeroConsortUseResultContainerItem _createItemWnd(GGUIMonoBagItemHeroConsortUseResultContainerItem _itemMono)
        {
            GGUIWndBagItemHeroConsortUseResultContainerItem item = new GGUIWndBagItemHeroConsortUseResultContainerItem(_itemMono);
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
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndBagItemHeroConsortUseResultContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_infoList"></param>
        public void showItemList(List<BagItemUse_ConsortShowInfo> _infoList)
        {
            if (wnd == null || _infoList == null || _m_lItemList == null)
                return;

            GGUIWndBagItemHeroConsortUseResultContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _infoList.Count; i++)
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
                itemWnd.setInfo(_infoList[i]);
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
        /// 设置信息
        /// </summary>
        /// <param name="_infoList"></param>
        public void showItemList(List<BagItemUse_HeroShowInfo> _infoList)
        {
            if (wnd == null || _infoList == null || _m_lItemList == null)
                return;

            GGUIWndBagItemHeroConsortUseResultContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _infoList.Count; i++)
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
                itemWnd.setInfo(_infoList[i]);
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
    }
}
