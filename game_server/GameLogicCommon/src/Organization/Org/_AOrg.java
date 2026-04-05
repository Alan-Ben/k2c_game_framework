package Organization.Org;

import Organization.Member.OrgMemberMgr;
import Organization.Member._AOrgMember;
import Organization.Member._AOrgMemberData;
import Organization._IOrgEnv;

/**
 * 组织对象
 * @param <OrgData>    组织信息
 * @param <MemberData> 成员信息
 */
public abstract class _AOrg<OrgData extends _AOrgData, MemberData extends _AOrgMemberData>
{
    //组织信息
    private OrgData _m_info;

    //成员管理器
    private OrgMemberMgr<OrgData, MemberData, _AOrg<OrgData, MemberData>> _m_memberMgr;

    public _AOrg(OrgData _info)
    {
        _m_info = _info;
        _m_memberMgr = new OrgMemberMgr<>(this);
    }

    public _IOrgEnv getEnv()
    {
        return _m_memberMgr.getEnv();
    }

    public OrgData getInfo()
    {
        return _m_info;
    }

    /**
     * 组织解散
     */
    public void dissolve()
    {
        //组织信息的处理
        _m_info.dissolve();
        //相关数据处理
        _onDissolve();

        //子管理器的处理
        _m_memberMgr.onOrgDissolve();

    }

    /**
     * 解散相关的处理
     */
    protected abstract void _onDissolve();

    /**
     * 成员新增
     * @param _member 新增成员对象
     */
    public abstract void onMemberAdd(_AOrgMember<MemberData> _member);

    /**
     * 成员离开
     * @param _member     离开成员对象
     * @param _isDissolve 是否是解散
     */
    public abstract void onMemberRemove(_AOrgMember<MemberData> _member, boolean _isDissolve);
}
