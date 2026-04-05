package NPUSServer.Guild.JoinRequest;

import AllRpcData.US_Service.Guild.GuildAddPlayerRequest_2C;
import AllRpcData.US_Service.Guild.GuildRmvGuildRequest;
import AllRpcData.US_Service.Guild.GuildRmvPlayerRequest_2C;
import Common.GuildEnum.EGuildPositionType;
import Common.GuildObj.Guild_JoinRequestInfo;
import GS2GC.p032_GuildOp.GS2GC_032_057_OnSelfRequestJoinGuildListChg;
import GS2GC.p032_GuildOp.GS2GC_032_063_OnGuildJoinRequestAdd;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.GuildMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.USLog;
import USDB.Bo.GuildJoinRequestBO;
import USDB.Bo.GuildPlayerRequestBO;

import java.util.*;

public class GuildJoinRequestMgr
{
    private GuildMgr _m_guildMgr;

    //公会申请映射
    private Map<Long, List<GuildJoinRequestInfo>> _m_guildRequestMap;
    //玩家申请映射
    private Map<Long, List<GuildPlayerJoinRequestInfo>> _m_playerRequestMap;

    public GuildJoinRequestMgr(GuildMgr _guildMgr)
    {
        _m_guildMgr = _guildMgr;

        _m_guildRequestMap = new HashMap<>();
        _m_playerRequestMap = new HashMap<>();
    }

    private void _lock()
    {
        _m_guildMgr.getMemberMutex().lock();
    }

    private void _unlock()
    {
        _m_guildMgr.getMemberMutex().unlock();
    }

    /**
     * 初始化申请
     * @param _bo
     */
    public void initGuildJoinRequest(GuildJoinRequestBO _bo)
    {
        GuildJoinRequestInfo joinRequestInfo = new GuildJoinRequestInfo(_bo);
        _m_guildRequestMap.computeIfAbsent(joinRequestInfo.getGuildId(), k -> new ArrayList<>()).add(joinRequestInfo);
    }
    public void initPlayerJoinRequest(GuildPlayerRequestBO _bo)
    {
        GuildPlayerJoinRequestInfo joinRequestInfo = new GuildPlayerJoinRequestInfo(_bo);
        _m_playerRequestMap.computeIfAbsent(joinRequestInfo.getCid(), k -> new ArrayList<>()).add(joinRequestInfo);
    }

    /**
     * 判断玩家是否可以发起加入申请
     * @param _cid
     * @return
     */
    public boolean canPlayerAddJoinRequestCount(long _cid)
    {
        _lock();
        try {
            //检查玩家申请数量是否已经达到上限
            List<GuildPlayerJoinRequestInfo> playerJoinRequestList = _m_playerRequestMap.get(_cid);
            if (playerJoinRequestList != null && playerJoinRequestList.size() >= RefGeneral.Ref().guild_join_request_limit_num)
                return false;

            return true;
        }
        finally {
            _unlock();
        }
    }

    /**
     * 添加申请
     */
    public Result addLocalGuildRequest(GuildInfo _guildInfo, long _cid)
    {
        _lock();
        try
        {
            //判断联盟是否满员
            if (_guildInfo.getMemberMgr().getCanJoinNum() <= 0)
                return GuildErr.GUILD_FULL;

            //构造数据
            BM bmObj = _m_guildMgr.getServer().getBM();
            GuildJoinRequestBO bo = new GuildJoinRequestBO();
            bo.setGuildId(bmObj, _guildInfo.getGuildId());
            bo.setCid(bmObj, _cid);
            bo.setRequestTimeMs(bmObj, CommonFunc.getNowTimeMS());
            bo.insert(bmObj);

            GuildJoinRequestInfo joinRequestInfo = new GuildJoinRequestInfo(bo);

            //加入公会申请列表
            _m_guildRequestMap.computeIfAbsent(_guildInfo.getGuildId(), k -> new ArrayList<>()).add(joinRequestInfo);
            //加入玩家申请列表
            addPlayerRequest(_guildInfo.getGuildId(), _cid);

            //通知工会列表变更
            _guildInfo.getMemberMgr().broadcastMsg(new GS2GC_032_063_OnGuildJoinRequestAdd(joinRequestInfo.makeProto()));

            return Result.SUCC;

        } finally
        {
            _unlock();
        }
    }
    public void addPlayerRequest(long _guildId, long _cid)
    {
        GuildAddPlayerRequest_2C rpc = new GuildAddPlayerRequest_2C();
        rpc.req().setCid(_cid);
        rpc.req().setGuildId(_guildId);

        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);

        _m_guildMgr.getServer().rpc2us().requestToRepeat(usId, rpc
                , null
                , 3
                , () -> {
                    USLog.error(_m_guildMgr.getServer(), "player:{} guild:{} send rpc addPlayerRequest fail.", _cid, _guildId);
                });
    }
    public void rmvPlayerRequest(long _guildId, long _cid)
    {
        GuildRmvPlayerRequest_2C rpc = new GuildRmvPlayerRequest_2C();
        rpc.req().setCid(_cid);
        rpc.req().setGuildId(_guildId);

        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);

        _m_guildMgr.getServer().rpc2us().requestToRepeat(usId, rpc
                , null
                , 3
                , () -> {
                    USLog.error(_m_guildMgr.getServer(), "player:{} guild:{} send rpc rmvPlayerRequest fail.", _cid, _guildId);
                });
    }
    public void rmvGuildRequest(long _guildId, long _cid)
    {
        GuildRmvGuildRequest rpc = new GuildRmvGuildRequest();
        rpc.req().setCid(_cid);
        rpc.req().setGuildId(_guildId);

        int usId = CommonFunc.parseServerTypeIdFromInstanced(_guildId);

        _m_guildMgr.getServer().rpc2us().requestToRepeat(usId, rpc
                , null
                , 3
                , () -> {
                    USLog.error(_m_guildMgr.getServer(), "player:{} guild:{} send rpc rmvGuildRequest fail.", _cid, _guildId);
                });
    }

    public Result rpcAddLocalPlayerRequest(long _guildId, long _cid)
    {
        _lock();
        try
        {
            //检查玩家申请数量是否已经达到上限
            List<GuildPlayerJoinRequestInfo> playerJoinRequestList = _m_playerRequestMap.get(_cid);

            //判断是否已经申请过了
            if (playerJoinRequestList != null)
            {
                for (GuildPlayerJoinRequestInfo joinRequestInfo : playerJoinRequestList)
                {
                    if (joinRequestInfo.getGuildId() == _guildId)
                        return GuildErr.JOIN_REQUEST_ALREADY_EXIST;
                }
            }

            //构造数据
            BM bmObj = _m_guildMgr.getServer().getBM();
            GuildPlayerRequestBO bo = new GuildPlayerRequestBO();
            bo.setGuildId(bmObj, _guildId);
            bo.setCid(bmObj, _cid);
            bo.setRequestTimeMs(bmObj, CommonFunc.getNowTimeMS());
            bo.insert(bmObj);

            GuildPlayerJoinRequestInfo joinRequestInfo = new GuildPlayerJoinRequestInfo(bo);

            //加入玩家申请列表
            playerJoinRequestList = _m_playerRequestMap.computeIfAbsent(_cid, k -> new ArrayList<>());
            playerJoinRequestList.add(joinRequestInfo);

            ArrayList<Long> guildIdList = new ArrayList<>();
            //构造玩家已加入的工会ID列表
            for (GuildPlayerJoinRequestInfo guildJoinRequestInfo : playerJoinRequestList)
            {
                guildIdList.add(guildJoinRequestInfo.getGuildId());
            }

            //通知玩家列表变更
            _m_guildMgr.getServer().sendMsgToGC(_cid, new GS2GC_032_057_OnSelfRequestJoinGuildListChg(guildIdList));

            return Result.SUCC;

        } finally
        {
            _unlock();
        }
    }
    public void rpcRmvLocalPlayerRequest(long _guildId, long _cid)
    {
        _lock();
        try
        {
            //检查玩家申请数量是否已经达到上限
            List<GuildPlayerJoinRequestInfo> playerJoinRequestList = _m_playerRequestMap.get(_cid);

            //判断是否已经申请过了
            if (playerJoinRequestList == null)
                return ;

            //查询数据
            for(GuildPlayerJoinRequestInfo joinRequestInfo : playerJoinRequestList)
            {
                if(joinRequestInfo.getGuildId() == _guildId)
                {
                    //删除数据库数据
                    joinRequestInfo.discard(_m_guildMgr.getServer().getBM());

                    //从玩家申请列表中移除
                    playerJoinRequestList.remove(joinRequestInfo);

                    break;
                }
            }

            //如果队列为空，从数据集中移除
            if(playerJoinRequestList.isEmpty())
                _m_playerRequestMap.remove(_cid);

            ArrayList<Long> guildIdList = new ArrayList<>();
            //构造玩家已加入的工会ID列表
            for (GuildPlayerJoinRequestInfo guildJoinRequestInfo : playerJoinRequestList)
            {
                guildIdList.add(guildJoinRequestInfo.getGuildId());
            }

            //通知玩家列表变更
            _m_guildMgr.getServer().sendMsgToGC(_cid, new GS2GC_032_057_OnSelfRequestJoinGuildListChg(guildIdList));

        } finally
        {
            _unlock();
        }
    }

    public void rpcRmvLocalGuildRequest(long _guildId, long _cid)
    {
        _lock();
        try
        {
            //检查玩家申请数量是否已经达到上限
            List<GuildJoinRequestInfo> guildJoinRequestList = _m_guildRequestMap.get(_guildId);

            //判断是否已经申请过了
            if (guildJoinRequestList == null)
                return ;

            //查询数据
            long rmvDbId = 0;
            for(GuildJoinRequestInfo joinRequestInfo : guildJoinRequestList)
            {
                if(joinRequestInfo.getCid() == _cid)
                {
                    rmvDbId = joinRequestInfo.getDbId();
                    //删除数据库数据
                    joinRequestInfo.discard(_m_guildMgr.getServer().getBM());

                    //从玩家申请列表中移除
                    guildJoinRequestList.remove(joinRequestInfo);

                    break;
                }
            }

            //如果队列为空，从数据集中移除
            if(guildJoinRequestList.isEmpty())
                _m_guildRequestMap.remove(_guildId);

            //通知公会成员申请列表变更
            if(rmvDbId != 0) {
                GuildInfo guildInfo = _m_guildMgr.lookupGuild(_guildId);
                if (guildInfo != null)
                    guildInfo.getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_064_OnGuildJoinRequestRemove(rmvDbId));
            }

        } finally
        {
            _unlock();
        }
    }

    /**
     * 处理申请
     * @param _guildInfo
     * @param _dbId
     * @param _bAccept
     * @return
     */
    public Result dealJoinRequest(GuildInfo _guildInfo, long _dbId, boolean _bAccept)
    {
        _lock();
        try
        {
            //检查玩家申请数量是否已经达到上限
            List<GuildJoinRequestInfo> guildJoinRequestList = _m_guildRequestMap.get(_guildInfo.getGuildId());

            //判断是否已经申请过了
            if (guildJoinRequestList == null)
                return GuildErr.JOIN_REQUEST_NOT_FOUND;

            //查询数据
            GuildJoinRequestInfo requestInfo = null;
            for(GuildJoinRequestInfo joinRequestInfo : guildJoinRequestList)
            {
                if(joinRequestInfo.getDbId() == _dbId)
                {
                    requestInfo = joinRequestInfo;
                    break;
                }
            }

            if(null == requestInfo)
                return GuildErr.JOIN_REQUEST_NOT_FOUND;

            //处理申请
            Result dealResult = _dealJoinRequest(_guildInfo, requestInfo, _bAccept);
            //通知工会列表变更
            _guildInfo.getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_064_OnGuildJoinRequestRemove(requestInfo.getDbId()));

            return dealResult;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 处理申请
     * @param _guildInfo
     * @param _requestInfo
     * @param _bAccept
     * @return
     */
    private Result _dealJoinRequest(GuildInfo _guildInfo, GuildJoinRequestInfo _requestInfo, boolean _bAccept)
    {
        _lock();
        try
        {
            //移除公会申请列表，调用的地方有发送消息这里不做联盟内消息处理
            List<GuildJoinRequestInfo> guildRequestList = _m_guildRequestMap.get(_requestInfo.getGuildId());
            if (guildRequestList != null)
                guildRequestList.remove(_requestInfo);

            _requestInfo.discard(_m_guildMgr.getServer().getBM());

            //删除玩家部分请求
            rmvPlayerRequest(_guildInfo.getGuildId(), _requestInfo.getCid());

            //如果是同意操作需要处理后续的加入流程
            if (_bAccept)
            {
                return _m_guildMgr.joinGuild(_guildInfo, _requestInfo.getCid(), EGuildPositionType.MEMBER, false, true);
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 一键处理所有的加入申请
     * @param _guildInfo
     * @param _isAgree
     * @return
     */
    public Result aKeyDealJoinRequest(GuildInfo _guildInfo, boolean _isAgree)
    {
        _lock();
        try
        {
            //移除公会申请列表
            List<GuildJoinRequestInfo> guildRequestList = _m_guildRequestMap.get(_guildInfo.getGuildId());
            if (guildRequestList == null || guildRequestList.isEmpty())
                return GuildErr.NO_JOIN_REQUESTS_TO_PROCESS;

            ArrayList<GuildJoinRequestInfo> copyList = new ArrayList<>(guildRequestList);
            if (_isAgree)
            {
                //按照申请时间排序 倒序排序
                copyList.sort(Comparator.comparingLong(GuildJoinRequestInfo::getRequestTimeMs));
            }

            List<Long> dealRequestIdList = new ArrayList<>();
            //遍历处理请求，直到达到上限
            for (GuildJoinRequestInfo joinRequestInfo : copyList)
            {
                //如果是同意操作并且工会人数已满则不再处理
                if (_isAgree && _guildInfo.getMemberMgr().getCanJoinNum() <= 0)
                    break;

                _dealJoinRequest(_guildInfo, joinRequestInfo, _isAgree);

                dealRequestIdList.add(joinRequestInfo.getDbId());
            }

            //通知工会列表变更
            _guildInfo.getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_064_OnGuildJoinRequestRemove(dealRequestIdList));

            return Result.SUCC;

        } finally
        {
            _unlock();
        }
    }

    /**
     * 移除玩家相关的申请
     * @param _cid 玩家ID
     * @param _guildId 校对的公会Id，只有在校对成功的情况下清空。如果为0表示需要清除所有数据
     */
    public boolean checkAndRmvJoinRequest(long _cid, long _guildId)
    {
        List<GuildPlayerJoinRequestInfo> playerRequestList;
        _lock();
        try
        {
            //如果公会Id有效，说明需要校验，否则视作直接清除
            if(_guildId > 0) {
                playerRequestList = _m_playerRequestMap.get(_cid);
                if (null == playerRequestList)
                    return false;

                //判断是否有对应公会申请
                boolean requestEnable = false;
                for (GuildPlayerJoinRequestInfo joinRequestInfo : playerRequestList) {
                    if (joinRequestInfo.getGuildId() == _guildId) {
                        requestEnable = true;
                        break;
                    }
                }

                //如果不存在直接返回失败
                if (!requestEnable)
                    return false;
            }

            //此时清除所有申请
            playerRequestList = _m_playerRequestMap.remove(_cid);
            if (playerRequestList != null)
            {
                for (GuildPlayerJoinRequestInfo joinRequestInfo : playerRequestList)
                {
                    //移除公会申请,此处通过RPC处理
                    rmvGuildRequest(joinRequestInfo.getGuildId(), _cid);

                    joinRequestInfo.discard(_m_guildMgr.getServer().getBM());
                }
            }
        } finally
        {
            _unlock();
        }

        //通知玩家列表变更
        NPUSUserData userData = _m_guildMgr.getServer().getUsUserMgr().lookupCacheUserData(_cid);
        if (userData != null)
            userData.sendMsgToGC(new GS2GC_032_057_OnSelfRequestJoinGuildListChg(new ArrayList<>()));

        return true;
    }


    /**
     * 移除对应联盟中的玩家相关的申请
     * @param _cid 玩家ID
     */
    public void rmvLocalGuildCidRequest(GuildInfo _guildInfo, long _cid)
    {
        if(null == _guildInfo)
            return ;

        //从公会申请列表中移除
        List<GuildJoinRequestInfo> guildRequestList = _m_guildRequestMap.get(_guildInfo.getGuildId());
        if(null == guildRequestList)
            return ;

        for(GuildJoinRequestInfo joinRequestInfo : guildRequestList)
        {
            if(joinRequestInfo.getCid() == _cid)
            {
                guildRequestList.remove(joinRequestInfo);
                //移除数据库数据
                joinRequestInfo.discard(_m_guildMgr.getServer().getBM());

                //广播消息
                _guildInfo.getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_064_OnGuildJoinRequestRemove(joinRequestInfo.getDbId()));

                break;
             }
        }
    }

    /**
     * 撤回加入申请
     * @param _userdata
     * @param _guildId
     * @return
     */
    public Result cancelJoinRequest(NPUSUserData _userdata, long _guildId)
    {
        ArrayList<Long> guildIdList = new ArrayList<>();

        _lock();
        try
        {
            //查询对应的请求
            List<GuildPlayerJoinRequestInfo> joinRequestInfoList = _m_playerRequestMap.get(_userdata.getCid());
            if (joinRequestInfoList == null || joinRequestInfoList.isEmpty())
                return GuildErr.JOIN_REQUEST_NOT_FOUND;

            GuildPlayerJoinRequestInfo needDelItem = null;
            for (int i = joinRequestInfoList.size() - 1; i >= 0; i--)
            {
                GuildPlayerJoinRequestInfo joinRequestInfo = joinRequestInfoList.get(i);
                //找到并做删除处理
                if (joinRequestInfo.getGuildId() == _guildId)
                {
                    needDelItem = joinRequestInfo;

                    //从公会申请列表中移除
                    rmvGuildRequest(_guildId, _userdata.getCid());

                    //删除数据
                    joinRequestInfo.discard(_m_guildMgr.getServer().getBM());

                    joinRequestInfoList.remove(i);
                }
                else
                {
                    guildIdList.add(joinRequestInfo.getGuildId());
                }
            }

            if (needDelItem == null)
                return GuildErr.JOIN_REQUEST_NOT_FOUND;

        } finally
        {
            _unlock();
        }

        //告知玩家申请列表变更
        _userdata.sendMsgToGC(new GS2GC_032_057_OnSelfRequestJoinGuildListChg(guildIdList));

        return Result.SUCC;
    }

    /**
     * 获取公会申请列表
     * @param _guildId
     * @return
     */
    public List<Guild_JoinRequestInfo> makeProtoGuildJoinRequestList(long _guildId)
    {
        _lock();
        try
        {
            List<Guild_JoinRequestInfo> list = new ArrayList<>();
            List<GuildJoinRequestInfo> joinRequestInfoList = _m_guildRequestMap.get(_guildId);
            if (joinRequestInfoList != null)
            {
                for (GuildJoinRequestInfo joinRequestInfo : joinRequestInfoList)
                {
                    list.add(joinRequestInfo.makeProto());
                }
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取玩家申请的公会列表
     * @param _cid
     * @return
     */
    public ArrayList<Long> makePlayerRequestGuildList(long _cid)
    {
        _lock();
        try
        {
            ArrayList<Long> list = new ArrayList<>();
            List<GuildPlayerJoinRequestInfo> joinRequestInfoList = _m_playerRequestMap.get(_cid);
            if (joinRequestInfoList != null)
            {
                for (GuildPlayerJoinRequestInfo joinRequestInfo : joinRequestInfoList)
                {
                    list.add(joinRequestInfo.getGuildId());
                }
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 根据工会ID移除所有的申请（工会解散时调用）
     * @param _guildId
     */
    public void removeJoinRequestByGuildId(long _guildId)
    {
        // 相关的申请数据id列表
        List<Long> relativeDbIdList = new ArrayList<>();
        List<GuildJoinRequestInfo> guildRequestList;

        _lock();
        try
        {
            guildRequestList = _m_guildRequestMap.remove(_guildId);
            if (guildRequestList != null)
            {
                for (GuildJoinRequestInfo joinRequestInfo : guildRequestList)
                {
                    //移除玩家发起部分的申请信息
                    rmvPlayerRequest(joinRequestInfo.getGuildId(), joinRequestInfo.getCid());

                    relativeDbIdList.add(joinRequestInfo.getDbId());
                }
            }


        } finally
        {
            _unlock();
        }

        //删除数据库数据
        if (!relativeDbIdList.isEmpty())
            _m_guildMgr.getServer().getBM().getBM(GuildJoinRequestBO.class).delAllInList("id", relativeDbIdList);
    }
}
