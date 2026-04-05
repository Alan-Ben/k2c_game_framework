using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class InnNormalGuestHandbookInfo : IComparable<InnNormalGuestHandbookInfo>
    {
        [NotNull] private readonly InnGuestRefObj _m_guestRef;
        private bool _m_isUnlock;
        private bool _m_hadDrawReward;
        private long _m_unlockGuestNum; // 在第几个客人时解锁
        
        
        public InnNormalGuestHandbookInfo([NotNull] InnGuestRefObj _guestRef)
        {
            _m_guestRef = _guestRef;
        }
        
        
        public long guestId { get { return _m_guestRef.id; } }
        [NotNull] public InnGuestRefObj refObj { get { return _m_guestRef; } }
        public bool isUnlock { get { return _m_isUnlock; } }
        public bool hadDrawReward { get { return _m_hadDrawReward || _m_guestRef.handbook_reward is not { Count: > 0 }; } }
        public string nameTranslated { get { return TextTranslate.instance.getLanguage(_m_guestRef.name); } }
        public string descTranslated { get { return TextTranslate.instance.getLanguage(_m_guestRef.desc); } }
        public long unlockGuestNum { get { return _m_unlockGuestNum; } }


        public bool checkCanUnlock()
        {
            List<_NPPlayerConditionSerializeInfo> conditionList = _m_guestRef.unlock_condition_list;
            if (conditionList is not { Count: > 0 })
                return true; // 没有解锁条件，直接解锁

            foreach (_NPPlayerConditionSerializeInfo condition in conditionList)
            {
                if (condition != null && !condition.IsEnable(null))
                    return false;
            }

            return true;
        }


        internal void _updateIsUnlock(bool _isUnlock, long _unlockGuestNum)
        {
            _m_unlockGuestNum = _unlockGuestNum;
            _m_isUnlock = _isUnlock;
        }
        internal void _updateHadDrawReward(bool _hadDrawReward)
        {
            _m_hadDrawReward = _hadDrawReward;
        }
        
        
        public int CompareTo(InnNormalGuestHandbookInfo _other)
        {
            if (_other == null)
                return -1;

            if (isUnlock && !_other.isUnlock)
                return -1;
            if (!isUnlock && _other.isUnlock)
                return 1;
            
            // 按照解锁顺序排列
            int result = unlockGuestNum.CompareTo(_other.unlockGuestNum);
            if (result != 0)
                return result;

            // 如果解锁顺序相同，则按照 ID 排序
            return guestId.CompareTo(_other.guestId);
        }
    }
}