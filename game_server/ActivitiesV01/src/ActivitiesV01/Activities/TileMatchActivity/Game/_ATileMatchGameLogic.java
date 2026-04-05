package ActivitiesV01.Activities.TileMatchActivity.Game;

import ActivitiesV01.Activities.TileMatchActivity.Game.Logic.ETileMatchLogicEnum;

public abstract class _ATileMatchGameLogic
{
    /**
     * 逻辑类型
     */
    public abstract ETileMatchLogicEnum type();

    /**
     * 执行逻辑
     * @param _blockList   方格数据源
     * @param _gameContext 游戏上下文数据
     */
    public abstract void runLogic(TileMatchBlockList _blockList, TileMatchGameLogicContext _gameContext);
}
