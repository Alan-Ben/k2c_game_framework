package NPUSServer.CommonActivityMgr.Activities.EmptyActivity;

import CommonEnum.ECommonActivityType;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import USDB.Bo.ActivityBaseBO;

/**
 * 排行榜礼包活动
 */
public class RankGiftPackActivity extends _AActivityBase
{
    public RankGiftPackActivity(NPUserServer _server, ActivityBaseBO _bo)
    {
        super(_server, _bo);
    }

    @Override
    protected boolean _subInitStatic()
    {
        return true;
    }

    @Override
    protected void _regExtraEvent()
    {

    }

    @Override
    protected boolean _subInitNew()
    {
        return true;
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
    public int getActivityTypeId()
    {
        return ECommonActivityType.RANK_GIFT_PACK.ordinal();
    }

    @Override
    protected void _discard()
    {
    }
}
