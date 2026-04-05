using System;
using System.Collections.Generic;
using GC2GS.p034_InnOp;
using GS2GC.p034_InnOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 旅店客人数据管理器
    /// </summary>
    public class InnGuestDataMgr
    {
        // 同时存在的最大客人数
        private int _m_maxGuestCount;
        
        // 当前存在的客人列表
        [NotNull] private readonly List<InnGuestInfo> _m_guestList;
        
        // 数据层的接待信息
        private InnReceiveInfo _m_receiveInfo;
        // 客人 id 当前是从什么开始的
        private long _m_guestIdBeginId;
        // 当前从开始 id 开始，一共有多少客人
        private long _m_guestCount;
        // 当前从开始 id 开始，一共接待了多少客人
        private long _m_settledCount;
        
        
        public InnGuestDataMgr()
        {
            _m_guestList = new List<InnGuestInfo>();
        }


        /// <summary>
        /// 当客人增加了
        /// </summary>
        public event Action<InnGuestInfo> onGuestAdd;
        /// <summary>
        /// 当前总共还剩下多少客人要接待
        /// </summary>
        public long totalGuestNum { get { return _m_guestCount - _m_settledCount; } }
        /// <summary>
        /// 当前积累的奖励数量
        /// </summary>
        public long rewardCount
        {
            get
            {
                if (_m_receiveInfo == null) return 0;
                long result = _m_settledCount - (_m_receiveInfo.guestIdBeginId - _m_guestIdBeginId);
                if (result < 0)
                    return 0;

                return result;
            }
        }
        /// <summary>
        /// 总共已经处理完的客人数量
        /// </summary>
        public long totalSettledGuestNum { get { return _m_guestIdBeginId + _m_settledCount; } }


        public void init()
        {
            _m_maxGuestCount = MainAdditionInnTDScene.instance.getMaxGuestCount();
            if (_m_maxGuestCount <= 0)
                _m_maxGuestCount = 10;
            
            _m_receiveInfo = NPPlayer.instance.innComp.receiveInfo;
            _m_guestCount = _m_receiveInfo.getGuestCountNow(out _, out _m_guestIdBeginId);
            _m_settledCount = 0;
            _tryCreateGuests();

            _m_receiveInfo.onUpdate += _onReceiveInfoChg;
        }
        public void discard()
        {
            _m_guestList.Clear();
            
            _m_receiveInfo.onUpdate -= _onReceiveInfoChg;
            _m_receiveInfo = null;
        }

        
        [ItemNotNull, NotNull]
        public List<InnGuestInfo> getGuestList()
        {
            return new List<InnGuestInfo>(_m_guestList);
        }
        public void getGuestListNonAlloc(List<InnGuestInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_guestList);
        }
        public void setGuestSettled(long _guestId)
        {
            int guestIndex = _m_guestList.FindIndex(_guest => _guest.guestInstanceId == _guestId);
            if (guestIndex < 0)
                return;
            
            _m_guestList.RemoveAt(guestIndex);
            _m_settledCount++;
            _tryCreateGuests();
        }
        public void getSettleReward(Action<bool, GS2GC_034_006_RetInnSettle> _complete)
        {
            if (_m_receiveInfo == null)
            {
                _complete?.Invoke(false, null);
                return;
            }
            
            long realRewardCount = rewardCount;
            if (realRewardCount <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.inn_nothingToGetInCashRegister_none);
                _complete?.Invoke(false, null);
                return;
            }
            
            NPPlayer.instance.innComp.getSettleReward((int)realRewardCount, _complete);
        }


        private void _tryCreateGuests()
        {
            while (_m_guestList.Count < _m_maxGuestCount && _m_guestList.Count < totalGuestNum)
            {
                long guestId = _m_guestIdBeginId + _m_settledCount + _m_guestList.Count + 1;
                InnGuestInfo guestInfo = NPPlayer.instance.innComp.getGuestInfoById(guestId);
                
                if (guestInfo != null)
                {
                    _m_guestList.Add(guestInfo);
                    onGuestAdd?.Invoke(guestInfo);
                }
                else
                    break;
            }
        }
        private void _onReceiveInfoChg()
        {
            if (_m_receiveInfo == null)
                return;
            
            _m_guestCount += _m_receiveInfo.getGuestCountAfter(_m_guestIdBeginId + _m_guestCount);
            _tryCreateGuests();
        }
    }
}