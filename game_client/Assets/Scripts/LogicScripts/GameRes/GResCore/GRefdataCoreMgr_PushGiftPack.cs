
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    //推送礼包相关
    public partial class GRefdataCoreMgr
    {
        private Dictionary<long, List<long>> _m_dItemSoleIdTriggerPushGiftGroupIdListDic;//物品触发的推送礼包组id列表字典(Key是NPCommonItem唯一标识id，Value是推送礼包组id列表)
        
        private void _initPushGiftPackData()
        {
            //构建物品触发的推送礼包组id列表字典
            _m_dItemSoleIdTriggerPushGiftGroupIdListDic = new Dictionary<long, List<long>>();
            foreach (var refObj in pushGiftItemTriggerRefCore.refList)
            {
                if(refObj == null || refObj.item_list == null)
                    continue;

                foreach (var item in refObj.item_list)
                {
                    if(item == null)
                        continue;
                    
                    List<long> tmpPushGiftGroupIdList;
                    long soleId = item.getId();
                    if(!_m_dItemSoleIdTriggerPushGiftGroupIdListDic.TryGetValue(soleId, out tmpPushGiftGroupIdList) || tmpPushGiftGroupIdList == null)
                    {
                        tmpPushGiftGroupIdList = new List<long>();
                        _m_dItemSoleIdTriggerPushGiftGroupIdListDic[soleId] = tmpPushGiftGroupIdList;
                    }
                 
                    tmpPushGiftGroupIdList.Add(refObj.trigger_push_gift_group_id);
                }
            }
        }
        
        /// <summary>
        /// 获取物品触发的推送礼包组id(取第一个展示)
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public long getItemTriggerPushGiftGroupId(NPCommonItem _item)
        {
            if(_m_dItemSoleIdTriggerPushGiftGroupIdListDic == null || _item == null)
                return 0;

            if(!_m_dItemSoleIdTriggerPushGiftGroupIdListDic.TryGetValue(_item.getId(), out List<long> pushGiftGroupIdList) || pushGiftGroupIdList == null)
                return 0;

            return pushGiftGroupIdList.GetFirst();
        }
        
        public long getItemTriggerPushGiftGroupId(ENPItemType _itemType, long _subId)
        {
            if(_m_dItemSoleIdTriggerPushGiftGroupIdListDic == null)
                return 0;

            long soleId = NPCommonItem.getId(_itemType, _subId);
            if(!_m_dItemSoleIdTriggerPushGiftGroupIdListDic.TryGetValue(soleId, out List<long> pushGiftGroupIdList) || pushGiftGroupIdList == null)
                return 0;

            return pushGiftGroupIdList.GetFirst();
        }
    }
}