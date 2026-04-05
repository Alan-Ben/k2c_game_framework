using NPEnum;

namespace GOE
{
    /// <summary>
    /// 获取途径信息-背包物品
    /// </summary>
    public class NPAccessBagItemInfo : _INPAccessInfoInterface
    {
        private BagItemRefObj _m_rBagItemRef;
        
        public NPAccessBagItemInfo(BagItemRefObj _bagItemRefObj)
        {
            _m_rBagItemRef = _bagItemRefObj;
        }

        public BagItemRefObj bagItemRefObj { get { return _m_rBagItemRef; } }

        /// <summary>
        /// 排序id
        /// </summary>
        public long sortId { get { return _m_rBagItemRef != null ? _m_rBagItemRef.id : 0; } }
        /// <summary>
        /// 类型
        /// </summary>
        public ENPAccessInfoType type { get { return ENPAccessInfoType.BAG_ITEM; } }
        /// <summary>
        /// 品质
        /// </summary>
        public EQuality quality { get { return GCommon.getItemQuality(ENPItemType.BAG_ITEM, _m_rBagItemRef?.id ?? 0); } }
    }
}
