package NPUSServer.CommonActivityMgr.Activities.EmptyActivity;

import CommonEnum.ECommonActivityType;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import USDB.Bo.ActivityBaseBO;

/**
 * 冲榜活动
 */
public class ActivityFundActivity extends _AActivityBase
{
    public ActivityFundActivity(NPUserServer _server, ActivityBaseBO _bo)
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
        return ECommonActivityType.ACTIVITY_FUND.ordinal();
    }

    @Override
    protected void _discard()
    {
    }
}
