package NPUSServer.NPUSUserMgr.UserComp.CountdownEventComp;

import Common.CountdownEventObj.CountdownEvent_Info;
import NPCommon.ErrMain.CountdownEventErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.CountdownEvent.RefCountdownEvent;
import NPGameRes.Refs.Quest.RefQuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerCountdownEventBO;

public class CountdownEventInfo
{
    private CountdownEventComponent _m_comp;
    private RefCountdownEvent _m_ref;

    private PlayerCountdownEventBO _m_bo;

    public CountdownEventInfo(CountdownEventComponent _comp,RefCountdownEvent _ref, PlayerCountdownEventBO _bo)
    {
        _m_comp = _comp;
        _m_ref = _ref;
        _m_bo = _bo;
    }

    public long getDbId()
    {
        return _m_bo.getId();
    }

    public RefCountdownEvent getRef() {return _m_ref;}

    public CountdownEventComponent getComp()
    {
        return _m_comp;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 是否已经激活
     * @return
     */
    public boolean isActive()
    {
        return _m_bo.getTriggerTimeMs() > 0;
    }

    /**
     * 激活事件
     */
    public void tryActivate()
    {
        getUserData().lockUser();
        try
        {
            // 如果已经激活，则不再重复激活
            if (_m_bo.getTriggerTimeMs() > 0)
                return;

            _m_bo.saveTriggerTimeMs(getComp().getUSServer().getBM(), CommonFunc.getNowTimeMS());

            RefQuest refQuest = RefQuest.getMgr().get(_m_ref.quest_id);
            if (refQuest == null)
            {
                USLog.error(getComp().getUSServer(), "RefCountdownEvent quest id error, eventId:{} questId:{}",
                        _m_ref.id, _m_ref.quest_id);
                return;
            }

            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.NONE);
            getUserData().getQuestComponent().removeQuest(_m_ref.quest_id, context);

            //激活事件后，开启任务
            getUserData().getQuestComponent().startQuestByServer(refQuest, context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否需要重置事件
     */
    public Result tryReset(boolean _isInit)
    {
        getUserData().lockUser();
        try
        {
            //检查是否需要重置
            if (_m_bo.getTriggerTimeMs() <= 0)
                return CountdownEventErr.COUNTDOWN_EVENT_IN_TIME;

            long nowTimeMs = CommonFunc.getNowTimeMS();
            if (nowTimeMs < _m_bo.getTriggerTimeMs() + _m_ref.duration * 1000L)
                return CountdownEventErr.COUNTDOWN_EVENT_IN_TIME;

            //判断是否需要重置任务计数
            if (_m_ref.undone_need_reset)
            {
                RefQuest refQuest = RefQuest.getMgr().get(_m_ref.quest_id);
                if (refQuest == null)
                {
                    USLog.error(getComp().getUSServer(), "RefCountdownEvent quest id error, eventId:{} questId:{}",
                            _m_ref.id, _m_ref.quest_id);
                    return CountdownEventErr.COUNTDOWN_EVENT_RELATIVE_QUEST_NOT_FOUND;
                }

                NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.NONE);
                getUserData().getQuestComponent().removeQuest(_m_ref.quest_id, context);

                getUserData().getQuestComponent().startQuestByServer(refQuest, context);
            }

            //重置事件，更新触发时间和重置次数
            _m_bo.setTriggerTimeMs(getComp().getUSServer().getBM(), nowTimeMs);
            _m_bo.setResetCount(getComp().getUSServer().getBM(), _m_bo.getResetCount() + 1);
            _m_bo.saveAllMarked(getComp().getUSServer().getBM());

            //发送事件变更消息到GC
            if (!_isInit)
                _m_comp.getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_076_OnCountdownEventChg(makeProto()));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 丢弃事件数据
     */
    public void discard()
    {
        getUserData().lockUser();
        try
        {
            //删除事件数据
            _m_bo.del(getComp().getUSServer().getBM());
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造协议
     * @return
     */
    public CountdownEvent_Info makeProto()
    {
        getUserData().lockUser();
        try
        {
            CountdownEvent_Info proto = new CountdownEvent_Info();
            proto.setDbId(_m_bo.getId());
            proto.setEventId(_m_bo.getEventId());
            proto.setTriggerTimeMs(_m_bo.getTriggerTimeMs());
            proto.setResetCount(_m_bo.getResetCount());
            return proto;
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
