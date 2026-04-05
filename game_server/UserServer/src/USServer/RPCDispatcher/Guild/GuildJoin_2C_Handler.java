package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildJoin_2C;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerThree;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffInfo;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 增加玩家的自动互助
 */
public class GuildJoin_2C_Handler extends _ATBasicUSRpc_Handler<GuildJoin_2C>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildJoin_2C _rpc)
	{
    	_usServer.getLoaderMgr().safeCall(()->
    	{
			PlayerCacheFunc.getData(_usServer, _rpc.req().getCid(), new HandlerThree<Boolean, NPUSUserData, UserOfflineTmpDataInfo_PlayerCache>() {
				@Override
				public void handle(Boolean _isExist, NPUSUserData _userData, UserOfflineTmpDataInfo_PlayerCache _cacheInfo) {

					//判断是否请求申请加入的，如果是则需要先校对申请是否存在
					if(_rpc.req().getIsRequestJoin())
					{
						//清除玩家所有申请信息，如果失败则返回失败
						if(!_usServer.getGuildMgr().getJoinRequestMgr().checkAndRmvJoinRequest(_rpc.req().getCid(), _rpc.req().getGuildId())) {
							_rpc.commitFail(GuildErr.JOIN_REQUEST_NOT_FOUND.getCode());
							return ;
						}
					}
					else if(null == _userData)
					{
						//如果是非申请加入的，玩家数据又不存在，直接报数据错误
						_rpc.commitFail(CommErr.OBJ_ERR.getCode());
						return ;
					}
					else
					{
						//如果非申请，或者玩家有数据说明操作合法，此时直接清除所有加入请求
						_usServer.getGuildMgr().getJoinRequestMgr().checkAndRmvJoinRequest(_rpc.req().getCid(), 0);
					}

					//进入本流程说明加入必须处理，如果玩家数据不存在则通过离线处理
					if(null != _userData)
					{
						//玩家有数据对象则进行相关处理
						_userData.safeCall(() ->
						{
							//修改玩家数据
							Result result = _userData.getGuildComponent().joinGuild(_rpc.req().getGuildId(), _rpc.req().getGuildName(), _rpc.req().getSimpleName(), _rpc.req().getBuildingAddPerArr());
							if (!result.isSucc()) {
								_rpc.commitFail(result.getCode());
								return;
							}

							//设置国力
							_rpc.retObj().setMaxEarning(_userData.getPlayerComponent().getParamV(ENPPlayerParam.EARNINGS_MAX_RECORD));
							//检查火星互助，这里如果用户数据在直接处理，如果用户数据不在，在guildcomponent初始化的时候有额外做判断处理
							PlayerBuffInfo guildMarsAutoHelpBuff = _userData.getBuffComponent().lookupBuff(RefGeneral.Ref().guild_mars_help_auto_deal_buff_id);
							if (null != guildMarsAutoHelpBuff) {
								_rpc.retObj().setMarsAutoHelpEndTimeMS(guildMarsAutoHelpBuff.getEndMs());
							} else {
								_rpc.retObj().setMarsAutoHelpEndTimeMS(0);
							}

							//返回
							_rpc.commit();
						});
					}
					else
					{
						//离线组件通知玩家加入公会
						OfflineRewardFunc.onPlayerJoinGuild(_usServer, _rpc.req().getCid(),
								_rpc.req().getGuildId(), _rpc.req().getGuildName(), _rpc.req().getSimpleName(), _rpc.req().getBuildingAddPerArr());

						if(!_isExist)
						{
							_rpc.commit();
							return ;
						}

						//设置国力
						_rpc.retObj().setMaxEarning(_cacheInfo.getPlayerCache().getMaxEarnings());

						_rpc.commit();
					}
				}
			});

    	});
	}
}
