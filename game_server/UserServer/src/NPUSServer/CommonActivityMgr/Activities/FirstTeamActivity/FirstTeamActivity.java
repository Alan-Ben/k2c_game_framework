package NPUSServer.CommonActivityMgr.Activities.FirstTeamActivity;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ServerObj.ServerObj_FirstTeam_Player;
import CommonEnum.ECommonActivityType;
import NPGameRes.Refs.Activity.RefActivityTeam;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.ActivityBaseBO;

/**
 * 第一组队活动
 */
public class FirstTeamActivity extends _AActivityBase
{
    // 组队活动子表
    private RefActivityTeam _m_refActivityTeam;

    public FirstTeamActivity(NPUserServer _server, ActivityBaseBO _bo)
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
        return ECommonActivityType.FIRST_TEAM.ordinal();
    }

    @Override
    protected void _discard()
    {
    }

    @Override
    public _IALProtocolStructure makePlayerObj(NPUSUserData _userData)
    {
        ServerObj_FirstTeam_Player obj = new ServerObj_FirstTeam_Player();

        return obj;
    }
}

