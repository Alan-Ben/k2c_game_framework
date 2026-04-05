package NPUSServer.Guild.Event;

import Common.GuildEnum.EGuildEventType;
import Common.GuildEnum.EGuildImpeachLeaderEventState;
import Common.GuildObj.GuildEvent_ImpeachLeader;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import USDB.Bo.GuildEventBO;

import java.nio.ByteBuffer;

public class GuildEventDealer_ImpeachLeader extends _AGuildEvent<Common.GuildObj.GuildEvent_ImpeachLeader>
{
    public GuildEventDealer_ImpeachLeader(GuildEventMgr _mgr, GuildEventBO _bo)
    {
        super(_mgr, _bo);
    }

    @Override
    public EGuildEventType getEventType()
    {
        return EGuildEventType.IMPEACH_LEADER;
    }

    @Override
    protected GuildEvent_ImpeachLeader _createData(byte[] _rawData)
    {
        GuildEvent_ImpeachLeader data = new GuildEvent_ImpeachLeader();
        if (_rawData != null)
            data.readPackage(ByteBuffer.wrap(_rawData));
        return data;
    }

    /**
     * 是否还可用
     */
    @Override
    public boolean isAvailable()
    {
        _lock();
        try
        {
            //判断盟主是否已经变更
            GuildMemberInfo leader = getMgr().getGuildInfo().getMemberMgr().getLeader();
            if (leader == null || leader.getCid() != getData().getLeaderCid())
                return false;

            //如果还在可弹劾状态, 则需要判断是否已经超过离线时间
            if (getData().getState() == EGuildImpeachLeaderEventState.CAN_IMPEACH)
                return leader.getOfflineTimeMs() > (long) RefGeneral.Ref().guild_leader_impeach_offline_beyond_hours * 3600 * 1000;

            //判断是否已经超过有效期
            if ((getData().getRequestTimeMs() + RefGeneral.Ref().guild_leader_impeach_message_available_within_hours * 3600 * 1000L) < CommonFunc.getNowTimeMS())
                return false;

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否事件完结
     * @return
     */
    @Override
    public boolean isDone()
    {
        _lock();
        try
        {
            //判断是否已经有超过一半的人同意
            return getData().getAgreeMemberCidList().size() >= Math.round(getMgr().getGuildInfo().getMemberMgr().getMemberCount() * RefGeneral.Ref().guild_impeach_succ_need_percent / 100.0d);
        } finally
        {
            _unlock();
        }
    }

    @Override
    public void doneAction()
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_LEADER_PASSIVE_TRANSFER_IMPEACH);
        //弹劾成功
        getMgr().getGuildInfo().getMemberMgr().passiveTransLeader(getData().getLeaderCid(), context);
    }

    /**
     * 判断事件是否还可以操作
     */
    public boolean canOperate()
    {
        return getData().getState() == EGuildImpeachLeaderEventState.CAN_IMPEACH
                || getData().getState() == EGuildImpeachLeaderEventState.IN_IMPEACH;
    }

    /**
     * 投票
     * @param _cid
     * @return
     */
    public Result vote(long _cid)
    {
        _lock();
        try
        {
            //判断事件是否还可以操作
            if (!canOperate())
                return GuildErr.IMPEACH_EVENT_CANT_OPERATE;

            //如果是可以弹劾状态，那么切换到弹劾中
            if (getData().getState() == EGuildImpeachLeaderEventState.CAN_IMPEACH)
            {
                getData().setState(EGuildImpeachLeaderEventState.IN_IMPEACH);
                getData().setRequestTimeMs(CommonFunc.getNowTimeMS());
            }

            //判断是否已经投过票
            if (getData().getAgreeMemberCidList().contains(_cid))
                return GuildErr.IMPEACH_EVENT_HAS_VOTE;

            //投票
            getData().addAgreeMemberCidList(_cid);

            //保存数据
            save();

        } finally
        {
            _unlock();
        }

        //通知客户端
        getMgr().getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_060_OnGuildEventChg(makeProto()));

        return Result.SUCC;
    }
}
