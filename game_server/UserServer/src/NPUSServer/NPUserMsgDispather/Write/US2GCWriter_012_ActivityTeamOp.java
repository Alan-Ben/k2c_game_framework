package NPUSServer.NPUserMsgDispather.Write;

import Common.CrossTeamObj.CrossTeam_ApplyInfo;
import Common.CrossTeamObj.CrossTeam_BaseInfo;
import Common.CrossTeamObj.CrossTeam_Info;
import Common.CrossTeamObj.CrossTeam_PlayerApplyInfo;
import GS2GC.p012_ActivityTeamOp.*;

import java.util.ArrayList;

/**
 * 012 协议
 */
public class US2GCWriter_012_ActivityTeamOp
{
    /**
     * 构造队伍列表响应
     * @param _teamBaseList 当前页队伍基础数据
     * @param _totalCount   队伍总数量
     */
    public static GS2GC_012_001_RetActivityTeamList make_001_RetActivityTeamList(ArrayList<CrossTeam_BaseInfo> _teamBaseList, int _totalCount)
    {
        GS2GC_012_001_RetActivityTeamList proto = new GS2GC_012_001_RetActivityTeamList();
        proto.setTotalCount(_totalCount);
        if (null != _teamBaseList)
        {
            for (CrossTeam_BaseInfo baseInfo : _teamBaseList)
                proto.addTeamBaseList(baseInfo);
        }
        return proto;
    }

    // ...existing code...
    public static GS2GC_012_002_RetActivityTeam make_002_RetActivityTeam(CrossTeam_Info _team)
    {
        GS2GC_012_002_RetActivityTeam proto = new GS2GC_012_002_RetActivityTeam();
        proto.setTeam(_team);
    	
        return proto;
    }

    public static GS2GC_012_003_RetSelfActivityTeam make_003_RetSelfActivityTeam(CrossTeam_Info _team)
    {
        GS2GC_012_003_RetSelfActivityTeam proto = new GS2GC_012_003_RetSelfActivityTeam();
        proto.setTeam(_team);

        return proto;
    }

    public static GS2GC_012_004_RetSetActivityTeamApplyCond make_004_RetSetActivityTeamApplyCond()
    {
        GS2GC_012_004_RetSetActivityTeamApplyCond proto = new GS2GC_012_004_RetSetActivityTeamApplyCond();

        return proto;
    }

    public static GS2GC_012_005_RetCreateActivityTeam make_005_RetCreateActivityTeam()
    {
        GS2GC_012_005_RetCreateActivityTeam proto = new GS2GC_012_005_RetCreateActivityTeam();

        return proto;
    }

    public static GS2GC_012_006_RetJoinActivityTeam make_006_RetJoinActivityTeam()
    {
        GS2GC_012_006_RetJoinActivityTeam proto = new GS2GC_012_006_RetJoinActivityTeam();

        return proto;
    }

    public static GS2GC_012_007_RetKickActivityTeamPlayer make_007_RetKickActivityTeamPlayer()
    {
        GS2GC_012_007_RetKickActivityTeamPlayer proto = new GS2GC_012_007_RetKickActivityTeamPlayer();

        return proto;
    }

    public static GS2GC_012_008_RetDissolveActivityTeam make_008_RetDissolveActivityTeam()
    {
        GS2GC_012_008_RetDissolveActivityTeam proto = new GS2GC_012_008_RetDissolveActivityTeam();

        return proto;
    }

    public static GS2GC_012_009_RetQuitActivityTeam make_009_RetQuitActivityTeam()
    {
        GS2GC_012_009_RetQuitActivityTeam proto = new GS2GC_012_009_RetQuitActivityTeam();

        return proto;
    }

    public static GS2GC_012_010_RetApplyJoinActivityTeam make_010_RetApplyJoinActivityTeam()
    {
        GS2GC_012_010_RetApplyJoinActivityTeam proto = new GS2GC_012_010_RetApplyJoinActivityTeam();

        return proto;
    }

    public static GS2GC_012_011_RetActivityTeamApplyList make_011_RetActivityTeamApplyList(ArrayList<CrossTeam_ApplyInfo> _applyList)
    {
        GS2GC_012_011_RetActivityTeamApplyList proto = new GS2GC_012_011_RetActivityTeamApplyList();
        if (_applyList != null)
        {
            for (CrossTeam_ApplyInfo info : _applyList)
            {
                proto.addApplyList(info);
            }
        }

        return proto;
    }

    public static GS2GC_012_012_RetSelfActivityTeamApplyList make_012_RetSelfActivityTeamApplyList(ArrayList<CrossTeam_PlayerApplyInfo> _applyList)
    {
        GS2GC_012_012_RetSelfActivityTeamApplyList proto = new GS2GC_012_012_RetSelfActivityTeamApplyList();
        if (_applyList != null)
        {
            for (CrossTeam_PlayerApplyInfo info : _applyList)
            {
                proto.addApplyList(info);
            }
        }

        return proto;
    }

    public static GS2GC_012_013_RetAgreeActivityTeamApply make_013_RetAgreeActivityTeamApply()
    {
        GS2GC_012_013_RetAgreeActivityTeamApply proto = new GS2GC_012_013_RetAgreeActivityTeamApply();

        return proto;
    }

    public static GS2GC_012_014_RetRefuseActivityTeamApply make_014_RetRefuseActivityTeamApply()
    {
        GS2GC_012_014_RetRefuseActivityTeamApply proto = new GS2GC_012_014_RetRefuseActivityTeamApply();
        return proto;
    }

    // 修改队伍设置响应
    public static GS2GC_012_015_RetSetActivityTeamSetting make_015_RetSetActivityTeamSetting()
    {
        return new GS2GC_012_015_RetSetActivityTeamSetting();
    }

    public static GS2GC_012_051_OnJoinActivityTeam make_051_OnJoinActivityTeam(CrossTeam_Info _team)
    {
        GS2GC_012_051_OnJoinActivityTeam proto = new GS2GC_012_051_OnJoinActivityTeam();
        proto.setTeamId(_team.getTeamBase().getTeamId());
        proto.setTeam(_team);

        return proto;
    }

    public static GS2GC_012_052_OnQuitActivityTeam make_052_OnQuitActivityTeam(long _teamId)
    {
        GS2GC_012_052_OnQuitActivityTeam proto = new GS2GC_012_052_OnQuitActivityTeam();
        proto.setTeamId(_teamId);
        return proto;
    }

    public static GS2GC_012_053_OnActivityTeamMemberRemove make_053_OnActivityTeamMemberRemove(long _teamId, long _cid)
    {
        GS2GC_012_053_OnActivityTeamMemberRemove proto = new GS2GC_012_053_OnActivityTeamMemberRemove();
        proto.setTeamId(_teamId);
        proto.setCid(_cid);

        return proto;
    }

    public static GS2GC_012_054_OnActivityTeamDissolve make_054_OnActivityTeamDissolve(long _teamId)
    {
        GS2GC_012_054_OnActivityTeamDissolve proto = new GS2GC_012_054_OnActivityTeamDissolve();
        proto.setTeamId(_teamId);

        return proto;
    }
}
