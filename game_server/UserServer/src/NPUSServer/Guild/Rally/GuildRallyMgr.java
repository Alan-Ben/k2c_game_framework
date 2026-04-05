package NPUSServer.Guild.Rally;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.Common_Long;
import Common.GuildEnum.EGuildRallyType;
import Common.MarsObj.MarsBattleV2_MemberInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildRallyErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import USDB.Bo.GuildRallyMainBO;
import USDB.Bo.GuildRallyMemberBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 联盟集结管理器（联盟级）
 *
 * 主要职责：
 * 1. 管理当前联盟下的所有集结对象
 * 2. 负责集结主表/成员表初始化装载（s_init阶段允许查库）
 * 3. 负责加入流程与UsMarsAction行为调度、到达完成与失败补偿
 *
 * 约束说明：
 * - 运行期逻辑（非s_init）只允许通过rallyId命中内存RallyInfo处理，不做数据库查询
 */
public class GuildRallyMgr {
    // 默认集结持续时长：10分钟
    private static final long RALLY_EXPIRE_MS_DEFAULT = 10 * 60 * 1000L;
    // 默认最大成员数（后续接配表）
    private static final int RALLY_MAX_MEMBER_DEFAULT = 5;
    // 加入集结行军默认触发时长（毫秒）
    private static final long JOIN_RALLY_ACTION_DELAY_MS = 3000L;

    // 所属联盟对象
    private GuildInfo _m_guildInfo;
    // 联盟内集结列表（按约束使用List管理）
    private List<GuildRallyInfo<?>> _m_rallyInfoList;
    // tick待处理列表（复用容器，避免频繁创建与回收）
    private List<GuildRallyInfo<?>> _m_tickTmpNeedDealList;
    // 运行期互斥锁（仅用于保护RallyInfo队列本身）
    private MutexAtom _m_mutexObj;

    /**
     * 构造函数
     * @param _guildInfo 所属联盟对象
     */
    public GuildRallyMgr(GuildInfo _guildInfo) {
        _m_guildInfo = _guildInfo;
        _m_rallyInfoList = new ArrayList<>();
        _m_tickTmpNeedDealList = new ArrayList<>();
        _m_mutexObj = new MutexAtom();
    }

    /**
     * 获取所属联盟
     */
    public GuildInfo getGuildInfo() {
        return _m_guildInfo;
    }

    /**
     * 拷贝当前联盟的集结列表
     */
    public List<GuildRallyInfo<?>> cpyRallyInfoList() {
        _lock();
        try {
            return new ArrayList<>(_m_rallyInfoList);
        } finally {
            _unlock();
        }
    }

    /**
     * 主表初始化
     */
    public void s_initRallyMainFromDB(GuildRallyMainBO _bo) {
        if (_bo == null) {
            return;
        }

        // 每条主表记录对应一个集结对象，构造时直接注入主表BO
        GuildRallyInfo<?> info = _createRallyInfo(_bo);
        //s_init函数不做加锁处理
        if (info != null) {
            _m_rallyInfoList.add(info);
        }
        else{
            CommLog.error("GuildRallyMgr.s_initRallyMainFromDB - create rally info failed, guildId={}, rallyId={}, rallyType={}",
                    _m_guildInfo.getGuildId(), _bo.getId(), _bo.getRallyType());
        }
    }

    /**
     * 成员表初始化
     */
    public void s_initRallyMemberFromDB(GuildRallyMemberBO _bo) {
        if (_bo == null) {
            return;
        }

        // 先按rallyId定位目标集结（仅队列读），s_init不做加锁处理
        GuildRallyInfo<?> info = _lookupRallyInList(_bo.getRallyId());

        if (info == null) {
            CommLog.error("GuildRallyMgr.s_initRallyMemberFromDB - rally not found, rallyId={}", _bo.getRallyId());
            return;
        }

        info._s_initAddMember(_bo);
    }

    /**
     * 请求创建集结
     *
     * 注意：队伍状态切换由玩家侧MarsExploreTeam状态机负责，Guild侧不做队伍状态缓存
     */
    public ResultOne<Long> requestCreateRally(long _cid, long _teamId, MarsBattleV2_MemberInfo _teamMemberInfo) {
        if (_teamId <= 0) {
            return ResultOne.failed(CommErr.PARAM_ERROR);
        }

        if (_lookupRallyByLeaderTeamIdInList(_teamId) != null) {
            return ResultOne.failed(GuildRallyErr.RALLY_ALREADY_EXIST);
        }

        try {
            long nowMs = CommonFunc.getNowTimeMS();
            GuildRallyMainBO bo = new GuildRallyMainBO();
            bo.setGuildId(getBM(), _m_guildInfo.getGuildId());
            bo.setRallyType(getBM(), EGuildRallyType.DEFAULT.ordinal());
            bo.setLeaderCid(getBM(), _cid);
            bo.setLeaderTeamId(getBM(), _teamId);
            bo.setCreateTimeMs(getBM(), nowMs);
            bo.setExpireTimeMs(getBM(), nowMs + RALLY_EXPIRE_MS_DEFAULT);
            bo.setMinPowerLimit(getBM(), 0);
            bo.setMaxMemberLimit(getBM(), RALLY_MAX_MEMBER_DEFAULT);
            bo.setTeamSnapshot(getBM(), _teamMemberInfo.makePackage().array());
            // 默认类型扩展数据按协议对象落库，便于后续平滑扩展
            Common_Long extDataObj = new Common_Long(0);
            bo.setExtData(getBM(), CommonFunc.ByteBfferToBytes(extDataObj.makePackage()));
            bo.insert(getBM());

            GuildRallyInfo<?> info = _createRallyInfo(bo);
            if (info == null) {
                bo.del(getBM());
                return ResultOne.failed(GuildRallyErr.RALLY_OP_DISABLE);
            }
            _m_rallyInfoList.add(info);

            return ResultOne.succ(bo.getId());
        } catch (Exception e) {
            CommLog.error("GuildRallyMgr.requestCreateRally - create failed, guildId={}, cid={}, teamId={}, err={}",
                    _m_guildInfo.getGuildId(), _cid, _teamId, e.getMessage());
            return ResultOne.failed(GuildRallyErr.RALLY_OP_DISABLE);
        }
    }

    /**
     * 请求加入集结
     *
     * 处理顺序：
     * 1. 校验集结与成员重复
     * 2. 创建RallyMember（is_ready=false）
     * 3. 创建UsMarsJoinRally行为并addAction到tick队列
     * 4. 若行为创建失败，执行补偿：删RallyMember、推042-064、返回错误
     */
    public Result requestJoinRally(long _cid, long _rallyId, long _teamId, MarsBattleV2_MemberInfo _teamInfo) {
        if (_rallyId <= 0 || _teamId <= 0) {
            return CommErr.PARAM_ERROR;
        }

        GuildRallyInfo<?> info = lookupRally(_rallyId);
        if (info == null) {
            return GuildRallyErr.RALLY_NOT_FOUND;
        }

        long nowMs = CommonFunc.getNowTimeMS();
        if (info.getExpireTimeMs() > 0 && nowMs >= info.getExpireTimeMs()) {
            return GuildRallyErr.RALLY_EXPIRED;
        }

        Result addMemberResult = info.addMemberByJoinRequest(_cid, _teamId, nowMs, _teamInfo);
        if (!addMemberResult.isSucc()) {
            return addMemberResult;
        }

        // 先落成员数据再挂行军行为；若挂行为失败，需要立即执行成员回滚
        Result actionResult = _m_guildInfo.getGuildMgr().getServer().getMarsActionCore()
                .addJoinRallyAction(_m_guildInfo.getGuildId(), _rallyId, _cid, _teamId, nowMs + JOIN_RALLY_ACTION_DELAY_MS);
        if (!actionResult.isSucc()) {
            //如果失败则直接删除加入数据，同时回滚报错
            info.removeMemberByFail(_cid, _teamId);
            //推送成员退出集结处理
            _pushMemberExit(_cid, _rallyId);
            return actionResult;
        }

        return Result.SUCC;
    }

    /**
     * 查询单个集结信息
     */
    public ResultOne<GuildRallyInfo<?>> queryRallyInfo(long _rallyId) {
        if (_rallyId <= 0) {
            return ResultOne.failed(CommErr.PARAM_ERROR);
        }

        GuildRallyInfo<?> info = lookupRally(_rallyId);
        if (info == null) {
            return ResultOne.failed(GuildRallyErr.RALLY_NOT_FOUND);
        }

        return ResultOne.succ(info);
    }

    /**
     * 行军到达后完成成员加入并置准备完成
     */
    public Result onJoinMarchArrive(long _cid, long _rallyId, long _teamId) {
        GuildRallyInfo<?> info = lookupRally(_rallyId);
        if (info == null) {
            return GuildRallyErr.RALLY_NOT_FOUND;
        }

        Result markResult = info.markMemberArrive(_cid, _teamId);
        if (!markResult.isSucc()) {
            return markResult;
        }

        _pushMemberArrive(info.getLeaderCid(), _cid, _rallyId);
        return Result.SUCC;
    }

    /**
     * 行军失败补偿：删除RallyMember并推送042-064
     */
    public Result onJoinMarchFail(long _cid, long _rallyId, long _teamId) {
        GuildRallyInfo<?> info = lookupRally(_rallyId);
        if (info == null) {
            return GuildRallyErr.RALLY_NOT_FOUND;
        }

        Result rmvResult = info.removeMemberByFail(_cid, _teamId);
        if (!rmvResult.isSucc()) {
            return rmvResult;
        }

        _pushMemberExit(_cid, _rallyId);
        return Result.SUCC;
    }

    /**
     * 运行期查询集结对象
     * @param _rallyId 集结ID
     */
    public GuildRallyInfo<?> lookupRally(long _rallyId) {
        _lock();
        try {
            return _lookupRallyInList(_rallyId);
        } finally {
            _unlock();
        }
    }

    /**
     * 1秒tick
     * 到达有效时间的集结会从活动列表摘除，并触发对应类型的dealRally逻辑
     */
    public void tick(long _nowTimeMs) {
        _m_tickTmpNeedDealList.clear();

        _lock();
        try {
            for (int i = _m_rallyInfoList.size() - 1; i >= 0; i--) {
                GuildRallyInfo<?> info = _m_rallyInfoList.get(i);
                if (info == null) {
                    continue;
                }

                if (info.getExpireTimeMs() > 0 && _nowTimeMs >= info.getExpireTimeMs()) {
                    _m_rallyInfoList.remove(i);
                    _m_tickTmpNeedDealList.add(info);
                }
            }
        } finally {
            _unlock();
        }

        // 锁外执行业务处理，避免阻塞集结列表读写
        for (GuildRallyInfo<?> info : _m_tickTmpNeedDealList) {
            if (info == null) {
                continue;
            }

            try {
                info.dealRally();
            } catch (Exception e) {
                CommLog.error("GuildRallyMgr.tick - deal rally failed, guildId={}, rallyId={}, rallyType={}, err={}",
                        _m_guildInfo.getGuildId(), info.getRallyId(), info.getRallyType(), e.getMessage());
            }
        }
    }

    /**
     * 在List中线性查找集结
     * @param _rallyId 集结ID
     */
    private GuildRallyInfo<?> _lookupRallyInList(long _rallyId) {
        for (GuildRallyInfo<?> info : _m_rallyInfoList) {
            if (info == null) {
                continue;
            }
            if (info.getRallyId() == _rallyId) {
                return info;
            }
        }
        return null;
    }

    /**
     * 按队长队伍ID查找集结（用于防重创建）
     */
    private GuildRallyInfo<?> _lookupRallyByLeaderTeamIdInList(long _teamId) {
        for (GuildRallyInfo<?> info : _m_rallyInfoList) {
            if (info == null) {
                continue;
            }
            if (info.getLeaderTeamId() == _teamId) {
                return info;
            }
        }
        return null;
    }

    // 加锁
    private void _lock() {
        _m_mutexObj.lock();
    }

    // 解锁
    private void _unlock() {
        _m_mutexObj.unlock();
    }

    /**
     * 获取BM入口
     */
    private BM getBM() {
        return _m_guildInfo.getGuildMgr().getServer().getBM();
    }

    /**
     * 根据集结类型创建对应子类对象
     */
    private GuildRallyInfo<?> _createRallyInfo(GuildRallyMainBO _bo) {
        if (_bo == null) {
            return null;
        }

        if (_bo.getRallyType() == EGuildRallyType.DEFAULT.ordinal()) {
            return new GuildRallyInfo_Default(this, _bo);
        }

        CommLog.error("GuildRallyMgr.createRallyInfo - rallyType not support, guildId={}, rallyId={}, rallyType={}",
                _m_guildInfo.getGuildId(), _bo.getId(), _bo.getRallyType());
        return null;
    }

    /**
     * 推送成员到达通知（队长与成员本人各推一次）
     */
    private void _pushMemberArrive(long _leaderCid, long _memberCid, long _rallyId) {
        if (_leaderCid > 0) {
            _m_guildInfo.getGuildMgr().getServer().sendMsgToGC(_leaderCid,
                    US2GCWriter_042_GuildRelatedOp.make_063_OnRallyMemberArrive(_rallyId, _memberCid));
        }

        if (_memberCid > 0 && _memberCid != _leaderCid) {
            _m_guildInfo.getGuildMgr().getServer().sendMsgToGC(_memberCid,
                    US2GCWriter_042_GuildRelatedOp.make_063_OnRallyMemberArrive(_rallyId, _memberCid));
        }
    }

    /**
     * 推送成员退出通知（队长与成员本人各推一次）
     */
    private void _pushMemberExit(long _memberCid, long _rallyId) {
        GuildRallyInfo<?> info = lookupRally(_rallyId);
        long leaderCid = info == null ? 0 : info.getLeaderCid();

        if (leaderCid > 0) {
            _m_guildInfo.getGuildMgr().getServer().sendMsgToGC(leaderCid,
                    US2GCWriter_042_GuildRelatedOp.make_064_OnRallyMemberExit(_rallyId, _memberCid));
        }

        if (_memberCid > 0 && _memberCid != leaderCid) {
            _m_guildInfo.getGuildMgr().getServer().sendMsgToGC(_memberCid,
                    US2GCWriter_042_GuildRelatedOp.make_064_OnRallyMemberExit(_rallyId, _memberCid));
        }
    }
}
