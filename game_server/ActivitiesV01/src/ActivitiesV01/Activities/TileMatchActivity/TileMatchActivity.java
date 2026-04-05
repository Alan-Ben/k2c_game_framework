package ActivitiesV01.Activities.TileMatchActivity;

import ActivitiesV01.Bo.TileMatchPlayerGameInfoBO;
import ActivitiesV01.Bo.TileMatchPlayerInfoBO;
import CommonEnum.ECommonActivityType;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.ActivityBaseBO;

import java.util.List;

public class TileMatchActivity extends _AActivityBase
{
    public static final int s_typeId = ECommonActivityType.TILE_MATCH.ordinal();

    private final TileMatchPlayerMgr _m_playerMgr;

    public TileMatchActivity(NPUserServer _server, ActivityBaseBO _bo)
    {
        super(_server, _bo);
        _m_playerMgr = new TileMatchPlayerMgr(this);
    }

    public TileMatchPlayerMgr getPlayerMgr()
    {
        return _m_playerMgr;
    }

    @Override
    public int getActivityTypeId()
    {
        return s_typeId;
    }

    @Override
    protected boolean _subInitNew()
    {
        return true;
    }

    @Override
    protected boolean _subInitStatic()
    {
        //初始化玩家管理器
        List<TileMatchPlayerInfoBO> playerBoList = getUSServer().getBM().getBM(TileMatchPlayerInfoBO.class).s_findAll("activity_instance_id", getInstanceId());
        if (playerBoList == null)
        {
            USLog.error(getUSServer(), "TileMatchActivity init failed, playerBoList is null, instanceId: " + getInstanceId());
            return false;
        }
        for (TileMatchPlayerInfoBO playerBo : playerBoList)
        {
            _m_playerMgr.initFromDB(playerBo);
        }

        //初始化玩家游戏数据
        List<TileMatchPlayerGameInfoBO> gameBoList = getUSServer().getBM().getBM(TileMatchPlayerGameInfoBO.class).s_findAll("activity_instance_id", getInstanceId());
        if (gameBoList == null)
        {
            USLog.error(getUSServer(), "TileMatchActivity init failed, gameBoList is null, instanceId: " + getInstanceId());
            return false;
        }
        for (TileMatchPlayerGameInfoBO gameBo : gameBoList)
        {
            _m_playerMgr.initGameFromDB(gameBo);
        }
        return true;
    }

    @Override
    protected void _regExtraEvent()
    {

    }

    @Override
    protected void _onActivityStart()
    {

    }

    @Override
    protected void _onActivityEnd()
    {

    }

    @Override
    protected void _onActivityClosed()
    {
        //补发未领取的阶段奖励
        _m_playerMgr.dispatchUnclaimedStepRewards();
    }

    @Override
    protected void _discard()
    {
        getUSServer().getBM().getBM(TileMatchPlayerInfoBO.class).delAll("activity_instance_id", getInstanceId());
        getUSServer().getBM().getBM(TileMatchPlayerGameInfoBO.class).delAll("activity_instance_id", getInstanceId());
    }
}
