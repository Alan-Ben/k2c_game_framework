using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(user-login：用户登录)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2Engine_user_login : MJSDK_2Engine_Base
    {
        //用户ID
        public string user_id;
        //登录token，客户端自行记录，需要验证登录的接口都必须带上该参数
        public string token;
        //token到期时间，10位时间戳
        public long token_expiry;
        /*用户绑定的帐号类型与昵称格式:*/
        public List<MJSDK_Login_2Engine_account_bind_info> bind;
        //用户充值价值
        public float user_pay;
        //用户信息
        public MJSDK_Login_2Engine_user_info_user user;
        //user_id和token验证签名
        public string token_sign;
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class MJSDK_Login_2Engine_user_info_user
    {
        public string uid;//用户ID
        public string nick_name;//昵称
        public string create_ip;//注册IP
        public long create_time;//    注册时间，10位时间戳
        public string last_login_ip;//最后登录IP
        public long last_login_time;//最后登录时间，10位时间戳
        public long is_guest;//是否游客，1表示是，0表示否
        public string app_id;//
        public string version;//注册版本
        public string adid;//注册设备id
        public string nation;//注册国家代码
        public string adfrom;//1级渠道
        public string adfrom2;//2级渠道
        public string afid;//广告afid
        public int status;//账号状态,1正常,2软删冷静期,3已软删，0封禁
        public long del_time; //账号软删时间,stauts=2才会有该值
    }
}