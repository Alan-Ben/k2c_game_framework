package NPUSServer.NPUserMsgDispather.Write;

import Common.TreasureHuntObj.*;
import GS2GC.p036_TreasureHuntOp.*;

import java.util.List;

/**
 * p036 太空寻宝操作协议 writer
 */
public class US2GCWriter_036_TreasureHuntOp
{
    // ============= 返回协议 (Ret) =============

    /**
     * 创建太空寻宝矿石捕捉返回消息
     * @param addExp 获得经验值
     * @param resultList 捕捉结果列表
     * @return 返回消息
     */
    public static GS2GC_036_001_RetTreasureHuntOreCapture make_001_RetTreasureHuntOreCapture(long addExp, List<TreasureHunt_CaptureResult> resultList)
    {
        GS2GC_036_001_RetTreasureHuntOreCapture proto = new GS2GC_036_001_RetTreasureHuntOreCapture();
        proto.setAddExp(addExp);
        proto.getResultList().addAll(resultList);
        return proto;
    }

    /**
     * 创建太空寻宝矿石技能激活返回消息
     * @return 返回消息
     */
    public static GS2GC_036_002_RetTreasureHuntOreSkillActive make_002_RetTreasureHuntOreSkillActive()
    {
        return new GS2GC_036_002_RetTreasureHuntOreSkillActive();
    }

    /**
     * 创建太空寻宝矿石技能升级返回消息
     * @return 返回消息
     */
    public static GS2GC_036_003_RetTreasureHuntOreSkillUpgrade make_003_RetTreasureHuntOreSkillUpgrade()
    {
        return new GS2GC_036_003_RetTreasureHuntOreSkillUpgrade();
    }

    /**
     * 创建太空寻宝奇物技能激活返回消息
     * @return 返回消息
     */
    public static GS2GC_036_004_RetTreasureHuntTreasureSkillActive make_004_RetTreasureHuntTreasureSkillActive()
    {
        return new GS2GC_036_004_RetTreasureHuntTreasureSkillActive();
    }

    /**
     * 创建太空寻宝奇物技能升级返回消息
     * @return 返回消息
     */
    public static GS2GC_036_005_RetTreasureHuntTreasureSkillUpgrade make_005_RetTreasureHuntTreasureSkillUpgrade()
    {
        return new GS2GC_036_005_RetTreasureHuntTreasureSkillUpgrade();
    }

    /**
     * 创建太空寻宝合成激活返回消息
     * @return 返回消息
     */
    public static GS2GC_036_006_RetTreasureHuntCompositeActive make_006_RetTreasureHuntCompositeActive()
    {
        return new GS2GC_036_006_RetTreasureHuntCompositeActive();
    }

    /**
     * 创建太空寻宝领取矿石记录奖励返回消息
     * @return 返回消息
     */
    public static GS2GC_036_007_RetTreasureHuntDrawOreRecordReward make_007_RetTreasureHuntDrawOreRecordReward()
    {
        return new GS2GC_036_007_RetTreasureHuntDrawOreRecordReward();
    }

    /**
     * 创建太空寻宝领取捕捉能力返回消息
     * @return 返回消息
     */
    public static GS2GC_036_008_RetTreasureHuntDrawCapturePower make_008_RetTreasureHuntDrawCapturePower()
    {
        return new GS2GC_036_008_RetTreasureHuntDrawCapturePower();
    }

    /**
     * 创建太空寻宝转化矿石返回消息
     * @return 返回消息
     */
    public static GS2GC_036_009_RetTreasureHuntTransOre make_009_RetTreasureHuntTransOre(List<TreasureHunt_TransOreResult> _transOreResultList)
    {
        GS2GC_036_009_RetTreasureHuntTransOre proto = new GS2GC_036_009_RetTreasureHuntTransOre();
        proto.getTransResultList().addAll(_transOreResultList);
        return proto;
    }

    /**
     * 创建太空寻宝转化矿石排行信息返回消息
     * @return 返回消息
     */
    public static GS2GC_036_010_RetTreasureHuntTransOreRankInfo make_010_RetTreasureHuntTransOreRankInfo(List<TreasureHunt_OreRankItem> _top3List, int _myRank)
    {
        GS2GC_036_010_RetTreasureHuntTransOreRankInfo proto = new GS2GC_036_010_RetTreasureHuntTransOreRankInfo();
        if (_top3List != null && !_top3List.isEmpty())
            proto.getTopThreeList().addAll(_top3List);
        proto.setRank(_myRank);
        return proto;
    }

    /**
     * 创建太空寻宝领取奇物产出返回消息
     * @return
     */
    public static GS2GC_036_011_RetTreasureHuntDrawTreasureOutput make_011_RetTreasureHuntDrawTreasureOutput()
    {
        return new GS2GC_036_011_RetTreasureHuntDrawTreasureOutput();
    }

    // ============= 推送协议 (On) =============

    /**
     * 创建太空寻宝获得奇物推送消息
     * @param treasureInfo 奇物信息
     * @return 推送消息
     */
    public static GS2GC_036_051_OnTreasureHuntTreasureAdd make_051_OnTreasureHuntTreasureAdd(TreasureHunt_TreasureInfo treasureInfo)
    {
        return new GS2GC_036_051_OnTreasureHuntTreasureAdd(treasureInfo);
    }

    /**
     * 创建太空寻宝奇物等级变更推送消息
     * @param treasureId 奇物ID
     * @param level 奇物等级
     * @return 推送消息
     */
    public static GS2GC_036_052_OnTreasureHuntTreasureLevelChg make_052_OnTreasureHuntTreasureLevelChg(long treasureId, int level)
    {
        return new GS2GC_036_052_OnTreasureHuntTreasureLevelChg(treasureId, level);
    }

    /**
     * 创建太空寻宝获得矿石推送消息
     * @param oreInfo 矿石信息
     * @return 推送消息
     */
    public static GS2GC_036_053_OnTreasureHuntOreAdd make_053_OnTreasureHuntOreAdd(TreasureHunt_OreInfo oreInfo)
    {
        return new GS2GC_036_053_OnTreasureHuntOreAdd(oreInfo);
    }

    /**
     * 创建太空寻宝矿石数量变更推送消息
     * @param oreId 矿石ID
     * @param numInfo 矿石数量信息
     * @return 推送消息
     */
    public static GS2GC_036_054_OnTreasureHuntOreNumChg make_054_OnTreasureHuntOreNumChg(long oreId, TreasureHunt_OreNumInfo numInfo)
    {
        return new GS2GC_036_054_OnTreasureHuntOreNumChg(oreId, numInfo);
    }

    /**
     * 创建太空寻宝矿石技能变更推送消息
     * @param oreId 矿石ID
     * @param isNormal 是否为普通技能
     * @param skillInfo 技能信息
     * @return 推送消息
     */
    public static GS2GC_036_055_OnTreasureHuntOreSkillChg make_055_OnTreasureHuntOreSkillChg(long oreId, boolean isNormal, TreasureHunt_OreSkillInfo skillInfo)
    {
        return new GS2GC_036_055_OnTreasureHuntOreSkillChg(oreId, isNormal, skillInfo);
    }

    /**
     * 创建太空寻宝合成变更推送消息
     * @param compositeInfo 合成信息
     * @return 推送消息
     */
    public static GS2GC_036_056_OnTreasureCompositeChg make_056_OnTreasureCompositeChg(TreasureHunt_CompositeInfo compositeInfo)
    {
        return new GS2GC_036_056_OnTreasureCompositeChg(compositeInfo);
    }

    /**
     * 创建太空寻宝工作台变更推送消息
     * @param stationInfo 工作台信息
     * @return 推送消息
     */
    public static GS2GC_036_057_OnTreasureStationChg make_057_OnTreasureStationChg(TreasureHunt_StationInfo stationInfo)
    {
        return new GS2GC_036_057_OnTreasureStationChg(stationInfo);
    }

    /**
     * 创建太空寻宝矿石最大记录变更推送消息
     * @param oreId 矿石ID
     * @param maxRecord 最大记录
     * @return 推送消息
     */
    public static GS2GC_036_058_OnTreasureHuntOreMaxRecordChg make_058_OnTreasureHuntOreMaxRecordChg(long oreId, int maxRecord)
    {
        return new GS2GC_036_058_OnTreasureHuntOreMaxRecordChg(oreId, maxRecord);
    }

    /**
     * 创建太空寻宝矿石已领取记录奖励变更推送消息
     * @param oreId 矿石ID
     * @param hadDrawRecordRewardList 已领取记录奖励列表
     * @return 推送消息
     */
    public static GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg make_059_OnTreasureHuntOreHadDrawRecordRewardChg(long oreId, List<Integer> hadDrawRecordRewardList)
    {
        GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg proto = new GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg();
        proto.setOreId(oreId);
        proto.getHadDrawRecordRewardList().addAll(hadDrawRecordRewardList);
        return proto;
    }

    /**
     * 创建太空寻宝奇物产出信息变更推送消息
     * @param _outputInfo 奇物产出信息
     * @return 推送消息
     */
    public static GS2GC_036_061_OnTreasureHuntTreasureOutputChg make_061_OnTreasureHuntTreasureOutputChg(TreasureHunt_TreasureOutputInfo _outputInfo)
    {
        GS2GC_036_061_OnTreasureHuntTreasureOutputChg proto = new GS2GC_036_061_OnTreasureHuntTreasureOutputChg();
        proto.setTreasureOutputInfo(_outputInfo);
        return proto;
    }
}