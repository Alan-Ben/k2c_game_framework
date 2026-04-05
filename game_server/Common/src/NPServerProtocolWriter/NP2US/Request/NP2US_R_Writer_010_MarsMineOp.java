package NPServerProtocolWriter.NP2US.Request;

import Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer;
import NP2US_R.p010_MarsMineOp.*;

/**
 * 火星矿操作请求协议Writer
 *
 * 提供构造火星矿相关跨服请求协议的便利方法
 */
public class NP2US_R_Writer_010_MarsMineOp
{
    /**
     * 构造获取火星矿数据请求协议
     *
     * @param _instanceId 矿实例ID
     * @return 请求协议对象
     */
    public static NP2US_R_010_001_ReqGetMarsMine make_001_ReqGetMarsMine(long _instanceId)
    {
        NP2US_R_010_001_ReqGetMarsMine proto = new NP2US_R_010_001_ReqGetMarsMine();
        proto.setInstanceId(_instanceId);
        return proto;
    }

    /**
     * 构造查询火星矿是否被其他公会攻击过请求协议
     *
     * @param _instanceId 矿实例ID
     * @param _guildId 公会ID
     * @return 请求协议对象
     */
    public static NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag make_002_ReqGetMarsHadAttackByOtherTag(
            long _instanceId, long _guildId)
    {
        NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag proto = new NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag();
        proto.setInstanceId(_instanceId);
        proto.setGuildId(_guildId);
        return proto;
    }

    /**
     * 构造前往占领火星矿请求协议
     *
     * @param _instanceId 矿实例ID
     * @param _isOtherTeamForward 是否有其他玩家前往
     * @param _occupyPlayer 占领玩家信息
     * @param _startCollectMs 开始采集时间（毫秒）
     * @param _collectSpeed 采集速度（秒）
     * @return 请求协议对象
     */
    public static NP2US_R_010_003_ReqGoToOccupyMarsMine make_003_ReqGoToOccupyMarsMine(
            long _instanceId, boolean _isOtherTeamForward, ServerObj_MarsTeam_OccupyMinePlayer _occupyPlayer,
            long _startCollectMs, long _collectSpeed)
    {
        NP2US_R_010_003_ReqGoToOccupyMarsMine proto = new NP2US_R_010_003_ReqGoToOccupyMarsMine();
        proto.setInstanceId(_instanceId);
        proto.setIsOtherTeamForward(_isOtherTeamForward);
        proto.setOccupyPlayer(_occupyPlayer);
        proto.setStartCollectMs(_startCollectMs);
        proto.setCollectSpeed(_collectSpeed);
        return proto;
    }

    /**
     * 构造离开火星矿请求协议
     *
     * @param _instanceId 矿实例ID
     * @param _cid 玩家CID
     * @param _teamId 队伍ID
     * @return 请求协议对象
     */
    public static NP2US_R_010_004_ReqLeaveMarsMine make_004_ReqLeaveMarsMine(
            long _instanceId, long _cid, long _teamId)
    {
        NP2US_R_010_004_ReqLeaveMarsMine proto = new NP2US_R_010_004_ReqLeaveMarsMine();
        proto.setInstanceId(_instanceId);
        proto.setCid(_cid);
        proto.setTeamId(_teamId);
        return proto;
    }

    /**
     * 构造设置火星矿剩余资源数量请求协议（GM命令）
     *
     * @param _instanceId 矿实例ID
     * @param _remainNum 剩余资源数量
     * @return 请求协议对象
     */
    public static NP2US_R_010_005_ReqSetMineRemainNum make_005_ReqSetMineRemainNum(
            long _instanceId, long _remainNum)
    {
        NP2US_R_010_005_ReqSetMineRemainNum proto = new NP2US_R_010_005_ReqSetMineRemainNum();
        proto.setInstanceId(_instanceId);
        proto.setRemainNum(_remainNum);
        return proto;
    }
}
