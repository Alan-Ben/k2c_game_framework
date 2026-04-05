
namespace MJSDK_Package
{
    public class MJSDK_Mgr_Mark
    {
        public static string MJSDK_Namespace = "MJSDK_Package.";

        /// <summary>
        /// 获取服务端支付类型标记
        /// /summary>
        public static string getMJSDKLibPayMainOrder(E_MJSDK_PayType payType)
        {
            string payTypeStr = payType.ToString();
            if (payType == E_MJSDK_PayType.huaweipay)
            {
                payTypeStr = "huawei";
            }
            if (payType == E_MJSDK_PayType.mipay)
            {
                payTypeStr = "xiaomi";
            }
            return payTypeStr;
        }

        /// <summary>
        /// 获取MJSDK库中账号主订单标记
        /// /summary>
        public static string getMJSDKLibAccountMainOrder(E_MJSDK_AccountType accountType)
        {
            string accountTypeStr = accountType.ToString();
            if (accountType == E_MJSDK_AccountType.mj)
            {
                accountTypeStr = "mjacc";
            }
            else if (accountType == E_MJSDK_AccountType.wechat)
            {
                accountTypeStr = "wx";
            }
            else if (accountType == E_MJSDK_AccountType.ios)
            {
                return "apple";
            }
            return accountTypeStr;
        }
    }


    /// <summary>
    /// 账号类型
    /// </summary>
    public enum E_MJSDK_AccountType
    {
        [EnumBindString("MJSDK_PhpApi_Common")]
        guest,     //游客
        [EnumBindString("MJSDK_GameCenter")]
        gamecenter,//游戏圈账号登录  -- iOS支持 android不支持
        [EnumBindString("MJSDK_AppleLogin")]
        ios,     //苹果账号登录    -- iOS支持 android不支持
        [EnumBindString("MJSDK_Google")]
        google,    //谷歌账号登录    -- iOS不支持 android支持
        [EnumBindString("MJSDK_Facebook")]
        facebook,  //facebook账号登录
        [EnumBindString("MJSDK_VK")]
        vk,        //vk账号登录
        [EnumBindString("MJSDK_Line")]
        line,      //line账号登录
        [EnumBindString("MJSDK_Twitter")]
        twitter,       //twitter账号登录
        [EnumBindString("MJSDK_QQ")]
        qq,            //qq账号登录
        [EnumBindString("MJSDK_WX")]
        wechat,        //wx账号登录
        [EnumBindString("MJSDK_MJAcc|MJSDK_Package.MJSDK_MJAcc_2SDK_mjacc_userInfo")]
        mj,             //梦加账号账号登录
        [EnumBindString("MJSDK_Amazon")]
        amazon,        //amazon账号登录
        [EnumBindString("MJSDK_TapTap")]
        taptap,        //taptap账号登录
    }

    /// <summary>
    /// 支付类型
    /// </summary>
    public enum E_MJSDK_PayType
    {
        [EnumBindString("MJSDK_ApplePay")]
        apple = 0,      //苹果支付       -- Android支持 iOS不支持
        [EnumBindString("MJSDK_Google")]
        google = 1,     //google支付    -- iOS支持 android不支持
        [EnumBindString("MJSDK_Huawei")]
        huaweipay = 2,     //华为支付    -- Android支持 iOS不支持
        [EnumBindString("MJSDK_Xiaomi")]
        mipay = 3,     //小米支付    -- Android支持 iOS不支持
        [EnumBindString("MJSDK_Samsung")]
        samsung = 4,     //samsung支付    -- Android支持 iOS不支持
        [EnumBindString("MJSDK_OneStore")]
        onestore = 5,     //OneStore支付    -- Android支持 iOS不支持
        [EnumBindString("MJSDK_Amazon")]
        amazon = 6,     //amazon支付    -- Android支持 iOS不支持
        [EnumBindString("MJSDK_RuStore")]
        rustore = 7     //RuStore支付    -- Android支持 iOS不支持
    }


    /// <summary>
    /// 支付商品类型
    /// </summary>
    public enum E_MJSDK_PayProductType
    {
        [EnumBindString("inapp")]
        inapp = 0,      //消耗性商品
        [EnumBindString("subs")]
        subs = 1,       //订阅型商品
    }


    /// <summary>
    /// 语言类型
    /// </summary>
    public enum E_MJSDK_LanType
    {
        [EnumBindString("英语")]
        en,
        [EnumBindString("中文简体")]
        zh_CN,
        [EnumBindString("中文繁体")]
        zh_TW,
        [EnumBindString("Korea(韩语)")]
        ko,
        [EnumBindString("Japanese(日语)")]
        ja,
        [EnumBindString("Россия(俄语)")]
        ru,
        [EnumBindString("阿拉伯")]
        ar,
        [EnumBindString("土耳其")]
        tr,
        [EnumBindString("德语")]
        de,
        [EnumBindString("法语")]
        fr,
        [EnumBindString("西班牙语")]
        es,
        [EnumBindString("葡萄牙语")]
        pt,
        [EnumBindString("意大利语")]
        it,
        [EnumBindString("越南")]
        vi,
        [EnumBindString("印尼语")]
        id,
        [EnumBindString("波兰")]
        pl,
        [EnumBindString("泰国")]
        th,
        [EnumBindString("希腊文")]
        el,
        [EnumBindString("波斯语")]
        fa,
        [EnumBindString("印地语")]
        hi,
        [EnumBindString("匈牙利语")]
        hu,
        [EnumBindString("马来语")]
        ms,
        [EnumBindString("缅甸语")]
        my,
        [EnumBindString("荷兰语")]
        nl,
        [EnumBindString("罗马尼亚语")]
        ro,
        [EnumBindString("瑞典语")]
        sv,
        [EnumBindString("泰米尔语")]
        ta,
        [EnumBindString("泰卢固语")]
        te,
        [EnumBindString("塔加路语（菲律宾语）")]
        tl,
    }


    /// <summary>
    /// 支付商品类型
    /// </summary>
    public enum E_MJSDK_Third_Trace_Type
    {
        [EnumBindString("MJSDK_FacebookLib")]
        facebook,        //facebook 事件
        [EnumBindString("MJSDK_AppsflyerLib")]
        appsflyer,       //appsflyer 事件
        [EnumBindString("MJSDK_FirebaseLib")]
        firebase,        //firebase 事件

        [EnumBindString("")]
        all,            //所有事件;
    }
}
