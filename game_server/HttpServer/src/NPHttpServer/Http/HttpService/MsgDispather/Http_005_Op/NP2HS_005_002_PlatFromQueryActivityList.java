package NPHttpServer.Http.HttpService.MsgDispather.Http_005_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ScheduleObj.Schedule_SimpleActivityInfo;
import NP2SS_R.p001_ScheduleOp.ToSS_R_001_014_QueryActivityByUs;
import NP2SS_RB.p001_ScheduleOp.ToSS_RB_001_014_QueryActivityByUs;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.EntityQueryActivityList;
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

import java.util.ArrayList;
import java.util.List;

/**
 * 协议5-2：查询指定服务器的活动列表
 *
 * 功能：整合ScheduleServer的待激活排期和UserServer的已激活活动
 *
 * 执行流程：
 * 1. 发送RPC请求到ScheduleServer查询待激活排期
 * 2. 在回调中将待激活排期转换为JSON格式
 * 3. 继续发送RPC请求到UserServer查询已激活活动
 * 4. 聚合两部分数据并返回
 */
public class NP2HS_005_002_PlatFromQueryActivityList
    extends _ANPPlatFormHttpSubDealer<EntityQueryActivityList>
{

    @Override
    public int subOrder()
    {
        return 2;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<EntityQueryActivityList> getDecoder()
    {
        return JsonDecoderFactory.getDecoder(EntityQueryActivityList.class);
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, EntityQueryActivityList _decodeObj)
    {
        // 第一步：查询ScheduleServer获取待激活排期
        NPHttpServer.getInstance().sendRequestToBSServer(
            EServerType.SINGLE.ordinal(),
            ENPSingleServerType.SCHEDULE.ordinal(),
            new ToSS_R_001_014_QueryActivityByUs(_decodeObj.getUsTypeId()),
            new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new ToSS_RB_001_014_QueryActivityByUs();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _retProto)
                {
                    ToSS_RB_001_014_QueryActivityByUs ssRet =
                        (ToSS_RB_001_014_QueryActivityByUs) _retProto;

                    // 转换待激活排期为JSON列表
                    List<JsonObject> resultList = new ArrayList<>();
                    for (Schedule_SimpleActivityInfo simpleInfo : ssRet.getActivityList())
                    {
                        resultList.add(convertSimpleInfoToJson(simpleInfo));
                    }

                    // 构造最终响应
                    JsonObject response = new JsonObject();
                    JsonArray activityArray = new JsonArray();
                    for (JsonObject obj : resultList)
                    {
                        activityArray.add(obj);
                    }
                    response.add("activity_list", activityArray);
                    _commiter.commitSuc("", response.toString());
                }

                @Override
                public void dealFail(int _errCode)
                {
                    _commiter.commitFail(_errCode);
                }
            });
    }

    /**
     * 转换简化活动信息为JSON
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
}
