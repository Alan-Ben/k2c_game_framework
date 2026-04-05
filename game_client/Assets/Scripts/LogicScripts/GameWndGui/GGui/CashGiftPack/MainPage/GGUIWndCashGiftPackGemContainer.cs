using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 钻石商店列表item容器
    /// </summary>
    public class GGUIWndCashGiftPackGemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoCashGiftPackGemContainerItem, GGUIMonoCashGiftPackGemContainer, GGUIWndCashGiftPackGemContainerItem>
    {
        //item列表
        protected List<GGUIWndCashGiftPackGemContainerItem> _m_lItemList;

        public GGUIWndCashGiftPackGemContainer(GGUIMonoCashGiftPackGemContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndCashGiftPackGemContainerItem _createItemWnd(GGUIMonoCashGiftPackGemContainerItem _itemMono)
        {
            GGUIWndCashGiftPackGemContainerItem item = new GGUIWndCashGiftPackGemContainerItem(_itemMono);
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
            _m_lItemList = new List<GGUIWndCashGiftPackGemContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<GiftPackRefObj> _refList)
        {
            if (_refList == null || _m_lItemList == null)
                return;

            GGUIWndCashGiftPackGemContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _refList.Count; i++)
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
                itemWnd.setInfo(_refList[i]);
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
