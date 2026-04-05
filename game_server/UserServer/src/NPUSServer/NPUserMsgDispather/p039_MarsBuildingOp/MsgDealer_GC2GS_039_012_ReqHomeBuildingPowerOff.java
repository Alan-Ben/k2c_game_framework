package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_012_ReqHomeBuildingPowerOff;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_039_012_ReqHomeBuildingPowerOff extends NPUserMsgDealer<GC2GS_039_012_ReqHomeBuildingPowerOff>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_012_ReqHomeBuildingPowerOff _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_HOME_POWER_CHG);
        userData.getMarsBuildingComponent().getHomeFunc().setPowerOff(context);
    }
}
