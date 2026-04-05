package NPUSServer.CommonActivityMgr.Activities.EmptyActivity;

import CommonEnum.ECommonActivityType;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import USDB.Bo.ActivityBaseBO;

/**
 * 冲榜活动
 */
public class SevenDayGoalsActivity extends _AActivityBase
{
    public SevenDayGoalsActivity(NPUserServer _server, ActivityBaseBO _bo)
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
        return ECommonActivityType.SEVEN_DAY_GOALS.ordinal();
    }

    @Override
    protected void _discard()
    {
    }
}
