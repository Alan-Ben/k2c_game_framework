using System;
using System.Collections.Generic;
using Common.InnObj;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class InnReceiveInfo
    {
        private long _m_liningGuestStartNum;
        [NotNull] private readonly List<GuestLiningData> _m_guestLiningDataList;


        public InnReceiveInfo()
        {
            _m_guestLiningDataList = new List<GuestLiningData>();
        }


        public event Action onUpdate;
        public long guestIdBeginId { get { return _m_liningGuestStartNum; } }


        /// <summary>
        /// 获取当前时间下的指定个数的客人 ID 列表，同时 out 已经积累的奖励数量
        /// </summary>
        public long getGuestCountNow(out long _rewardCount, out long _guestIdBeginId)
        {
            _rewardCount = getRewardCountNow();
            _guestIdBeginId = _m_liningGuestStartNum + _rewardCount;
            return getGuestCountAfter(_guestIdBeginId);
        }
        public long getGuestCountAfter(long _startGuestId)
        {
            if (_m_guestLiningDataList.Count <= 0)
                return 0;
            
            // 计算总的客人数量
            long totalGuestCount = 0;
            foreach (GuestLiningData liningData in _m_guestLiningDataList)
            {
                totalGuestCount += liningData.guestNum;
            }
            
            // 计算起始客人ID相对于队列开始的偏移量
            long offsetFromStart = _startGuestId - _m_liningGuestStartNum;
            long result = totalGuestCount - offsetFromStart;
            if (result < 0)
                return 0;

            return result;
        }
        public long getHadSettleGuestsCount()
        {
            return _m_liningGuestStartNum + getRewardCountNow();
        }
        /// <summary>
        /// 获得当前积累的奖励数量
        /// </summary>
        public long getRewardCountNow()
        {
            if (_m_guestLiningDataList.Count <= 0)
                return 0;
            
            long result = 0;
            long timeMs = FpsAndPingMgr.instance.serverTimeTag;
            float receiveCostSec = GRefdataCoreMgr.instance.npGeneral.inn_receive_cost_sec;
            // 计算奖励积累数量（也就是已经接待完的客人数量）
            for (int i = 0; i < _m_guestLiningDataList.Count ; i++)
            {
                GuestLiningData liningData = _m_guestLiningDataList[i];
                long serveTime = timeMs - liningData.liningStartTimeMs;
                // 如果还没到这个队列的接待时间，直接跳出循环，后面队列的接待开始时间只会更大
                if (serveTime <= 0)
                    break;

                // 如果不需要处理时间，就直接全部处理完了
                if (receiveCostSec <= 0)
                {
                    result += liningData.guestNum;
                    continue;
                }
                
                // 计算当前时间在这个队列里能处理多少客人
                int serveNum = (int)(serveTime / 1000f / receiveCostSec);
                // 设置处理完的客人数量（也就是奖励积累数量）
                result += Mathf.Min(serveNum, liningData.guestNum);
            }

            return result;
        }
        
        
        internal void _update([NotNull] Inn_ReceiveList _receiveInfo)
        {
            _m_liningGuestStartNum = _receiveInfo.getHadBeenReceiveNum();
            List<Inn_ReceiveInfo> guestList = _receiveInfo.getReceiveList();
            _m_guestLiningDataList.Clear();
            if (guestList is { Count: > 0 })
            {
                foreach (Inn_ReceiveInfo guestInfo in guestList)
                {
                    GuestLiningData liningData = new GuestLiningData
                    {
                        liningStartTimeMs = guestInfo.getStartTimeMs(),
                        guestNum = guestInfo.getNeedReceiveNum()
                    };
                    
                    _m_guestLiningDataList.Add(liningData);
                }
                
                _m_guestLiningDataList.Sort();
            }
            
            onUpdate?.Invoke();
        }


        private struct GuestLiningData : IComparable<GuestLiningData>
        {
            public long liningStartTimeMs;
            public int guestNum;


            public int CompareTo(GuestLiningData other)
            {
                return liningStartTimeMs.CompareTo(other.liningStartTimeMs);
            }
        }
    }
}