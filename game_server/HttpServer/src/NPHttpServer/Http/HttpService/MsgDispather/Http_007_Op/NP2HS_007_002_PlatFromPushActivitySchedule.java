package NPHttpServer.Http.HttpService.MsgDispather.Http_007_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ScheduleObj.Schedule_PhpInfo;
import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_013_CmdAddActivitySchedule;
import NP2SS_RB.p001_ScheduleOp.NP2SS_RB_001_013_CmdAddActivitySchedule;
import NPHttpServer.Http.HttpService.Decoder.PlatFormPushActivityScheduleDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class NP2HS_007_002_PlatFromPushActivitySchedule extends _ANPPlatFormHttpSubDealer<Schedule_PhpInfo>
{

    @Override
    public int subOrder()
    {
        return 2;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<Schedule_PhpInfo> getDecoder()
    {
        return PlatFormPushActivityScheduleDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, Schedule_PhpInfo _decodeObj)
    {
        //发送到ScheduleServer
        NPHttpServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.SCHEDULE.ordinal(),
                new NP2SS_R_001_013_CmdAddActivitySchedule(_decodeObj, false), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2SS_RB_001_013_CmdAddActivitySchedule();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                        _commiter.commitSuc();
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _commiter.commitFail(_errCode);
                    }
                });
    }
}
