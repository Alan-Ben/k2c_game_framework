using JetBrains.Annotations;

namespace GOE
{
    public class InnSpecialGuestHandbookInfo
    {
        [NotNull] private readonly InnSpecialGuestRefObj _m_guestRef;
        private bool _m_isUnlock;
        private bool _m_hadDrawReward;
        
        
        public InnSpecialGuestHandbookInfo([NotNull] InnSpecialGuestRefObj _specialGuestRef)
        {
            _m_guestRef = _specialGuestRef;
        }
        
        
        public long guestId { get { return _m_guestRef.id; } }
        [NotNull] public InnSpecialGuestRefObj refObj { get { return _m_guestRef; } }
        public bool isUnlock { get { return _m_isUnlock; } }
        public bool hadDrawReward { get { return _m_hadDrawReward || _m_guestRef.handbook_reward is not { Count: > 0 }; } }
        public string nameTranslated { get { return TextTranslate.instance.getLanguage(_m_guestRef.name); } }
        public string descTranslated { get { return TextTranslate.instance.getLanguage(_m_guestRef.desc); } }
        

        internal void _updateIsUnlock(bool _isUnlock)
        {
            _m_isUnlock = _isUnlock;
        }
        internal void _updateHadDrawReward(bool _hadDrawReward)
        {
            _m_hadDrawReward = _hadDrawReward;
        }
        public bool conditionEnable()
        {
            foreach (_NPPlayerConditionSerializeInfo condition in _m_guestRef.unlock_condition_list)
            {
                if (condition != null && !condition.IsEnable(null))
                    return false;
            }

            return true;
        }
    }
}