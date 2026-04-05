package NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.US_Service.Guild.GuildAddMarsBattleReport;
import AllRpcData.US_Service.Mars.*;
import Common.MarsEnum.EMarsExplorePVPLogType;
import Common.NpChatObj.ChatContent_GuildMarsMineAttackShare;
import Common.NpChatObj.NPCommon_ChatSystemPlayerContent;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_MarsExplorePVPLog;
import Common.ServerObj.ServerObj_MarsMine;
import Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackInt;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Mars.RefMarsExploreMine;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.ChatSys.ChatRoomApi;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState.ExploreTeamState_COLLECT;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UsMars.MineCore.UsMarsMineObj;
import NPUSServer.UsMars.UsMarsAction.UsMineAction.UsMarsMineAttackAction;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;
import RPC._ARpcCallBack;

public class MarsMineSystem 
{
	/**
	 * 获取指定火星矿数据
	 * @param _usServer
	 * @param _instanceId
	 * @param _callback
	 */
	public static void GetMarsMine(NPUserServer _usServer, long _instanceId, _ICallBackIntT<ServerObj_MarsMine> _callback)
	{
		int usId = CommonFunc.parseServerTypeIdFromInstanced(_instanceId);
		if(usId == _usServer.getServerTypeId())
		{
			UsMarsMineObj usMine = _usServer.getMarsMineCore().lookupData(_instanceId);
			if(null == usMine)
			{
				_callback.onRunOver(MarsErr.MARS_MINE_NOT_FOUND.getCode(), null);
			}
			else
			{
				_callback.onRunOver(0, usMine.toServerProto());
			}
		}
		else
		{
			//发送RPC向对应矿派遣队伍
			MarsMineInfoReq rpc = new MarsMineInfoReq();
			rpc.req().setMineInstanceId(_instanceId);

			_usServer.rpc2us().requestToRepeat(usId, rpc
					, new _ARpcCallBack<MarsMineInfoReq>() {
						@Override
						public void call_back(int _errCode, MarsMineInfoReq _rpc) {
							//直接调用回调
							_callback.onRunOver(_errCode, _rpc.retObj().getInfo());
						}
					}
					, 3
					, () -> {
						USLog.error(_usServer, "send rpc GetMarsMine mineInstanceId{} fail.", _instanceId);

						//调用回调
						_callback.onRunOver(CommErr.SYS_ERR.getCode(), null);
					});
		}
	}

	/**
	 * 获取指定火星矿数据
	 * @param _usServer
	 * @param _instanceId
	 * @param _callback
	 */
	public static void GetMarsHadAttackByOtherTag(NPUserServer _usServer, long _instanceId,long _guildId, _ICallBackIntT<Boolean> _callback)
	{
		int usId = CommonFunc.parseServerTypeIdFromInstanced(_instanceId);
		if(usId == _usServer.getServerTypeId())
		{
			UsMarsMineObj usMine = _usServer.getMarsMineCore().lookupData(_instanceId);
			if(null == usMine)
			{
				_callback.onRunOver(MarsErr.MARS_MINE_NOT_FOUND.getCode(), null);
			}
			else
			{
				_callback.onRunOver(0, usMine.hasOtherGuildAttacked(_guildId));
			}
		}
		else
		{
			//发送RPC向对应矿派遣队伍
			MarsMineHasOtherGuildAttacked rpc = new MarsMineHasOtherGuildAttacked();
			rpc.req().setGuildId(_guildId);
			rpc.req().setMineInstanceId(_instanceId);

			_usServer.rpc2us().requestToRepeat(usId, rpc
					, new _ARpcCallBack<MarsMineHasOtherGuildAttacked>() {
						@Override
						public void call_back(int _errCode, MarsMineHasOtherGuildAttacked _rpc) {
							//直接调用回调
							_callback.onRunOver(_errCode, _rpc.retObj().getHasOtherGuildAttacked());
						}
					}
					, 3
					, () -> {
						USLog.error(_usServer, "send rpc GetMarsMine mineInstanceId{} fail.", _instanceId);

						//调用回调
						_callback.onRunOver(CommErr.SYS_ERR.getCode(), null);
					});
		}
	}
	
	/**
	 * 创建一个新矿
	 * @param _usServer
	 * @param _mineRef
	 * @param _callback
	 */
	public static void CreateNewMine(NPUserServer _usServer, RefMarsExploreMine _mineRef, _ICallBackIntT<ServerObj_MarsMine> _callback)
	{
		//当前只处理本服
		UsMarsMineObj usMine = _usServer.getMarsMineCore().popNewMine(_mineRef);
		if(null == usMine)
		{
			_callback.onRunOver(MarsErr.MARS_MINE_NOT_FOUND.getCode(), null);
		}
		else
		{
			_callback.onRunOver(0, usMine.toServerProto());
		}
	}

	/**
	 * 刷出其他玩家的火星矿
	 * @param _usServer
	 * @param _mineRef
	 * @param _callback
	 */
	public static void RandOtherPlayerMine(NPUserServer _usServer, RefMarsExploreMine _mineRef, _ICallBackIntT<ServerObj_MarsMine> _callback)
	{
		//当前只处理本服
		UsMarsMineObj usMine = _usServer.getMarsMineCore().popExistMine(_mineRef);
		if(null == usMine)
		{
			_callback.onRunOver(MarsErr.MARS_MINE_NOT_FOUND.getCode(), null);
		}
		else
		{
			_callback.onRunOver(0, usMine.toServerProto());
		}
	}
	
	/**
	 * 前往占领火星矿
	 * @param _usServer
	 * @param _instanceId
	 * @param _isOtherTeamForward 是否有其他玩家前往，如果false且有其他玩家前往则报错MARS_MINE_OTHER_PLAYER_FORWARD
	 * @param _occupyPlayer
	 * @param _startCollectMs
	 * @param _collectSpeed
	 * @param _callback
	 */
	public static void GoToOccupyMarsMine(NPUserServer _usServer, long _instanceId, boolean _isOtherTeamForward
			, ServerObj_MarsTeam_OccupyMinePlayer _occupyPlayer, long _startCollectMs, long _collectSpeed, _ICallBackInt _callback)
	{
		int usId = CommonFunc.parseServerTypeIdFromInstanced(_instanceId);
		if(usId == _usServer.getServerTypeId())
		{
			Result result = _usServer.getMarsActionCore().getMineActionMgr().addAttackAction(_instanceId, _isOtherTeamForward, _occupyPlayer, _startCollectMs, _collectSpeed);
			if(result != Result.SUCC)
			{
				_callback.onRunOver(result.getCode());
				return;
			}

			_callback.onRunOver(0);
		}
		else
		{
			//发送RPC向对应矿派遣队伍
			MarsMineOccupyReq rpc = new MarsMineOccupyReq();
			rpc.req().setMineInstanceId(_instanceId);
			rpc.req().setIsOtherTeamForward(_isOtherTeamForward);
			rpc.req().setPlayerInfo(_occupyPlayer);
			rpc.req().setStartCollectMs(_startCollectMs);
			rpc.req().setCollectSpeed(_collectSpeed);

			//此处不重复发送,如无法送达则在超时中处理
			_usServer.rpc2us().requestTo(usId, rpc
					, new _ARpcCallBack<MarsMineOccupyReq>() {
						@Override
						public void call_back(int _errCode, MarsMineOccupyReq _rpc) {
							//直接调用回调
							_callback.onRunOver(_errCode);
						}
					});
		}
	}
	
	/**
	 * 离开火星矿资源
	 * @param _usServer
	 * @param _teamState
	 * @param _callback
	 */
	public static void LeaveMarsMine(NPUserServer _usServer, ExploreTeamState_COLLECT _teamState, _ICallBackInt _callback)
	{
		long instanceId = _teamState.getExtData().getMineInstanceId();
		int usId = CommonFunc.parseServerTypeIdFromInstanced(instanceId);
		if(usId == _usServer.getServerTypeId())
		{
			Result result = _usServer.getMarsMineCore().leaveMine(instanceId, _teamState.getTeam().getCid(), _teamState.getTeam().getTeamId());

			//调用回调
			_callback.onRunOver(result.getCode());
		}
		else
		{
			//发送RPC向对应矿派遣队伍
			MarsMineLeave rpc = new MarsMineLeave();
			rpc.req().setMineInstanceId(instanceId);
			rpc.req().setCid(_teamState.getTeam().getCid());
			rpc.req().setCid(_teamState.getTeam().getTeamId());

			//此处不重复发送,如无法送达则在超时中处理
			_usServer.rpc2us().requestTo(usId, rpc
					, new _ARpcCallBack<MarsMineLeave>() {
						@Override
						public void call_back(int _errCode, MarsMineLeave _rpc) {
							//直接调用回调
							_callback.onRunOver(_errCode);
						}
					});
		}
	}

    /**
     * 设置火星矿的剩余资源数量（GM命令）
     * <p>
     * 功能：修改指定火星矿的剩余资源数量
     * <p>
     * 处理逻辑：
     * 1. 根据instanceId解析出所属服务器ID
     * 2. 如果是本服矿，直接调用矿对象的gmSetRemainNum方法
     * 3. 如果是跨服矿，通过RPC调用目标服务器（待实现）
     * @param _usServer   当前UserServer实例
     * @param _instanceId 矿实例ID
     * @param _remainNum  新的剩余资源数量
     * @param _callback   回调接口，返回错误码（0表示成功）
     */
    public static void SetMineRemainNum(NPUserServer _usServer, long _instanceId, long _remainNum, _ICallBackInt _callback)
    {
        int usId = CommonFunc.parseServerTypeIdFromInstanced(_instanceId);
        if (usId == _usServer.getServerTypeId())
        {
            //本服矿：直接调用矿对象的设置方法
            UsMarsMineObj usMine = _usServer.getMarsMineCore().lookupData(_instanceId);
            if (null == usMine)
            {
                _callback.onRunOver(MarsErr.MARS_MINE_NOT_FOUND.getCode());
            } else
            {
                Result result = usMine.gmSetRemainNum(_remainNum);
                _callback.onRunOver(result.getCode());
            }
        } else
        {
            //跨服矿：需要通过RPC调用目标服务器（暂时返回参数错误，待实现跨服逻辑）
            _callback.onRunOver(CommErr.OBJ_ERR.getCode());
        }
    }

    /**
     * 发送日志数据
     * @param _usServer
     * @param _cid
     * @param _logType
     * @param _logData
     */
	public static void SendLog(NPUserServer _usServer, long _cid, EMarsExplorePVPLogType _logType, byte[] _logData)
	{
		int usId = CommonFunc.parseServerTypeIdFromCid(_cid);

        ServerObj_MarsExplorePVPLog obj = new ServerObj_MarsExplorePVPLog();
        obj.setLogType(_logType);
        obj.setLogData(_logData);
        obj.setCreatedAt(CommonFunc.getNowTimeMS());

        MarsMineAddPVPLog rpc = new MarsMineAddPVPLog();
        rpc.req().setCid(_cid);
        rpc.req().setLogObj(obj);

        _usServer.rpc2us().requestTo(usId, rpc, null);
	}

    /**
     * 处理 发送日志数据
     * @param _usServer
     * @param _cid
     * @param _logType
     * @param _logData
     */
	public static void DealLog(NPUserServer _usServer, long _cid, EMarsExplorePVPLogType _logType, byte[] _logData)
	{
		ServerObj_MarsExplorePVPLog obj = new ServerObj_MarsExplorePVPLog();
		obj.setLogType(_logType);
		obj.setLogData(_logData);
		obj.setCreatedAt(CommonFunc.getNowTimeMS());

        // 添加离线奖励记录
		OfflineRewardFunc.addPlayerOfflineReward(
                _usServer,
                _cid,
                EOfflineRewardEnum.MARS_EXPLORE_PVP_LOG,
                obj,
                null,
                null,
                NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));

        //目前只有矿挑战和防守日志需要发送给联盟，其他日志只记录离线奖励
        if (_logType == EMarsExplorePVPLogType.MINE_ATTACK || _logType == EMarsExplorePVPLogType.MINE_DEFEND)
        {
            // 统一通过缓存接口获取联盟信息
            PlayerCacheFunc.getData(_usServer, _cid, new HandlerTwo<Boolean, UserOfflineTmpDataInfo_PlayerCache>()
            {
                @Override
                public void handle(Boolean _isSucc, UserOfflineTmpDataInfo_PlayerCache _cacheInfo)
                {
                    if (!_isSucc || _cacheInfo == null)
                        return;

                    if (_cacheInfo.getPlayerCache() == null || _cacheInfo.getPlayerCache().getGuildInfo() == null)
                        return;

                    long guildId = _cacheInfo.getPlayerCache().getGuildInfo().getGuildId();
                    if (guildId <= 0)
                        return;

                    GuildAddMarsBattleReport rpc = new GuildAddMarsBattleReport();
                    rpc.req().setCid(_cid);
                    rpc.req().setGuildId(guildId);
                    rpc.req().setLogObj(obj);

                    int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(guildId);
                    _usServer.rpc2us().requestToRepeat(guildUsId, rpc, null, 3, () ->
                        USLog.error(_usServer, "MarsMineSystem.DealLog - guildBattleReport: send rpc fail, cid={}, guildId={}", _cid, guildId));
                }
            });
        }
	}

    /**
     * 发送聊天消息
     * @param _usServer
     * @param _mineInstanceId
     * @param _guildId
     * @param _cname
     * @param _attckInfo
     */
    public static void SendDefenceChatMessage(NPUserServer _usServer, long _mineInstanceId, long _guildId, String _cname, UsMarsMineAttackAction _attckInfo)
    {
        if(_guildId <= 0)
            return;

        ALSynTaskManager.getInstance().regTask(() -> DealDefenceChatMessage(_usServer, _mineInstanceId, _guildId, _cname, _attckInfo));
    }

    /**
     * 处理 发送聊天消息
     * @param _usServer
     * @param _mineInstanceId
     * @param _guildId
     * @param _cname
     * @param _attckInfo
     */
    public static void DealDefenceChatMessage(NPUserServer _usServer, long _mineInstanceId, long _guildId, String _cname, UsMarsMineAttackAction _attckInfo)
    {
        //发送用户展示信息，系统发送，空对象
        NPCommon_ChatSystemPlayerContent user = new NPCommon_ChatSystemPlayerContent();

        //发送聊天消息内容
        ChatContent_GuildMarsMineAttackShare content = new ChatContent_GuildMarsMineAttackShare();
        content.setAttackCname(_attckInfo.getPlayer().getCname());
        content.setAttackGuildSimpleName(_attckInfo.getPlayer().getGuildBName());
        content.setCname(_cname);
        content.setMineInstanceId(_mineInstanceId);

        ChatRoomApi.sendGuildRoomSysMsg(_usServer, _guildId, ENPChatMsgType.GUILD_MARS_MINE_OCCUPY, user.makePackage(), content.makePackage(),
                (_result)->
                {
                    if(!_result.isSucc())
                    {
                        USLog.error(_usServer, "SendDefenceChatMessage mineInstanceId:{} guildId:{} fail.", _mineInstanceId, _guildId);
                    }
                });
    }
}
