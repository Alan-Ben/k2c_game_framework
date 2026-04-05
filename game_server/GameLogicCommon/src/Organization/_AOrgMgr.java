package Organization;


import ALBasicServer.ALBasicMutex.MutexAtom;
import Organization.Member._AOrgMemberData;
import Organization.Org._AOrg;
import Organization.Org._AOrgData;

import java.util.HashMap;
import java.util.Map;

/**
 * 组织管理器
 * @param <OrgData>    组织信息
 * @param <MemberData> 成员信息
 * @param <Org>        组织对象
 */
public class _AOrgMgr<OrgData extends _AOrgData, MemberData extends _AOrgMemberData, Org extends _AOrg<OrgData, MemberData>>
{
    //环境信息
    private final _IOrgEnv _m_env;
    //组织对象列表
    private final Map<Long, Org> _m_orgMap;
    //组织操作锁对象
    private final MutexAtom _m_mutex;

    public _AOrgMgr(_IOrgEnv _env)
    {
        _m_env = _env;
        _m_orgMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    public _IOrgEnv getEnv()
    {
        return _m_env;
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
     * 通过组织ID查询组织
     * @param _orgId 组织ID
     * @return Org
     */
    public Org lookup(long _orgId)
    {
        _lock();
        try
        {
            return _m_orgMap.get(_orgId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 初始化一个组织对象数据，加入管理
     *
     * @param _newOrg 组织对象数据
     */
    public void initOrg(Org _newOrg)
    {
        _lock();
        try
        {
            _m_orgMap.put(_newOrg.getInfo().getOrgId(), _newOrg);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 新增一个组织对象，加入管理
     *
     * @param _newOrg 组织对象数据
     */
    public boolean addOrg(Org _newOrg)
    {
        _lock();
        try
        {
            if (_newOrg == null)
                return false;

            //已存在相同对象，不再创建
            if (_m_orgMap.containsKey(_newOrg.getInfo().getOrgId()))
                return false;

            //放入管理器
            _m_orgMap.put(_newOrg.getInfo().getOrgId(), _newOrg);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 移除组织对象
     *
     * @param _orgId 组织序列号
     */
    public boolean dissolve(long _orgId)
    {
        _lock();
        try
        {
            //不做移除处理，避免后续要查询相关信息
            Org org = _m_orgMap.get(_orgId);
            if (org == null)
                return false;

            //销毁组织对象
            org.dissolve();

            return true;
        } finally
        {
            _unlock();
        }
    }
}
