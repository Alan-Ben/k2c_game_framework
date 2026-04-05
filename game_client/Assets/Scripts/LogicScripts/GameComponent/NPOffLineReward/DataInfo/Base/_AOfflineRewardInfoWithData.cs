using ALBasicProtocolPack;
using Common.OfflineRewardObj;
using GS2GC.p007_CommOp;
using JetBrains.Annotations;

namespace GOE
{
    public abstract class _AOfflineRewardInfoWithData<T_OFFLINE_DATA, T_REWARD_DATA> : _ABasicOfflineRewardInfo
        where T_OFFLINE_DATA : _IALProtocolStructure, new()
        where T_REWARD_DATA : _IALProtocolStructure, new()
    {
        [NotNull] private readonly T_OFFLINE_DATA _m_offlineData;
        
        
        protected _AOfflineRewardInfoWithData(OfflineReward_Info _info) 
            : base(_info)
        {
            _m_offlineData = new T_OFFLINE_DATA();
            if (_info != null)
                _m_offlineData.readPackage(new ALProtocolBuf(_info.getRewardShow()));
        }
        
        
        public T_OFFLINE_DATA offlineData { get { return _m_offlineData; } }
        
        
        /// <summary>
        /// 领取奖励
        /// </summary>
        protected sealed override void _reqTakeOfflineReward(bool _isInit)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_020_ReqTakeOfflineReward(id), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_007_020_RetTakeOfflineReward>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        T_REWARD_DATA rewardData = new T_REWARD_DATA();
                        if(_msg.getExtData() != null && _msg.getExtData().Length > 0)
                            rewardData.readPackage(new ALProtocolBuf(_msg.getExtData()));
                        _onTakeReward(true, _isInit, rewardData);
                        return;
                    }
                    
                    _onTakeReward(false, _isInit, default);
                }));
        }
        protected abstract void _onTakeReward(bool _isSuc, bool _isInit, T_REWARD_DATA _rewardData);
    }
}