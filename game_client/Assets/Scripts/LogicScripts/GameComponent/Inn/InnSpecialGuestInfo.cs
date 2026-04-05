using JetBrains.Annotations;

namespace GOE
{
    public class InnSpecialGuestInfo
    {
        [NotNull] private readonly InnSpecialGuestHandbookInfo _m_specialGuestHandbookInfo;
        
        
        public InnSpecialGuestInfo([NotNull] InnSpecialGuestHandbookInfo _specialGuestHandbookInfo)
        {
            _m_specialGuestHandbookInfo = _specialGuestHandbookInfo;
        }
        
        
        public long guestId { get { return _m_specialGuestHandbookInfo.guestId; } }
        [NotNull] public InnSpecialGuestRefObj refObj { get { return _m_specialGuestHandbookInfo.refObj; } }
        public string nameTranslated { get { return _m_specialGuestHandbookInfo.nameTranslated; } }
        public string descTranslated { get { return _m_specialGuestHandbookInfo.descTranslated; } }
        public bool isUnlock { get { return _m_specialGuestHandbookInfo.isUnlock; } }
    }
}