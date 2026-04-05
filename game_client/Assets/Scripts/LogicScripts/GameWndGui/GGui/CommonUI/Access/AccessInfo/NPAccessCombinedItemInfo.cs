using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 获取途径信息-默认
    /// </summary>
    public class NPAccessCombinedItemInfo : _INPAccessInfoInterface
    {

        /// <summary>
        /// 排序id
        /// </summary>
        public long sortId { get { return 0; } }
        /// <summary>
        /// 类型
        /// </summary>
        public ENPAccessInfoType type { get { return ENPAccessInfoType.COMBIEND; } }
        /// <summary>
        /// 品质
        /// </summary>
        public EQuality quality { get { return EQuality.NONE; } }

        public ENPItemType itemType { get { return _m_itemType; } }
        public long itemId { get { return _m_itemId; } }

        public long oriItemId { get { return _m_oriItemId; } }

        private ENPItemType _m_itemType;
        private long _m_itemId;

        private long _m_oriItemId;

        public NPAccessCombinedItemInfo(ENPItemType _itemType, long _itemId)
        {
            _m_itemType = _itemType;
            _m_itemId = _itemId;

            List<ItemConvertRefObj> list = GRefdataCoreMgr.instance.itemConvertCore.refList;
            ItemConvertRefObj temp = null;
            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i];
                if (null == temp)
                    continue;

                if (temp.target_item.itemId == _itemId && temp.target_item.itemType == _itemType)
                {
                    _m_oriItemId = temp.bag_item_id;
                    break;
                }
            }
        }

    }
}
