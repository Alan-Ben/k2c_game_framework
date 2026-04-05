package CrossTeamServer.CrossTeam;

import ALBasicProtocolPack._IALProtocolStructure;
import CTSDB.Bo.CrossTeamMemberBO;
import Common.CrossTeamEnum.ENPCrossTeamMemberPos;
import Common.CrossTeamObj.CrossTeamMember_Info;
import CrossTeamServer.CrossTeamServer;
import GS2GC.p012_ActivityTeamOp.GS2GC_012_052_OnQuitActivityTeam;

import java.nio.ByteBuffer;

/**
 * 队伍成员数据
 */
public class CrossTeamMemberInfo
{
    //队伍数据
    private CrossTeamInfo _m_team;

    //队伍成员数据
    private CrossTeamMemberBO _m_bo;

    public CrossTeamMemberInfo(CrossTeamInfo _team, CrossTeamMemberBO _bo)
    {
        _m_team = _team;

        _m_bo = _bo;
    }

    public CrossTeamInfo getTeam() {return _m_team;}
    public CrossTeamMemberBO getBo() {return _m_bo;}

    public long getMemberCid() {return _m_bo.getMemberCid();}
    public ENPCrossTeamMemberPos getMemberPos() {return ENPCrossTeamMemberPos.ENPCrossTeamMemberPos_FromInt(_m_bo.getMemberPos());}
    public long getJoinMs() {return _m_bo.getJoinMs();}

    protected void _discard()
    {
        _m_bo.del(_m_team.getBM());
    }

    public CrossTeamMember_Info toProto()
    {
        CrossTeamMember_Info info = new CrossTeamMember_Info();
        info.setCid(getMemberCid());
        info.setPos(getMemberPos());
        info.setJoinMs(getJoinMs());

        return info;
    }

    /**
     * 发送协议给成员
     * @param _proto 已序列化的消息
     */
    public void sendMsg(ByteBuffer _proto)
    {
        CrossTeamServer.getInstance().sendMsg2GC(getMemberCid(), _proto);
    }

    /**
     * 发送协议给成员
     * @param _proto
     */
    public void sendMsg(_IALProtocolStructure _proto)
    {
        sendMsg(_proto.makeFullPackage());
    }

    /**
     * 发送退出队伍协议
     */
    public void sendQuitTeamMsg()
    {
        GS2GC_012_052_OnQuitActivityTeam proto = new GS2GC_012_052_OnQuitActivityTeam();
        proto.setTeamId(getTeam().getTeamId());

        sendMsg(proto);
    }
}
