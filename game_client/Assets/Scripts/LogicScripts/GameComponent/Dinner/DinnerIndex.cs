using Common.DinnerEnum;
using Common.DinnerObj;
using Unity.Mathematics;

namespace GOE
{
    public class DinnerIndex
    {
        private Dinner_Idx _m_dinnerIdx;
        private long _m_instanceId;
        private long _m_dinnerId;
        private long _m_ownerCid;
        private int _m_joinerCount;
        private long _m_score;
        private int _m_endTs;
        private bool _m_isJoined;
        private EDinnerPermitType _m_permitType;// 凭证类型
        private long _m_permitTypeId;// 凭证类型ID
        
        // private long sortSerial{ get => ; }// 排序序列号
        public long instanceId{ get => _m_instanceId; }// 宴会实例ID
        public long dinnerId{ get => _m_dinnerId; }// 宴会配置ID
        public long ownerCid{ get => _m_ownerCid; }// 开宴玩家CID
        public int joinerCount{ get => _m_joinerCount; }// 当前赴宴玩家数量
        public long score{ get => _m_score; }// 当前宴会人气
        public int endTs{ get => _m_endTs; }// 结束时间戳（秒）
        public bool isJoined{ get => _m_isJoined; }// 是否加入宴会
        
        public EDinnerPermitType permitType => _m_permitType;
        public long permitTypeId => _m_permitTypeId;

        public DinnerIndex(Dinner_Idx _dinnerIdx)
        {
            if (_dinnerIdx == null) return;
            _m_dinnerIdx = _dinnerIdx;
            _m_instanceId = _dinnerIdx.getInstanceId();
            _m_dinnerId = _dinnerIdx.getDinnerId();
            _m_ownerCid = _dinnerIdx.getOwnerCid();
            _m_joinerCount = _dinnerIdx.getJoinerCount();
            _m_score = _dinnerIdx.getScore();
            _m_endTs = _dinnerIdx.getEndTs();
            _m_isJoined = _dinnerIdx.getIsJoined();
            _m_permitType = _dinnerIdx.getPermitType();
            _m_permitTypeId = _dinnerIdx.getPermitTypeId();
        }
        /// <summary>
        /// 获取宴会剩余时间
        /// </summary>
        /// <returns></returns>
        public long getRemainTimeMs()
        {
            long remainTimeS = _m_endTs - FpsAndPingMgr.instance.serverTimeTagS;
            return math.max(remainTimeS, 0) * 1000;
        }
    }
}