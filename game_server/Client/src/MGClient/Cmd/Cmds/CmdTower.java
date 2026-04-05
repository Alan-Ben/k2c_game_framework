package MGClient.Cmd.Cmds;

import Common.TowerObj.Tower_PosInfo;
import GC2GS.p023_ArenaOp.*;
import GS2GC.p023_ArenaOp.*;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "爬塔", name = "tower")
public class CmdTower extends CmdBase
{
    @Command(comment = "攻击")
    public void attack(long _chapterId, int _level)
    {
        GC2GS_023_021_ReqTowerFight proto = new GC2GS_023_021_ReqTowerFight();
        proto.setTargetPos(new Tower_PosInfo(_chapterId,_level));
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_023_021_RetTowerFight>(GS2GC_023_021_RetTowerFight.class)
        {
            @Override
            public void handle(GS2GC_023_021_RetTowerFight _response)
            {
            }
        });
    }

    @Command(comment = "攻击列表")
    public void challengeList()
    {
        GC2GS_023_022_ReqTowerChallengeList proto = new GC2GS_023_022_ReqTowerChallengeList();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_023_022_RetTowerChallengeList>(GS2GC_023_022_RetTowerChallengeList.class)
        {
            @Override
            public void handle(GS2GC_023_022_RetTowerChallengeList _response)
            {
            }
        });
    }

    @Command(comment = "预览列表")
    public void forwardList(long _chapterId, int _level, int _num)
    {
        GC2GS_023_023_ReqTowerChapterList proto = new GC2GS_023_023_ReqTowerChapterList();
        proto.setChapterId(_chapterId);
        proto.setLevel(_level);
        proto.setNum(_num);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_023_023_RetTowerChapterList>(GS2GC_023_023_RetTowerChapterList.class)
        {
            @Override
            public void handle(GS2GC_023_023_RetTowerChapterList _response)
            {
            }
        });
    }

    @Command(comment = "激活研究")
    public void activeResearch(long _chapterId, int _level)
    {
        GC2GS_023_024_ReqTowerResearchActive proto = new GC2GS_023_024_ReqTowerResearchActive();
        proto.setTargetPos(new Tower_PosInfo(_chapterId,_level));
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_023_024_RetTowerResearchActive>(GS2GC_023_024_RetTowerResearchActive.class)
        {
            @Override
            public void handle(GS2GC_023_024_RetTowerResearchActive _response)
            {
            }
        });
    }

    @Command(comment = "领取研究奖励")
    public void drawResearch(long _chapterId)
    {
        GC2GS_023_025_ReqTowerDrawResearchReward proto = new GC2GS_023_025_ReqTowerDrawResearchReward();
        proto.setChapterId(_chapterId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_023_025_RetTowerDrawResearchReward>(GS2GC_023_025_RetTowerDrawResearchReward.class)
        {
            @Override
            public void handle(GS2GC_023_025_RetTowerDrawResearchReward _response)
            {
            }
        });
    }
}
