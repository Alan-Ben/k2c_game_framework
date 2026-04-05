using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗选择增益道具列表
    /// </summary>
    public class GGUIWndArenaBattleSelectBuffContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoArenaBattleSelectBuffContainerItem, GGUIMonoArenaBattleSelectBuffContainer, GGUIWndArenaBattleSelectBuffContainerItem>
    {
        //item列表
        protected List<GGUIWndArenaBattleSelectBuffContainerItem> _m_lItemList;
        //点击购买回调
        private Action<GGUIWndArenaBattleSelectBuffContainerItem> _m_aOnClickBuy;


        /// <summary>
        /// 点击购买回调
        /// </summary>
        public Action<GGUIWndArenaBattleSelectBuffContainerItem> onClickBuy
        {
            get => _m_aOnClickBuy;
            set => _m_aOnClickBuy = value;
        }

        public GGUIWndArenaBattleSelectBuffContainer(GGUIMonoArenaBattleSelectBuffContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndArenaBattleSelectBuffContainerItem _createItemWnd(GGUIMonoArenaBattleSelectBuffContainerItem _itemMono)
        {
            GGUIWndArenaBattleSelectBuffContainerItem item = new GGUIWndArenaBattleSelectBuffContainerItem(_itemMono);
            item.onClickBuy += _onClickItemBuy;
            return item;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (_m_lItemList != null)
            {
                foreach (GGUIWndArenaBattleSelectBuffContainerItem item in _m_lItemList)
                {
                    item?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            if (_m_lItemList != null)
            {
                foreach (GGUIWndArenaBattleSelectBuffContainerItem item in _m_lItemList)
                {
                    item?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lItemList != null)
            {
                foreach (GGUIWndArenaBattleSelectBuffContainerItem item in _m_lItemList)
                {
                    item?.discard();
                }
                _m_lItemList.Clear();
                _m_lItemList = null;
            }

            _m_aOnClickBuy = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndArenaBattleSelectBuffContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_infoList"></param>
        public void showItemList(List<long> _infoList)
        {
            if (_infoList == null || _m_lItemList == null)
                return;

            GGUIWndArenaBattleSelectBuffContainerItem itemWnd = null;
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

        //点击购买
        private void _onClickItemBuy(GGUIWndArenaBattleSelectBuffContainerItem _item)
        {
            _m_aOnClickBuy?.Invoke(_item);
        }
    }
}
