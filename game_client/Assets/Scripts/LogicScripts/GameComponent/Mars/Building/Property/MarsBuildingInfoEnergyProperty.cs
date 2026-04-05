
using System;
using Common.MarsEnum;
using Common.MarsObj;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class MarsBuildingInfoEnergyProperty : _AMarsBuildingInfoProperty
    {
        private long _m_outputValuePerMin;
        private long _m_peopleOutputValuePerMin;
        private long _m_maxStorage;
        
        private long _m_lastUpdateTimeMS;
        private long _m_lastUpdateStoredEnergy;
        
        private long _m_preComputeStoredEnergy;//上一次计算的存储能量
        

        public MarsBuildingInfoEnergyProperty([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo, true)
        {
            _m_outputValuePerMin = 0;
            _m_peopleOutputValuePerMin = 0;
            _m_maxStorage = 0;
            _m_lastUpdateTimeMS = 0;
            _m_lastUpdateStoredEnergy = 0;
        }
        

        public long maxStorage { get { return _m_maxStorage; } }
        public long storedEnergy
        {
            get
            {
                if (_m_lastUpdateTimeMS == 0)
                    return _m_lastUpdateStoredEnergy;

                long elapsedTimeMS = FpsAndPingMgr.instance.serverTimeTag - _m_lastUpdateTimeMS;
                long producedEnergy = elapsedTimeMS / 60000 * value; // 每分钟产出
                long finalEnergy = Math.Min(_m_maxStorage, _m_lastUpdateStoredEnergy + producedEnergy);
                if (_m_preComputeStoredEnergy != finalEnergy)
                {
                    _m_preComputeStoredEnergy = finalEnergy;
                    WinMsg.SendMsg(WinMsgType.ON_MARS_STORED_ENERGY_CHG, buildingInfo);
                }
                
                return finalEnergy;
            }
        }
        public long lastUpdateTimeMS { get { return _m_lastUpdateTimeMS; } }
        /// <summary>
        /// 剩余存储时间（秒）
        /// </summary>
        public long leftStoreTimeSec
        {
            get
            {
                long leftEnergy = maxStorage - storedEnergy;
                return leftEnergy > 0 ? (long)(leftEnergy * 1.0f / value * 60): 0;
            }
        }
        

        private protected override long _computeValue()
        {
            long count = buildingInfo.type == EMarsBuildingType.ENERGY ? peopleCount : 0;
            long baseValue = _m_outputValuePerMin + count/* * _m_peopleOutputValuePerMin */; 
            float outputPer = 1 + NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_ENERGY_OUTPUT_PER) / 10000f;
            return (long)(baseValue * outputPer);
        }
        private protected override void _onDirty()
        {
        }


        internal void _updateValue(Mars_BuildingEnergyOutput _msg)
        {
            if (_msg == null)
                return;

            _m_lastUpdateTimeMS = _msg.getLastSettleMs();
            long preComputeStoredEnergy = _m_preComputeStoredEnergy;//上一次计算的存储能量
            _m_lastUpdateStoredEnergy = _m_preComputeStoredEnergy = _msg.getOutput();
            if(preComputeStoredEnergy != _m_preComputeStoredEnergy)
            {
                WinMsg.SendMsg(WinMsgType.ON_MARS_STORED_ENERGY_CHG, buildingInfo);
            }
        }
        internal void _addOutputValuePerMin(long _outputPerMin)
        {
            _m_outputValuePerMin += _outputPerMin;
            _setDirty();
        }
        internal void _addPeopleOutputValuePerMin(long _peopleOutputValuePerMin)
        {
            _m_peopleOutputValuePerMin += _peopleOutputValuePerMin;
            _setDirty();
        }
        internal void _addMaxStorage(long _maxStorage)
        {
            _m_maxStorage += _maxStorage;
        }
        internal void _removeOutputValuePerMin(long _outputPerMin)
        {
            _m_outputValuePerMin -= _outputPerMin;
            _setDirty();
        }
        internal void _removeMaxStorage(long _maxStorage)
        {
            _m_maxStorage -= _maxStorage;
        }
        internal void _removePeopleOutputValuePerMin(long _peopleOutputValuePerMin)
        {
            _m_peopleOutputValuePerMin -= _peopleOutputValuePerMin;
            _setDirty();
        }
    }
}