using ALBasicProtocolPack;
using Common.OfflineRewardObj;
using GS2GC.p007_CommOp;

namespace GOE
{
    public abstract class _AOfflineRewardInfoWithRewardData<T_REWARD_DATA> : _ABasicOfflineRewardInfo
        where T_REWARD_DATA : _IALProtocolStructure, new()
    {
        protected _AOfflineRewardInfoWithRewardData(OfflineReward_Info _info) 
            : base(_info)
        {
        }
        
        
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