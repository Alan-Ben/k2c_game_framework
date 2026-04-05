using ALBasicProtocolPack;
using Common.OfflineRewardObj;
using JetBrains.Annotations;

namespace GOE
{
    public abstract class _AOfflineRewardInfoWithOfflineData<T_OFFLINE_DATA> : _AOfflineRewardInfo
        where T_OFFLINE_DATA : _IALProtocolStructure, new()
    {
        [NotNull] private readonly T_OFFLINE_DATA _m_offlineData;
        
        
        protected _AOfflineRewardInfoWithOfflineData(OfflineReward_Info _info) 
            : base(_info)
        {
            _m_offlineData = new T_OFFLINE_DATA();
            if (_info != null)
                _m_offlineData.readPackage(new ALProtocolBuf(_info.getRewardShow()));
        }
        
        
        [NotNull] public T_OFFLINE_DATA offlineData { get { return _m_offlineData; } }
    }
}