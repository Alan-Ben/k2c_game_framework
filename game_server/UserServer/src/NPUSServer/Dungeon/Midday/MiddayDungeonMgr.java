package NPUSServer.Dungeon.Midday;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.DungeonObj.MiddayDungeon_TimeInfo;
import Common.NpChatObj.ChatObj_SystemLog;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import GS2GC.p024_DungeonOp.GS2GC_024_052_OnMiddayDungeonTimeInfoChg;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPChatMsgType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.ChatSys.ChatRoomApi;
import NPUSServer.Dungeon.Midday.Box.MiddayDungeonBoxMgr;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.MiddayDungeonBO;

import java.util.List;

public class MiddayDungeonMgr
{
    private NPUserServer _m_server;
    private MiddayDungeonBO _m_bo;
    private MutexAtom _m_mutex;

    private MiddayDungeonBoxMgr _m_boxMgr;

    public MiddayDungeonMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_boxMgr = new MiddayDungeonBoxMgr(this);
        _m_mutex = new MutexAtom();
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public MiddayDungeonBoxMgr getBoxMgr()
    {
        return _m_boxMgr;
    }

    /**
     * 初始化
     * @return
     */
    public boolean init()
    {
        //初始化副本数据
        if (!__initDungeonInfo())
            return false;

        //初始化副本宝箱数据
        if (!_m_boxMgr.init())
            return false;

        return true;
    }

    /**
     * 初始化晚间副本数据
     * @return
     */
    private boolean __initDungeonInfo()
    {
        List<MiddayDungeonBO> boList = getServer().getBM().getBM(MiddayDungeonBO.class).s_findAll();
        if (boList == null)
            return false;

        if (boList.isEmpty())
        {
            MiddayDungeonBO bo = new MiddayDungeonBO();
            bo.setRoundPreviewTimeMs(getServer().getBM(), -1);
            bo.setRoundStartTimeMs(getServer().getBM(), -1);
            bo.setRoundEndTimeMs(getServer().getBM(), -1);
            bo.insert(getServer().getBM());

            _m_bo = bo;
        }else
        {
            _m_bo = boList.get(0);
        }

        return true;
    }

    /**
     * 获取当前轮次的开始时间
     * @return
     */
    public WCGPairLong getRoundTime()
    {
        _lock();
        try{
            return new WCGPairLong(_m_bo.getRoundStartTimeMs(), _m_bo.getRoundEndTimeMs());
        }finally
        {
            _unlock();
        }
    }

    /**
     * tick - 每秒执行
     *
     * 执行流程：
     * 1. 检查是否需要发送开启通知（仅一次）
     * 2. 检查是否需要进入下一个轮次
     * 3. 计算新轮次时间并重置状态
     */
    public void tick1Sec()
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();

        // 检查是否需要发送开启通知
        // 条件：当前时间在轮次开始和结束之间，且未发送过通知
        if (nowTimeMS >= _m_bo.getRoundStartTimeMs() &&
            nowTimeMS <= _m_bo.getRoundEndTimeMs() &&
            !_m_bo.getHadSendOpenNotice())
        {
            _lock();
            try {
                // 双重检查，避免并发问题
                if (!_m_bo.getHadSendOpenNotice()) {
                    // 构造聊天系统消息
                    NPCommon_ChatPlayerContent userProto = new NPCommon_ChatPlayerContent();
                    ChatObj_SystemLog proto = new ChatObj_SystemLog();
                    proto.setLogType(RefGeneral.Ref().midday_dungeon_system_log_id);

                    // 发送到全服聊天频道
                    ALSynTaskManager.getInstance().regTask(() ->
                            ChatRoomApi.sendUsRoomSysMsg(getServer(),
                                    ENPChatMsgType.SYSTEM_LOG,
                                    userProto.makePackage(),
                                    proto.makePackage(),
                                    null));

                    // 设置标志位，避免重复发送
                    _m_bo.saveHadSendOpenNotice(getServer().getBM(), true);
                }
            } finally {
                _unlock();
            }
        }

        if (nowTimeMS < _m_bo.getRoundStartTimeMs())
            return;

        //检查当前时间是否在轮次内
        if (nowTimeMS <= _m_bo.getRoundEndTimeMs())
            return;

        //如果已经结算，则检查是否需要进入下一个轮次
        long roundStartTimeMs;
        long roundEndTimeMs;

        long beforeStartTimeTagMS = RefGeneral.Ref().midday_dungeon_start_fight_time.getBeforeFreshTimeTagMS(nowTimeMS);
        long beforeEndTimeTagMS = RefGeneral.Ref().midday_dungeon_end_fight_time.getNextFreshTimeTagMS(beforeStartTimeTagMS);
        if (nowTimeMS < beforeEndTimeTagMS)
        {
            //使用当前时间计算当前时间所在轮的开始和结束时间
            roundStartTimeMs = beforeStartTimeTagMS;
            roundEndTimeMs = beforeEndTimeTagMS;
        } else
        {
            //如果当前时间在前一天的结束时间之后，则使用当前时间
            roundStartTimeMs = RefGeneral.Ref().midday_dungeon_start_fight_time.getNextFreshTimeTagMS(nowTimeMS);
            roundEndTimeMs = RefGeneral.Ref().midday_dungeon_end_fight_time.getNextFreshTimeTagMS(roundStartTimeMs);
        }

        //检查时间是否合法
        if (roundStartTimeMs == -1 || roundEndTimeMs == -1)
            return;

        _lock();
        try{
            //计算预告时间
            long roundPreviewTimeMs = RefGeneral.Ref().midday_dungeon_preview_fight_time.getBeforeFreshTimeTagMS(roundStartTimeMs);

            //重置boss状态和标志位
            _m_bo.setRoundPreviewTimeMs(getServer().getBM(), roundPreviewTimeMs);
            _m_bo.setRoundStartTimeMs(getServer().getBM(), roundStartTimeMs);
            _m_bo.setRoundEndTimeMs(getServer().getBM(), roundEndTimeMs);
            _m_bo.setRoundDropBoxNum(getServer().getBM(), 0);
            _m_bo.setHadSendOpenNotice(getServer().getBM(), false);  // 重置通知标志位，为下一轮准备
            _m_bo.saveAllMarked(getServer().getBM());
        }finally
        {
            _unlock();
        }

        //时间变更推送
        getServer().getUsUserMgr().broadCastMessage(new GS2GC_024_052_OnMiddayDungeonTimeInfoChg(makeTimeInfo()));

        USLog.info(getServer(), "MiddayDungeonMgr.tick1Sec - enter next round: previewTime={}, startTime={}, endTime={}",
                _m_bo.getRoundPreviewTimeMs(), roundStartTimeMs, roundEndTimeMs);
    }

    /**
     * 尝试添加宝箱
     * @return
     */
    public boolean tryAddBox()
    {
        _lock();
        try
        {
            if (_m_bo.getRoundDropBoxNum() >= RefGeneral.Ref().midday_dungeon_server_round_drop_box_limit)
                return false;

            _m_bo.saveRoundDropBoxNum(getServer().getBM(), _m_bo.getRoundDropBoxNum() + 1);
            return true;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取当前轮次的时间
     * @return
     */
    public MiddayDungeon_TimeInfo makeTimeInfo()
    {
        MiddayDungeon_TimeInfo info = new MiddayDungeon_TimeInfo();
        info.setPreviewTimeMs(_m_bo.getRoundPreviewTimeMs());
        info.setStartTimeMs(_m_bo.getRoundStartTimeMs());
        info.setEndTimeMs(_m_bo.getRoundEndTimeMs());
        return info;
    }

    public void startTick()
    {
        ALSynTaskManager.getInstance().regTask(new MiddayDungeonTickTask(getServer()), 1000);
    }

    /**
     * 修改boss时间
     * @param _preview
     * @param _start
     * @param _end
     */
    public boolean cmdChgBossTime(int _preview, int _start, int _end)
    {
        NPRefreshTimeObj previewTime = new NPRefreshTimeObj();
        previewTime.parseFromString("REF_CLOCK:" + _preview);
        NPRefreshTimeObj startTime = new NPRefreshTimeObj();
        startTime.parseFromString("REF_CLOCK:" + _start);
        NPRefreshTimeObj endTime = new NPRefreshTimeObj();
        endTime.parseFromString("REF_CLOCK:" + _end);

        long nowTimeMS = CommonFunc.getNowTimeMS();

        long roundStartTimeMs;
        long roundEndTimeMs;

        long beforeStartTimeTagMS = startTime.getBeforeFreshTimeTagMS(nowTimeMS);
        long beforeEndTimeTagMS = endTime.getNextFreshTimeTagMS(beforeStartTimeTagMS);
        if (nowTimeMS < beforeEndTimeTagMS)
        {
            //使用当前时间计算当前时间所在轮的开始和结束时间
            roundStartTimeMs = beforeStartTimeTagMS;
            roundEndTimeMs = beforeEndTimeTagMS;
        } else
        {
            //如果当前时间在前一天的结束时间之后，则使用当前时间
            roundStartTimeMs = startTime.getNextFreshTimeTagMS(nowTimeMS);
            roundEndTimeMs = endTime.getNextFreshTimeTagMS(roundStartTimeMs);
        }

        //检查时间是否合法
        if (roundStartTimeMs == -1 || roundEndTimeMs == -1)
            return false;

        //计算预告时间
        long roundPreviewTimeMs = previewTime.getBeforeFreshTimeTagMS(roundStartTimeMs);

        _lock();
        try
        {
            _m_bo.setRoundPreviewTimeMs(getServer().getBM(), roundPreviewTimeMs);
            _m_bo.setRoundStartTimeMs(getServer().getBM(), roundStartTimeMs);
            _m_bo.setRoundEndTimeMs(getServer().getBM(), roundEndTimeMs);
            _m_bo.saveAllMarked(getServer().getBM());
        } finally
        {
            _unlock();
        }

        //时间变更推送
        getServer().getUsUserMgr().broadCastMessage(new GS2GC_024_052_OnMiddayDungeonTimeInfoChg(makeTimeInfo()));

        return true;
    }

    @Override
    public String toString()
    {
        return "middayDungeon " +
                "preview: " + CommonFunc.getTimeStringMs(_m_bo.getRoundPreviewTimeMs()) +
                ", start: " + CommonFunc.getTimeStringMs(_m_bo.getRoundStartTimeMs()) +
                ", end: " + CommonFunc.getTimeStringMs(_m_bo.getRoundEndTimeMs()) +
                ", hadDropBoxNum: " + _m_bo.getRoundDropBoxNum();
    }
}
