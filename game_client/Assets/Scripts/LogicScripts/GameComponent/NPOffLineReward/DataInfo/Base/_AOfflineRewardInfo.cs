using Common.OfflineRewardObj;
using GS2GC.p007_CommOp;

namespace GOE
{
    public abstract class _AOfflineRewardInfo : _ABasicOfflineRewardInfo
    {
        protected _AOfflineRewardInfo(OfflineReward_Info _info) 
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
                    _onTakeReward(_isSuc, _isInit);
                }));
        }
        protected abstract void _onTakeReward(bool _isSuc, bool _isInit);
    }
}