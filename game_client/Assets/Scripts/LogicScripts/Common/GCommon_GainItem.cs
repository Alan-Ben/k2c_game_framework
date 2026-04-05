using NPCommon;
using NPEnum;
using System;
using System.Collections.Generic;
using Common.PlayerEnum;
using CommonEnum;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 获得物品过滤的数据集合
        /// </summary>
        public class GainItemFilterData
        {
            //需要物品转换表现的数据
            public List<NPCommon_ItemInfo> itemExchangeInfoList = null;
            //获得骑士表现的数据
            public List<NPCommon_ItemInfo> heroInfoList = null;
            //获得任务表现数据
            public List<NPCommon_ItemInfo> questInfoList = null;
            //获得炫耀性外观表现数据
            public List<NPCommon_ItemInfo> showOffInfoList = null;
            //玩家皮肤
            public List<NPCommon_ItemInfo> playerSkinList = null;
            //获得情人
            public List<NPCommon_ItemInfo> consortList = null;
            //获得藏品
            public List<NPCommon_ItemInfo> equipList = null;
            //获得sysInfo道具
            public List<NPCommon_ItemInfo> sysInfoList = null;

            public GainItemFilterData()
            {
                itemExchangeInfoList = new List<NPCommon_ItemInfo>();
                heroInfoList = new List<NPCommon_ItemInfo>();
                questInfoList = new List<NPCommon_ItemInfo>();
                showOffInfoList = new List<NPCommon_ItemInfo>();
                playerSkinList = new List<NPCommon_ItemInfo>();
                consortList = new List<NPCommon_ItemInfo>();
                equipList = new List<NPCommon_ItemInfo>();
                sysInfoList = new List<NPCommon_ItemInfo>();
            }

            /// <summary>
            /// 数据总数
            /// </summary>
            /// <returns></returns>
            public long totalCount()
            {
                long itemExchangeInfoListCount = itemExchangeInfoList != null ? itemExchangeInfoList.Count : 0;
                long heroInfoListCount = heroInfoList != null ? heroInfoList.Count : 0;
                long questInfoListCount = questInfoList != null ? questInfoList.Count : 0;
                long showOffInfoListCount = showOffInfoList != null ? showOffInfoList.Count : 0;
                long playerSkinCount = playerSkinList != null ? playerSkinList.Count : 0;
                long consortListCount = consortList != null ? consortList.Count : 0;
                long equipCount = equipList != null ? equipList.Count : 0;
                long sysInfoCount = sysInfoList != null ? sysInfoList.Count : 0;
                return itemExchangeInfoListCount + heroInfoListCount + questInfoListCount + showOffInfoListCount + playerSkinCount + consortListCount + equipCount + sysInfoCount;
            }
            
            public void clear()
            {
                itemExchangeInfoList?.Clear();
                heroInfoList?.Clear();
                questInfoList?.Clear();
                showOffInfoList?.Clear();
                playerSkinList?.Clear();
                consortList?.Clear();
                equipList?.Clear();
            }
        }
        
        /// <summary>
        /// 通用的处理获得特殊物品的数据整理，并返回剔除后的结果
        /// </summary>
        public static List<NPCommon_ItemInfo> commonDealGainSpecialItem(List<NPCommon_ItemInfo> _srcItemList)
        {
            GainItemFilterData itemFilterData = null;
            return commonDealGainSpecialItem(_srcItemList, ref itemFilterData);
        }

        /// <summary>
        /// 通用的处理获得特殊物品的数据整理，并返回剔除后的结果
        /// </summary>
        public static List<NPCommon_ItemInfo> commonDealGainSpecialItem(List<NPCommon_ItemInfo> _srcItemList, ref GainItemFilterData _itemFilterData)
        {
            if (null == _srcItemList || _srcItemList.Count == 0)
            {
                return null;
            }
            
            //最终的物品数据列表
            List<NPCommon_ItemInfo> finalItemList = new List<NPCommon_ItemInfo>();
            finalItemList.AddRange(_srcItemList);

            //循环剔除一些需要单独表现的类型物品
            for (int i = finalItemList.Count - 1; i >= 0; i--)
            {
                NPCommon_ItemInfo itemInfo = finalItemList[i];
                if (null == itemInfo)
                {
                    finalItemList.RemoveAt(i);
                    continue;
                }

                //判断是否需要处理的类型
                ENPItemType itemType = (ENPItemType)itemInfo.getItemType();

                //物品转换的类型
                if (willItemExchange(itemInfo))
                {
                    _itemFilterData?.itemExchangeInfoList?.Add(itemInfo);
                    finalItemList.RemoveAt(i);
                }
                //骑士类型
                else if (itemType == ENPItemType.HERO)
                {
                    _itemFilterData?.heroInfoList?.Add(itemInfo);
                    finalItemList.RemoveAt(i);
                }
                else if (itemType == ENPItemType.QUEST)
                {
                    _itemFilterData?.questInfoList?.Add(itemInfo);
                    //目前先不展示任务 从结果中剔除
                    finalItemList.RemoveAt(i);
                }
                else if (itemType == ENPItemType.MAIL)
                {
                    //邮件走邮件系统，不做展示
                    finalItemList.RemoveAt(i);
                }
                else if (itemType == ENPItemType.BUILDING)
                {
                    //建筑获得，不做展示
                    finalItemList.RemoveAt(i);
                }
                else if (itemType == ENPItemType.ICON || itemType == ENPItemType.ICON_BGK || itemType == ENPItemType.BUBBLE || itemType == ENPItemType.TITLE)
                {
                    _itemFilterData?.showOffInfoList?.Add(itemInfo);
                    finalItemList.RemoveAt(i);
                }
                else if (itemType == ENPItemType.PLAYER_SKIN)
                {
                    _itemFilterData?.playerSkinList?.Add(itemInfo);
                    finalItemList.RemoveAt(i);
                }
                else if (itemType == ENPItemType.CONSORT)
                {
                    _itemFilterData?.consortList?.Add(itemInfo);
                    finalItemList.RemoveAt(i);
                }
                //藏品类型
                else if (itemType == ENPItemType.EQUIP)
                {
                    //获得过的次数
                    long equipOwnCount = NPPlayer.instance.eventRecordComp.getValue(EPlayerEventRecordType.GAIN_EQUIP, itemInfo.getSubId());
                    //品质
                    EQuality equipQuality = GCommon.getItemQuality(ENPItemType.EQUIP, itemInfo.getSubId());
                    //第一次获得或者UR品质才需要特殊展示
                    if (equipOwnCount == itemInfo.getCount() && equipQuality != EQuality.RED)
                    {
                        //首次获得需要展示一次，其余的展示通用的弹窗
                        _itemFilterData?.equipList?.Add(new NPCommon_ItemInfo(itemInfo.getItemType(), itemInfo.getSubId(), 1, null));
                        if(itemInfo.getCount() == 1)
                            finalItemList.RemoveAt(i);
                    }
                    else if (equipQuality == EQuality.RED)
                    {
                        //UR品质需要一个一个展示
                        for (int j = 0; j < itemInfo.getCount(); j++)
                        {
                            _itemFilterData?.equipList?.Add(new NPCommon_ItemInfo(itemInfo.getItemType(), itemInfo.getSubId(), 1, null));
                        }
                        finalItemList.RemoveAt(i);
                    }
                }
                else if (itemType == ENPItemType.SYS_INFO)
                {
                    //特殊展示的sysInfo道具
                    //是否是触发商店好评的道具
                    if (itemInfo.getSubId() == GRefdataCoreMgr.instance.npGeneral.store_reviews_trigger_common_item.itemId)
                    {
                        _itemFilterData?.sysInfoList?.Add(itemInfo);
                        finalItemList.RemoveAt(i);
                    }
                    
                    // 判断是否是触发游历地点解锁展示的道具
                    TravelPosRefObj travelUnlockPos = null;
                    GRefdataCoreMgr.instance.travelPosCore.dealAllRef((posRefObj) =>
                    {
                        if(travelUnlockPos == null && posRefObj != null && posRefObj.unlock_sys_info_id == itemInfo.getSubId())
                        {
                            travelUnlockPos = posRefObj;
                        }
                    });
                    if(travelUnlockPos != null)
                    {
                        WinMsg.SendMsg(WinMsgType.TRAVEL_POS_UNLOCK, travelUnlockPos);
                        finalItemList.RemoveAt(i);
                    }
                }
            }

            return finalItemList;
        }
        
        public static bool willItemExchange(NPCommon_ItemInfo _itemInfo)
        {
            byte[] exchangeData = _itemInfo?.getExtData();
            if (exchangeData == null || exchangeData.Length == 0)
                return false;

            return true;
        }
        
        /// <summary>
        /// 获取奖励的通用处理，后续业务协议单独处理的奖励也走这个
        /// </summary>
        public static void dealGainItem(List<NPCommon_ItemInfo> _srcItemList, string _titleKey = TransKeyConst.common_getreward_tip, Action _closeAction = null)
        {
            if (null == _srcItemList)
            {
                _closeAction?.Invoke();
                return;
            }

            RewardQueueMgr.instance.addDealer(new CommonRewardDealer(_srcItemList, _titleKey, _closeAction));
        }

        /// <summary>
        /// 获取奖励的通用处理, 非Notice版本
        /// </summary>
        /// <param name="_srcItemList"></param>
        /// <param name="_titleKey"></param>
        /// <param name="_closeAction"></param>
        public static void dealGainItemDontUseNotice(List<NPCommon_ItemInfo> _srcItemList, string _titleKey = TransKeyConst.common_getreward_tip, Action _closeAction = null)
        {
            RewardQueueMgr.instance.addDealer(new CommonRewardDealer(_srcItemList, _titleKey, _closeAction));
        }

        #region 获取奖励的tip展示

        /// <summary>
        /// 显示获得奖励tip
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_useCommaNum">是否使用带逗号的数字，如10,000</param>
        public static void showGainRewardTip(List<NPCommon_ItemInfo> _itemList, bool _useCommaNum = true)
        {
            if (_itemList == null)
                return;

            //最终的物品数据列表
            List<NPCommon_ItemInfo> finalItemList = dealGainSpecialItem(_itemList);
            if (null == finalItemList)
                return;

            //无数据则不处理
            if (null == finalItemList || finalItemList.Count <= 0)
                return;

            NPCommon_ItemInfo itemInfo = null;
            for (int i = 0; i < finalItemList.Count; i++)
            {
                itemInfo = finalItemList[i];
                if (itemInfo == null)
                    continue;
                
                showRewardTip((ENPItemType)itemInfo.getItemType(), itemInfo.getSubId(), itemInfo.getCount(), _useCommaNum);
            }
        }

        /// <summary>
        /// 显示获得奖励tip
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_useCommaNum">是否使用带逗号的数字，如10,000</param>
        public static void showGainRewardTip(List<_IItem> _itemList, bool _useCommaNum = true)
        {
            if (_itemList == null)
                return;

            _IItem itemInfo = null;
            for (int i = 0; i < _itemList.Count; i++)
            {
                itemInfo = _itemList[i];
                if (itemInfo == null)
                    continue;

                showRewardTip(itemInfo.getItemType(), itemInfo.subId, itemInfo.getCount(), _useCommaNum);
            }
        }

        /// <summary>
        /// 显示获得奖励tip
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_useCommaNum">是否使用带逗号的数字，如10,000</param>
        public static void showGainRewardTip(List<CommonItemData> _itemList, bool _useCommaNum = true)
        {
            if (_itemList == null)
                return;

            _IItem itemInfo = null;
            for (int i = 0; i < _itemList.Count; i++)
            {
                itemInfo = _itemList[i];
                if (itemInfo == null)
                    continue;

                showRewardTip(itemInfo.getItemType(), itemInfo.subId, itemInfo.getCount(), _useCommaNum);
            }
        }
        
        /// <summary>
        /// 获取物品数量文本
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_itemId"></param>
        /// <param name="_count"></param>
        /// <param name="_useCommaNum"></param>
        /// <returns></returns>
        private static string getItemValueStr(ENPItemType _itemType, long _itemId, long _count, bool _useCommaNum = true)
        {
            
            //部分特殊类型需要将数量转化为1
            switch (_itemType)
            {
                case ENPItemType.TITLE:
                case ENPItemType.ICON:
                case ENPItemType.ICON_BGK:
                case ENPItemType.BUBBLE:
                case ENPItemType.QUEST:
                    _count = 1;
                    break;
                default:
                    break;
            }

            if (_itemType == ENPItemType.CURRENCY && _itemId == (long) ECurrency.SILVER)
            {
                return _count.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD);
            }
            else
            {
                return getCommaValueStr(_count, _useCommaNum);
            }
        }
        /// <summary>
        /// 统一调用的展示单个物品tip信息
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_itemId"></param>
        /// <param name="_count"></param>
        public static void showRewardTip(ENPItemType _itemType, long _itemId, long _count, bool _useCommaNum = true)
        {
            //邮件类型的不做展示
            if (_itemType == ENPItemType.MAIL)
                return;
            
            NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(_itemType, _itemId), getItemValueStr(_itemType, _itemId, _count, _useCommaNum));
        }
        
        /// <summary>
        /// 显示获得奖励tip
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_itemType"></param>
        /// <param name="_itemId"></param>
        /// <param name="_count"></param>
        /// <param name="_useCommaNum"></param>
        public static void showTextRewardTip(string _text, ENPItemType _itemType, long _itemId, long _count,
            bool _useCommaNum = true)
        {
            NPGUIAddSceneCenterTip.instance.showIconTextTextTip(getItemTexIcon(_itemType, _itemId), _text, getItemValueStr(_itemType, _itemId, _count, _useCommaNum));
        }

        #endregion
        
        /// <summary>
        /// 处理获得特殊物品的表现，并返回剔除后的结果
        /// </summary>
        /// <param name="_srcItemList"></param>
        /// <returns></returns>
        public static List<NPCommon_ItemInfo> dealGainSpecialItem(List<NPCommon_ItemInfo> _srcItemList)
        {
            if (null == _srcItemList || _srcItemList.Count == 0)
                return null;

            //过滤的
            GainItemFilterData gainItemFilterData = new GainItemFilterData();

            //最终的物品数据列表
            List<NPCommon_ItemInfo> finalItemList = commonDealGainSpecialItem(_srcItemList, ref gainItemFilterData);

            if (null == gainItemFilterData)
                return null;

            CommonRewardDealer.showSpecial(gainItemFilterData, null);

            return finalItemList;
        }
    }
}