using System;
using Common.OfflineRewardEnum;
using Common.OfflineRewardObj;

namespace GOE
{
    /// <summary>
    /// 离线奖励信息基类
    /// </summary>
    /// <remarks>
    /// 不需要处理具体的奖励数据和领奖后的奖励结果的基类
    /// </remarks>
    public abstract class _ABasicOfflineRewardInfo
    {
        /// <summary>
        /// 离线奖励唯一 id
        /// </summary>
        private readonly long _m_id;
        /// <summary>
        /// 奖励类型 - ECommonOfflineRewardType 类型
        /// </summary>
        private readonly EOfflineRewardEnum _m_rewardType;
        

        protected _ABasicOfflineRewardInfo(OfflineReward_Info _info)
        {
            if (_info == null)
                return;
            
            _m_id = _info.getId();
            _m_rewardType = (EOfflineRewardEnum)_info.getRewardType();
        }
        

        public long id { get { return _m_id; } }
        public EOfflineRewardEnum type { get { return _m_rewardType; } }
        

        /// <summary>
        /// 处理奖励信息
        /// </summary>
        public void dealReward(bool _isInit)
        {
            _onDealReward(_isInit, ()=>
            {
                _reqTakeOfflineReward(_isInit);
            });
        }


        protected abstract void _reqTakeOfflineReward(bool _isInit);
        protected abstract void _onDealReward(bool _isInit, Action _reqTakeReward);
    }
}