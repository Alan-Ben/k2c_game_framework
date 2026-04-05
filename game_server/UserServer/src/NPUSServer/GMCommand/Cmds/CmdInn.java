package NPUSServer.GMCommand.Cmds;

import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.InnErr;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.Inn.RefInnDish;
import NPGameRes.Refs.Inn.RefInnStation;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Dish.InnDishInfo;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Station.InnStationInfo;


/**
 * 玩家属性作弊命令
 */
@ACommander(comment = "旅店", name = "inn")
public class CmdInn extends UsCmdBase
{
    @ACommand(comment = "设置等级[等级]")
    public String setLevel(int _level)
    {
        return getOwner().getInnComponent().cheatSetLevel(_level, getContext()).toString();
    }

    @ACommand(comment = "设置奖牌等级[等级]")
    public String setMedalLevel(int _level)
    {
        return getOwner().getInnComponent().cheatSetMedalLevel(_level, getContext()).toString();
    }

    @ACommand(comment = "增加蓝图[数量]")
    public String addBlueprint(int _count)
    {
        getOwner().gainItem(RefGeneral.Ref().inn_station_blueprint_item, _count, getContext());
        return "ok";
    }

    @ACommand(comment = "增加人气[数量]")
    public String addPopularity(int _count)
    {
        getOwner().getInnComponent().addPopularity(_count, getContext());
        return "ok";
    }

    @ACommand(comment = "增加心意值[数量]")
    public String addAffection(int _count)
    {
        getOwner().gainItem(RefGeneral.Ref().inn_affection_item, _count, getContext());
        return "ok";
    }

    @ACommand(comment = "增加客人[数量]")
    public String addGuest(int _count)
    {
        return getOwner().getInnComponent().addGuest(_count, getContext()).toString();
    }

    @ACommand(comment = "获取菜谱[菜品ID]")
    public String getRecipe(int _recipeId)
    {
        getOwner().getInnComponent().getDishMgr().gainItem(_recipeId,1,false,getContext());
        return "ok";
    }

    @ACommand(comment = "解锁菜品[菜品ID]")
    public String unlockDish(int _dishId)
    {
        // 检查是否已获得菜谱
        InnDishInfo existingDish = getOwner().getInnComponent().getDishMgr().lookupDish(_dishId);
        boolean hasRecipe = existingDish != null && existingDish.getHadGainRecipe();
        if (!hasRecipe)
            getRecipe(_dishId);

        if (existingDish == null)
            return "fail";

        return getOwner().getInnComponent().getDishMgr().cheatUnlockDish(_dishId, getContext()).toString();
    }

    @ACommand(comment = "增加菜品熟练度[菜品ID][数量]")
    public String addDishFinesse(int _dishId,long _count)
    {
        InnDishInfo dishInfo = getOwner().getInnComponent().getDishMgr().lookupDish(_dishId);
        if (dishInfo == null)
            return InnErr.INN_DISH_NOT_FOUND.toString();
        dishInfo.addFinesse(_count, getContext());
        return "ok";
    }

    @ACommand(comment = "解锁设备[设备ID]")
    public String unlockStation(long _stationId)
    {
        RefInnStation refInnStation = RefInnStation.getMgr().get(_stationId);
        if (refInnStation == null)
            return CommErr.REF_NOT_FOUND.toString();

        return getOwner().getInnComponent().getStationMgr().build(_stationId, refInnStation).toString();
    }

    @ACommand(comment = "设置设备等级[设备ID][等级]")
    public String setStationLevel(long _stationId,int _level)
    {
        InnStationInfo stationInfo = getOwner().getInnComponent().getStationMgr().lookupStation(_stationId);
        if (stationInfo == null)
            return InnErr.INN_STATION_NOT_FOUND.toString();

        return stationInfo.cheatSetLevel(_level).toString();
    }

    @ACommand(comment = "获得所有特殊客人首次接待珍宝奖励")
    public String giveAllFirstGainGift()
    {
        getOwner().getInnComponent().getSpecialGuestMgr().cheatServeAllSpecialGuests(getContext());
        return "ok";
    }

    @ACommand(comment = "解锁所有菜品")
    public String unlockAllDish()
    {
        for (RefInnDish refDish : RefInnDish.getMgr().getList())
        {
            long dishId = refDish.id;
            InnDishInfo existingDish = getOwner().getInnComponent().getDishMgr().lookupDish(dishId);
            if (existingDish == null || !existingDish.getHadGainRecipe())
                getOwner().getInnComponent().getDishMgr().gainItem(dishId, 1, false, getContext());
            getOwner().getInnComponent().getDishMgr().cheatUnlockDish(dishId, getContext());
        }
        return "ok";
    }
}
