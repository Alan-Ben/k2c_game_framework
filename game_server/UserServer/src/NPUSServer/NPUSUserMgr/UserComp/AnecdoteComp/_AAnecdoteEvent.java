package NPUSServer.NPUSUserMgr.UserComp.AnecdoteComp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.AnecdoteObj.Anecdote_EventInfo;
import NPGameRes.Refs.Anecdote.RefAnecdoteEvent;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import USDB.Bo.PlayerAnecdoteEventBO;

/**
 * 章节事件处理器基类
 */
public abstract class _AAnecdoteEvent
{
    private AnecdoteComponent _m_comp;
    private PlayerAnecdoteEventBO _m_bo;
    private RefAnecdoteEvent _m_eventRef;

    public _AAnecdoteEvent(AnecdoteComponent _comp, PlayerAnecdoteEventBO _bo, RefAnecdoteEvent _ref)
    {
        _m_comp = _comp;
        _m_bo = _bo;
        _m_eventRef = _ref;
    }

    public AnecdoteComponent getComp()
    {
        return _m_comp;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    public PlayerAnecdoteEventBO getBo()
    {
        return _m_bo;
    }

    public long getDbId()
    {
        return _m_bo.getId();
    }

    public RefAnecdoteEvent getEventRef()
    {
        return _m_eventRef;
    }

    public long getEventId()
    {
        return _m_eventRef.Id();
    }

    public long getPosId()
    {
        return _m_bo.getPosId();
    }

    public boolean isDirectGain()
    {
        return _m_bo.getIsDirectGain();
    }

    /**
     * 构造基础信息
     */
    public Anecdote_EventInfo makeInfo()
    {
        Anecdote_EventInfo _info = new Anecdote_EventInfo();
        _info.setInstanceId(_m_bo.getId());
        _info.setEventId(_m_bo.getEventId());
        _info.setPosId(_m_bo.getPosId());
        //构造事件的额外数据
        _IALProtocolStructure detailProto = makeExtraData();
        if (detailProto!= null)
            _info.setExtraData(detailProto.makePackage().array());
        return _info;
    }

    /**
     * 事件数据变更
     */
    protected void onDataChg(boolean _isInit)
    {
        //构造事件的额外数据
        _IALProtocolStructure extraProto = makeExtraData();
        if (extraProto != null)
            _m_bo.saveExtraData(getUserData().getUSServer().getBM(), extraProto.makePackage().array());

        //发送事件数据变更消息
        if (!_isInit)
        {
            getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_070_OnAnecdoteChg(this));
        }
    }

    /**
     * 销毁事件
     */
    public void dispose()
    {
        _m_bo.del(getUserData().getUSServer().getBM());
    }

    /**
     * 构造事件的额外数据
     */
    public abstract _IALProtocolStructure makeExtraData();

    /**
     * 是否完成
     * @return
     */
    public abstract boolean isDone();
}
