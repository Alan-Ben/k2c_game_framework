using System;
using System.Collections.Generic;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;


namespace GOE
{
    /// <summary>
    /// 联盟相关保存数据
    /// </summary>
    [System.Serializable]
    public class GuildSaverData
    {
        public long guildId;//联盟id
        public GuildBoxesMgrData guildBoxesMgrData = new GuildBoxesMgrData();//联盟宝箱数据管理
    }

    #region 联盟宝箱相关

    /// <summary>
    /// 联盟宝箱保存数据
    /// </summary>
    [Serializable]
    public class GuildBoxSaveData
    {
        public long boxInstanceId;
        public long endTimeMs;
        public long guildBoxId;
        public long senderCid;
        public NPCommonCostItem rewardItem;
    }

    /// <summary>
    /// 联盟宝箱数据管理
    /// </summary>
    [Serializable]
    public class GuildBoxesMgrData
    {
        //免费宝箱列表
        public List<GuildBoxSaveData> freeBoxList = new List<GuildBoxSaveData>();
        //礼包宝箱列表
        public List<GuildBoxSaveData> giftBoxList = new List<GuildBoxSaveData>();

        /// <summary>
        /// 清空无效宝箱数据
        /// </summary>
        public bool clearInvalidBox()
        {
            bool isClear = false;
            for (int i = freeBoxList.Count - 1; i >= 0; i--)
            {
                GuildBoxSaveData boxSaveData = freeBoxList[i];
                if (boxSaveData == null || boxSaveData.endTimeMs <= FpsAndPingMgr.instance.serverTimeTag)
                {
                    freeBoxList.RemoveAt(i);
                    isClear = true;
                }
            }
            for (int i = giftBoxList.Count - 1; i >= 0; i--)
            {
                GuildBoxSaveData boxSaveData = giftBoxList[i];
                if (boxSaveData == null || boxSaveData.endTimeMs <= FpsAndPingMgr.instance.serverTimeTag)
                {
                    giftBoxList.RemoveAt(i);
                    isClear = true;
                }
            }

            return isClear;
        }

        /// <summary>
        /// 添加免费宝箱数据
        /// </summary>
        /// <param name="_boxInstance"></param>
        /// <param name="_boxId"></param>
        /// <param name="_endTimeMs"></param>
        /// <param name="_senderCid"></param>
        /// <param name="_rewardItem"></param>
        public void addFreeBoxData(long _boxInstance, long _boxId, long _endTimeMs, long _senderCid, NPCommonCostItem _rewardItem)
        {
            foreach (var boxSaveData in freeBoxList)
            {
                if (boxSaveData != null && boxSaveData.boxInstanceId == _boxInstance)
                    return;
            }
            freeBoxList.Add(new GuildBoxSaveData
            {
                boxInstanceId = _boxInstance,
                endTimeMs = _endTimeMs,
                guildBoxId = _boxId,
                senderCid = _senderCid,
                rewardItem = _rewardItem
            });
        }

        /// <summary>
        /// 添加礼包宝箱数据
        /// </summary>
        /// <param name="_boxInstance"></param>
        /// <param name="_boxId"></param>
        /// <param name="_endTimeMs"></param>
        /// <param name="_senderCid"></param>
        /// <param name="_rewardItem"></param>
        public void addGiftBoxData(long _boxInstance, long _boxId, long _endTimeMs, long _senderCid, NPCommonCostItem _rewardItem)
        {
            foreach (var boxSaveData in giftBoxList)
            {
                if (boxSaveData != null && boxSaveData.boxInstanceId == _boxInstance)
                    return;
            }
            giftBoxList.Add(new GuildBoxSaveData
            {
                boxInstanceId = _boxInstance,
                endTimeMs = _endTimeMs,
                guildBoxId = _boxId,
                senderCid = _senderCid,
                rewardItem = _rewardItem
            });
        }

        /// <summary>
        /// 清空全部数据
        /// </summary>
        public void clearAll()
        {
            freeBoxList?.Clear();
            giftBoxList?.Clear();
        }
    }

    #endregion
}
