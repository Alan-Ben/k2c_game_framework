using System;
using JetBrains.Annotations;

namespace GOE
{
    public class TraceStepData
    {
        private string _m_defineMark;//定义的mark

        public int ID;//埋点ID
        public int sendLevel;//打开第几等级的埋点时，会发送这个埋点，埋点发送等级为3时，等级123的埋点都会发送
        public string mark;//埋点备注

        public TraceStepData(int _id, int _sendInLevel, string _mark = "")
        {
            ID = _id;
            sendLevel = _sendInLevel;
            mark = _mark;
            _m_defineMark = _mark;
        }

        public TraceStepData setMark(string _mark)
        {
            mark = _mark;
            return this;
        }

        public TraceStepData setMarkParam(params object[] _params)
        {
            try
            {
                if (!string.IsNullOrEmpty(_m_defineMark) && _params != null)
                    mark = string.Format(_m_defineMark, _params);
            }
            catch (Exception e)
            {
                // ignored
            }

            return this;
        }

        public override string ToString() { return $"{ID}:{sendLevel}:{mark}"; }
    }

    /// <summary>
    /// 埋点信息
    /// </summary>
    public static class TraceConst
    {
        /*
         * 100~999：（三位数）特殊点位，比如客户端报错、服务端报错等
         * 1000~9999：（四位数）初始化及登录流程
         * 10000~99999：（五位数）系统功能相关，每个系统功能100点，例如10000~10099对话系统用、10100~10199骑士系统用
         * 1000000~：（七位数）引导相关，规则：引导ID*1000+引导步骤*10，具体策划配置在引导UI上
         */


        //==========================================================================
        //100~999：（三位数）特殊点位，比如客户端报错、服务端报错等
        //==========================================================================
        [NotNull] public static TraceStepData LOG_ERROR = new TraceStepData(100,1);//通用错误日志埋点
        [NotNull] public static TraceStepData SERVER_ERROR = new TraceStepData(110,1);//服务端错误码埋点
        [NotNull] public static TraceStepData VIDEO_MAX_LOAD_PIXELS = new TraceStepData(111,1, "初始化获取视频校验上限:{0},检测时间：{1},设备基准值：{2}，当前测试次数：{3}");//服务端错误码埋点
        [NotNull] public static TraceStepData PATCH_LIST_USE_TIME = new TraceStepData(120, 1, "热更配表耗时:{0} 秒");//热更配表补丁完成
        [NotNull] public static TraceStepData THIRD_LOGIN = new TraceStepData(130, 1, "发送第三方埋点-login");//发送第三方埋点-login
        [NotNull] public static TraceStepData THIRD_ROLE = new TraceStepData(131, 1, "发送第三方埋点-role");//发送第三方埋点-role
        [NotNull] public static TraceStepData THIRD_TUTORIAL = new TraceStepData(132, 1, "发送第三方埋点-tutorial");//发送第三方埋点-tutorial
        [NotNull] public static TraceStepData THIRD_STAGE_2 = new TraceStepData(133, 1, "发送第三方埋点-stage2");//发送第三方埋点-stage2
        [NotNull] public static TraceStepData THIRD_STAGE_8 = new TraceStepData(134, 1, "发送第三方埋点-stage8");//发送第三方埋点-stage8
        [NotNull] public static TraceStepData CLICK_AIHELP_ENTRANCE = new TraceStepData(140, 1, "点击客服入口");
        [NotNull] public static TraceStepData AVERAGE_PING = new TraceStepData(200, 1);//游戏平均PING值
        [NotNull] public static TraceStepData LOW_FPS = new TraceStepData(210, 1, "持续了{0}秒低帧率，当前帧率：{1}");
        [NotNull] public static TraceStepData CITY_FPS = new TraceStepData(220, 1);//主城帧率值
        [NotNull] public static TraceStepData GAME_PAUSE = new TraceStepData(230, 1, "游戏切到后台");//游戏切到后台
        [NotNull] public static TraceStepData GAME_RESUME = new TraceStepData(240, 1, "后台切回游戏");//后台切回游戏
        [NotNull] public static TraceStepData SYSTEM_INFO = new TraceStepData(250, 1);//SystemInfo信息
        [NotNull] public static TraceStepData VIDEO_PLAY_ERROR = new TraceStepData(251,1, "视频播放错误：视频资源：{0}，错误消息：{1}");
        [NotNull] public static TraceStepData LOW_FRAME_RATE_TIP = new TraceStepData(260, 1,"展示低帧率提示，当前帧率：{0}，当前画质：{1}，降低后画质：{2}，玩家选择：{3}");
        [NotNull] public static TraceStepData START_TUTORIAL_WND = new TraceStepData(280,1, "开始引导：{0}");
        [NotNull] public static TraceStepData SHOW_TUTORIAL_WND = new TraceStepData(281,1, "展示引导窗口：{0}");
        [NotNull] public static TraceStepData TIME_30 = new TraceStepData(290, 1, "游戏运行30分钟");
        [NotNull] public static TraceStepData THIRD_PURCHASE = new TraceStepData(300, 1, "发送第三方支付埋点,mj_order_id:{0},app_order_id:{1},revenue:{2},currency:{3},goods_id:{4},server_id:{5}");
        [NotNull] public static TraceStepData OFFLINE_ORDER_DELIVERY = new TraceStepData(301, 1, "Offline订单发货,orderId:{0},giftPackId:{1},sdkOrderId:{2},payMoney:{3},payCurrency:{4},orderType:{5} [1内购2网页充值3福利95代金券]");
        [NotNull] public static TraceStepData CLICK_WEB_RECHARGE_ENTRANCE = new TraceStepData(302, 1, "点击网页充值入口");
        [NotNull] public static TraceStepData START_DEAL_OFFLINE_ORDER_DELIVERY = new TraceStepData(303, 1, "开始处理Offline订单,orderId:{0},giftPackId:{1},sdkOrderId:{2},payMoney:{3},payCurrency:{4},orderType:{5} [1内购2网页充值3福利95代金券]");


        //==========================================================================
        //1000~9999：（四位数）初始化及登录流程
        //==========================================================================
        [NotNull] public static TraceStepData ON_START = new TraceStepData(1000, 1, "开始onStart");
        [NotNull] public static TraceStepData LOW_MEMORY_CALL_BACK = new TraceStepData(1001,1, "触发低内存警告:运行时间{0}，当前node:{1}");
        [NotNull] public static TraceStepData START_INIT_CDN = new TraceStepData(1020,1, "开始初始化CDN");
        [NotNull] public static TraceStepData START_INIT_CLIENT_CONFIG = new TraceStepData(1040,1, "CDN-开始初始化ClientConfig");
        [NotNull] public static TraceStepData INIT_CLIENT_CONFIG_SUC = new TraceStepData(1041,1, "CDN-初始化ClientConfig成功");
        [NotNull] public static TraceStepData INIT_CLIENT_CONFIG_FAIL = new TraceStepData(1042,1, "CDN-初始化ClientConfig失败");
        [NotNull] public static TraceStepData CLIENT_CONFIG_PLATFORM_ID_CHG = new TraceStepData(1043,1, "CDN-开始初始化platformid发生变化，需要清除其他cdn");
        [NotNull] public static TraceStepData START_INIT_CLIENT_CONFIGF_FAIL_POP_WND = new TraceStepData(1045,1, "CDN-开始初始化ClientConfig-失败，弹窗对话框等待玩家确认重试");
        [NotNull] public static TraceStepData START_INIT_CLIENT_CONFIGF_FAILCLICK = new TraceStepData(1046,1, "CDN-开始初始化ClientConfig-失败，对话框点击确认重试");

        [NotNull] public static TraceStepData START_INIT_AREA = new TraceStepData(1060,1, "CDN-开始初始化Area");
        [NotNull] public static TraceStepData INIT_AREA_SDK_CITY = new TraceStepData(1061,1, "CDN-初始化Area，获取SDK区域：{0}，获取到的配表推荐大区：{1}");
        [NotNull] public static TraceStepData INIT_AREA_SDK_CITY_NULL = new TraceStepData(1062,1, "CDN-初始化Area，获取SDK区域为空");
        [NotNull] public static TraceStepData INIT_AREA_SUC = new TraceStepData(1063,1, "CDN-初始化Area成功,{0}");
        [NotNull] public static TraceStepData INIT_AREA_FAIL = new TraceStepData(1064,1, "CDN-初始化Area失败，直接使用默认大区配置：{0}");
        [NotNull] public static TraceStepData INIT_AREA_USE_DEFAULT = new TraceStepData(1065,1, "CDN-初始化Area，在CDN未找到推荐大区，使用默认大区：{0}");
        [NotNull] public static TraceStepData START_INIT_LOGIN_SERVER_URL = new TraceStepData(1080,1, "CDN-开始初始化登录服务器地址");
        [NotNull] public static TraceStepData INIT_LOGIN_SERVER_URL_SUC_DEFAULT = new TraceStepData(1081,1, "CDN-初始化登录服务器地址成功,使用默认配置");
        [NotNull] public static TraceStepData INIT_LOGIN_SERVER_URL_SUC_CDN = new TraceStepData(1082,1, "CDN-初始化登录服务器地址成功,使用CDN配置");
        [NotNull] public static TraceStepData INIT_LOGIN_SERVER_URL_FAIL = new TraceStepData(1083,1, "CDN-初始化登录服务器地址失败");
        [NotNull] public static TraceStepData START_INIT_GAME_BEFORE_NOTICE = new TraceStepData(1100, 1, "CDN-开始初始化登录前公告");
        [NotNull] public static TraceStepData INIT_GAME_BEFORE_NOTICE_SUC = new TraceStepData(1101, 1, "CDN-初始化登录前公告成功，数量：{0}");
        [NotNull] public static TraceStepData INIT_GAME_BEFORE_NOTICE_FAIL = new TraceStepData(1102, 1, "CDN-初始化登录前公告失败");
        [NotNull] public static TraceStepData START_INIT_MAINTAIN_NOTICE = new TraceStepData(1120, 1, "CDN-开始初始化维护公告");
        [NotNull] public static TraceStepData INIT_MAINTAIN_NOTICE_SUC = new TraceStepData(1121, 1, "CDN-初始化维护公告成功");
        [NotNull] public static TraceStepData INI_MAINTAIN_NOTICE_FAIL = new TraceStepData(1122, 1, "CDN-初始化维护公告失败");
        [NotNull] public static TraceStepData START_INIT_GAME_AFTER_NOTICE = new TraceStepData(1140, 1, "CDN-开始初始化登录后公告");
        [NotNull] public static TraceStepData INIT_GAME_BEFORE_AFTER_SUC = new TraceStepData(1141, 1, "CDN-初始化登录后公告成功");
        [NotNull] public static TraceStepData INIT_GAME_BEFORE_AFTER_FAIL = new TraceStepData(1142, 1, "CDN-初始化登录后公告失败");
        [NotNull] public static TraceStepData START_INIT_SERVER_LIST = new TraceStepData(1160, 1, "CDN-开始初始化服务器列表");
        [NotNull] public static TraceStepData INIT_SERVER_LIST_SUC = new TraceStepData(1161, 1, "CDN-初始化服务器列表成功");
        [NotNull] public static TraceStepData INIT_SERVER_LIST_FAIL = new TraceStepData(1162, 1, "CDN-初始化服务器列表失败");
        [NotNull] public static TraceStepData INIT_CDN_DONE = new TraceStepData(1180, 1, "初始化CDN完成");
        [NotNull] public static TraceStepData INIT_SET_QUALITY = new TraceStepData(1200, 1, "初始化设置画质,{0}");
        [NotNull] public static TraceStepData START_INIT_PLAT_RES = new TraceStepData(1220, 1, "开始初始化平台资源");
        [NotNull] public static TraceStepData INIT_PLAT_RES_COMMON_INFO = new TraceStepData(1221, 1, "初始化登录部分的通用资源");
        [NotNull] public static TraceStepData INIT_PLAT_RES_LANGUAGE = new TraceStepData(1222, 1, "初始化设置游戏语言,{0}");
        [NotNull] public static TraceStepData INIT_PLAT_RES_LANGUAGE_SQL = new TraceStepData(1223, 1, "初始化平台翻译表数据库");
        [NotNull] public static TraceStepData INIT_PLAT_RES_AUDIO = new TraceStepData(1224, 1, "初始化音效");
        [NotNull] public static TraceStepData INIT_PLAT_RES_TRANSPARENTBK = new TraceStepData(1225, 1, "初始化透明背景缓存");
        [NotNull] public static TraceStepData INIT_PLAT_RES_DONE = new TraceStepData(1226, 1, "初始化平台资源完成");
        [NotNull] public static TraceStepData INIT_PLAT_COUNTRY_REF = new TraceStepData(1228, 1, "开始初始化国家配表");
        [NotNull] public static TraceStepData INIT_PLAT_COUNTRY_REF_DONE = new TraceStepData(1230, 1, "开始初始化国家配表完成");

        [NotNull] public static TraceStepData START_ENTER_LOGIN_NODE = new TraceStepData(1240, 1, "开始进入登录主界面");
        [NotNull] public static TraceStepData START_INIT_GPM = new TraceStepData(1241, 1, "开始初始化GPM");
        [NotNull] public static TraceStepData INIT_GPM_DONE = new TraceStepData(1242, 1, "初始化GPM完成:{0}");
        [NotNull] public static TraceStepData START_INIT_SDK = new TraceStepData(1260, 1, "开始初始化SDK");
        [NotNull] public static TraceStepData INIT_SDK_SUC = new TraceStepData(1261, 1, "初始化SDK成功");
        [NotNull] public static TraceStepData INIT_SDK_FAIL = new TraceStepData(1262, 1, "初始化SDK失败");
        [NotNull] public static TraceStepData NO_USE_SDK = new TraceStepData(1263, 1, "未使用SDK");
        [NotNull] public static TraceStepData INIT_SDK_IP_SUC = new TraceStepData(1264, 1, "初始化IP信息成功，IP:{0}，CITY:{1}，REGION:{2}, TIME:{3}");
        [NotNull] public static TraceStepData INIT_SDK_IP_FAIL = new TraceStepData(1265, 1, "初始化IP信息失败,TIME:{0}");
        [NotNull] public static TraceStepData INIT_SDK_PARAM_SUC = new TraceStepData(1266, 1, "初始化SDK参数完成");
        [NotNull] public static TraceStepData INIT_AIHELP_START = new TraceStepData(1267, 1, "开始初始化AIHelp");
        [NotNull] public static TraceStepData INIT_AIHELP_SUC = new TraceStepData(1268, 1, "初始化AIHelp成功");
        [NotNull] public static TraceStepData INIT_AIHELP_FAIL = new TraceStepData(1269, 1, "初始化AIHelp失败，code:{0}，msg:{1}");
        [NotNull] public static TraceStepData START_JUDGE_VERSION_UPDATE = new TraceStepData(1280, 1, "开始检查是否需要更新客户端");
        [NotNull] public static TraceStepData START_JUDGE_VERSION_UPDATE_STATE = new TraceStepData(1281, 1, "判断更新客户端方式1：强；2：否；3：提示，结果：{0}");
        [NotNull] public static TraceStepData START_JUDGE_VERSION_UPDATE_CONFIRM = new TraceStepData(1282, 1, "玩家选择更新客户端");
        [NotNull] public static TraceStepData START_JUDGE_VERSION_UPDATE_CANCEL = new TraceStepData(1283, 1, "玩家选择不更新客户端");
        [NotNull] public static TraceStepData SHOW_POLICY_WND = new TraceStepData(1300, 1, "判断是否需要打开隐私协议界面，{0}");
        [NotNull] public static TraceStepData SHOW_POLICY_WND_OPEN = new TraceStepData(1305, 1, "打开隐私界面");
        [NotNull] public static TraceStepData SHOW_LOADING_BK_WND = new TraceStepData(1306, 1, "显示登入进度条界面");
        [NotNull] public static TraceStepData START_INIT_REMOTE_RES = new TraceStepData(1315, 1, "开始确认远端资源地址");
        [NotNull] public static TraceStepData START_INIT_REMOTE_RES_DONE = new TraceStepData(1316, 1, "开始确认远端资源地址完成");
        [NotNull] public static TraceStepData START_UPDATE_RES = new TraceStepData(1320, 1, "开始更新资源");
        [NotNull] public static TraceStepData UPDATE_RES_25 = new TraceStepData(1321, 1, "更新资源25%");
        [NotNull] public static TraceStepData UPDATE_RES_50 = new TraceStepData(1322, 1, "更新资源50%");
        [NotNull] public static TraceStepData UPDATE_RES_75 = new TraceStepData(1323, 1, "更新资源75%");
        [NotNull] public static TraceStepData UPDATE_RES_DONE = new TraceStepData(1324,1, "更新资源完成");
        [NotNull] public static TraceStepData START_UPDATE_LANGUAGE_RES = new TraceStepData(1340,1, "开始更新语言资源，{0}");
        [NotNull] public static TraceStepData UPDATE_LANGUAGE_RES_DONE = new TraceStepData(1341,1, "更新语言资源完成");
        [NotNull] public static TraceStepData GAME_RES_INITED = new TraceStepData(1360,1, "游戏资源初始化成功的处理");
        [NotNull] public static TraceStepData START_LOAD_COMMON_AB = new TraceStepData(1380,1, "开始加载通用资源对象");
        [NotNull] public static TraceStepData LOAD_COMMON_AB_DONE = new TraceStepData(1381,1, "加载通用资源对象完成");
        [NotNull] public static TraceStepData START_INIT_COMMON_INFO = new TraceStepData(1400,1, "开始加载通用assetbundle(配表、数据库等)");
        [NotNull] public static TraceStepData INIT_COMMON_INFO_DONE = new TraceStepData(1401,1, "加载通用assetbundle完成");
        [NotNull] public static TraceStepData START_INIT_BASIC_UI = new TraceStepData(1420,1, "开始初始化基础UI");
        [NotNull] public static TraceStepData INIT_BASIC_UI_DONE = new TraceStepData(1421,1, "初始化基础UI完成");
        [NotNull] public static TraceStepData INIT_HOTFIX_RES = new TraceStepData(1440,1, "初始化HotFix热更配表资源");
        [NotNull] public static TraceStepData START_INIT_AUDION_MIXER = new TraceStepData(1460,1, "开始初始化混音器");
        [NotNull] public static TraceStepData INIT_AUDION_MIXER_DONE = new TraceStepData(1461,1, "初始化混音器完成");
        [NotNull] public static TraceStepData LOCAL_RES_LOAD_OVER = new TraceStepData(1480,1, "所有本地资源加载完成");
        [NotNull] public static TraceStepData INIT_SHOW_CASE = new TraceStepData(1500,1, "初始化ShowCase");
        [NotNull] public static TraceStepData START_HOTFIX_LOAD = new TraceStepData(1520,1, "开始热更补丁加载");
        [NotNull] public static TraceStepData INIT_HOTFIX_URL = new TraceStepData(1540,1, "开始初始化补丁更新地址");
        [NotNull] public static TraceStepData INIT_HOTFIX_URL_SUC = new TraceStepData(1541,1, "初始化补丁更新地址成功，injectFixRemoteUrl：{0}，ilRuntimeRemoteUrl：{1}");
        [NotNull] public static TraceStepData INIT_LOAD_INJECT_FIX = new TraceStepData(1560,1, "初始化加载InjectFix");
        [NotNull] public static TraceStepData LOAD_INJECT_FIX_SUC = new TraceStepData(1561,1, "加载InjectFix成功");
        [NotNull] public static TraceStepData LOAD_INJECT_FIX_FAIL = new TraceStepData(1562,1, "加载InjectFix失败");
        [NotNull] public static TraceStepData SKIP_LOAD_INJECT_FIX = new TraceStepData(1563,1, "不需要加载InjectFix");
        [NotNull] public static TraceStepData INIT_LOAD_HOT_FIX = new TraceStepData(1580,1, "初始化加载HotFix");
        [NotNull] public static TraceStepData LOAD_HOT_FIX_SUC = new TraceStepData(1581,1, "下载加载HotFix成功");
        [NotNull] public static TraceStepData LOAD_HOT_FIX_FAIL = new TraceStepData(1582,1, "下载加载HotFix失败");
        [NotNull] public static TraceStepData LOAD_HOT_FIX_LOCAL_SUC = new TraceStepData(1583,1, "本地加载HotFix成功");
        [NotNull] public static TraceStepData LOAD_HOT_FIX_LOCAL_FAIL = new TraceStepData(1584,1, "本地加载HotFix失败");
        [NotNull] public static TraceStepData START_TOKEN_LOGIN = new TraceStepData(1600,1, "开始Token登录,loginUid:{0},loginToken:{1}");
        [NotNull] public static TraceStepData START_SDK_LOGIN = new TraceStepData(1620,1, "开始SDK登录");
        [NotNull] public static TraceStepData SDK_LOGIN_SUC = new TraceStepData(1621, 1, "SDK登录成功-{0}，userId:{1},token:{2}");
        [NotNull] public static TraceStepData SDK_LOGIN_FAIL = new TraceStepData(1622, 1, "SDK登录失败-{0}");
        [NotNull] public static TraceStepData LOGIN_LS_SUC = new TraceStepData(1640, 1, "登录LS成功,IP:{0},Port:{1}");
        [NotNull] public static TraceStepData LOGIN_LS_FAIL = new TraceStepData(1641, 1, "登录LS失败,IP:{0},Port:{1}");
        [NotNull] public static TraceStepData START_LOGIN_GS = new TraceStepData(1660, 1, "开始登陆GS服务器");
        [NotNull] public static TraceStepData LOGIN_GS_SUC = new TraceStepData(1661, 1, "登录GS服务器成功,IP:{0},Port:{1},Msg:{2}");
        [NotNull] public static TraceStepData LOGIN_GS_FAIL = new TraceStepData(1662, 1, "登录GS服务器失败,reason:{0},IP:{1},Port:{2}");
        [NotNull] public static TraceStepData START_INIT_LOCAL_PRODUCT = new TraceStepData(1680, 1, "开始初始化商品档位本地化信息");
        [NotNull] public static TraceStepData INIT_LOCAL_PRODUCT_SUC = new TraceStepData(1681, 1, "初始化商品档位本地化信息成功");
        [NotNull] public static TraceStepData INIT_LOCAL_PRODUCT_FAIL = new TraceStepData(1682, 1, "初始化商品档位本地化信息失败，重试剩余次数：{0}");
        [NotNull] public static TraceStepData ENTER_START_GAME_WND = new TraceStepData(1700, 1, "进入开始游戏界面");
        [NotNull] public static TraceStepData GET_RECOMMOND_SERVER = new TraceStepData(1701, 1, "开始获取推荐服务器");
        [NotNull] public static TraceStepData GET_RECOMMOND_SERVER_DONE = new TraceStepData(1702, 1, "获取推荐服务器成功,serverId:{0}");
        [NotNull] public static TraceStepData CLICK_START_GAME = new TraceStepData(1705, 1, "点击开始游戏");
        [NotNull] public static TraceStepData START_REQ_ENTER_US = new TraceStepData(1720, 1, "开始请求进入US,serverId:{0}");
        [NotNull] public static TraceStepData START_REQ_ENTER_US_RET = new TraceStepData(1721, 1, "US协议回包");
        [NotNull] public static TraceStepData ENTER_US_FREEZE = new TraceStepData(1722, 1, "进入US账号被冻结");
        [NotNull] public static TraceStepData ENTER_US_DONE = new TraceStepData(1723, 1, "进入US完成");
        [NotNull] public static TraceStepData GET_PLAYER_CID = new TraceStepData(1724,1, "服务器发放cid成功,cid:{0}");
        [NotNull] public static TraceStepData START_INIT_PLAYER_COMP = new TraceStepData(1740,1, "开始初始化玩家数据组件");
        [NotNull] public static TraceStepData INIT_PLAYER_COMP_SUC = new TraceStepData(1741,1, "初始化玩家数据组件成功");
        [NotNull] public static TraceStepData INIT_PLAYER_COMP_FAIL = new TraceStepData(1742,1, "初始化玩家数据组件失败");
        [NotNull] public static TraceStepData ENTER_GAME = new TraceStepData(1760,1, "正式进入游戏界面");
        [NotNull] public static TraceStepData ENTER_GAME_CITY = new TraceStepData(1761,1, "初始进入主城界面");
        [NotNull] public static TraceStepData ENTER_GAME_PER_CREATE = new TraceStepData(1762,1, "初始进入第一次进行游戏创角");
        [NotNull] public static TraceStepData SUC_ENTER_PER_CREATE_NODE = new TraceStepData(1763,1, "游戏界面进入完成");
        [NotNull] public static TraceStepData FIRST_TRIGGER_TUTORIAL = new TraceStepData(1764,1, "首次触发引导1000");
        [NotNull] public static TraceStepData ENTER_GAME_CREATE = new TraceStepData(1765,1, "初始进入创角界面");
        [NotNull] public static TraceStepData START_INIT_SERVER_RELATE_CDN = new TraceStepData(1780,1, "CDN-开始初始化服务器id相关CDN，服务器id:{0}");
        [NotNull] public static TraceStepData START_INIT_ANNOUNCEMENT = new TraceStepData(1800,1, "CDN-开始初始化运营公告");
        [NotNull] public static TraceStepData START_INIT_ANNOUNCEMENT_SUC = new TraceStepData(1801,1, "CDN-初始化运营公告成功，数量：{0}");
        [NotNull] public static TraceStepData START_INIT_ANNOUNCEMENT_FAIL = new TraceStepData(1802,1, "CDN-初始化运营公告失败");


        [NotNull] public static TraceStepData INIT_LOGIN_FAIL = new TraceStepData(2000,1, "登入初始化过程失败，原因：{0}、{1}");
        
        //==========================================================================
        //10000~99999：（五位数）系统功能相关，每个系统功能100点
        //==========================================================================
        //10000~10099 对话系统
        [NotNull] public static TraceStepData START_DIALOGUE = new TraceStepData(10000, 2);//开启对话，mark1:dialogue_id
        [NotNull] public static TraceStepData END_DIALOGUE = new TraceStepData(10001, 2);//结束对话，mark1:dialogue_id
        [NotNull] public static TraceStepData SKIP_DIALOGUE = new TraceStepData(10002, 2);//点击跳过对话，mark1:dialogue_id,dialogue_sentence_id
        [NotNull] public static TraceStepData SHOW_DIALOGUE_SENTENCE = new TraceStepData(10003, 2);//展示对话句子，mark1:dialogue_sentence_id

        //10100~10199 支付相关
        [NotNull] public static TraceStepData PAY_CLICK = new TraceStepData(10100, 1, "点击支付，payRefId:{0}，sdkPayId:{1}，giftPackId:{2}");
        [NotNull] public static TraceStepData PAY_CREATE_ORDERID_START = new TraceStepData(10101, 1, "开始创建支付订单");
        [NotNull] public static TraceStepData PAY_CREATE_ORDERID_SUC = new TraceStepData(10102, 1, "支付订单创建成功，orderId:{0}，giftPackId:{1}");
        [NotNull] public static TraceStepData PAY_CREATE_ORDERID_FAIL = new TraceStepData(10103, 1, "支付订单创建失败，服务端错误码:{0}，giftPackId:{1}");
        [NotNull] public static TraceStepData PAY_CHECK_VOUCHER = new TraceStepData(10104, 1, "开始检查是否弹窗使用代金券");
        [NotNull] public static TraceStepData PAY_NO_VOUCHER = new TraceStepData(10105, 1, "没有代金券也没有开启网页支付，进入支付流程");
        [NotNull] public static TraceStepData PAY_HAVE_VOUCHER = new TraceStepData(10106, 1, "拥有代金券或者开启了网页支付，弹窗确认是否使用代金券");
        [NotNull] public static TraceStepData PAY_NO_USE_VOUCHER = new TraceStepData(10107, 1, "不使用代金券，使用现金支付");
        [NotNull] public static TraceStepData PAY_START_USE_VOUCHER = new TraceStepData(10108, 1, "拥有代金券并且请求使用代金券");
        [NotNull] public static TraceStepData PAY_USE_VOUCHER_SUC = new TraceStepData(10109, 1, "使用代金券成功");
        [NotNull] public static TraceStepData PAY_USE_VOUCHER_FAIL = new TraceStepData(10110, 1, "使用代金券失败");
        [NotNull] public static TraceStepData PAY_SDK_START = new TraceStepData(10111, 1, "开始调用SDK支付");
        [NotNull] public static TraceStepData PAY_SUCCEED = new TraceStepData(10112, 1, "支付成功，orderId:{0}，giftPackId:{1}");
        [NotNull] public static TraceStepData PAY_CANCEL = new TraceStepData(10113, 1, "支付取消，orderId:{0}，giftPackId:{1}，code:{2}，msg:{3}");
        [NotNull] public static TraceStepData PAY_FAILED = new TraceStepData(10114, 1, "支付失败，orderId:{0}，giftPackId:{1}，code:{2}，msg:{3}");
        [NotNull] public static TraceStepData PAY_GO_TO_WEB_RECHARGE = new TraceStepData(10115, 1, "代金券不足，点击前往网页充值");
        [NotNull] public static TraceStepData PAY_REQUEST_SERIALIZE_DIFFER = new TraceStepData(10120, 1, "支付请求序列号不一致");
        [NotNull] public static TraceStepData PAY_SET_DONE_ORDERID_DIFFER = new TraceStepData(10121, 1, "设置支付完成的订单id和当前订单id不一致，setDoneOrderId:{0}，curOrderId:{1}");
        [NotNull] public static TraceStepData PAY_SET_CANCEL_ORDERID_DIFFER = new TraceStepData(10122, 1, "设置支付取消的订单id和当前订单id不一致，setCancelOrderId:{0}，curOrderId:{1}");
        [NotNull] public static TraceStepData PAY_LAST_ORDER_NOT_FINISH = new TraceStepData(10123, 1, "上一个订单未完成就有新的订单了，lastOrderId:{0}，newOrderId:{1}");
        [NotNull] public static TraceStepData PAY_ORDER_TIMEOUT = new TraceStepData(10124, 1, "订单在一段时间内没有支付完成或取消支付，orderId:{0}");

        //10200~10299 小游戏
        [NotNull] public static TraceStepData ENTER_MINI_GAME = new TraceStepData(10200, 2,"进入小游戏，{0}");//进入小游戏
        [NotNull] public static TraceStepData FINISH_MINI_GAME = new TraceStepData(10201, 2,"完成小游戏，{0}，{1}");//完成小游戏
        
        //10300~10399 视频播放
        [NotNull] public static TraceStepData ADD_INTRO_STORY_NODE = new TraceStepData(10300, 1, "添加开篇剧情节点");//添加开篇剧情节点
        [NotNull] public static TraceStepData VIDEO_PREPARE_COMPLETED = new TraceStepData(10301, 1, "视频准备完成");//视频准备完成
        [NotNull] public static TraceStepData VIDEO_START_PLAY = new TraceStepData(10302, 1, "视频开始播放");//视频开始播放
        [NotNull] public static TraceStepData VIDEO_PLAY_END = new TraceStepData(10303, 1, "视频播放结束");//视频播放结束
        [NotNull] public static TraceStepData VIDEO_SKIP = new TraceStepData(10304, 1, "跳过视频播放埋点");//跳过视频播放埋点
        [NotNull] public static TraceStepData VIDEO_ERROR_SKIP = new TraceStepData(10305, 1, "视频加载失败，直接跳过的埋点");//视频加载失败，直接跳过的埋点

        //10400~10499 火星系统
        [NotNull] public static TraceStepData CLICK_START_GO_TO_MARS = new TraceStepData(10400, 1, "点击发射火箭，开始前往火星");
        [NotNull] public static TraceStepData GO_TO_MARS_ARRIVED_SRAGE = new TraceStepData(10401, 1, "前往火星，到达阶段{0}");
        [NotNull] public static TraceStepData ARRIVED_MARS = new TraceStepData(10402, 1, "到达火星");
        [NotNull] public static TraceStepData CLICK_LANDING_MARS = new TraceStepData(10403, 1, "点击确认着陆火星");

        //10500~10599 商店好评系统
        [NotNull] public static TraceStepData TRIGGER_STORE_REVIEWS = new TraceStepData(10500, 1, "触发商店好评");
        [NotNull] public static TraceStepData TRIGGER_STORE_REVIEWS_NOT_OPEN_WND = new TraceStepData(10501, 1, "触发商店好评，但是已评价过或在CD中，不打开窗口");
        [NotNull] public static TraceStepData STORE_REVIEWS_CLICK_PRAISE = new TraceStepData(10502, 1, "商店好评界面点击赞扬");
        [NotNull] public static TraceStepData STORE_REVIEWS_CLICK_ROAST = new TraceStepData(10503, 1, "商店好评界面点击吐槽");
        [NotNull] public static TraceStepData STORE_REVIEWS_CLICK_SEND_ROAST = new TraceStepData(10504, 1, "吐槽界面点击发送");
        
        //10600~10699 关卡系统
        [NotNull] public static TraceStepData CHAPTER_FORWARD_COST_ENOUGH = new TraceStepData(10600, 1, "关卡前进消耗不足，当前关卡：{0}");
    }
}
