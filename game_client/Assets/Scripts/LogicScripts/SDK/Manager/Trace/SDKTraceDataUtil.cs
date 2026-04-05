using ALPackage;
using MJSDK_Package;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 埋点工具类
    /// </summary>
    public static class SDKTraceDataUtil
    {
        /// <summary>
        /// 获取激活埋点信息
        /// </summary>
        /// <returns></returns>
        public static MJSDK_PhpApiCommon_2SDK_trace_gameActivty getActivateData(string _extend = "")
        {
            MJSDK_PhpApiCommon_2SDK_trace_gameActivty traceInfo = new MJSDK_PhpApiCommon_2SDK_trace_gameActivty();
            traceInfo.platform_id = CDNSetting_ClientConfigInfo.instance.platformId.ToString();
            traceInfo.uid = Game.instance.uid != null ? Game.instance.uid : "0"; ;
            traceInfo.cid = (NPPlayer.instance != null && NPPlayer.instance.playerInfo != null) ? NPPlayer.instance.playerInfo.CID.ToString() : "0";
            traceInfo.sdkId = SDKMgr.instance.appsflyerId;
            traceInfo.extend = _extend;
            traceInfo.login_tag = Application.version;

            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【MJSDK】【Trace】======>激活埋点信息\n" +
                          $"┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈\n" +
                          $"┊deviceId:{SDKMgr.instance.deviceId}\n" +
                          $"┊platform_id:{traceInfo.platform_id}\n" +
                          $"┊uid:{traceInfo.uid}\n" +
                          $"┊cid:{traceInfo.cid}\n" +
                          $"┊sdkId:{traceInfo.sdkId}\n" +
                          $"┊login_tag:{traceInfo.login_tag}\n" +
                          $"┊extend:{traceInfo.extend}\n" +
                          $"└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈");
            }

            return traceInfo;
        }

        /// <summary>
        /// 获取事件埋点信息
        /// </summary>
        /// <returns></returns>
        public static MJSDK_PhpApiCommon_2SDK_trace_gameStep getStepData(TraceStepData _stepData)
        {
            MJSDK_SDKEvent_2SDK_step_info stepInfo = new MJSDK_SDKEvent_2SDK_step_info();
            stepInfo.uid = Game.instance.uid != null ? Game.instance.uid : "0";
            stepInfo.cid = (NPPlayer.instance != null && NPPlayer.instance.playerInfo != null) ? NPPlayer.instance.playerInfo.CID.ToString() : "0";
            stepInfo.step_id = _stepData != null ? _stepData.ID : 0;
            stepInfo.op_time = ALCommon.getNowTimeSec().ToString();

            MJSDK_PhpApiCommon_2SDK_trace_gameStep traceInfo = new MJSDK_PhpApiCommon_2SDK_trace_gameStep();
            traceInfo.platform_id = CDNSetting_ClientConfigInfo.instance.platformId.ToString();
            traceInfo.area_id = CDNSetting_AreaInfo.instance.areaId;
            traceInfo.server_id = GameInit_SelectServer.instance.loginServerLogicId.ToString();
            traceInfo.sdkId = SDKMgr.instance.appsflyerId;
            traceInfo.step_info = stepInfo;
            traceInfo.support_gpu_instancing = SystemInfo.supportsInstancing ? "1" : "0";
            traceInfo.extend = _stepData != null ? _stepData.mark : "";
            traceInfo.mark2 = GCommon.getStepReportMark2String();
            traceInfo.mark3 = "";
            traceInfo.login_tag = Application.version;

            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【MJSDK】【Trace】======>事件埋点信息\n" +
                          $"┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈\n" +
                          $"┊deviceId:{SDKMgr.instance.deviceId}\n" +
                          $"┊uid:{stepInfo.uid}\n" +
                          $"┊cid:{stepInfo.cid}\n" +
                          $"┊step_id:{stepInfo.step_id}\n" +
                          $"┊op_time:{stepInfo.op_time}\n" +
                          $"┊platform_id:{traceInfo.platform_id}\n" +
                          $"┊area_id:{traceInfo.area_id}\n" +
                          $"┊server_id:{traceInfo.server_id}\n" +
                          $"┊sdkId:{traceInfo.sdkId}\n" +
                          $"┊support_gpu_instancing:{traceInfo.support_gpu_instancing}\n" +
                          $"┊login_tag:{traceInfo.login_tag}\n" +
                          $"┊extend:{(traceInfo.extend.Length > 80 ? traceInfo.extend.Substring(0, 80):traceInfo.extend)}\n" +
                          $"┊mark2:{traceInfo.mark2}\n" +
                          $"└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈");
            }

            return traceInfo;
        }

        /// <summary>
        /// 获取异常埋点信息
        /// </summary>
        /// <returns></returns>
        public static MJSDK_PhpApiCommon_2SDK_trace_gameErr getErrorData(E_Err_level _errLevel, string _content)
        {
            MJSDK_PhpApiCommon_2SDK_trace_gameErr traceInfo = new MJSDK_PhpApiCommon_2SDK_trace_gameErr();
            traceInfo.err_level = _errLevel.ToString();
            traceInfo.step = TraceConst.LOG_ERROR.ID.ToString();
            traceInfo.content = _content;
            traceInfo.login_tag = Application.version;

            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【MJSDK】【Trace】======>异常埋点信息\n" +
                          $"┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈\n" +
                          $"┊deviceId:{SDKMgr.instance.deviceId}\n" +
                          $"┊err_level:{traceInfo.err_level}\n" +
                          $"┊step:{traceInfo.step}\n" +
                          $"┊content:{(traceInfo.content.Length > 80 ? traceInfo.content.Substring(0, 80) : traceInfo.content)}\n" +
                          $"┊login_tag:{traceInfo.login_tag}\n" +
                          $"└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈");
            }

            return traceInfo;
        }

        /// <summary>
        /// 获取广告变现埋点信息
        /// </summary>
        /// <returns></returns>
        public static MJSDK_PhpApiCommon_2SDK_trace_gameAd getAdData(string _eventId, string _adType)
        {
            MJSDK_PhpApiCommon_2SDK_trace_gameAd traceInfo = new MJSDK_PhpApiCommon_2SDK_trace_gameAd();
            traceInfo.platform_id = CDNSetting_ClientConfigInfo.instance.platformId.ToString();
            traceInfo.area_id = CDNSetting_AreaInfo.instance.areaId;
            traceInfo.server_id = GameInit_SelectServer.instance.loginServerLogicId.ToString();
            traceInfo.event_id = _eventId;
            traceInfo.uid = Game.instance.uid != null ? Game.instance.uid : "0";
            traceInfo.cid = (NPPlayer.instance != null && NPPlayer.instance.playerInfo != null) ? NPPlayer.instance.playerInfo.CID.ToString() : "0";
            traceInfo.level = (NPPlayer.instance != null && NPPlayer.instance.playerInfo != null) ? NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LEVEL).ToString() : "0";
            traceInfo.vip_level = (NPPlayer.instance != null && NPPlayer.instance.playerInfo != null) ? NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL).ToString() : "0";
            traceInfo.source = "";
            traceInfo.ad_type = _adType;
            traceInfo.role_ct = "";
            traceInfo.ar_version = "";
            traceInfo.ar_adfrom2 = "";
            traceInfo.ar_nation = SDKMgr.instance.sysCountry;
            traceInfo.login_tag = Application.version;

            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【MJSDK】【Trace】======>广告变现埋点信息\n" +
                          $"┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈\n" +
                          $"┊deviceId:{SDKMgr.instance.deviceId}\n" +
                          $"┊platform_id:{traceInfo.platform_id}\n" +
                          $"┊area_id:{traceInfo.area_id}\n" +
                          $"┊server_id:{traceInfo.server_id}\n" +
                          $"┊event_id:{traceInfo.event_id}\n" +
                          $"┊uid:{traceInfo.uid}\n" +
                          $"┊cid:{traceInfo.cid}\n" +
                          $"┊level:{traceInfo.level}\n" +
                          $"┊vip_level:{traceInfo.vip_level}\n" +
                          $"┊source:{traceInfo.source}\n" +
                          $"┊ad_type:{traceInfo.ad_type}\n" +
                          $"┊role_ct:{traceInfo.role_ct}\n" +
                          $"┊ar_version:{traceInfo.ar_version}\n" +
                          $"┊ar_adfrom2:{traceInfo.ar_adfrom2}\n" +
                          $"┊ar_nation:{traceInfo.ar_nation}\n" +
                          $"┊login_tag:{traceInfo.login_tag}\n" +
                          $"└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈");
            }

            return traceInfo;
        }

        /// <summary>
        /// 获取第三方埋点信息
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_ext"></param>
        /// <returns></returns>
        public static MJSDK_2SDK_ThirdParty_customEvent_base getThirdPartyEventData(string _eventKey, string _ext)
        {
            MJSDK_2SDK_ThirdParty_customEvent_base info = new MJSDK_2SDK_ThirdParty_customEvent_base();
            info.event_key = _eventKey;
            info.level = NPPlayer.instance != null && NPPlayer.instance.playerInfo != null ? NPPlayer.instance.playerInfo[ENPPlayerParam.LEVEL].ToString() : "0";
            info.ext = _ext;

            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【MJSDK】【Trace】======>获取第三方埋点信息\n" +
                          $"┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈\n" +
                          $"┊event_key:{info.event_key}\n" +
                          $"┊level:{info.level}\n" +
                          $"┊ext:{info.ext}\n" +
                          $"└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈");
            }

            return info;
        }

        /// <summary>
        /// 获取Adjust埋点信息
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_ext"></param>
        /// <param name="_callbackId"></param>
        /// <returns></returns>
        public static MJSDK_Adjust_2SDK_adjust_customEvent getAdjustEventData(string _eventKey, string _ext, string _callbackId)
        {
            MJSDK_Adjust_2SDK_adjust_customEvent info = new MJSDK_Adjust_2SDK_adjust_customEvent();
            info.callbackId = _callbackId;
            info.event_key = _eventKey;
            info.level = NPPlayer.instance.playerInfo != null ? NPPlayer.instance.playerInfo[ENPPlayerParam.LEVEL].ToString() : "0";
            info.ext = _ext;

            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【MJSDK】【Trace】======>获取Adjust埋点信息\n" +
                          $"┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈\n" +
                          $"┊callbackId:{info.callbackId}\n" +
                          $"┊event_key:{info.event_key}\n" +
                          $"┊level:{info.level}\n" +
                          $"┊ext:{info.ext}\n" +
                          $"└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈");
            }

            return info;
        }
    }
}
