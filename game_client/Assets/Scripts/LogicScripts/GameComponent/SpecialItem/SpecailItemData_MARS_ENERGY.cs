using System;
using ALPackage;
using Common.PlayerObj;
using CommonEnum;
using GC2GS.p002_InitOp;
using GS2GC.p002_InitOp;
using GS2GC.p039_MarsBuildingOp;

namespace GOE
{
    /// <summary>
    /// 火星能量的数据组件
    /// </summary>
    public class SpecailItemData_MARS_ENERGY : _ASpecialItemData
    {
        // 服务端的当前值和这个值的结算时间，以及当前的每分钟能量消耗速度
        private long _m_serverValue;
        private long _m_lastSettleTimeMS;
        private long _m_serverCostSpeed; // 每分钟消耗速度
        // 每分钟往外部抛出一次变更事件，提醒刷新相关 UI 等
        private ALCommonEnableTaskController _m_valueChgTask;
        private long _m_lastTickMinute;


        public SpecailItemData_MARS_ENERGY()
        {
        }


        public override ESpecialItemType type { get { return ESpecialItemType.MARS_ENERGY; } }
        public long costSpeed { get { return _m_serverCostSpeed; } }
        public event Action onValueChg;


        public override void presendInitProtocol(Action<Action> _preInitFunc)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_002_076_ReqMarsEnergyInit(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_076_RetMarsEnergyInit>(_msg => { _preInitFunc(() => _initData(_msg)); }));
        }
        public override void init()
        {
            _m_valueChgTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_tickPerSecond, 1f);
        }
        public override void discard()
        {
            _m_valueChgTask.setDisable();
        }
        public override long getValue()
        {
            // 能量值只在每分钟整点时减少，不是连续变化
            long elapsedTimeMS = FpsAndPingMgr.instance.serverTimeTag - _m_lastSettleTimeMS;
            long completedMinutes = elapsedTimeMS / 60000; // 完整经过的分钟数
            long consumedEnergy = completedMinutes * _m_serverCostSpeed; // 只按完整分钟计算消耗
            return Math.Max(0, _m_serverValue - consumedEnergy);
        }


        internal void _setValue(long _value)
        {
            long lastValue = _m_serverValue;
            _m_serverValue = _value;

            onValueChg?.Invoke();
            // 这里调用 resource 的事件，把 currency 的逻辑连接到这边，主要是为了旧模块处理不改动
            NPPlayer.instance.rescourceComp.onResourceCountChg?.Invoke(ECurrency.MARS_ENERGY, lastValue, _value);
            //事件广播
            WinMsg.SendMsg(WinMsgType.ON_PLAYER_RES_CHANGE, ECurrency.MARS_ENERGY, lastValue, _value);
        }


        private void _initData(GS2GC_002_076_RetMarsEnergyInit _msg)
        {
            if (_msg == null)
                return;

            Player_MarsEnergyInfo energyInfo = _msg.getInfo();
            _m_serverValue = energyInfo.getCount();
            _m_lastSettleTimeMS = energyInfo.getLastSettleTimeMs();
            _m_serverCostSpeed = energyInfo.getCostSpeed();

            long elapsedTimeMS = FpsAndPingMgr.instance.serverTimeTag - _m_lastSettleTimeMS;
            _m_lastTickMinute = elapsedTimeMS / 60000;
        }


        internal void _onMarsEnergyInfoChg(GS2GC_039_055_OnMarsEnergyChg _msg)
        {
            if (_msg == null)
                return;

            long lastValue = getValue();
            Player_MarsEnergyInfo energyInfo = _msg.getInfo();
            _m_serverValue = energyInfo.getCount();
            _m_lastSettleTimeMS = energyInfo.getLastSettleTimeMs();
            _m_serverCostSpeed = energyInfo.getCostSpeed();

            long elapsedTimeMS = FpsAndPingMgr.instance.serverTimeTag - _m_lastSettleTimeMS;
            _m_lastTickMinute = elapsedTimeMS / 60000;

            onValueChg?.Invoke();
            // 这里调用 resource 的事件，把 currency 的逻辑连接到这边，主要是为了旧模块处理不改动
            NPPlayer.instance.rescourceComp.onResourceCountChg?.Invoke(ECurrency.MARS_ENERGY, lastValue, getValue());
        }


        private void _tickPerSecond()
        {
            long elapsedTimeMS = FpsAndPingMgr.instance.serverTimeTag - _m_lastSettleTimeMS;
            long currentMinute = elapsedTimeMS / 60000;

            if (currentMinute > _m_lastTickMinute)
            {
                _m_lastTickMinute = currentMinute;

                onValueChg?.Invoke();
                long currentValue = getValue();
                // 这里调用 resource 的事件，把 currency 的逻辑连接到这边，主要是为了旧模块处理不改动
                NPPlayer.instance?.rescourceComp?.onResourceCountChg?.Invoke(ECurrency.MARS_ENERGY, currentValue + _m_serverCostSpeed, currentValue);
                //事件广播
                WinMsg.SendMsg(WinMsgType.ON_PLAYER_RES_CHANGE, ECurrency.MARS_ENERGY, currentValue + _m_serverCostSpeed, currentValue);
            }
        }
    }
}