package NPUSServer.Guild.Rally;

import Common.MarsObj.MarsBattleV2_MemberInfo;
import NPCommon.DB.BM.BM;
import USDB.Bo.GuildRallyMemberBO;

import java.nio.ByteBuffer;

/**
 * 集结成员信息
 *
 * 主要职责：
 * 1. 保存成员基础数据
 * 2. 在初始化阶段将team_snapshot反序列化为对象
 */
public class RallyMemberInfo {
    // 对应数据库成员BO（运行期状态变更通过该对象落库）
    private GuildRallyMemberBO _m_bo;
    // 成员队伍快照（一个成员对应一个MemberInfo）
    private MarsBattleV2_MemberInfo _m_teamSnapshot;

    /**
     * 构造函数
     * 直接使用BO初始化成员运行期字段，并缓存BO引用
     * @param _bo 成员BO
     */
    public RallyMemberInfo(GuildRallyMemberBO _bo) {
        _m_bo = _bo;

        // 将bytes快照反序列化为成员对象，供后续逻辑直接读取
        _m_teamSnapshot = new MarsBattleV2_MemberInfo();
        if (_bo.getTeamSnapshot() != null) {
            _m_teamSnapshot.readPackage(ByteBuffer.wrap(_bo.getTeamSnapshot()));
        }
    }

    /**
     * 获取成员CID
     */
    public long getCid() {
        return _m_bo.getCid();
    }

    /**
     * 获取成员队伍ID
     */
    public long getTeamId() {
        return _m_bo.getTeamId();
    }

    /**
     * 获取加入时间
     */
    public long getJoinTimeMs() {
        return _m_bo.getJoinTimeMs();
    }

    /**
     * 是否准备完成
     */
    public boolean isReady() {
        return _m_bo.getIsReady();
    }

    /**
     * 获取队伍快照
     */
    public MarsBattleV2_MemberInfo getTeamSnapshot() {
        return _m_teamSnapshot;
    }

    /**
     * 保存成员准备状态
     * 持久化副作用：直接更新guild_rally_member.is_ready字段
     * @param _bm BM对象
     * @param _isReady 目标准备状态
     */
    public void saveReady(BM _bm, boolean _isReady) {
        _m_bo.saveIsReady(_bm, _isReady);
    }

    /**
     * 删除成员数据
     * 持久化副作用：直接删除对应成员记录
     * @param _bm BM对象
     */
    public void del(BM _bm) {
        _m_bo.del(_bm);
    }

}


