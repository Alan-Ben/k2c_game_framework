package ActivitiesV01.Cmds;


import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchPlayerGameInfo;
import ActivitiesV01.Activities.TileMatchActivity.TileMatchActivity;
import ActivitiesV01.Activities.TileMatchActivity.TileMatchPlayerInfo;
import CommonEnum.ECommonActivityType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "三消", name = "tilematch")
public class CmdTileMatch extends UsCmdBase
{
    @ACommand(comment = "打印地图[模式]")
    public String print(ETileMatch_ModeType _mode)
    {
        TileMatchActivity activity = getUserServer().getCommActivityMgr().lookupOneActivityByType(ECommonActivityType.TILE_MATCH, TileMatchActivity.class);
        if (activity == null || !activity.isRunning())
            return "activity is not running!";

        TileMatchPlayerInfo playerInfo = activity.getPlayerMgr().ensure(getOwner().getCid());
        //获取游戏信息
        ResultOne<TileMatchPlayerGameInfo> gameInfoResult = playerInfo.ensureGame(getOwner(), _mode, getContext());
        if (!gameInfoResult.isSucc())
            return "game data not found!";

        return gameInfoResult.getData().toString();
    }

    @ACommand(comment = "设置打印分步日志[模式][开启]")
    public String setPrint(ETileMatch_ModeType _mode, boolean _open)
    {
        TileMatchActivity activity = getUserServer().getCommActivityMgr().lookupOneActivityByType(ECommonActivityType.TILE_MATCH, TileMatchActivity.class);
        if (activity == null || !activity.isRunning())
            return "activity is not running!";

        TileMatchPlayerInfo playerInfo = activity.getPlayerMgr().ensure(getOwner().getCid());
        //获取游戏信息
        ResultOne<TileMatchPlayerGameInfo> gameInfoResult = playerInfo.ensureGame(getOwner(), _mode, getContext());
        if (!gameInfoResult.isSucc())
            return "game data not found!";

        gameInfoResult.getData().openPrint(_open);
        return "ok";
    }

    @ACommand(comment = "重置地图[模式]")
    public String reset(ETileMatch_ModeType _mode)
    {
        TileMatchActivity activity = getUserServer().getCommActivityMgr().lookupOneActivityByType(ECommonActivityType.TILE_MATCH, TileMatchActivity.class);
        if (activity == null || !activity.isRunning())
            return "activity is not running!";

        TileMatchPlayerInfo playerInfo = activity.getPlayerMgr().ensure(getOwner().getCid());
        //获取游戏信息
        ResultOne<TileMatchPlayerGameInfo> gameInfoResult = playerInfo.ensureGame(getOwner(), _mode, getContext());
        if (!gameInfoResult.isSucc())
            return "game data not found!";

        return gameInfoResult.getData().resetMap(getOwner(),getContext()).toString();
    }

    @ACommand(comment = "修改格子[模式][索引][方块ID][原方块ID]")
    public String chgBlock(ETileMatch_ModeType _mode, int _index, int _blockId, int _fromBlockId)
    {
        TileMatchActivity activity = getUserServer().getCommActivityMgr().lookupOneActivityByType(ECommonActivityType.TILE_MATCH, TileMatchActivity.class);
        if (activity == null || !activity.isRunning())
            return "activity is not running!";

        TileMatchPlayerInfo playerInfo = activity.getPlayerMgr().ensure(getOwner().getCid());
        //获取游戏信息
        ResultOne<TileMatchPlayerGameInfo> gameInfoResult = playerInfo.ensureGame(getOwner(), _mode, getContext());
        if (!gameInfoResult.isSucc())
            return "game data not found!";

        gameInfoResult.getData().chgBlock(getOwner(), _index, _blockId, _fromBlockId, getContext());
        return "ok";
    }

    @ACommand(comment = "设置当前任务已走步数[模式][步数]")
    public String setHadGoStep(ETileMatch_ModeType _mode, int _step)
    {
        TileMatchActivity activity = getUserServer().getCommActivityMgr().lookupOneActivityByType(ECommonActivityType.TILE_MATCH, TileMatchActivity.class);
        if (activity == null || !activity.isRunning())
            return "activity is not running!";

        TileMatchPlayerInfo playerInfo = activity.getPlayerMgr().ensure(getOwner().getCid());
        //获取游戏信息
        ResultOne<TileMatchPlayerGameInfo> gameInfoResult = playerInfo.ensureGame(getOwner(), _mode, getContext());
        if (!gameInfoResult.isSucc())
            return "game data not found!";

        gameInfoResult.getData().setHadGoStep(getOwner(), _step);
        return "ok";
    }

    @ACommand(comment = "增加分数[分数]")
    public String addScore(int _score)
    {
        TileMatchActivity activity = getUserServer().getCommActivityMgr().lookupOneActivityByType(ECommonActivityType.TILE_MATCH, TileMatchActivity.class);
        if (activity == null || !activity.isRunning())
            return "activity is not running!";

        TileMatchPlayerInfo playerInfo = activity.getPlayerMgr().ensure(getOwner().getCid());
        playerInfo.addScore(getOwner(), _score, getContext());

        return "ok";
    }

}
