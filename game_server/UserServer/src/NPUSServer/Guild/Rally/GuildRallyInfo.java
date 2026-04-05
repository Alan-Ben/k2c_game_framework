package NPUSServer.Guild.Rally;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.GuildEnum.EGuildRallyType;
import Common.MarsObj.MarsBattleV2_MemberInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildRallyErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPUSServer.USLog;
import USDB.Bo.GuildRallyMainBO;
import USDB.Bo.GuildRallyMemberBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

/**
 * 单个集结数据对象
 *
 * 主要职责：
 * 1. 保存单个集结的主数据与成员列表
 * 2. 构造时完成主数据装载
 * 3. 提供运行期线程安全读写接口
 *
 * 约束说明：
 * - 运行期成员变更通过本对象完成，外层管理器不直接查成员表
 */
public abstract class GuildRallyInfo<T extends _IALProtocolStructure> {
    // 所属联盟集结管理器
    private GuildRallyMgr _m_rallyMgr;
    // 对应数据库主表BO（保留引用，便于后续扩展直接访问原始字段）
    private GuildRallyMainBO _m_bo;
    // 运行期互斥锁
    private MutexAtom _m_mutexObj;
    // 创建者队伍快照对象（由BO中的team_snapshot反序列化得到）
    private MarsBattleV2_MemberInfo _m_teamSnapshot;
    // 集结扩展数据对象（直接以内存对象形式保存，按不同集结类型解析）
    private T _m_extDataObj;

    // 成员列表（按约束使用List管理）
    private List<RallyMemberInfo> _m_memberInfoList;

    /**
     * 构造函数
     * @param _rallyMgr 所属集结管理器
     * @param _bo 集结主表BO
     */
    public GuildRallyInfo(GuildRallyMgr _rallyMgr, GuildRallyMainBO _bo) {
        _m_rallyMgr = _rallyMgr;
        _m_bo = _bo;
        _m_mutexObj = new MutexAtom();
        _m_memberInfoList = new ArrayList<>();
        _m_extDataObj = _readTxtData(_bo.getExtData());

        // team_snapshot 仅在构造阶段反序列化一次，后续统一通过对象读取
        _m_teamSnapshot = new MarsBattleV2_MemberInfo();
        if (_bo.getTeamSnapshot() != null) {
            _m_teamSnapshot.readPackage(ByteBuffer.wrap(_bo.getTeamSnapshot()));
        }
    }

    /**
     * 到达有效时间后触发集结处理
     * 不同集结类型由子类实现各自业务逻辑
     */
    public abstract void dealRally();

    /**
     * 读取附加数据结构体
     * @param _data
     * @return
     */
    protected abstract T _readTxtData(byte[] _data);

    /**
     * 获取集结ID
     */
    public long getRallyId() {
        return _m_bo.getId();
    }

    /**
     * 获取集结类型
     */
    public int getRallyType() {
        return _m_bo.getRallyType();
    }

    /**
     * 获取集结类型枚举
     */
    public EGuildRallyType getRallyTypeEnum() {
        EGuildRallyType type = EGuildRallyType.EGuildRallyType_FromInt(_m_bo.getRallyType());
        return type == null ? EGuildRallyType.NONE : type;
    }

    /**
     * 获取创建者CID
     */
    public long getLeaderCid() {
        return _m_bo.getLeaderCid();
    }

    /**
     * 获取创建者队伍ID
     */
    public long getLeaderTeamId() {
        return _m_bo.getLeaderTeamId();
    }

    /**
     * 获取创建时间
     */
    public long getCreateTimeMs() {
        return _m_bo.getCreateTimeMs();
    }

    /**
     * 获取过期时间
     */
    public long getExpireTimeMs() {
        return _m_bo.getExpireTimeMs();
    }

    /**
     * 获取最低战力限制
     */
    public long getMinPowerLimit() {
        return _m_bo.getMinPowerLimit();
    }

    /**
     * 获取最大成员数限制
     */
    public int getMaxMemberLimit() {
        return _m_bo.getMaxMemberLimit();
    }

    /**
     * 获取扩展数据字节流
     */
    public byte[] getExtData() {
        return _m_bo.getExtData();
    }


    /**
     * 获取扩展数据对象
     */
    public T getExtDataObj() {
        return _m_extDataObj;
    }

    /**
     * 获取创建者队伍快照对象
     */
    public MarsBattleV2_MemberInfo getTeamSnapshot() {
        return _m_teamSnapshot;
    }

    /**
     * 获取成员列表副本（避免外部直接修改内部List）
     */
    public List<RallyMemberInfo> cpyMemberInfoList() {
        _lock();
        try {
            return new ArrayList<>(_m_memberInfoList);
        } finally {
            _unlock();
        }
    }

    /**
     * 查询成员信息（线程安全）
     * @param _cid 成员CID
     * @return 命中返回成员对象，未命中返回null
     */
    public RallyMemberInfo lookupMemberInfo(long _cid) {
        _lock();
        try {
            return _lookupMemberInfoInList(_cid);
        } finally {
            _unlock();
        }
    }

    /**
     * 运行期添加成员并写入数据库（默认未到达）
     *
     * @param _cid 成员CID
     * @param _teamId 成员队伍ID
     * @param _joinTimeMs 加入时间
     * @param _teamInfo 成员队伍快照
     * @return 成功返回SUCC；重复/满员/参数错误返回对应错误码
     */
    public Result addMemberByJoinRequest(long _cid, long _teamId, long _joinTimeMs, MarsBattleV2_MemberInfo _teamInfo) {
        if (_cid <= 0 || _teamId <= 0) {
            return CommErr.PARAM_ERROR;
        }

        _lock();
        try {
            if (null != _lookupMemberInfoInList(_cid)) {
                return GuildRallyErr.RALLY_MEMBER_EXIST;
            }

            if (_m_memberInfoList.size() >= getMaxMemberLimit()) {
                return GuildRallyErr.RALLY_MEMBER_FULL;
            }

            // 先插入成员BO，再同步加入内存列表，保证内存态与持久化一致
            GuildRallyMemberBO bo = new GuildRallyMemberBO();
            bo.setGuildId(getBM(), _m_rallyMgr.getGuildInfo().getGuildId());
            bo.setRallyId(getBM(), getRallyId());
            bo.setCid(getBM(), _cid);
            bo.setTeamId(getBM(), _teamId);
            bo.setJoinTimeMs(getBM(), _joinTimeMs);
            bo.setIsReady(getBM(), false);
            bo.setTeamSnapshot(getBM(), _teamInfo.makePackage().array());
            bo.insert(getBM());

            _m_memberInfoList.add(new RallyMemberInfo(bo));
            return Result.SUCC;
        } catch (Exception e) {
            CommLog.error("GuildRallyInfo.addMemberByJoinRequest - insert member failed, guildId={}, rallyId={}, cid={}, teamId={}, err={}",
                    _m_rallyMgr.getGuildInfo().getGuildId(), getRallyId(), _cid, _teamId, e.getMessage());
            return GuildRallyErr.RALLY_OP_DISABLE;
        } finally {
            _unlock();
        }
    }

    /**
     * 标记成员到达（置为ready）
     *
     * @param _cid 成员CID
     * @param _teamId 成员队伍ID
     * @return 成功返回SUCC；成员不存在返回RALLY_NOT_FOUND；队伍不匹配返回PARAM_ERROR
     */
    public Result markMemberArrive(long _cid, long _teamId) {
        if (_cid <= 0 || _teamId <= 0) {
            return CommErr.PARAM_ERROR;
        }

        _lock();
        try {
            RallyMemberInfo memberInfo = _lookupMemberInfoInList(_cid);
            if (memberInfo == null) {
                // 这里表示“成员未命中”，不是“集结对象不存在”
                return GuildRallyErr.RALLY_NOT_FOUND;
            }

            if (memberInfo.getTeamId() != _teamId) {
                return CommErr.PARAM_ERROR;
            }

            if (!memberInfo.isReady()) {
                memberInfo.saveReady(getBM(), true);
            }
            return Result.SUCC;
        } finally {
            _unlock();
        }
    }

    /**
     * 移除成员（失败补偿）
     *
     * @param _cid 成员CID
     * @param _teamId 成员队伍ID
     * @return 成功返回SUCC；成员不存在返回RALLY_NOT_FOUND；队伍不匹配返回PARAM_ERROR
     */
    public Result removeMemberByFail(long _cid, long _teamId) {
        if (_cid <= 0 || _teamId <= 0) {
            return CommErr.PARAM_ERROR;
        }

        _lock();
        try {
            RallyMemberInfo memberInfo = _lookupMemberInfoInList(_cid);
            if (memberInfo == null) {
                return GuildRallyErr.RALLY_NOT_FOUND;
            }

            if (memberInfo.getTeamId() != _teamId) {
                return CommErr.PARAM_ERROR;
            }

            // 失败补偿顺序：先删持久化，再删内存对象
            memberInfo.del(getBM());
            _m_memberInfoList.remove(memberInfo);
            return Result.SUCC;
        } finally {
            _unlock();
        }
    }

    /**
     * 初始化阶段添加成员（不去重）
     * 仅用于服务器启动装载，按DB数据原样恢复
     * @param _bo 集结成员表BO
     */
    protected void _s_initAddMember(GuildRallyMemberBO _bo) {
        if (_bo == null) {
            return;
        }

        if (null != _lookupMemberInfoInList(_bo.getCid())) {
            USLog.error(_m_rallyMgr.getGuildInfo().getGuildMgr().getServer(),
                    "GuildRallyInfo.s_initAddMember - duplicate member cid in init load, rallyId={}, cid={}",
                    getRallyId(), _bo.getCid());
        }

        RallyMemberInfo memberInfo = new RallyMemberInfo(_bo);
        _m_memberInfoList.add(memberInfo);
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
     * 在成员列表中线性查找（调用方自行保证锁语义）
     */
    private RallyMemberInfo _lookupMemberInfoInList(long _cid) {
        for (RallyMemberInfo memberInfo : _m_memberInfoList) {
            if (memberInfo == null) {
                continue;
            }
            if (memberInfo.getCid() == _cid) {
                return memberInfo;
            }
        }

        return null;
    }

    /**
     * 获取BM入口
     */
    private BM getBM() {
        return _m_rallyMgr.getGuildInfo().getGuildMgr().getServer().getBM();
    }

    /**
     * 子类访问BM入口
     */
    protected BM _getBM() {
        return getBM();
    }

    /**
     * 子类访问管理器
     */
    protected GuildRallyMgr _getRallyMgr() {
        return _m_rallyMgr;
    }
}





