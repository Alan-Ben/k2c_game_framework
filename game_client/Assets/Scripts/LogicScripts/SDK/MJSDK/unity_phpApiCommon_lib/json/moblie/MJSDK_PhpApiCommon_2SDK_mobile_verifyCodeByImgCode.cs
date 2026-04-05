using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// [SDK->Engine] mobile-verifyCodeByImgCode：通过图片验证码获取手机验证码
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_mobile_verifyCodeByImgCode
    {
        //图形验证码的值
        public string v_code;
        //校验md5
        public string v_code_md5;
        //手机号码
        public string mobile;
    }
}