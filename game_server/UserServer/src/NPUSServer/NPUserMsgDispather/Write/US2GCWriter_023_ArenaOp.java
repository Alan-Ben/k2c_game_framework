package NPUSServer.NPUserMsgDispather.Write;

import Common.ArenaObj.*;
import Common.TowerObj.Tower_OpponentInfo;
import Common.TowerObj.Tower_PosInfo;
import Common.TowerObj.Tower_ReportInfo;
import GS2GC.p023_ArenaOp.*;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Tower.TowerAttackResult;

import java.util.List;

public class US2GCWriter_023_ArenaOp
{
    public static GS2GC_023_001_RetRandomAttackPlayer make_001_RetRandomAttackPlayer()
    {
    	return new GS2GC_023_001_RetRandomAttackPlayer();
    }
    public static GS2GC_023_002_RetRandomAttackSelectHero make_002_RetRandomAttackSelectHero()
    {
    	return new GS2GC_023_002_RetRandomAttackSelectHero();
    }

    public static GS2GC_023_003_RetSelectAttackSelectHero make_003_RetSelectAttackSelectHero()
    {
    	return new GS2GC_023_003_RetSelectAttackSelectHero();
    }

    public static GS2GC_023_004_RetChooseBuff make_004_RetChooseBuff()
    {
    	return new GS2GC_023_004_RetChooseBuff();
    }

    public static GS2GC_023_005_RetRoundAttack make_005_RetRoundAttack()
    {
    	return new GS2GC_023_005_RetRoundAttack();
    }

    public static GS2GC_023_006_RetFightBackSelectHero make_006_RetFightBackSelectHero()
    {
    	return new GS2GC_023_006_RetFightBackSelectHero();
    }

    public static GS2GC_023_007_RetCelebrityRank make_007_RetCelebrityRank(List<Arena_CelebrityRankInfo> _rankList)
    {
        GS2GC_023_007_RetCelebrityRank proto = new GS2GC_023_007_RetCelebrityRank();
        proto.getRankList().addAll(_rankList);
        return proto;
    }

    public static GS2GC_023_008_RetArenaStationUpgrade make_008_RetArenaStationUpgrade()
    {
        return new GS2GC_023_008_RetArenaStationUpgrade();
    }

    public static GS2GC_023_009_RetArenaStationCollect make_009_RetArenaStationCollect()
    {
        return new GS2GC_023_009_RetArenaStationCollect();
    }


    public static GS2GC_023_010_RetArenaBattleReport make_010_RetArenaBattleReport(List<Arena_BattleReport> _reportList)
    {
        GS2GC_023_010_RetArenaBattleReport proto = new GS2GC_023_010_RetArenaBattleReport();
        proto.getReportList().addAll(_reportList);
        return proto;
    }

    public static GS2GC_023_011_RetArenaFightBackData make_011_RetArenaFightBackData(List<Arena_FightBackInfo> _reportList)
    {
        GS2GC_023_011_RetArenaFightBackData proto = new GS2GC_023_011_RetArenaFightBackData();
        proto.getFightBackList().addAll(_reportList);
        return proto;
    }

    public static GS2GC_023_012_RetArenaBuyRandomAttack make_012_RetArenaBuyRandomAttack()
    {
    	return new GS2GC_023_012_RetArenaBuyRandomAttack();
    }

    public static GS2GC_023_013_RetArenaAKeyAttack make_013_RetArenaAKeyAttack()
    {
    	return new GS2GC_023_013_RetArenaAKeyAttack();
    }

    public static GS2GC_023_021_RetTowerFight make_021_RetTowerFight(TowerAttackResult _attackSucc, NPPlayerContext _context)
    {
        GS2GC_023_021_RetTowerFight proto = new GS2GC_023_021_RetTowerFight();
    	proto.setIsDefeat(_attackSucc.isWin());
    	proto.setCid(_attackSucc.getTargetCid());
    	_context.getCollector().fillProtoList(proto.getRewardList());
        return proto;
    }

    public static GS2GC_023_022_RetTowerChallengeList make_022_RetTowerChallengeList(List<Tower_OpponentInfo> _opponentList)
    {
        GS2GC_023_022_RetTowerChallengeList proto = new GS2GC_023_022_RetTowerChallengeList();
    	proto.getOpponentList().addAll(_opponentList);
        return proto;
    }

    public static GS2GC_023_023_RetTowerChapterList make_023_RetTowerChapterList(List<Tower_OpponentInfo> _opponentList)
    {
        GS2GC_023_023_RetTowerChapterList proto = new GS2GC_023_023_RetTowerChapterList();
        proto.getOpponentList().addAll(_opponentList);
        return proto;
    }

    public static GS2GC_023_024_RetTowerResearchActive make_024_RetTowerResearchActive()
    {
        return new GS2GC_023_024_RetTowerResearchActive();
    }

    public static GS2GC_023_025_RetTowerDrawResearchReward make_025_RetTowerDrawResearchReward()
    {
        return new GS2GC_023_025_RetTowerDrawResearchReward();
    }

	public static GS2GC_023_026_RetTowerReportList make_026_RetTowerReportList(List<Tower_ReportInfo> _list)
    {
		GS2GC_023_026_RetTowerReportList proto = new GS2GC_023_026_RetTowerReportList();
        for(int i = 0; i < _list.size(); i++)
        {
        	Tower_ReportInfo info = _list.get(i);
        	if(null == info)
        		continue;
        	
        	proto.addReportList(info);
        }
        
        return proto;
    }

    public static GS2GC_023_051_OnArenaBattleInfoChg make_051_OnArenaBattleInfoChg(Arena_BattleInfo _battleInfo)
    {
    	return new GS2GC_023_051_OnArenaBattleInfoChg(_battleInfo);
    }

    public static GS2GC_023_052_OnArenaBaseInfoChg make_052_OnArenaBaseInfoChg(Arena_BaseInfo _baseInfo)
    {
    	return new GS2GC_023_052_OnArenaBaseInfoChg(_baseInfo);
    }

    public static GS2GC_023_054_OnArenaBattleReset make_054_OnArenaBattleReset()
    {
    	return new GS2GC_023_054_OnArenaBattleReset();
    }

    public static GS2GC_023_061_OnTowerPosChg make_061_OnTowerPosChg(Tower_PosInfo _info)
    {
    	GS2GC_023_061_OnTowerPosChg proto = new GS2GC_023_061_OnTowerPosChg();
    	proto.setPosInfo(_info);
    	return proto;
    }

    public static GS2GC_023_062_OnTowerResearchActivePosChg make_062_OnTowerResearchActivePosChg(Tower_PosInfo _info,int _buildingProfitAddPer)
    {
        GS2GC_023_062_OnTowerResearchActivePosChg proto = new GS2GC_023_062_OnTowerResearchActivePosChg();
    	proto.setPos(_info);
        proto.setBuildingProfitAddPer(_buildingProfitAddPer);
    	return proto;
    }

    public static GS2GC_023_063_OnTowerResearchRewardDraw make_063_OnTowerResearchRewardDraw(long _chapterId)
    {
        GS2GC_023_063_OnTowerResearchRewardDraw proto = new GS2GC_023_063_OnTowerResearchRewardDraw();
    	proto.setChapterId(_chapterId);
    	return proto;
    }

    public static GS2GC_023_064_OnTowerHighestPosHadReachChg make_064_OnTowerHighestPosHadReachChg(Tower_PosInfo _info)
    {
        GS2GC_023_064_OnTowerHighestPosHadReachChg proto = new GS2GC_023_064_OnTowerHighestPosHadReachChg();
    	proto.setPos(_info);
    	return proto;
    }
}
