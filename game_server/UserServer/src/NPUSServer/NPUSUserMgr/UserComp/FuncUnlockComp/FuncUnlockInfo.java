package NPUSServer.NPUSUserMgr.UserComp.FuncUnlockComp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpChatObj.ChatObj_SystemLog;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPChatRoomType;
import NPEnum.ENPFunctionType;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.Share.RefBoxComm;
import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerFuncUnlockBO;

/**
 * 功能解锁信息
 * <p>
 * 主要功能：
 * 1. 缓存功能解锁的状态数据
 * 2. 提供客户端通知状态管理
 * 3. 提供奖励领取状态管理
 */
public class FuncUnlockInfo
{
    private FuncUnlockComponent _m_comp;
    // 数据库ID
    private long _m_dbId;
    // 功能类型
    private ENPFunctionType _m_type;
    // 客户端是否已通知
    private boolean _m_isClientNotified;
    // 是否有未领取的奖励
    private boolean _m_notDrawReward;

    public FuncUnlockInfo(FuncUnlockComponent _comp, PlayerFuncUnlockBO _bo)
    {
        _m_comp = _comp;
        _m_dbId = _bo.getId();
        _m_type = ENPFunctionType.ENPFunctionType_FromInt(_bo.getFuncType());
        _m_isClientNotified = _bo.getIsClientNotified();
        _m_notDrawReward = _bo.getNotDrawReward();
    }

    /**
     * 获取BM对象
     */
    private BM getBM()
    {
        return getUSServer().getBM();
    }

    public NPUserServer getUSServer()
    {
        return _m_comp.getUSServer();
    }

    public ENPFunctionType getFuncType()
    {
        return _m_type;
    }

    /**
     * 获取客户端是否已通知
     */
    public boolean isClientNotified()
    {
        return _m_isClientNotified;
    }

    /**
     * 获取是否有未领取的奖励
     */
    public boolean hasUnreceivedReward()
    {
        return _m_notDrawReward;
    }

    public boolean hadDrawReward()
    {
        return !_m_notDrawReward;
    }

    /**
     * 标记客户端已通知
     * 更新内存状态并保存到数据库
     */
    public void markClientNotified()
    {
        if (_m_isClientNotified)
            return;

        _m_isClientNotified = true;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("is_client_notified", 1);
        getBM().getBM(PlayerFuncUnlockBO.class).update("id", _m_dbId, updateValue);

        onClientNotifyed();
    }

    /**
     * 客户端已通知后的处理
     */
    private void onClientNotifyed()
    {
        //仅处理华尔街功能解锁的宝箱发送
        if (_m_type != ENPFunctionType.WALL_STREET)
            return;

        //发送宝箱聊天
        RefBoxComm ref = RefBoxComm.getMgr().get(RefGeneral.Ref().arrive_space_box_id);
        if (null == ref)
        {
            USLog.warn("FuncUnlockInfo.onClientNotifyed failed to find box comm ref, cid:{} type:{} id:{}"
                    , _m_comp.getUserData().getCid(), getFuncType().name(), RefGeneral.Ref().arrive_space_box_id);
            return;
        }

        //聊天用户数据
        NPCommon_ChatPlayerContent userProto = _m_comp.getUserData().toChatPlayerProto();

        //创建宝箱
        CommBoxInfo boxInfo = _m_comp.getUserData().getUSServer().getCommBoxMgr().buildBox(ref,
                _m_comp.getUserData().getCid(), NPPlayerContext.createNew(ENPGameEvent.CLIENT_NOTIFY_FUNC_UNLOCK));
        _m_comp.getUserData().getPlayerChatRoomDealer().sendRoomMsg(ENPChatRoomType.US_SERVER.ordinal()
                , 0
                , ENPChatMsgType.COMM_BOX.ordinal()
                , userProto.makePackage()
                , boxInfo.toChatProto().makePackage()
                , null);

        // 构造聊天系统消息
        ChatObj_SystemLog proto = new ChatObj_SystemLog();
        proto.setLogType(RefGeneral.Ref().arrive_space_chat_system_log);
        // 发送到联盟聊天频道
        ALSynTaskManager.getInstance().regTask(() ->
                _m_comp.getUserData().getPlayerChatRoomDealer().sendRoomMsg(ENPChatRoomType.GUILD.ordinal()
                        , 0
                        , ENPChatMsgType.SYSTEM_LOG.ordinal()
                        , userProto.makePackage()
                        , proto.makePackage(),
                        null)
        );
    }

    /**
     * 标记奖励已领取
     * 更新内存状态并保存到数据库
     */
    public Result markRewardReceived()
    {
        if (!_m_notDrawReward)
            return PlayerErr.PLAYER_HAD_UNLOCK_FUNC;

        _m_notDrawReward = false;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("not_draw_reward", 0);
        getBM().getBM(PlayerFuncUnlockBO.class).update("id", _m_dbId, updateValue);

        return Result.SUCC;
    }

    /**
     * 销毁方法
     */
    public void discard()
    {
        getUSServer().getBM().getBM(PlayerFuncUnlockBO.class).delAll("id", _m_dbId);
    }
}
