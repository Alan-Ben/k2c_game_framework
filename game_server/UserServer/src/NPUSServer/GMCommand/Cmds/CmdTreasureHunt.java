package NPUSServer.GMCommand.Cmds;

import Common.TreasureHuntEnum.ETreasureHuntCaptureType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ErrMain.TreasureHuntErr;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntOre;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntStationLevel;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntTreasure;
import NPGameRes.Refs.TreasureHunt.TreasureHuntMassGradeList;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure.TreasureHuntTreasureOutputInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureAnalyseCollector;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntCaptureResult;

import java.util.List;

@ACommander(comment = "太空寻宝", name = "treasureHunt")
public class CmdTreasureHunt extends UsCmdBase
{
    @ACommand(comment = "增加经验[数量]")
    public String addExp(long _exp)
    {
        getOwner().getTreasureHuntComponent().addExp(_exp, getContext());
        return "ok";
    }

    @ACommand(comment = "增加矿石[矿石ID][重量]")
    public String addOre(long _oreId, int _weight)
    {
        RefTreasureHuntOre refOre = RefTreasureHuntOre.getMgr().get(_oreId);
        if (refOre == null)
            return CommErr.REF_NOT_FOUND.toString();
        
        getOwner().getTreasureHuntComponent().getOreMgr().addOre(refOre, _weight, getContext());
        return "ok";
    }

    @ACommand(comment = "增加奇物[奇物ID]")
    public String addTreasure(long _treasureId)
    {
        RefTreasureHuntTreasure refTreasure = RefTreasureHuntTreasure.getMgr().get(_treasureId);
        if (refTreasure == null)
            return CommErr.REF_NOT_FOUND.toString();
        
        getOwner().getTreasureHuntComponent().getTreasureMgr().addTreasure(refTreasure, getContext());
        return "ok";
    }

    @ACommand(comment = "设置太空舱等级[等级]")
    public String setLevel(int _level)
    {
        getOwner().getTreasureHuntComponent().setStationLevel(_level, getContext());
        return "ok";
    }

    @ACommand(comment = "清除领取产出记录[奇物id]")
    public String clearGemDrawRecord(long _treasureId)
    {
        TreasureHuntTreasureOutputInfo outputInfo = getOwner().getTreasureHuntComponent().getTreasureMgr().lookupTreasureOutput(_treasureId);
        if (outputInfo == null)
            return TreasureHuntErr.TREASURE_HUNT_TREASURE_NO_OUTPUT.toString();

        outputInfo.clearNextCanDrawTime();
        return "ok";
    }

    @ACommand(comment = "查询太空寻宝技能信息")
    public String getSkillInfo()
    {
        return getOwner().getTreasureHuntComponent().getSkillInfo();
    }

    @ACommand(comment = "模拟多次寻宝[太空舱等级][所处寻宝区域][普通体力1高级体力2][寻宝次数]")
    public String dataAnalyse(int _level, long _areaId, int _type, int _count)
    {
        RefTreasureHuntStationLevel refLevel = RefTreasureHuntStationLevel.getMgr().get(_level);
        if (refLevel == null)
            return CommErr.REF_NOT_FOUND.toString();

        TreasureAnalyseCollector analyseCollector = new TreasureAnalyseCollector(refLevel, _count);
        ResultOne<TreasureHuntCaptureResult> captureResult = getOwner().getTreasureHuntComponent().capture(ETreasureHuntCaptureType.DATA_ANALYSE, _type == 2,
                0, _areaId, getContext(), analyseCollector);
        if (!captureResult.isSucc())
            return captureResult.toString();

        return analyseCollector.toString();
    }

    @ACommand(comment = "设置矿石技能等级[矿石ID][是否高级技能0普通1高级][等级]")
    public String setOreSkillLevel(long _oreId, int _isAdvanced, int _level)
    {
        getOwner().getTreasureHuntComponent().setOreSkillLevel(_oreId, _isAdvanced == 1, _level, getContext());
        return "ok";
    }

    @ACommand(comment = "重置矿石高级技能[矿石ID]")
    public String resetOreAdvancedSkill(long _oreId)
    {
        getOwner().getTreasureHuntComponent().resetOreAdvancedSkill(_oreId, getContext());
        return "ok";
    }

    @ACommand(comment = "设置奇物技能等级[奇物ID][等级]")
    public String setTreasureSkillLevel(long _treasureId, int _level)
    {
        getOwner().getTreasureHuntComponent().setTreasureSkillLevel(_treasureId, _level, getContext());
        return "ok";
    }

    @ACommand(comment = "重置组合高级技能[组合ID]")
    public String resetCompositeAdvancedSkill(long _compositeId)
    {
        getOwner().getTreasureHuntComponent().resetCompositeAdvancedSkill(_compositeId, getContext());
        return "ok";
    }

    @ACommand(comment = "设置矿石技能点数[矿石ID][是否高级技能点0普通1高级][点数]")
    public String setOreSkillPoint(long _oreId, int _isAdvanced, int _point)
    {
        getOwner().getTreasureHuntComponent().setOreSkillPoint(_oreId, _isAdvanced == 1, _point, getContext());
        return "ok";
    }

    @ACommand(comment = "一键获得所有回收物")
    public String gainAllOre()
    {
        List<RefTreasureHuntOre> list = RefTreasureHuntOre.getMgr().getList();
        int gainCount = 0;
        for (RefTreasureHuntOre refOre : list)
        {
            List<TreasureHuntMassGradeList.MassGradeItem> gradeList = refOre.mass_reward_grade_list.getRewardGradeList();
            int maxWeight = gradeList.isEmpty() ? 10000 : gradeList.get(gradeList.size() - 1)._m_maxMass - 1;
            boolean succ = getOwner().getTreasureHuntComponent().getOreMgr().addOre(refOre, maxWeight, getContext());
            if (succ)
                gainCount++;
        }
        return "done, gained " + gainCount + " ore(s).";
    }

    @ACommand(comment = "一键获得所有奇物")
    public String gainAllTreasure()
    {
        List<RefTreasureHuntTreasure> list = RefTreasureHuntTreasure.getMgr().getList();
        int gainCount = 0;
        for (RefTreasureHuntTreasure refTreasure : list)
        {
            getOwner().getTreasureHuntComponent().getTreasureMgr().addTreasure(refTreasure, getContext());
            gainCount++;
        }
        return "done, gained " + gainCount + " treasure(s).";
    }
}
