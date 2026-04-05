package NPUSServer.Guild.Event;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.GuildEnum.EGuildEventType;
import Common.GuildObj.Guild_EventInfo;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.USLog;
import USDB.Bo.GuildEventBO;

import java.util.ArrayList;
import java.util.List;

public class GuildEventMgr
{
    private GuildInfo _m_guildInfo;
    private List<_AGuildEvent<?>> _m_eventList;
    private MutexAtom _m_mutex;

    public GuildEventMgr(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
        _m_mutex = new MutexAtom();
        _m_eventList = new ArrayList<>();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    /**
     * 初始化事件
     * @param _eventBO
     */
    public void initEvent(GuildEventBO _eventBO)
    {
        _AGuildEvent<?> event = _createEvent(EGuildEventType.values()[_eventBO.getType()], _eventBO);
        if (event == null)
            return;

        //初始化事件
        try
        {
            event.init();
        } catch (Exception e)
        {
            USLog.error(_m_guildInfo.getGuildMgr().getServer(), "GuildEventMgr initEvent fail. guildId:{} eventType:{}",
                    _m_guildInfo.getGuildId(), EGuildEventType.values()[_eventBO.getType()], e);
            _eventBO.del(_m_guildInfo.getGuildMgr().getServer().getBM());
            return;
        }

        _lock();
        try
        {
            //添加事件
            _m_eventList.add(event);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找事件对象
     * @param _dbId
     * @return
     */
    public _AGuildEvent<?> lookup(long _dbId)
    {
        _lock();
        try
        {
            for (_AGuildEvent<?> _event : _m_eventList)
            {
                if (_event.getDbId() == _dbId)
                {
                    return _event;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 是否已经存在事件
     * @param _type
     * @return
     */
    public boolean hasExistEvent(EGuildEventType _type)
    {
        _lock();
        try
        {
            for (_AGuildEvent<?> _event : _m_eventList)
            {
                if (_event.getEventType() == _type)
                {
                    return true;
                }
            }
            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 添加事件对象
     * @param _eventType
     */
    public _AGuildEvent<?> createEvent(EGuildEventType _eventType, _IALProtocolStructure _initData)
    {
        GuildEventBO bo = new GuildEventBO();
        bo.setGuildId(_m_guildInfo.getGuildMgr().getServer().getBM(), _m_guildInfo.getGuildId());
        bo.setType(_m_guildInfo.getGuildMgr().getServer().getBM(), _eventType.ordinal());
        bo.setData(_m_guildInfo.getGuildMgr().getServer().getBM(), _initData.makePackage().array());
        bo.insert(_m_guildInfo.getGuildMgr().getServer().getBM());

        _AGuildEvent<?> event = _createEvent(_eventType, bo);
        if (event == null)
            return null;

        //初始化事件
        try
        {
            event.init();
        } catch (Exception e)
        {
            USLog.error(_m_guildInfo.getGuildMgr().getServer(), "GuildEventMgr createEvent fail. guildId:{} eventType:{}",
                    _m_guildInfo.getGuildId(), _eventType, e);
            bo.del(_m_guildInfo.getGuildMgr().getServer().getBM());
            return null;
        }

        _lock();
        try
        {
            //添加事件
            _m_eventList.add(event);
        } finally
        {
            _unlock();
        }

        //通知客户端
        getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_059_OnGuildEventAdd(event.makeProto()));

        return event;
    }

    /**
     * 创建事件
     * @param _eventType
     * @param _bo
     * @return
     */
    public _AGuildEvent<?> _createEvent(EGuildEventType _eventType, GuildEventBO _bo)
    {
        _AGuildEvent<?> _event;
        if (_eventType == EGuildEventType.IMPEACH_LEADER)
        {
            _event = new GuildEventDealer_ImpeachLeader(this, _bo);
        } else
        {
            return null;
        }
        return _event;
    }

    /**
     * 取消事件
     */
    public Result cancelEvent(long _dbId)
    {
        _AGuildEvent<?> event = lookup(_dbId);
        if (event == null)
            return GuildErr.EVENT_NOT_FOUND;

        _lock();
        try
        {
            //删除事件
            _m_eventList.remove(event);
        } finally
        {
            _unlock();
        }

        //销毁事件
        event.discard();
        //通知客户端
        getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_061_OnGuildEventRemove(event.getDbId()));

        return Result.SUCC;
    }

    /**
     * 强制完成事件
     */
    public Result forceDoneEvent(long _dbId)
    {
        _AGuildEvent<?> event = lookup(_dbId);
        if (event == null)
            return GuildErr.EVENT_NOT_FOUND;

        _lock();
        try
        {
            //删除事件
            _m_eventList.remove(event);
        } finally
        {
            _unlock();
        }

        //完成事件
        event.doneAction();
        //销毁事件
        event.discard();
        //通知客户端
        getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_061_OnGuildEventRemove(event.getDbId()));

        return Result.SUCC;
    }

    /**
     * tick处理
     */
    public void tick()
    {
        if (_m_eventList.isEmpty())
            return;

        //需要删除的事件列表
        List<_AGuildEvent<?>> needDelEventList = new ArrayList<>();
        //需要完成的事件列表
        List<_AGuildEvent<?>> needDoneEventList = new ArrayList<>();

        _lock();
        try
        {
            for (int i = _m_eventList.size() - 1; i >= 0; i--)
            {
                _AGuildEvent<?> event = _m_eventList.get(i);
                if (event == null)
                    continue;

                //判断事件是否失效
                if (!event.isAvailable())
                {
                    _m_eventList.remove(i);
                    needDelEventList.add(event);
                    continue;
                }

                //判断事件是否完结
                if (event.isDone())
                {
                    _m_eventList.remove(i);
                    needDoneEventList.add(event);
                }
            }
        } finally
        {
            _unlock();
        }

        //完成事件
        for (_AGuildEvent<?> event : needDoneEventList)
        {
            event.doneAction();
            //销毁事件
            event.discard();
            //通知客户端
            getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_061_OnGuildEventRemove(event.getDbId()));
        }

        //删除事件
        for (_AGuildEvent<?> event : needDelEventList)
        {
            //销毁事件
            event.discard();
            //通知客户端
            getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_061_OnGuildEventRemove(event.getDbId()));
        }
    }

    /**
     * 构造成员信息
     * @return
     */
    public List<Guild_EventInfo> makeProtoList()
    {
        _lock();
        try
        {
            List<Guild_EventInfo> eventList = new ArrayList<>();
            for (_AGuildEvent<?> event : _m_eventList)
            {
                eventList.add(event.makeProto());
            }
            return eventList;
        } finally
        {
            _unlock();
        }
    }

    public void discard()
    {
        _lock();
        try{
            _m_eventList.clear();
        }finally
        {
            _unlock();
        }

        getGuildInfo().getGuildMgr().getServer().getBM().getBM(GuildEventBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
    }
}
