

using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    //SDK服务端消息错误模块（请求错误）
    public class MJSDK_PhpApiCommon_SDKServer_ErrorMsg
    {
        //错误信息
        [Preserve]
        public string msg;
        //错误码
        [Preserve]
        public string code;
        //扩展错误信息--SDK附带
        [Preserve]
        public string sdk_msg;

        //是否解析成功
        [Preserve]
        public bool parse_suc = false;
        //解析失败-错误描述
        [Preserve]
        public string parse_errMsg;


        /// <summary>
        /// 解析服务端错误信息
        /// </summary>
        /// <param name="errCode">SDK返回通用错误码</param>
        /// <param name="errMsg">SDK返回的错误信息</param>
        /// <returns></returns>
        public static MJSDK_PhpApiCommon_SDKServer_ErrorMsg parseServerErrorMsg(int errCode,string errMsg) {

            //初始化默认对象
            MJSDK_PhpApiCommon_SDKServer_ErrorMsg sDKServer_ErrorMsg = new MJSDK_PhpApiCommon_SDKServer_ErrorMsg();
            try
            {
                //SDK返回的错误码必须为30301
                if (errCode == MJSDK_PhpApiCommonError.C_Unity_PhpApi_LoginOrPay_Msg_DealFail && !string.IsNullOrEmpty(errMsg))
                {
                    sDKServer_ErrorMsg = JsonUtility.FromJson<MJSDK_PhpApiCommon_SDKServer_ErrorMsg>(errMsg);
                    sDKServer_ErrorMsg.parse_suc = true;
                }
                else {
                    sDKServer_ErrorMsg.parse_errMsg = "errCode=" + errCode + "   errMsg=" + errMsg;
                }
            }
            catch (System.Exception ex)
            {
                sDKServer_ErrorMsg.parse_errMsg = ex.ToString();
                MJSDK_Log.mjsdkLog("parseServerErrorMsg-->error:" + ex.ToString(), E_MJSDK_BusType.Error);
            }
            
            return sDKServer_ErrorMsg;
        }
    }
}