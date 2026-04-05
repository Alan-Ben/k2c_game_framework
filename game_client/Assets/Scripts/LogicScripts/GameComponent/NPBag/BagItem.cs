using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPEnum;

namespace GOE
{ 
    // 背包物品数据结构
    public class BagItem
    {
        // 物品配置
        private UniformItemObj _m_baseItemData;//物品基础数据
        private BagItemRefObj _m_rItemRefObj = null; //物品其他数据
        private BagItemUseRefObj _m_rUseRef;//背包物品使用相关数据
        private NPQualityRefObj _m_qrQualityRef = null; //品质数据
        private NPSORewardRefObj _m_rRewardRefObj;//物品奖励数据       


        //物品数量
        private long _m_cItemCount;
        // 最新获得的时间戳
        private int _m_iLastGetTimeS;
        //首次获得物品的时间戳
        private int _m_sNewItemTimeS;
        //最后一次点击物品的时间戳
        private int _m_sClickItemTimeS;


        public BagItem(NPCommon.NPCommon_BagItemInfo _itemInfo)
        {
            //属性赋值
            _m_iLastGetTimeS = _itemInfo.getLastGetTimeS();
            _m_sNewItemTimeS = _itemInfo.getNewItemTimeS();
            _m_sClickItemTimeS = _itemInfo.getClickItemTimeS();
            _m_cItemCount = _itemInfo.getItemCount();

            _m_rItemRefObj = GRefdataCoreMgr.instance.bagItemCore.getRef(_itemInfo.getItemId());
            //必须要存在
            if (null == _m_rItemRefObj)
            {
                Debug.LogError("错误：bag_item表查找不到此id:" + _itemInfo.getItemId());
                return;
            }
            //物品基础数据 必须要存在
            _m_baseItemData = UniformItemSqliteAssistant.getUnifromItem(NPEnum.ENPItemType.BAG_ITEM, _m_rItemRefObj.id);
            if (null == _m_baseItemData)
            {
                Debug.LogError("错误：uniform表查找不到此bag_item id:" + _itemInfo.getItemId());
                return;
            }
            //品质相关数据
            _m_qrQualityRef = GRefdataCoreMgr.instance.getQuality(ENPQualityClass.BAG_ITEM, _m_baseItemData.quality);

            //物品使用相关数据
            _m_rUseRef = GRefdataCoreMgr.instance.bagItemUseCore.getRef(_m_rItemRefObj.id);

            //物品奖励相关数据
            if (null != _m_rUseRef)
                _m_rRewardRefObj = GRefdataCoreMgr.instance.rewardMap.getRef(_m_rUseRef.get_reward_id);
        }
        //通用数据
        public UniformItemObj baseItemData { get { return _m_baseItemData; } }
        //其他数据
        public BagItemRefObj itemRefObj { get { return _m_rItemRefObj; } }
        //使用数据 可能为空
        public BagItemUseRefObj itemUseRefObj { get { return _m_rUseRef; } }
        //奖励数据 可能为空
        public NPSORewardRefObj itemRewardRefObj { get { return _m_rRewardRefObj; } }

        public long itemId { get { return _m_rItemRefObj.id; } }

        public ENPItemType itemType { get { return _m_baseItemData.item_type; } }

        public long count { get { return _m_cItemCount; } }
        public EQuality quality { get { return null == _m_baseItemData ? EQuality.NONE : _m_baseItemData.quality; } }

        public NPEnum.ENPBagItemType bagItemType { get { return _m_rItemRefObj.bag_item_type; } }   // 背包物品类型
        public long sortId { get { return _m_rItemRefObj.sort_id; } }   // 排序id
        public int lastGetTimeS { get { return _m_iLastGetTimeS; } }
        public int newItemTiemS { get { return _m_sNewItemTimeS; } }
        public int clickItemTimeS { get { return _m_sClickItemTimeS; } }
        public bool isItemCanUse
        {
            get
            {
                if (null == itemUseRefObj)
                    return false;
                if (_m_cItemCount == 0)
                    return false;
                return _m_rUseRef.use_cond.IsEnable(null);
            }
        }
        /// <summary>
        /// 是否为新增加的物品
        /// </summary>
        public bool isNewAddItem
        {
            get
            {
                //获得的时候比查看背包的时间
                bool isCheckBagTime = _m_iLastGetTimeS - NPPlayer.instance.playerInfo[NPEnum.ENPPlayerParam.LAST_LEFT_BAG_TIME] > 0;

                //获得的时间比点击的时间
                bool isClickTime = _m_iLastGetTimeS - _m_sClickItemTimeS > 0;

                return isCheckBagTime && isClickTime;
            }
        }
        /// <summary>
        /// 是否为新物品
        /// </summary>
        public bool isNew
        {
            get
            {
                //新物品必须要点击才表示查看过
                //如果最后一次点击物品时间大于首次获取时间，则不是新物品
                if (_m_sClickItemTimeS - _m_sNewItemTimeS > 0)
                    return false;
                return true;
            }
        }




        // 更新物品数量
        public void updateCount(long _count)
        {
            _m_cItemCount = _count;
        }

        // 更新获取物品的时间
        public void updateGetTime(int _getTime)
        {
            _m_iLastGetTimeS = _getTime;
        }

        // 更新点击事件标记
        public void updateClickTimeS(int _clickTime)
        {
            _m_sClickItemTimeS = _clickTime;
        }
        public override string ToString()
        {
            return string.Format("bagItemId:  {0},  Name: {1} , Count: {2}, isCanUse:  {3},  isCanSell:{4}", itemId, _m_baseItemData.transName, count, _m_rUseRef != null);
        }
    }
}
