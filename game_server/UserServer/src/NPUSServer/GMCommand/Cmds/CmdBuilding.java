package NPUSServer.GMCommand.Cmds;

import Common.BuildingEnum.EBuildingFuncEnum;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.Building.RefBusinessBuildingLevel;
import NPGameRes.Refs.Building.RefFarmingBuildingLevel;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingFarmFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function._ABuildingFunc;


/**
 * 建筑GM命令
 */
@ACommander(comment = "建筑", name = "building")
public class CmdBuilding extends UsCmdBase
{
    @ACommand(comment = "打印所有建筑")
    public String printAll()
    {
        return getOwner().getBuildingComponent().toString();
    }

    @ACommand(comment = "打印建筑产出")
    public String printEarnings(long _buildingId)
    {
        BuildingInfo buildingInfo = getOwner().getBuildingComponent().lookupBuilding(_buildingId);
        if (buildingInfo == null)
            return "building not found";

        _ABuildingFunc buildingFunc = buildingInfo.getBuildingFunc(EBuildingFuncEnum.BUSINESS);
        BuildingBusinessFunc businessFunc = buildingFunc instanceof BuildingBusinessFunc ? ((BuildingBusinessFunc) buildingFunc) : null;
        if (businessFunc == null)
            return "business building not found";

        StringBuilder stringBuilder = new StringBuilder();
        businessFunc.calOutputSpeed(stringBuilder);

        return stringBuilder.toString();
    }

    @ACommand(comment = "设置农田等级[建筑id][等级]")
    public String setFarmLevel(long _buildingId, int _level)
    {
        BuildingInfo buildingInfo = getOwner().getBuildingComponent().lookupBuilding(_buildingId);
        if (buildingInfo == null)
            return "building not found, buildingId=" + _buildingId;

        BuildingFarmFunc farmFunc = buildingInfo.getFarm();
        if (farmFunc == null)
            return "not a farm building, buildingId=" + _buildingId;

        RefFarmingBuildingLevel lvlRef = farmFunc.getRef().getLevelMapMgr().getLevelData(_level);
        if (lvlRef == null)
            return "level config not found, level=" + _level;

        farmFunc.setLvl(lvlRef, _level, getContext());
        return "ok";
    }

    @ACommand(comment = "设置普通建筑等级[建筑id][等级]")
    public String setBusinessLevel(long _buildingId, int _level)
    {
        BuildingInfo buildingInfo = getOwner().getBuildingComponent().lookupBuilding(_buildingId);
        if (buildingInfo == null)
            return "building not found, buildingId=" + _buildingId;

        BuildingBusinessFunc businessFunc = buildingInfo.getBusiness();
        if (businessFunc == null)
            return "not a business building, buildingId=" + _buildingId;

        RefBusinessBuildingLevel lvlRef = businessFunc.getRef().getLevelMapMgr().getLevelData(_level);
        if (lvlRef == null)
            return "level config not found, level=" + _level;

        businessFunc.setLvl(lvlRef, getContext());
        return "ok";
    }

    @ACommand(comment = "设置普通建筑雇员数量[建筑id][数量]")
    public String setEmployeeCount(long _buildingId, int _count)
    {
        BuildingInfo buildingInfo = getOwner().getBuildingComponent().lookupBuilding(_buildingId);
        if (buildingInfo == null)
            return "building not found, buildingId=" + _buildingId;

        BuildingBusinessFunc businessFunc = buildingInfo.getBusiness();
        if (businessFunc == null)
            return "not a business building, buildingId=" + _buildingId;

        if (_count < 0)
            return "employee count cannot be negative, count=" + _count;

        businessFunc.setEmployeeCount(_count, getContext());
        return "ok";
    }
}
