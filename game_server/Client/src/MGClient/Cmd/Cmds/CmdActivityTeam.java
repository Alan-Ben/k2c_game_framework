package MGClient.Cmd.Cmds;

import Common.CrossTeamEnum.ENPCrossTeamJoinCond;
import Common.CrossTeamEnum.ENPCrossTeamJoinType;
import Common.CrossTeamObj.CrossTeam_SetInfo_Join;
import GC2GS.p012_ActivityTeamOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * 活动组队相关GM命令
 *
 * 主要功能：
 * 1. 覆盖 GC2GS_012_001 ~ GC2GS_012_014 全部请求协议
 * 2. 复杂嵌套结构（SetInfo/joinCond）展开为扁平参数传入
 */
@Commander(comment = "活动组队", name = "activityTeam")
public class CmdActivityTeam extends CmdBase
{
    @Command(comment = "获取队伍列表(活动实例ID, 页码从1开始)")
    public void GetTeamList(long _instanceId, int _page)
    {
        GC2GS_012_001_ReqActivityTeamList msg = new GC2GS_012_001_ReqActivityTeamList();
        msg.setInstanceId(_instanceId);
        msg.setPage(_page);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "获取队伍数据(队伍ID)")
    public void GetTeam(long _teamId)
    {
        GC2GS_012_002_ReqActivityTeam msg = new GC2GS_012_002_ReqActivityTeam();
        msg.setTeamId(_teamId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "获取自己队伍数据(活动实例ID)")
    public void GetSelfActivityTeam(long _instanceId)
    {
        GC2GS_012_003_ReqSelfActivityTeam msg = new GC2GS_012_003_ReqSelfActivityTeam();
        msg.setInstanceId(_instanceId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "设置队伍申请条件(队伍ID, 申请条件枚举序号ENPCrossTeamJoinCond, 条件数值)")
    public void SetActivityTeamApplyCond(long _teamId, int _condIndex, long _condValue)
    {
        // 将整数序号转换为申请条件枚举
        ENPCrossTeamJoinCond cond = ENPCrossTeamJoinCond.ENPCrossTeamJoinCond_FromInt(_condIndex);
        if (cond == null)
        {
            System.out.println("无效的申请条件枚举序号: " + _condIndex);
            return;
        }

        CrossTeam_SetInfo_Join joinCond = new CrossTeam_SetInfo_Join();
        joinCond.setCond(cond);
        joinCond.setValue(_condValue);

        GC2GS_012_004_ReqSetActivityTeamApplyCond msg = new GC2GS_012_004_ReqSetActivityTeamApplyCond();
        msg.setTeamId(_teamId);
        msg.setJoinCond(joinCond);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "创建活动组队(活动实例ID, 加入方式序号ENPCrossTeamJoinType(1=自由2=禁止3=条件), 申请条件序号ENPCrossTeamJoinCond, 条件数值, 队伍名称, 队伍宣言)")
    public void CreateTeam(long _instanceId, int _joinTypeIndex, int _condIndex, long _condValue, String _teamName, String _teamDec)
    {
        ENPCrossTeamJoinType joinType = ENPCrossTeamJoinType.ENPCrossTeamJoinType_FromInt(_joinTypeIndex);
        if (joinType == null || joinType == ENPCrossTeamJoinType.NONE)
        {
            System.out.println("无效的加入方式枚举序号: " + _joinTypeIndex);
            return;
        }

        ENPCrossTeamJoinCond cond = ENPCrossTeamJoinCond.ENPCrossTeamJoinCond_FromInt(_condIndex);
        if (cond == null)
        {
            System.out.println("无效的申请条件枚举序号: " + _condIndex);
            return;
        }

        CrossTeam_SetInfo_Join joinCond = new CrossTeam_SetInfo_Join();
        joinCond.setCond(cond);
        joinCond.setValue(_condValue);

        GC2GS_012_005_ReqCreateActivityTeam msg = new GC2GS_012_005_ReqCreateActivityTeam();
        msg.setInstanceId(_instanceId);
        msg.setJoinType(joinType);
        msg.setJoinCond(joinCond);
        msg.setTeamName(_teamName);
        msg.setTeamDec(_teamDec);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "修改队伍设置(队伍ID, 队伍名称, 队伍说明, 加入方式序号ENPCrossTeamJoinType, 申请条件序号ENPCrossTeamJoinCond, 条件数值)")
    public void SetTeamSetting(long _teamId, String _teamName, String _teamDec, int _joinTypeIndex, int _condIndex, long _condValue)
    {
        ENPCrossTeamJoinType joinType = ENPCrossTeamJoinType.ENPCrossTeamJoinType_FromInt(_joinTypeIndex);
        if (joinType == null || joinType == ENPCrossTeamJoinType.NONE)
        {
            System.out.println("无效的加入方式枚举序号: " + _joinTypeIndex);
            return;
        }

        ENPCrossTeamJoinCond cond = ENPCrossTeamJoinCond.ENPCrossTeamJoinCond_FromInt(_condIndex);
        if (cond == null)
        {
            System.out.println("无效的申请条件枚举序号: " + _condIndex);
            return;
        }

        CrossTeam_SetInfo_Join joinCond = new CrossTeam_SetInfo_Join();
        joinCond.setCond(cond);
        joinCond.setValue(_condValue);

        GC2GS_012_015_ReqSetActivityTeamSetting msg = new GC2GS_012_015_ReqSetActivityTeamSetting();
        msg.setTeamId(_teamId);
        msg.setTeamName(_teamName);
        msg.setTeamDec(_teamDec);
        msg.setJoinType(joinType);
        msg.setJoinCond(joinCond);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "加入队伍(队伍ID)")
    public void JoinTeam(long _teamId)
    {
        GC2GS_012_006_ReqJoinActivityTeam msg = new GC2GS_012_006_ReqJoinActivityTeam();
        msg.setTeamId(_teamId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "踢出队伍成员(队伍ID, 被踢玩家CID)")
    public void KickTeamPlayer(long _teamId, long _kickCid)
    {
        GC2GS_012_007_ReqKickActivityTeamPlayer msg = new GC2GS_012_007_ReqKickActivityTeamPlayer();
        msg.setTeamId(_teamId);
        msg.setKickCid(_kickCid);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "解散队伍(队伍ID)")
    public void DissolveTeam(long _teamId)
    {
        GC2GS_012_008_ReqDissolveActivityTeam msg = new GC2GS_012_008_ReqDissolveActivityTeam();
        msg.setTeamId(_teamId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "退出队伍(队伍ID)")
    public void QuitTeam(long _teamId)
    {
        GC2GS_012_009_ReqQuitActivityTeam msg = new GC2GS_012_009_ReqQuitActivityTeam();
        msg.setTeamId(_teamId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "申请加入队伍(队伍ID)")
    public void ApplyJoinTeam(long _teamId)
    {
        GC2GS_012_010_ReqApplyJoinActivityTeam msg = new GC2GS_012_010_ReqApplyJoinActivityTeam();
        msg.setTeamId(_teamId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "获取队伍申请列表(队伍ID)")
    public void GetTeamApplyList(long _teamId)
    {
        GC2GS_012_011_ReqActivityTeamApplyList msg = new GC2GS_012_011_ReqActivityTeamApplyList();
        msg.setTeamId(_teamId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "获取自己发起的队伍申请列表(活动实例ID)")
    public void GetSelfTeamApplyList(long _instanceId)
    {
        GC2GS_012_012_ReqSelfActivityTeamApplyList msg = new GC2GS_012_012_ReqSelfActivityTeamApplyList();
        msg.setInstanceId(_instanceId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "同意队伍申请(队伍ID, 申请玩家CID)")
    public void AgreeTeamApply(long _teamId, long _applyCid)
    {
        GC2GS_012_013_ReqAgreeActivityTeamApply msg = new GC2GS_012_013_ReqAgreeActivityTeamApply();
        msg.setTeamId(_teamId);
        msg.setApplyCid(_applyCid);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "拒绝队伍申请(队伍ID, 申请玩家CID)")
    public void RefuseTeamApply(long _teamId, long _applyCid)
    {
        GC2GS_012_014_ReqRefuseActivityTeamApply msg = new GC2GS_012_014_ReqRefuseActivityTeamApply();
        msg.setTeamId(_teamId);
        msg.setApplyCid(_applyCid);
        getOwner().sendGameMsg(msg);
    }
}
