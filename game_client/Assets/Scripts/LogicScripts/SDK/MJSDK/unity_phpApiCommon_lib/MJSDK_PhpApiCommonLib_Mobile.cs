using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MJSDK_Package
{
    /// <summary>
    /// 服务端公共接口支持组件的相关接口类  --- 手机验证码
    /// </summary>
    public class MJSDK_PhpApiCommonLib_Mobile
    {

        #region mobile_verifyCode（手机验证码）

        /// <summary>
        /// mobile-verifyCode：发送手机登录验证码
        /// </summary>
        /// <param name="_moblieNum">手机号</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void mobile_verifyCode(string _moblieNum, System.Action<MJSDK_PhpApiCommon_2Engine_mobile_verifyCode> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (string.IsNullOrEmpty(_moblieNum))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "mobile_verifyCode --> Parameters are missing,_moblieNum is null");
                }
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("mobile", _moblieNum);



            MJSDK.sendMsgToPhonePlatform("mobile", "verifyCode", ht.toJson(), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_mobile_verifyCode>(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// mobile-imgCode：获取图片验证码
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void mobile_imgCode(System.Action<MJSDK_PhpApiCommon_2Engine_mobile_imgCode> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("mobile", "imgCode", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_mobile_imgCode>(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// mobile-verifyCodeByImgCode：通过图片验证码获取手机验证码
        /// </summary>
        /// <param name="_moblieNum">手机号</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void mobile_verifyCodeByImgCode(MJSDK_PhpApiCommon_2SDK_mobile_verifyCodeByImgCode _2SDK_Mobile_VerifyCodeByImgCode, System.Action<MJSDK_PhpApiCommon_2Engine_mobile_verifyCode> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_Mobile_VerifyCodeByImgCode == null || string.IsNullOrEmpty(_2SDK_Mobile_VerifyCodeByImgCode.mobile)
                || string.IsNullOrEmpty(_2SDK_Mobile_VerifyCodeByImgCode.v_code)
                || string.IsNullOrEmpty(_2SDK_Mobile_VerifyCodeByImgCode.v_code_md5))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "mobile_verifyCodeByImgCode --> Parameters are missing,v_code/v_code_md5/mobile is null");
                }
                return;
            }



            MJSDK.sendMsgToPhonePlatform("mobile", "verifyCodeByImgCode", JsonUtility.ToJson(_2SDK_Mobile_VerifyCodeByImgCode), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_mobile_verifyCode>(_sucDelegate, _failDelegate));
        }


        #endregion
    }
}