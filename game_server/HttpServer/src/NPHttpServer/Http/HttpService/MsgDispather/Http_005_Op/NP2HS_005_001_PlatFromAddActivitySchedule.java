package NPHttpServer.Http.HttpService.MsgDispather.Http_005_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.Common_IntList;
import Common.ScheduleObj.Schedule_ActivityInfo;
import Common.ScheduleObj.Schedule_GroupInfo;
import Common.ScheduleObj.Schedule_PhpInfo;
import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_013_CmdAddActivitySchedule;
import NP2SS_RB.p001_ScheduleOp.NP2SS_RB_001_013_CmdAddActivitySchedule;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.EntityAddActivitySchedule;
import NPHttpServer.Http.HttpService.AutoDecoder.JsonDecoderFactory;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

public class NP2HS_005_001_PlatFromAddActivitySchedule
    extends _ANPPlatFormHttpSubDealer<EntityAddActivitySchedule>
{

    @Override
    public int subOrder()
    {
        return 1;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<EntityAddActivitySchedule> getDecoder()
    {
        return JsonDecoderFactory.getDecoder(EntityAddActivitySchedule.class);
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, EntityAddActivitySchedule _decodeObj)
    {
        if (_decodeObj.getUsTypeIdList().isEmpty())
        {
            _commiter.commitFail(CommErr.PARAM_ERROR);
            return;
        }

        Schedule_PhpInfo scheduleInfo = new Schedule_PhpInfo();
        scheduleInfo.setPhpScheduleId(-1);
        scheduleInfo.setPrePushTimeMs(0);

        Schedule_ActivityInfo scheduleActivity = new Schedule_ActivityInfo();
        scheduleActivity.setActivityId(_decodeObj.getLaunchId());
        long startTimeMs = CommonFunc.simpleDateFormatTimeMs(_decodeObj.getStartTime());
        scheduleActivity.setStartTimeMs(startTimeMs);
        long endTimeMs = CommonFunc.simpleDateFormatTimeMs(_decodeObj.getEndTime());
        scheduleActivity.setEndTimeMs(endTimeMs);
        long closeTimeMs = CommonFunc.simpleDateFormatTimeMs(_decodeObj.getShowEndTime()) ;
        scheduleActivity.setCloseTimeMs(closeTimeMs);
        scheduleInfo.setActivity(scheduleActivity);

        Schedule_GroupInfo scheduleGroupInfo = new Schedule_GroupInfo();
        for (Integer usId : _decodeObj.getUsTypeIdList())
        {

            Common_IntList usIdList = new Common_IntList();
            usIdList.addValueList(usId);
            scheduleGroupInfo.addUsGroupList(usIdList);
        }
        scheduleInfo.addGroupList(scheduleGroupInfo);

        //发送到ScheduleServer
        NPHttpServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.SCHEDULE.ordinal(),
                new NP2SS_R_001_013_CmdAddActivitySchedule(scheduleInfo, true), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2SS_RB_001_013_CmdAddActivitySchedule();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                        // 构造最终响应
                        JsonObject response = new JsonObject();
                        JsonArray activityArray = new JsonArray();
                        for (Integer usId : _decodeObj.getUsTypeIdList())
                        {
                            JsonObject item = new JsonObject();
                            item.addProperty("is_succ", true);
                            item.addProperty("us_type_id", usId);
                            item.addProperty("msg", "ok");
                            activityArray.add(item);
                        }
                        response.add("ret_list", activityArray);
                        _commiter.commitSuc("", response.toString());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _commiter.commitFail(_errCode);
                    }
                });
    }
}
