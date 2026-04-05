package NPUSServer.NPUSUserMgr.UserComp.PlayerEventRecordComp;

import Common.PlayerObj.Player_EventRecordInfo;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import USDB.Bo.PlayerEventRecordBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * @description: 玩家事件计数器组件 与 ENPPlayerEventRecordType 绑定使用
 * @author: ricci
 * @date: 2022-08-26 15:42:15
 */
public class NPPlayerEventRecordComp extends _ANPUserComponent
{
    /**
     * 计数器容器 ENPPlayerEventRecordType: NPPlayerEventRecordInfoMgr
     */
    private final HashMap<Integer, NPPlayerEventRecordInfoMgr> _m_mapRecordInfoMgrMap;

    public NPPlayerEventRecordComp(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.EVENT_RECORD_COMP);
        _m_mapRecordInfoMgrMap = new HashMap<>();
    }

    private NPPlayerEventRecordComp __getThis()
    {
        return this;
    }

    public long getCid()
    {
        return getUserData().getCid();
    }

    protected void _lock()
    {
        getUserData().lockUser();
    }

    protected void _unlock()
    {
        getUserData().unlockUser();
    }

    /**
     * 查找或创建记录对象
     * @param _recordTypeId 记录类型
     * @return NPPlayerEventRecordInfoMgr
     */
    private NPPlayerEventRecordInfoMgr __ensure(int _recordTypeId)
    {
        _lock();
        try
        {
            NPPlayerEventRecordInfoMgr recordInfo = _m_mapRecordInfoMgrMap.get(_recordTypeId);
            if (recordInfo == null)
            {
                recordInfo = new NPPlayerEventRecordInfoMgr(_recordTypeId, this);
                _m_mapRecordInfoMgrMap.put(_recordTypeId, recordInfo);
            }
            return recordInfo;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造协议对象
     * @param _list
     */
    public void makeProto(ArrayList<Player_EventRecordInfo> _list)
    {
        _lock();

        try
        {
            for (NPPlayerEventRecordInfoMgr mgr : _m_mapRecordInfoMgrMap.values())
            {
                if (null == mgr)
                    continue;

                mgr.makeProto(_list);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取对象计数值
     * @param _recordTypeId 计数类型
     * @param _recordSubId  细分id，如果取-1，表示取子计数总和
     * @return long
     */
    public long getCount(int _recordTypeId, long _recordSubId)
    {
        _lock();
        try
        {
            NPPlayerEventRecordInfoMgr mgr = _m_mapRecordInfoMgrMap.get(_recordTypeId);
            if (mgr == null)
            {
                return 0;
            }

            //-1：取所有计数之和
            if (-1 == _recordSubId)
            {
                return mgr.getSum();
            }

            return mgr.getCount(_recordSubId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取对象计数值
     * @param _recordTypeId 计数类型
     * @param _subIdRange  细分id，如果取-1，表示取子计数总和
     * @return long
     */
    public long getCount(int _recordTypeId, WCGIntRange _subIdRange)
    {
        _lock();
        try
        {
            NPPlayerEventRecordInfoMgr mgr = _m_mapRecordInfoMgrMap.get(_recordTypeId);
            if (mgr == null)
            {
                return 0;
            }

            return mgr.getSum(_subIdRange);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加计数
     * @param _recordTypeId 计数类型
     * @param _recordSubId  计数细分id
     * @param _count        次数
     */
    public void addRecord(int _recordTypeId, long _recordSubId, long _count)
    {
        _lock();
        try
        {
            if (_count == 0)
            {
                return;
            }
            //根据类型查询记录组
            NPPlayerEventRecordInfoMgr recordMgr = __ensure(_recordTypeId);
            //根据子id查询记录信息
            NPPlayerEventRecordInfo info = recordMgr._ensure(_recordSubId);
            //增加值
            long oldCount = info.getCount();
            long newVal = oldCount + _count;
            info.setCount(newVal);

            getUserData().Events.OnEventRecordChg.onAsyncEvent(_recordTypeId, _recordSubId, oldCount, newVal);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 减少计数
     * @param _recordTypeId 计数类型
     * @param _recordSubId  计数细分id
     * @param _count        次数
     */
    public void reduceRecord(int _recordTypeId, long _recordSubId, long _count)
    {
        _lock();
        try
        {
            if (_count == 0)
            {
                return;
            }
            //根据类型查询记录组
            NPPlayerEventRecordInfoMgr recordMgr = __ensure(_recordTypeId);
            //根据子id查询记录信息
            NPPlayerEventRecordInfo info = recordMgr._ensure(_recordSubId);
            //减少值
            long oldCount = info.getCount();
            long newVal = oldCount - _count;
            info.setCount(newVal);

            getUserData().Events.OnEventRecordChg.onAsyncEvent(_recordTypeId, _recordSubId, oldCount, newVal);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加计数
     * @param _recordTypeId 计数类型
     * @param _recordSubId  计数细分id
     * @param _count        次数
     */
    public void setRecord(int _recordTypeId, long _recordSubId, long _count)
    {
        _lock();
        try
        {
            //根据类型查询记录组
            NPPlayerEventRecordInfoMgr recordMgr = __ensure(_recordTypeId);
            //根据子id查询记录信息
            NPPlayerEventRecordInfo info = recordMgr._ensure(_recordSubId);
            //设置
            long oldCount = info.getCount();
            info.setCount(_count);

            getUserData().Events.OnEventRecordChg.onAsyncEvent(_recordTypeId, _recordSubId, oldCount, _count);

        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置更高的计数
     * @param _recordTypeId 计数类型
     * @param _recordSubId  计数细分id
     * @param _count        次数
     */
    public void setRecordGT(int _recordTypeId, long _recordSubId, long _count)
    {
        _lock();
        try
        {
            //根据类型查询记录组
            NPPlayerEventRecordInfoMgr recordMgr = __ensure(_recordTypeId);
            //根据子id查询记录信息
            NPPlayerEventRecordInfo info = recordMgr._ensure(_recordSubId);
            //计数更大才记录
            if (info.getCount() >= _count)
            {
                return;
            }
            long oldCount = info.getCount();
            info.setCount(_count);

            getUserData().Events.OnEventRecordChg.onAsyncEvent(_recordTypeId, _recordSubId, oldCount, _count);
        } finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (NPPlayerEventRecordInfoMgr mgr : _m_mapRecordInfoMgrMap.values())
        {
            sb.append("\n").append(mgr).append("\n");
        }
        return "{" +
                "_m_recordList=" + sb +
                '}';
    }


    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerEventRecordBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerEventRecordBO>>()
        {

            @Override
            public void dealSuc(List<PlayerEventRecordBO> _boList)
            {
                for (PlayerEventRecordBO bo : _boList)
                {
                    //按类型初始化管理组
                    NPPlayerEventRecordInfoMgr mgr = _m_mapRecordInfoMgrMap.get(bo.getRecordTypeId());
                    if (mgr == null)
                    {
                        mgr = new NPPlayerEventRecordInfoMgr(bo.getRecordTypeId(), __getThis());
                        _m_mapRecordInfoMgrMap.put(bo.getRecordTypeId(), mgr);
                    }
                    //将数据放入管理组中初始化
                    mgr._initFromDB(bo);
                }
                setInited();
            }

            @Override
            public void dealFail()
            {

            }
        });
    }


    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {

    }

    @Override
    public void dispose()
    {

    }

}
