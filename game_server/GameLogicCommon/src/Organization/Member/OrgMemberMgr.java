package Organization.Member;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Organization.Org._AOrg;
import Organization.Org._AOrgData;
import Organization._IOrgEnv;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 组织成员管理器
 * @param <OrgData>    组织信息
 * @param <MemberData> 成员信息
 * @param <Org>        组织对象
 */
public class OrgMemberMgr<OrgData extends _AOrgData, MemberData extends _AOrgMemberData,
        Org extends _AOrg<OrgData, MemberData>>
{
    private final Org _m_org;
    private final List<_AOrgMember<MemberData>> _m_memberList;
    private final Map<Long, _AOrgMember<MemberData>> _m_memberMap;
    private final MutexAtom _m_mutex;

    public OrgMemberMgr(Org _org)
    {
        _m_org = _org;
        _m_memberList = new ArrayList<>();
        _m_memberMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 获取组织对象
     * @return
     */
    public Org getOrg()
    {
        return _m_org;
    }

    /**
     * 获取环境信息
     * @return
     */
    public _IOrgEnv getEnv()
    {
        return _m_org.getEnv();
    }

    /**
     * 通过成员serial查询
     * @param _memberId long
     * @return _AOrgMember<MemberInfo>
     */
    public _AOrgMember<MemberData> lookup(long _memberId)
    {
        _lock();
        try
        {
            return _m_memberMap.get(_memberId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 初始化恢复一个成员数据，
     * 同时维护 list 和 map 两个数据集
     */
    public void initMember(_AOrgMember<MemberData> _member)
    {
        _lock();
        try
        {
            //已存在相同成员
            if (_m_memberMap.containsKey(_member.getMemberId()))
                return;

            //map不存在给定成员则添加到map中
            _m_memberMap.putIfAbsent(_member.getMemberId(), _member);
            //list不存在给定成员添加到list中
            _m_memberList.add(_member);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加成员
     */
    public void addMember(_AOrgMember<MemberData> _member)
    {
        _lock();
        try
        {
            //已存在相同成员
            if (_m_memberMap.containsKey(_member.getMemberId()))
                return;

            //map不存在给定成员则添加到map中
            _m_memberMap.putIfAbsent(_member.getMemberId(), _member);
            //list不存在给定成员添加到list中
            _m_memberList.add(_member);
        } finally
        {
            _unlock();
        }

        //添加成员通知
        _m_org.onMemberAdd(_member);
    }

    /**
     * 移除成员
     * @param _memberId 成员Id
     */
    public void removeMember(long _memberId)
    {
        _AOrgMember<MemberData> member;
        _lock();
        try
        {
            member = _m_memberMap.remove(_memberId);
            if (member == null)
                return;

            _m_memberList.remove(member);
            member.discard();
        } finally
        {
            _unlock();
        }

        //成员移除通知
        _m_org.onMemberRemove(member, false);
    }

    /**
     * 组织解散处理
     */
    public void onOrgDissolve()
    {
        List<_AOrgMember<MemberData>> memberList;

        _lock();
        try
        {
            memberList = new ArrayList<>(_m_memberList);

            //清空数据集
            _m_memberList.clear();
            _m_memberMap.clear();
        } finally
        {
            _unlock();
        }

        //组织解散通知
        for (_AOrgMember<MemberData> member : memberList)
        {
            _m_org.onMemberRemove(member, true);
        }
    }
}
