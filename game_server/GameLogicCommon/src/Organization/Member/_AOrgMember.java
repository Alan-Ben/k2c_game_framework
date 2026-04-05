package Organization.Member;

/**
 * 成员对象
 * @param <MemberInfo> 成员信息
 */
public abstract class _AOrgMember<MemberInfo extends _AOrgMemberData>
{
    private final MemberInfo _m_info;

    public _AOrgMember(MemberInfo _info)
    {
        _m_info = _info;
    }

    public MemberInfo getInfo()
    {
        return _m_info;
    }

    public Long getMemberId()
    {
        return _m_info.getMemberId();
    }

    /**
     * 成员对象销毁的处理
     */
    public void discard()
    {
        _m_info.discard();
    }
}
