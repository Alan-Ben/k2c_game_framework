package NPUSServer.Guild.Event;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.GuildEnum.EGuildEventType;
import Common.GuildObj.Guild_EventInfo;
import USDB.Bo.GuildEventBO;

public abstract class _AGuildEvent<Data extends _IALProtocolStructure>
{
    private GuildEventMgr _m_mgr;
    private GuildEventBO _m_bo;
    private MutexAtom _m_mutex;
    private Data _m_data;

    public _AGuildEvent(GuildEventMgr _mgr, GuildEventBO _bo)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;
        _m_mutex = new MutexAtom();
        _m_mutex.reducePriority(10);
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public GuildEventBO getBo()
    {
        return _m_bo;
    }

    public GuildEventMgr getMgr()
    {
        return _m_mgr;
    }

    public long getDbId()
    {
        return _m_bo.getId();
    }

    public Data getData()
    {
        return _m_data;
    }

    /**
     * 获取事件类型
     * @return
     */
    public abstract EGuildEventType getEventType();

    /**
     * 初始化数据
     */
    public boolean init()
    {
        _m_data = _createData(_m_bo.getData());
        return true;
    }

    /**
     * 事件是否还可用
     */
    public abstract boolean isAvailable();

    /**
     * 事件是否完结
     */
    public abstract boolean isDone();

    /**
     * 事件完结处理
     */
    public abstract void doneAction();

    /**
     * 创建数据
     * @return
     */
    protected abstract Data _createData(byte[] data);

    /**
     * 保存数据
     */
    public void save()
    {
        _m_bo.saveData(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM(), _m_data.makePackage().array());
    }

    /**
     * 构造协议结构
     * @return
     */
    public Guild_EventInfo makeProto()
    {
        Guild_EventInfo proto = new Guild_EventInfo();
        proto.setDbId(_m_bo.getId());
        proto.setType(getEventType());
        proto.setData(_m_data.makePackage().array());
        return proto;
    }

    /**
     * 销毁数据
     */
    public void discard()
    {
        getBo().del(_m_mgr.getGuildInfo().getGuildMgr().getServer().getBM());
    }
}
