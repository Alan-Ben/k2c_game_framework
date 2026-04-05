package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import Common.MarsObj.Mars_BuildingGainEnergyResult;
import GC2GS.p039_MarsBuildingOp.GC2GS_039_006_ReqGetBuildingEnergy;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;

import java.util.ArrayList;

public class MsgDealer_GC2GS_039_006_ReqGetBuildingEnergy extends NPUserMsgDealer<GC2GS_039_006_ReqGetBuildingEnergy>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_006_ReqGetBuildingEnergy _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        ArrayList<Mars_BuildingGainEnergyResult> list = new ArrayList<>();
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_GAIN_ALL_ENERGY);
        userData.getMarsBuildingComponent().gainAllBuildingEnergy(list, context);

        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_006_RetGetBuildingEnergy(list));
        
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_GET_ENERGY_COUNT, 1, context);
    }
}
