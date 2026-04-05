package MGClient.Cmd.Cmds;

import Common.GuildCooperateObj.GuildCooperate_RewardPointPos;
import Common.GuildEnum.EGuildJoinLimitType;
import Common.GuildEnum.EGuildJoinType;
import Common.GuildObj.Guild_JoinLimitInfo;
import GC2GS.p032_GuildOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

@Commander(comment = "联盟", name = "guild")
public class CmdGuild extends CmdBase
{

    @Command(comment = "创建联盟")
    public void create(long _flagId, String _name, String _simpleName, String _declaration, boolean _canFreeJoin)
    {
        GC2GS_032_001_ReqCreateGuild msg = new GC2GS_032_001_ReqCreateGuild(_flagId, _name, _simpleName, _declaration, _canFreeJoin);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "加入联盟")
    public void join(long _guildId)
    {
        GC2GS_032_002_ReqJoinGuild msg = new GC2GS_032_002_ReqJoinGuild(_guildId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "随机加入联盟")
    public void randomJoin()
    {
        GC2GS_032_003_ReqRandomJoinGuild msg = new GC2GS_032_003_ReqRandomJoinGuild();
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求其他联盟信息")
    public void getInfo(long _guildId)
    {
        GC2GS_032_004_ReqOtherGuildInfo msg = new GC2GS_032_004_ReqOtherGuildInfo(_guildId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求转让联盟")
    public void transfer(long _memberId)
    {
        GC2GS_032_005_ReqTransferGuild msg = new GC2GS_032_005_ReqTransferGuild(_memberId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求解散联盟")
    public void dissolve()
    {
        GC2GS_032_006_ReqDissolveGuild msg = new GC2GS_032_006_ReqDissolveGuild();
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求联盟职位任命")
    public void appoint(long _memberId, long _positionId)
    {
        GC2GS_032_007_ReqGuildPositionAppoint msg = new GC2GS_032_007_ReqGuildPositionAppoint(_memberId, _positionId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求变更联盟旗帜")
    public void changeFlag(long _flagId)
    {
        GC2GS_032_008_ReqChgGuildFlag msg = new GC2GS_032_008_ReqChgGuildFlag(_flagId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求变更联盟名称")
    public void changeName(String _name, String _simpleName)
    {
        GC2GS_032_009_ReqChgGuildName msg = new GC2GS_032_009_ReqChgGuildName(_name, _simpleName);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求变更联盟宣言")
    public void changeDeclaration(String _declaration)
    {
        GC2GS_032_010_ReqChgGuildDeclaration msg = new GC2GS_032_010_ReqChgGuildDeclaration(_declaration);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求变更联盟公告")
    public void changeAnnouncement(String _announcement)
    {
        GC2GS_032_011_ReqChgGuildAnnouncement msg = new GC2GS_032_011_ReqChgGuildAnnouncement(_announcement);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "切换联盟加入类型")
    public void setJoinType(EGuildJoinType _type)
    {
        GC2GS_032_012_ReqSetGuildJoinType msg = new GC2GS_032_012_ReqSetGuildJoinType();
        msg.setType(_type);
        msg.addJoinLimitInfo(new Guild_JoinLimitInfo(EGuildJoinLimitType.LEVEL, 0));
        msg.addJoinLimitInfo(new Guild_JoinLimitInfo(EGuildJoinLimitType.NATION_POWER, 0));
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "处理联盟加入请求")
    public void processJoinRequest(long _requestId, boolean _isAccept)
    {
        GC2GS_032_013_ReqProcessGuildJoinRequest msg = new GC2GS_032_013_ReqProcessGuildJoinRequest(_requestId, _isAccept);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求踢出联盟成员")
    public void kickOut(long _memberId)
    {
        GC2GS_032_014_ReqGuildKickOutMember msg = new GC2GS_032_014_ReqGuildKickOutMember(_memberId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求群发消息")
    public void broadcast(String _message)
    {
        GC2GS_032_015_ReqGuildBroadcastMessage msg = new GC2GS_032_015_ReqGuildBroadcastMessage();
        msg.setMessage(_message);
        msg.addCidList(1);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求退出联盟")
    public void leave()
    {
        GC2GS_032_016_ReqLeaveGuild msg = new GC2GS_032_016_ReqLeaveGuild();
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求建造联盟")
    public void construct(long _refId)
    {
        GC2GS_032_017_ReqGuildConstruct msg = new GC2GS_032_017_ReqGuildConstruct(_refId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求弹劾盟主")
    public void impeach(long _eventDbId)
    {
        GC2GS_032_018_ReqGuildImpeachLeader msg = new GC2GS_032_018_ReqGuildImpeachLeader(_eventDbId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求联盟成员贡献列表")
    public void contribute(long _memberId)
    {
        GC2GS_032_019_ReqMemberContribute msg = new GC2GS_032_019_ReqMemberContribute(_memberId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求联盟加入请求列表")
    public void search(String _searchData)
    {
        GC2GS_032_021_ReqSearchGuild msg = new GC2GS_032_021_ReqSearchGuild(_searchData);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求联盟排行榜列表")
    public void rankList()
    {
        GC2GS_032_022_ReqGuildRankList msg = new GC2GS_032_022_ReqGuildRankList();
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "一键处理入盟请求")
    public void dealJoinRequest(boolean _isAgree)
    {
        GC2GS_032_023_ReqGuildJoinRequestAKeyDeal msg = new GC2GS_032_023_ReqGuildJoinRequestAKeyDeal(_isAgree);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "取消自己的入盟申请")
    public void cancelJoinRequest(long _guildId)
    {
        GC2GS_032_024_ReqCancelSelfJoinRequest msg = new GC2GS_032_024_ReqCancelSelfJoinRequest(_guildId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "取消弹劾事件")
    public void cancelImpeach(long _eventDbId)
    {
        GC2GS_032_025_ReqCancelLeaderImpeachEvent msg = new GC2GS_032_025_ReqCancelLeaderImpeachEvent(_eventDbId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "批准弹劾事件")
    public void approveImpeach(long _eventDbId)
    {
        GC2GS_032_026_ReqApproveLeaderImpeachEvent msg = new GC2GS_032_026_ReqApproveLeaderImpeachEvent(_eventDbId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求公会合作攻击日志列表")
    public void cooperateAttackLogList(int _page, int _pageSize)
    {
        GC2GS_032_041_ReqGuildCooperateAttackLogList msg = new GC2GS_032_041_ReqGuildCooperateAttackLogList(_page, _pageSize);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "设置推荐奖励据点")
    public void setRecommendRewardPoint(long _areaId, int _index)
    {
        GC2GS_032_042_ReqSetRecommendRewardPoint msg = new GC2GS_032_042_ReqSetRecommendRewardPoint();
        msg.setPos(new GuildCooperate_RewardPointPos(_areaId, _index));
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "领取奖励点奖励")
    public void drawRewardPointReward()
    {
        GC2GS_032_043_ReqDrawRewardPointReward msg = new GC2GS_032_043_ReqDrawRewardPointReward();
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "攻击属性据点")
    public void attackPropertyPoint(long _areaId, int _index, int _propertyPointIndex, long _heroId)
    {
        GC2GS_032_044_ReqAttackPropertyPoint msg = new GC2GS_032_044_ReqAttackPropertyPoint();
        msg.setPos(new GuildCooperate_RewardPointPos(_areaId, _index));
        msg.setPropertyPointIndex(_propertyPointIndex);
        msg.setHeroId(_heroId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "增加英雄恢复次数")
    public void addHeroRecoveryCount(long _heroId)
    {
        GC2GS_032_045_ReqAddHeroRecoveryCount msg = new GC2GS_032_045_ReqAddHeroRecoveryCount(_heroId);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "请求公会合作伤害排行")
    public void cooperateDamageRank()
    {
        GC2GS_032_046_ReqGuildCooperateDamageRank msg = new GC2GS_032_046_ReqGuildCooperateDamageRank();
        getOwner().sendGameMsg(msg);
    }
}
