using System.Collections.Generic;
using Common.GachaObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 抽卡卡池信息
    /// </summary>
    public class GachaPoolInfo
    {
        /// <summary>
        /// 卡池id
        /// </summary>
        private long _m_lPoolId;
        
        /// <summary>
        /// 保底信息列表
        /// </summary>
        private List<GachaGuaranteeInfo> _m_lGuaranteeList;
        
        /// <summary>
        /// 抽卡累计奖励次数
        /// </summary>
        private int _m_iCumulativeRewardTimes;

        private GachaPoolRefObj _m_wPoolRefObj;

        public GachaPoolInfo(long _poolId)
        {
            _m_lPoolId = poolId;
        }

        public GachaPoolInfo([NotNull]GachaPoolRefObj _gachaPoolRefObj)
        {
            _m_lPoolId = _gachaPoolRefObj.id;
            _m_wPoolRefObj = _gachaPoolRefObj;
        }
        
        public GachaPoolInfo(Gacha_PoolInfo _serverInfo)
        {
            update(_serverInfo);
        }
        
        
        public long poolId => _m_lPoolId;
        public List<GachaGuaranteeInfo> guaranteeList => _m_lGuaranteeList;
        public int cumulativeRewardTimes => _m_iCumulativeRewardTimes;
        
        public GachaPoolRefObj poolRefObj
        {
            get
            {
                if (_m_wPoolRefObj == null || _m_wPoolRefObj.id != _m_lPoolId)
                    _m_wPoolRefObj = GRefdataCoreMgr.instance.gachaPoolRefCore.getRef(_m_lPoolId);
                
                return _m_wPoolRefObj;
            }
        }

        public void update(Gacha_PoolInfo _serverInfo)
        {
            if(_serverInfo == null)
                return;

            _m_lPoolId = _serverInfo.getPoolId();
            updateGuaranteeInfo(_serverInfo.getGuaranteeList());
            updateCumulativeRewardTimes(_serverInfo.getCumulativeRewardTimes());
        }
        
        /// <summary>
        /// 更新保底信息
        /// </summary>
        public void updateGuaranteeInfo(List<Common.GachaObj.Gacha_GuaranteeInfo> _serverGuaranteeList)
        {
            if (_m_lGuaranteeList == null)
                _m_lGuaranteeList = new List<GachaGuaranteeInfo>();

            if (_serverGuaranteeList == null || _serverGuaranteeList.Count <= 0)
            {
                _m_lGuaranteeList.Clear();
                return;
            }

            int i = 0;
            GachaGuaranteeInfo guaranteeInfo = null;
            for (; i < _serverGuaranteeList.Count; i++)
            {
                if (i < _m_lGuaranteeList.Count)
                {
                    guaranteeInfo = _m_lGuaranteeList[i];
                    guaranteeInfo.update(_serverGuaranteeList[i]);
                }
                else
                {
                    guaranteeInfo = new GachaGuaranteeInfo(_serverGuaranteeList[i]);
                    _m_lGuaranteeList.Add(guaranteeInfo);
                }
            }
            
            if(i < _m_lGuaranteeList.Count)
                _m_lGuaranteeList.RemoveRange(i, _m_lGuaranteeList.Count - i);
        }
        
        /// <summary>
        /// 更新抽卡累计奖励次数
        /// </summary>
        /// <param name="_cumulativeRewardTimes"></param>
        public void updateCumulativeRewardTimes(int _cumulativeRewardTimes)
        {
            _m_iCumulativeRewardTimes = _cumulativeRewardTimes;
        }
    }
}