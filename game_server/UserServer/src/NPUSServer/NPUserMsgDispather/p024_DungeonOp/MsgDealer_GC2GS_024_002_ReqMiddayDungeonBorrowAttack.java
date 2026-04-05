package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import AllRpcData.US_Service.Guild.GuildGetDispatchHeroInfo;
import Common.DungeonObj.MiddayDungeon_SettleInfo;
import GC2GS.p024_DungeonOp.GC2GS_024_002_ReqMiddayDungeonBorrowAttack;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;
import NPUSServer.USLog;
import RPC._ARpcCallBack;

public class MsgDealer_GC2GS_024_002_ReqMiddayDungeonBorrowAttack extends NPUserMsgDealer<GC2GS_024_002_ReqMiddayDungeonBorrowAttack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_002_ReqMiddayDungeonBorrowAttack _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MIDDAY_DUNGEON_BORROW_ATTACK);

        //请求其他玩家派遣英雄信息
        if(_commiter.getUserData().getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //发送RPC请求信息
        GuildGetDispatchHeroInfo rpc = new GuildGetDispatchHeroInfo();
        rpc.req().setGuildId(_commiter.getUserData().getGuildComponent().getGuildId());
        rpc.req().setCid(_msg.getCid());
        rpc.req().setHeroId(_msg.getHeroId());

        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_commiter.getUserData().getGuildComponent().getGuildId());
        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc
                , new _ARpcCallBack<GuildGetDispatchHeroInfo>() {
                    @Override
                    public void call_back(int _errCode, GuildGetDispatchHeroInfo _rpc)
                    {
                        if(_errCode != 0)
                        {
                            _commiter.commitFailRes(_errCode);
                            return ;
                        }

                        ResultOne<MiddayDungeon_SettleInfo> attackResult = userData.getMiddayDungeonComponent().getDungeonInfo().borrowAttack(_msg.getCid(),_msg.getHeroId(), context);
                        if (!attackResult.isSucc())
                        {
                            _commiter.commitFailRes(attackResult.getCode());
                            return;
                        }

                        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_002_RetMiddayDungeonBorrowAttack(attackResult.getData()));
                    }
                }
                , 3
                , () -> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc GuildGetDispatchHeroInfo fail.", _commiter.getUserData().getCid(), _commiter.getUserData().getGuildComponent().getGuildId());
                });
    }
}
