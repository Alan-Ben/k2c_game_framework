package NPUSServer.NPUserMsgDispather.p037_GuildDungeonOp;

import AllRpcData.US_Service.Guild.GuildPlayerRequestDungeonLvl;
import Common.GuildDungeonEnum.EGuildDungeon_StartType;
import GC2GS.p037_GuildDungeonOp.GC2GS_037_003_ReqStartDungeon;
import GS2GC.p037_GuildDungeonOp.GS2GC_037_003_RetStartDungeon;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeon;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USLOGDB.OptBo.Opt037003GuildDungeonStartBO;

public class MsgDealer_GC2GS_037_003_ReqStartDungeon extends NPUserMsgDealer<GC2GS_037_003_ReqStartDungeon>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_037_003_ReqStartDungeon _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //rpc获取副本等级
        GuildPlayerRequestDungeonLvl rpc = new GuildPlayerRequestDungeonLvl();
        rpc.req().setCid(userData.getCid());
        rpc.req().setGuildId(userData.getGuildComponent().getGuildId());
        rpc.req().setDungeonId(_msg.getDungeonId());

        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(userData.getGuildComponent().getGuildId());

        //rpc获取副本等级
        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc
                , new _ARpcCallBack<GuildPlayerRequestDungeonLvl>() {
                    @Override
                    public void call_back(int _errCode, GuildPlayerRequestDungeonLvl _rpc) {
                        RefGuildDungeon dungeonRef = RefGuildDungeon.getMgr().get(_msg.getDungeonId());
                        if(null == dungeonRef)
                        {
                            _commiter.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_FOUND.getCode());
                            return ;
                        }

                        RefGuildDungeonLvl lvlRef = dungeonRef.getLevelMapMgr().getLevelData(_rpc.retObj().getLvl());
                        if(null == lvlRef)
                        {
                            _commiter.commitFailRes(GuildErr.GUILD_DUNGEON_NOT_FOUND.getCode());
                            return ;
                        }

                        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_START);

                        //判断开启方式是否消耗开启，是则准备消耗玩家信息
                        if(_msg.getStartType() == EGuildDungeon_StartType.COMMON_ITEM)
                        {
                            //扣除玩家物品
                            if(!userData.spendItem(lvlRef.star_cost_item, context))
                            {
                                _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                                return ;
                            }
                        }

                        //此时转发消息开启
                        //转化为统一跨服消息进行处理
                        getUSServer().dealGuildMsgByRedirectCommiter(
                                new _ATGuildUserMsgRedirectCommiter<GS2GC_037_003_RetStartDungeon>(_commiter) {
                                    @Override
                                    protected GS2GC_037_003_RetStartDungeon _createNewTmpObj() {
                                        return new GS2GC_037_003_RetStartDungeon();
                                    }

                                    @Override
                                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GS2GC_037_003_RetStartDungeon _retMsg) {
                                        //日志数据
                                        Opt037003GuildDungeonStartBO optBo = new Opt037003GuildDungeonStartBO();
                                        optBo.setGuildId(getUSServer().getBM(), userData.getGuildComponent().getGuildId());
                                        optBo.setDungeonId(getUSServer().getBM(), _msg.getDungeonId());
                                        optBo.setStartType(getUSServer().getBM(), _msg.getStartType().ordinal());
                                        userData.logEvent(optBo, context);

                                        //返回操作结果
                                        _commiter.commitSucRes(_retMsg);
                                    }
                                },
                                _commiter.getUserData().getCid(),
                                _commiter.getUserData().getGuildComponent().getGuildId(),
                                _msg,
                                null,
                                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                                    @Override
                                    public void onRunOver(int _errCode, _ANPUSUserBasicMsgItem _commiter) {
                                        //如果是消耗方式，需要返还用户消耗
                                        if(_msg.getStartType() == EGuildDungeon_StartType.COMMON_ITEM)
                                        {
                                            //返还玩家物品
                                            userData.gainItem(lvlRef.star_cost_item, context);
                                        }

                                        _commiter.commitFailRes(_errCode);
                                    }
                                }
                        );
                    }
                }
                , 3
                , () -> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc memberRmv fail.", userData.getCid(), userData.getGuildComponent().getGuildId());
                });
    }
}
