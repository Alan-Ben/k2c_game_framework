using Common.DinnerEnum;
using Common.DinnerObj;

namespace GOE
{
    /// <summary>
    /// 宴会凭证数据类
    /// </summary>
    public class DinnerPermit
    {
        private EDinnerPermitType _m_type;// 宴会凭证类型
        private long _m_id;// 凭证实例ID
        private long _m_typeId;// 凭证类型ID
        private int _m_expiredTs;// 凭证过期时间（秒）

        public DinnerPermit(Dinner_Permit _permit)
        {
            _m_type = _permit.getPermitType();
            _m_id = _permit.getId();
            _m_typeId = _permit.getTypeId();
            _m_expiredTs = _permit.getExpiredTs();
        }

        // 宴会凭证类型
        public EDinnerPermitType type { get => _m_type; }
        // 凭证实例ID
        public long id { get => _m_id; }
        // 凭证类型ID,如果是妃子则是妃子id，如爬塔则是塔的层数
        public long typeId { get => _m_typeId; }
        // 凭证过期时间（秒）
        public int expiredTs { get => _m_expiredTs; }
        // 是否过期
        public bool isExpired { get => _m_expiredTs - FpsAndPingMgr.instance.serverTimeTagS <= 0; }

    }
}