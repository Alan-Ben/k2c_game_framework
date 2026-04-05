using System;
using System.Collections.Generic;
using ALPackage;
using Common.InnObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 旅店客人信息
    /// </summary>
    public class InnGuestInfo
    {
        private readonly long _m_guestInstanceId;
        private readonly long _m_guestId;
        [NotNull] private readonly InnGuestRefObj _m_refObj;
        [NotNull] private readonly InnDishInfo _m_dishInfo;
        
        
        public InnGuestInfo(long _guestInstanceId, [NotNull] InnGuestRefObj _guestRef, [NotNull] InnDishInfo _dishInfo)
        {
            _m_guestInstanceId = _guestInstanceId;
            _m_guestId = _guestRef.id;
            _m_refObj = _guestRef;
            _m_dishInfo = _dishInfo;
        }
        
        
        public long guestInstanceId { get { return _m_guestInstanceId; } }
        /// <summary>
        /// 客人 ID
        /// </summary>
        public long guestId { get { return _m_guestId; } }
        /// <summary>
        /// 客人配置
        /// </summary>
        [NotNull] public InnGuestRefObj refObj { get { return _m_refObj; } }
        /// <summary>
        /// 客人菜品数据
        /// </summary>
        [NotNull] public InnDishInfo dishInfo { get { return _m_dishInfo; } }


        public long calculatePopularityGain()
        {
            return _m_dishInfo.popularityAdd;
        }
        public long calculateFinesseGain()
        {
            return (long) Math.Ceiling(_m_dishInfo.finesseAdd * (10000 + _m_refObj.finesse_add) / 10000d);
        }
        public long calculateAffectionGain()
        {
            return _m_dishInfo.affectionAdd;
        }
    }
}