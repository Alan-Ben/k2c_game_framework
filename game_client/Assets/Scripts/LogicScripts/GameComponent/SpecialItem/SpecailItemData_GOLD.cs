using System;
using ALPackage;
using Common.PlayerObj;
using CommonEnum;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 金币的数据组件
    /// </summary>
    public class SpecailItemData_GOLD : _ASpecialItemData
    {
        // 服务端的当前值和这个值的结算时间，以及当前的每秒金币收益
        private long _m_serverValue;
        private long _m_lastSettleTimeMS;
        private long _m_serverMoneySpeed; // 这个收益包括了农田收益
        private long _m_totalConsumeCount; // 总消耗量
        // 每秒往外部抛出一次变更事件，提醒刷新相关 UI 等
        private ALCommonEnableTaskController _m_valueChgTask;
        
        // 客户端自己的金币收益的存值，这个值不包括农田收益，纯粹为了展示而存在，展示国力变更
        private bool _m_moneySpeedInited;
        private long _m_lEarnings;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalEarningsDealer;
        // 离线收益数据
        private OfflineGoldData _m_offlineData;
        private bool _m_isShowedOfflineEarnings;


        public SpecailItemData_GOLD()
        {
            _m_recalEarningsDealer = new LazyNextFrameTaskDealer(new _RecalEarnings(this));
        }


        public long earnings { get { return _m_lEarnings; } }
        public override ESpecialItemType type { get { return ESpecialItemType.GOLD; } }
        public OfflineGoldData offlineData { get { return _m_offlineData; } }
        public bool isShowedOfflineEarnings { get { return _m_isShowedOfflineEarnings; } }
        public event Action onValueChg;
        public event Action onEarningsChg;
        

        public override void presendInitProtocol(Action<Action> _preInitFunc)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_002_InitOp.make_011_ReqGoldInit(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_011_RetGoldInit>(_msg =>
                    { _preInitFunc(() => _initData(_msg)); }));
        }
        public override void init()
        {
        }
        public override void discard()
        {
            _m_valueChgTask.setDisable();
            _m_isShowedOfflineEarnings = false;
        }
        public override void onAllCompInited()
        {
            _m_lEarnings = _calculateEarnings();
            _m_moneySpeedInited = true;
        }
        public override long getValue()
        {
            return _m_serverValue + (FpsAndPingMgr.instance.serverTimeTag - _m_lastSettleTimeMS) / 1000 * _m_serverMoneySpeed;
        }
        public void recalEarnings(bool _isDealNextFrame = true)
        {
            //不是下一帧处理直接计算
            if (!_isDealNextFrame)
            {
                long oriValue = _m_lEarnings;
                long newValue = _calculateEarnings();
                _m_lEarnings = newValue;
                if (oriValue != newValue)
                {
                    onEarningsChg?.Invoke();
                    WinMsg.SendMsg(WinMsgType.ON_EARNINGS_CHG);
                }
                return;
            }

            if (!_m_moneySpeedInited)
                return;
            
            _m_recalEarningsDealer.setNeedDeal();   
        }
        public void setIsShowedOfflineEarnings()
        {
            _m_isShowedOfflineEarnings = true;
        }

        /// <summary>
        /// 获取获得的金币总数量
        /// </summary>
        /// <returns></returns>
        public long getTotalGainGoldCount()
        {
            return getValue() + _m_totalConsumeCount;
        }

        internal void _addValue(long _value)
        {
            _m_serverValue += _value;
            long curValue = getValue();
            
            onValueChg?.Invoke();
            // 这里调用 resource 的事件，把 currency 的逻辑连接到这边，主要是为了旧模块处理不改动
            NPPlayer.instance.rescourceComp.onResourceCountChg?.Invoke(ECurrency.SILVER, curValue - _value, curValue);
        }

        
        
        private void _initData(GS2GC_002_011_RetGoldInit _msg)
        {
            if (_msg == null)
                return;

            Player_GoldInfo goldInfo = _msg.getGoldInfo();
            _m_serverValue = goldInfo.getCount();
            _m_lastSettleTimeMS = goldInfo.getLastSettleTimeMs();
            _m_serverMoneySpeed = goldInfo.getOutputSpeed();
            _m_totalConsumeCount = goldInfo.getTotalConsumeCount();
            _m_valueChgTask.setDisable();
            _m_valueChgTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_invokeValueChgAction, 1f, (1000 - (FpsAndPingMgr.instance.serverTimeTag - _m_lastSettleTimeMS) % 1000) / 1000f);

            _m_offlineData = new OfflineGoldData(_msg.getOfflineProduceCount(), _msg.getOfflineProduceTime());
        }
        private long _calculateEarnings()
        {
            long result = 0;
            result += NPPlayer.instance.buildingComp.getTotalBusinessBuildingEarningsPerS();
            result += NPPlayer.instance.childComp.totalChildEarnings;
            result += NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.EXTRA_EARNINGS);//加上额外的赚速
            return result;
        }
        private void _invokeValueChgAction()
        {
            onValueChg?.Invoke();
            long currentValue = getValue();
            // 这里调用 resource 的事件，把 currency 的逻辑连接到这边，主要是为了旧模块处理不改动
            NPPlayer.instance?.rescourceComp?.onResourceCountChg?.Invoke(ECurrency.SILVER, currentValue - _m_serverMoneySpeed, currentValue);
            //事件广播
            WinMsg.SendMsg(WinMsgType.ON_PLAYER_RES_CHANGE, ECurrency.SILVER, currentValue - _m_serverMoneySpeed, currentValue);
        }
        
        
        internal void _onGoldInfoChg(GS2GC_004_056_OnGoldInfoChg _msg)
        {
            if (_msg == null)
                return;

            long lastValue = getValue();
            Player_GoldInfo goldInfo = _msg.getGoldInfo();
            _m_serverValue = goldInfo.getCount();
            _m_lastSettleTimeMS = goldInfo.getLastSettleTimeMs();
            _m_serverMoneySpeed = goldInfo.getOutputSpeed();
            _m_totalConsumeCount = goldInfo.getTotalConsumeCount();
            _m_valueChgTask.setDisable();
            _m_valueChgTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_invokeValueChgAction, 1f, (1000 - (FpsAndPingMgr.instance.serverTimeTag - _m_lastSettleTimeMS) % 1000) / 1000f);
            
            onValueChg?.Invoke();
            // 这里调用 resource 的事件，把 currency 的逻辑连接到这边，主要是为了旧模块处理不改动
            NPPlayer.instance.rescourceComp.onResourceCountChg?.Invoke(ECurrency.SILVER, lastValue, getValue());
        }
        
        
        private class _RecalEarnings : _IALBaseMonoTask
        {
            [NotNull] private readonly SpecailItemData_GOLD _m_component;
            
            
            public _RecalEarnings([NotNull] SpecailItemData_GOLD _component)
            {
                _m_component = _component;
            }
            
            
            public void deal()
            {
                long oriValue = _m_component._m_lEarnings;
                long newValue = _m_component._calculateEarnings();
                _m_component._m_lEarnings = newValue;

                if (newValue <= oriValue)
                    return;

                TipQueueMgr.instance.addDealer(new TipQueueDealer_EarningsChg(oriValue, newValue));
                _m_component.onEarningsChg?.Invoke();
                WinMsg.SendMsg(WinMsgType.ON_EARNINGS_CHG);
            }
        }
    }

    public struct OfflineGoldData
    {
        private readonly long _m_count;
        private readonly long _m_time;
        
        
        public OfflineGoldData(long _count, long _time)
        {
            _m_count = _count;
            _m_time = _time;
        }
        
        
        public long count { get { return _m_count; } }
        public long time { get { return _m_time; } }
    }
}