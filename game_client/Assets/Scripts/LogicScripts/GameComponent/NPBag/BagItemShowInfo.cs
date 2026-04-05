
namespace GOE
{
    /// <summary>
    /// 背包物品的展示数据，在UI中存储本数据进行展示处理
    /// </summary>
    public class BagItemShowInfo
    {
        public BagItem bagItem { get { return _m_bagItem; } }
        public CommonItemData itemData { get { return _m_itemData; } }
        public BagItemRefObj bagItemRef { get { return _m_bagItemRef; } }

        public BagItemShowInfo(BagItem _bagItem)
        {
            _m_bagItem = _bagItem;
            if(_m_bagItem !=null)
                _m_bagItemRef = _m_bagItem.itemRefObj;
            _m_itemData = new CommonItemData(_bagItem);
        }

        public BagItemShowInfo(BagItem _bagItem, CommonItemData _itemData, BagItemRefObj _bagItemRef)
        {
            _m_bagItem = _bagItem;
            _m_itemData = _itemData;
            _m_bagItemRef = _bagItemRef;
        }

        private CommonItemData _m_itemData; //基本数据
        private BagItem _m_bagItem; //其他数据
        private BagItemRefObj _m_bagItemRef; //其他数据

        /// <summary>
        /// 能否使用
        /// </summary>
        public bool isUseItem { get { return _m_bagItem != null && _m_bagItem.itemUseRefObj != null; } }

        /// <summary>
        /// 是否达到使用条件
        /// </summary>
        public bool isItemCanUse{get{ return _m_bagItem != null && _m_bagItem.isItemCanUse; }}

        /// <summary>
        /// 是否为新增加的物品
        /// </summary>
        public bool isNewAddItem { get { return _m_bagItem != null && _m_bagItem.isNewAddItem; } }
        /// <summary>
        /// 是否为新物品
        /// </summary>
        public bool isNew { get { return _m_bagItem != null && _m_bagItem.isNew; } }

        public void setBagItem(BagItem _bagItem)
        {
            _m_bagItem = _bagItem;
            //同步个数
            _m_itemData.setCount(_bagItem.count);
        }
        public void setCount(long count)
        {
            _m_itemData.setCount(count);
        }

    }
}
