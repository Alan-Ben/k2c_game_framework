package ActivitiesV01.Activities.RegularActivity;

import ActivitiesV01.Activities.RegularActivity.Player.RegularActivityPlayerMgr;
import ActivitiesV01.Bo.RegularActivityPlayerInfoBO;
import ActivitiesV01.Bo.RegularActivityPlayerShopBuyRecordBO;
import CommonEnum.ECommonActivityType;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.ActivityBaseBO;

import java.util.List;

public class RegularActivity extends _AActivityBase
{
    public static final int s_typeId = ECommonActivityType.REGULAR_EVENT.ordinal();
    private RegularActivityPlayerMgr _m_playerMgr;

    public RegularActivity(NPUserServer _server, ActivityBaseBO _bo)
    {
        super(_server, _bo);
        _m_playerMgr = new RegularActivityPlayerMgr(this);
    }

    public RegularActivityPlayerMgr getPlayerMgr()
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
        List<RegularActivityPlayerInfoBO> playerBoList = getUSServer().getBM().getBM(RegularActivityPlayerInfoBO.class).s_findAll("instance_id", getInstanceId());
        if (playerBoList == null)
        {
            USLog.error(getUSServer(), "RegularActivity init failed, playerBoList is null, instanceId: " + getInstanceId());
            return false;
        }
        for (RegularActivityPlayerInfoBO playerBo : playerBoList)
        {
            _m_playerMgr.initShopInfoFromDB(playerBo);
        }

        //初始化玩家购买记录
        List<RegularActivityPlayerShopBuyRecordBO> buyRecordBoList = getUSServer().getBM().getBM(RegularActivityPlayerShopBuyRecordBO.class).s_findAll("instance_id", getInstanceId());
        if (buyRecordBoList == null)
        {
            USLog.error(getUSServer(), "RegularActivity init failed, buyRecordBoList is null, instanceId: " + getInstanceId());
            return false;
        }
        for (RegularActivityPlayerShopBuyRecordBO buyRecordBO : buyRecordBoList)
        {
            _m_playerMgr.initShopBuyRecordFromDB(buyRecordBO);
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

    }

    @Override
    protected void _discard()
    {
        getUSServer().getBM().getBM(RegularActivityPlayerInfoBO.class).delAll("instance_id", getInstanceId());
        getUSServer().getBM().getBM(RegularActivityPlayerShopBuyRecordBO.class).delAll("instance_id", getInstanceId());
    }
}
