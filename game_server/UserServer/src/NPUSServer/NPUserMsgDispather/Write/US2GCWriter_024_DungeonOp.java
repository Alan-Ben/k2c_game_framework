package NPUSServer.NPUserMsgDispather.Write;

import Common.DungeonObj.*;
import Common.RankObj.Rank_BaseItem;
import GS2GC.p024_DungeonOp.*;
import NPUSServer.Dungeon.Evening.EveningDungeonAttackResult;

import java.util.List;

public class US2GCWriter_024_DungeonOp
{
    public static GS2GC_024_001_RetMiddayDungeonAttack make_001_RetMiddayDungeonAttack(MiddayDungeon_SettleInfo _settleInfo)
    {
        GS2GC_024_001_RetMiddayDungeonAttack proto = new GS2GC_024_001_RetMiddayDungeonAttack();
        if (_settleInfo != null)
            proto.setSettleInfo(_settleInfo);
        return proto;
    }

    public static GS2GC_024_002_RetMiddayDungeonBorrowAttack make_002_RetMiddayDungeonBorrowAttack(MiddayDungeon_SettleInfo _settleInfo)
    {
        GS2GC_024_002_RetMiddayDungeonBorrowAttack proto = new GS2GC_024_002_RetMiddayDungeonBorrowAttack();
        if (_settleInfo != null)
            proto.setSettleInfo(_settleInfo);
        return proto;
    }

    public static GS2GC_024_003_RetMiddayDungeonBoxList make_003_RetMiddayDungeonBoxList(List<MiddayDungeon_BoxInfo> _boxList)
    {
        GS2GC_024_003_RetMiddayDungeonBoxList proto = new GS2GC_024_003_RetMiddayDungeonBoxList();
        proto.getBoxList().addAll(_boxList);
        return proto;
    }

    public static GS2GC_024_004_RetMiddayDungeonDrawBox make_004_RetMiddayDungeonDrawBox()
    {
        return new GS2GC_024_004_RetMiddayDungeonDrawBox();
    }

    public static GS2GC_024_005_RetMiddayDungeonBoxCanDraw make_005_RetMiddayDungeonBoxCanDraw(boolean _canDraw, int _remainDrawCount)
    {
        GS2GC_024_005_RetMiddayDungeonBoxCanDraw proto = new GS2GC_024_005_RetMiddayDungeonBoxCanDraw();
        proto.setCanDraw(_canDraw);
        proto.setRemainDrawCount(_remainDrawCount);
        return proto;
    }

    public static GS2GC_024_006_RetMiddayDungeonBoxDrawRecord make_006_RetMiddayDungeonBoxDrawRecord(List<MiddayDungeon_DrawRecord> _drawRecordList)
    {
        GS2GC_024_006_RetMiddayDungeonBoxDrawRecord proto = new GS2GC_024_006_RetMiddayDungeonBoxDrawRecord();
        proto.getDrawRecordList().addAll(_drawRecordList);
        return proto;
    }

    public static GS2GC_024_011_RetEveningDungeonAttack make_011_RetEveningDungeonAttack(EveningDungeonAttackResult _attackResult)
    {
        GS2GC_024_011_RetEveningDungeonAttack proto = new GS2GC_024_011_RetEveningDungeonAttack();
        proto.setHarmHp(_attackResult.harmHp);
        proto.setIsDefeat(_attackResult.isKill);
        proto.getAttackRewardList().addAll(_attackResult.attackRewardList);
        proto.getDefeatRewardList().addAll(_attackResult.defeatRewardList);
        return proto;
    }

    public static GS2GC_024_012_RetEveningDungeonBossInfo make_012_RetEveningDungeonBossInfo(EveningDungeon_BossInfo _bossInfo)
    {
        GS2GC_024_012_RetEveningDungeonBossInfo proto = new GS2GC_024_012_RetEveningDungeonBossInfo();
        proto.setBossInfo(_bossInfo);
        return proto;
    }

    public static GS2GC_024_013_RetEveningDungeonAttackLog make_013_RetEveningDungeonAttackLog(List<EveningDungeon_AttackLog> _logList)
    {
        GS2GC_024_013_RetEveningDungeonAttackLog proto = new GS2GC_024_013_RetEveningDungeonAttackLog();
        proto.getLogList().addAll(_logList);
        return proto;

    }

    public static GS2GC_024_014_RetEveningDungeonRankList make_014_RetEveningDungeonRankList(List<Rank_BaseItem> _rankList, Rank_BaseItem _selfItem)
    {
        GS2GC_024_014_RetEveningDungeonRankList proto = new GS2GC_024_014_RetEveningDungeonRankList();
        if (_rankList != null)
            proto.getRankList().addAll(_rankList);
        if (_selfItem != null)
            proto.setSelfRankItem(_selfItem);
        return proto;
    }

    public static GS2GC_024_015_RetEveningDungeonDefeatLog make_015_RetEveningDungeonDefeatLog(List<EveningDungeon_DefeatInfo> _logList)
    {
        GS2GC_024_015_RetEveningDungeonDefeatLog proto = new GS2GC_024_015_RetEveningDungeonDefeatLog();
        proto.getLogList().addAll(_logList);
        return proto;
    }
}
