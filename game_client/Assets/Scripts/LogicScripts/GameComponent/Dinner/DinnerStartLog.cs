using Common.DinnerEnum;
using Common.DinnerObj;

namespace GOE
{
    public class DinnerStartLogIdx
    {
        private Dinner_StartLogIdx _m_dinnerStartLogIdx;
        public DinnerStartLogIdx(Dinner_StartLogIdx _dinnerStartLogIdx)
        {
            _m_dinnerStartLogIdx = _dinnerStartLogIdx;
        }
        
        /// <summary>
        /// 宴会实例ID
        /// </summary>
        public long instanceId
        {
            get => _m_dinnerStartLogIdx.getInstanceId();
        }
        /// <summary>
        /// 宴会配置ID
        /// </summary>
        public long dinnerId
        {
            get => _m_dinnerStartLogIdx.getDinnerId();
        }
        /// <summary>
        /// 获得的宴会人气
        /// </summary>
        public long gainScore
        {
            get => _m_dinnerStartLogIdx.getGainScore();
        }
        /// <summary>
        /// 开始时间戳（秒）
        /// </summary>
        public int startTs
        {
            get => _m_dinnerStartLogIdx.getStartTs();
        }
        /// <summary>
        /// 赴宴玩家数量
        /// </summary>
        public int joinerCount
        {
            get => _m_dinnerStartLogIdx.getJoinerCount();
        }
        /// <summary>
        /// 凭证类型
        /// </summary>
        public EDinnerPermitType permitType
        {
            get => _m_dinnerStartLogIdx.getPermitType();
        }

        /// <summary>
        /// 宴会类型id，妃子宴会为妃子id，爬塔为爬塔层数
        /// </summary>
        public long permitTypeId
        {
            get => _m_dinnerStartLogIdx.getPermitTypeId();
        }
        /// <summary>
        /// 获取妃子宴会 的妃子名
        /// </summary>
        /// <returns></returns>
        public string getPermitConsortName()
        {
            if (_m_dinnerStartLogIdx.getPermitType() == EDinnerPermitType.FAMILY)
            {
                GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_dinnerStartLogIdx.getPermitTypeId());
                if (null != consortRefObj)
                {
                    return consortRefObj.transName;
                }
            }

            return null;
        }
    }
}