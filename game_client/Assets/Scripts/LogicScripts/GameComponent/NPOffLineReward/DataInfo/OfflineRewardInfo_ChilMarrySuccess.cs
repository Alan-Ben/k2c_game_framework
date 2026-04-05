using System;
using Common.ChildObj;
using Common.OfflineRewardObj;

namespace GOE
{
    public class OfflineRewardInfo_ChilMarrySuccess : _AOfflineRewardInfoWithOfflineData<Adult_MarryReward>
    {
        public OfflineRewardInfo_ChilMarrySuccess(OfflineReward_Info _info) : base(_info)
        {
        }
        

        protected override void _onDealReward(bool _isInit, Action _reqTakeReward)
        {
            MarriedInfo marriedInfo = new MarriedInfo(offlineData.getMarriedInfo());
            NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildMarrySuccess(marriedInfo, new NPCommonCostItem(offlineData.getItem())
                , _isInit ? EMainCityPushNoticeTriggerType.LOGIN : EMainCityPushNoticeTriggerType.OTHER));
            _reqTakeReward?.Invoke();
        }
        protected override void _onTakeReward(bool _isSuc, bool _isInit)
        {
        }
    }
}