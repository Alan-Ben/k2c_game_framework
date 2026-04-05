package NPCommonServer.USServerHandleUserMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo;

import java.util.ArrayList;
import java.util.Collection;

/**
 * @description: 汇总 US 服务器统计负
 * 每次来获取，都是负载统计的一个截面数据，
 * 不能拿来做精准判断。
 * @author: ricci
 * @date: 2022-06-29 11:14:12
 */
public class USServerHandleUserMgr
{

    /**
     * 服务器负载信息列表
     */
    private ArrayList<NpServerObj_SYS_ServerHoldInfo> _m_holdInfoList;
    /**
     * 保护 _m_holdInfoList 锁对象
     */
    private MutexAtom _m_mutex;
    //////单例的//////
    private static final USServerHandleUserMgr _s_instance = new USServerHandleUserMgr();

    public static USServerHandleUserMgr getInstance()
    {
        return _s_instance;
    }

    private USServerHandleUserMgr()
    {
        _m_mutex = new MutexAtom();
        _m_holdInfoList = new ArrayList<>();
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
     * 查询服务器负载信息
     * @param _serverTypeId 服务器typeId
     * @return NpServerObj_SYS_ServerHoldInfo
     */
    public NpServerObj_SYS_ServerHoldInfo lookup(int _serverTypeId)
    {
        _lock();
        try
        {
            for (NpServerObj_SYS_ServerHoldInfo info : _m_holdInfoList)
            {
                if (info == null)
                {
                    continue;
                }
                if (info.getServerTypeId() == _serverTypeId)
                {
                    return info;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新数据
     * @param _holdInfo 负载信息
     */
    public void update(NpServerObj_SYS_ServerHoldInfo _holdInfo)
    {
        _lock();
        try
        {
            NpServerObj_SYS_ServerHoldInfo info
                    = lookup(_holdInfo.getServerTypeId());
            if (info == null)
            {
                //直接加
                _m_holdInfoList.add(_holdInfo);
            } else
            {
                info.setTotalCount(_holdInfo.getTotalCount());
                info.setHoldCount(_holdInfo.getHoldCount());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 给定一批服务器typeId列表
     * @param _serverTypeIdList 服务器typeId列表
     * @return NpServerObj_SYS_ServerHoldInfo
     */
    public Collection<? extends NpServerObj_SYS_ServerHoldInfo> lookup(ArrayList<Integer> _serverTypeIdList)
    {
        ArrayList<NpServerObj_SYS_ServerHoldInfo> list = new ArrayList<>();
        _lock();
        try
        {
            for (NpServerObj_SYS_ServerHoldInfo info : _m_holdInfoList)
            {
                if (info == null)
                {
                    continue;
                }
                if (!_serverTypeIdList.contains(info.getServerTypeId()))
                {
                    continue;
                }
                list.add(info);
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    private String getString(NpServerObj_SYS_ServerHoldInfo _item)
    {
        return String.format("[usId:%d totalCount:%d holdCount:%d remainCount:%d]"
                , _item.getServerTypeId()
                , _item.getTotalCount()
                , _item.getHoldCount()
                , _item.getTotalCount() - _item.getHoldCount()
        );
    }

    /**
     * 输出所有的负载信息
     */
    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        _lock();
        try
        {
            for (NpServerObj_SYS_ServerHoldInfo info : _m_holdInfoList)
            {
                if (info == null)
                    continue;

                sb.append(getString(info)).append("\n");
            }
            return sb.toString();
        } finally
        {
            _unlock();
        }
    }
}
