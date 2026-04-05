using System;
using System.Collections.Generic;
using ALPackage;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 通用事件-事件奖励
    /// </summary>
    [Serializable]
    public class CommonEventRewardRefObj
    {
        public long id;//奖励id
        public long Id { get { return id; } set { id = value; } }

        
        public string item_list;//道具列表
        public string itemList { get { return item_list; } set { item_list = value; } }
        private List<NPCommonCostItem> _m_lItemList;//道具列表
        [Ignore]
        public List<NPCommonCostItem> lItemList
        {
            get
            {
                if (_m_lItemList == null)
                    _m_lItemList = NPCommonCostItem.readList(item_list);

                return _m_lItemList;
            }
        }

        public static string assetPath { get { return "refdata_db/common_event.unity3d"; } }
        public static string objName { get { return "refdata_db/common_event_reward.txt"; } }
        public static string tableName { get { return "common_event_reward"; } }
    }
}