package NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ChapterObj.Chapter_EventInfo;
import NPGameRes.Refs.Chapter.RefChapterEvent;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;
import USDB.Bo.PlayerChapterEventBO;

/**
 * 章节事件处理器基类
 */
public abstract class _AChapterEvent
{
    private ChapterComponent _m_comp;
    private PlayerChapterEventBO _m_bo;
    private RefChapterEvent _m_eventRef;

    public _AChapterEvent(ChapterComponent _comp, PlayerChapterEventBO _bo, RefChapterEvent _ref)
    {
        _m_comp = _comp;
        _m_bo = _bo;
        _m_eventRef = _ref;
    }

    public ChapterComponent getComp()
    {
        return _m_comp;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    public PlayerChapterEventBO getBo()
    {
        return _m_bo;
    }

    public long getDbId()
    {
        return _m_bo.getId();
    }

    public RefChapterEvent getEventRef()
    {
        return _m_eventRef;
    }

    /**
     * 构造基础信息
     */
    public Chapter_EventInfo makeInfo()
    {
        Chapter_EventInfo _info = new Chapter_EventInfo();
        _info.setEventId(_m_bo.getEventId());
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
            getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_053_OnChapterEventChg(makeInfo()));
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
