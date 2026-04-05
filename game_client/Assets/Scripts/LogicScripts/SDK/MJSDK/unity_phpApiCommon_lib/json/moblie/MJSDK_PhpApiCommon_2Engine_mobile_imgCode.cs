using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// [SDK->Engine] mobile-imgCode：获取图片验证码
    /// </summary>
    public class MJSDK_PhpApiCommon_2Engine_mobile_imgCode
    {
        //图片的base64
        public string img_base64;
        //校验md5
        public string v_code_md5;
        //图形验证码失效时间戳
        public string expiry_time;
    }
}