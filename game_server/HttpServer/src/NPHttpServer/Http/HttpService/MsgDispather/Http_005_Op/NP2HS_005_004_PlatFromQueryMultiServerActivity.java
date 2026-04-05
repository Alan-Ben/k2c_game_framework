package NPHttpServer.Http.HttpService.MsgDispather.Http_005_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ScheduleObj.Schedule_SimpleActivityInfo;
import Common.ScheduleObj.Schedule_SimpleActivityInfoByUs;
import NP2SS_R.p001_ScheduleOp.ToSS_R_001_015_QueryActivityByUsAndActivityId;
import NP2SS_RB.p001_ScheduleOp.ToSS_RB_001_015_QueryActivityByUsAndActivityId;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.EntityQueryMultiServerActivity;
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

/**
 * 协议5-4：查询多服务器指定活动状态
 *
 * 功能：查询多个UserServer和ScheduleServer中指定活动ID的状态
 *
 * 执行流程：
 * 1. 并发查询所有UserServer的已激活活动
 * 2. 查询ScheduleServer获取指定活动在所有US的排期信息
 * 3. 在各服务器的活动列表中查找指定活动
 * 4. 使用AtomicInteger计数等待所有响应完成
 * 5. 聚合所有结果后统一返回
 */
public class NP2HS_005_004_PlatFromQueryMultiServerActivity
    extends _ANPPlatFormHttpSubDealer<EntityQueryMultiServerActivity>
{
    @Override
    public int subOrder()
    {
        return 4;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<EntityQueryMultiServerActivity> getDecoder()
    {
        return JsonDecoderFactory.getDecoder(EntityQueryMultiServerActivity.class);
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, EntityQueryMultiServerActivity _decodeObj)
    {
        JsonArray resultArray = new JsonArray();

        // 查询ScheduleServer中所有US的待激活排期
        queryScheduleServerActivity(_commiter, _decodeObj, resultArray);
    }

    /**
     * 查询ScheduleServer中所有UserServer的待激活排期
     */
    private void queryScheduleServerActivity(NPPlatFormCommiter _commiter,
                                            EntityQueryMultiServerActivity _decodeObj,
                                            JsonArray resultArray)
    {
        // 构造请求协议，传入usId列表和activityId
        ToSS_R_001_015_QueryActivityByUsAndActivityId request =
            new ToSS_R_001_015_QueryActivityByUsAndActivityId();
        request.getUsIdList().addAll(_decodeObj.getUsTypeIdList());
        request.setActivityId(_decodeObj.getActivityId());

        NPHttpServer.getInstance().sendRequestToBSServer(
            EServerType.SINGLE.ordinal(),
            ENPSingleServerType.SCHEDULE.ordinal(),
            request,
            new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new ToSS_RB_001_015_QueryActivityByUsAndActivityId();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _retProto)
                {
                    ToSS_RB_001_015_QueryActivityByUsAndActivityId ssRet =
                        (ToSS_RB_001_015_QueryActivityByUsAndActivityId) _retProto;

                    // 使用Set记录返回的usId
                    java.util.Set<Integer> returnedUsIds = new java.util.HashSet<>();

                    // ScheduleServer返回的活动列表，每个活动都需要添加到结果中
                    for (Schedule_SimpleActivityInfoByUs activity : ssRet.getActivityList())
                    {
                        JsonObject serverResult = new JsonObject();
                        serverResult.addProperty("us_type_id", activity.getUsId());
                        serverResult.addProperty("is_succ", true);
                        serverResult.addProperty("msg", "success");
                        serverResult.add("activityInfo", convertSimpleInfoToJson(activity.getActivityInfo()));
                        resultArray.add(serverResult);
                        returnedUsIds.add(activity.getUsId());
                    }

                    // 为没有返回数据的usId填充默认信息
                    for (Integer usId : _decodeObj.getUsTypeIdList())
                    {
                        if (!returnedUsIds.contains(usId))
                        {
                            JsonObject serverResult = new JsonObject();
                            serverResult.addProperty("us_type_id", usId);
                            serverResult.addProperty("is_succ", false);
                            serverResult.addProperty("msg", "activity not found for activityId:" + _decodeObj.getActivityId());
                            serverResult.add("activityInfo", createEmptyActivityInfo());
                            resultArray.add(serverResult);
                        }
                    }

                    JsonObject response = new JsonObject();
                    response.add("activityList", resultArray);
                    _commiter.commitSuc("", response.toString());
                }

                @Override
                public void dealFail(int _errCode)
                {
                    JsonObject response = new JsonObject();
                    response.add("activityList", resultArray);
                    _commiter.commitSuc("", response.toString());
                }
            });
    }


    /**
     * 转换活动为JSON
     */
    private JsonObject convertSimpleInfoToJson(Schedule_SimpleActivityInfo simpleInfo)
    {
        JsonObject obj = new JsonObject();
        obj.addProperty("activity_id", simpleInfo.getActivityId());
        obj.addProperty("launchId", simpleInfo.getLaunchId());
        obj.addProperty("start_time", CommonFunc.getTimeStringMs(simpleInfo.getStartTimeMs()));
        obj.addProperty("end_time", CommonFunc.getTimeStringMs(simpleInfo.getEndTimeMs()));
        obj.addProperty("show_end_time", CommonFunc.getTimeStringMs(simpleInfo.getCloseTimeMs()));
        obj.addProperty("is_hide", false);
        obj.addProperty("state", simpleInfo.getState());
        return obj;
    }

    /**
     * 创建空的活动信息（用于没有找到活动的情况）
     */
    private JsonObject createEmptyActivityInfo()
    {
        JsonObject obj = new JsonObject();
        obj.addProperty("activity_id", 0);
        obj.addProperty("launchId", 0);
        obj.addProperty("start_time", "");
        obj.addProperty("end_time", "");
        obj.addProperty("show_end_time", "");
        obj.addProperty("is_hide", false);
        obj.addProperty("state", 0);
        return obj;
    }
}
