using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 现金礼包组商品容器
    /// </summary>
    public class GGUIWndCashGiftPackGroupGoodsContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoCashGiftPackPageGoodsContainerItem, GGUIMonoCashGiftPackPageGoodsContainer, GGUIWndCashGiftPackGroupGoodsContainerItem>
    {
        //item列表
        protected List<GGUIWndCashGiftPackGroupGoodsContainerItem> _m_lItemList;

        public GGUIWndCashGiftPackGroupGoodsContainer(GGUIMonoCashGiftPackPageGoodsContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndCashGiftPackGroupGoodsContainerItem _createItemWnd(GGUIMonoCashGiftPackPageGoodsContainerItem _itemMono)
        {
            GGUIWndCashGiftPackGroupGoodsContainerItem item = new GGUIWndCashGiftPackGroupGoodsContainerItem(_itemMono);
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
            _m_lItemList = new List<GGUIWndCashGiftPackGroupGoodsContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<long> _giftPackIdList)
        {
            if (_giftPackIdList == null || _m_lItemList == null)
                return;

            _giftPackIdList.Sort(_sortGiftPack);
            GGUIWndCashGiftPackGroupGoodsContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _giftPackIdList.Count; i++)
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
                itemWnd.setInfo(_giftPackIdList[i]);
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

        //排序，未售罄>已售罄，免费>付费，排序id从小到大，id从小到大
        private int _sortGiftPack(long _idA,long _idB)
        {
            //未售罄>已售罄
            bool sellOutA = NPPlayer.instance.giftPackComp.isGiftPackSellOut(_idA);
            bool sellOutB = NPPlayer.instance.giftPackComp.isGiftPackSellOut(_idB);
            if (sellOutA != sellOutB)
                return sellOutA.CompareTo(sellOutB);

            GiftPackRefObj gfiPackRefObjA = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_idA);
            GiftPackRefObj gfiPackRefObjB = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_idB);
            if (gfiPackRefObjA == null || gfiPackRefObjB == null)
                return 0;

            //免费>付费
            if (gfiPackRefObjA.isFree != gfiPackRefObjB.isFree)
                return -(gfiPackRefObjA.isFree.CompareTo(gfiPackRefObjB.isFree));

            //排序id从小到大
            if (gfiPackRefObjA.sort_id != gfiPackRefObjB.sort_id)
                return gfiPackRefObjA.sort_id.CompareTo(gfiPackRefObjB.sort_id);

            //id从小到大
            return _idA.CompareTo(_idB);
        }
    }
}
