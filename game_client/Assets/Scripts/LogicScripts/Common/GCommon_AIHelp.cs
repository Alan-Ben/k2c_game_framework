using System.Text;
using MJSDK_Package;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// AIHelp相关的通用处理
    /// </summary>
    public static partial class GCommon
    {
        /// <summary>
        /// 打开AIHelp帮助界面
        /// </summary>
        public static void showAIHelp()
        {
            if (!SDKMgr.instance.isUseSDK)
                return;

            //发送埋点-点击客服入口
            GCommon.sendStepReport(TraceConst.CLICK_AIHELP_ENTRANCE);

            //先设置用户信息
            setAIHelpUserInfo();

            //打开帮助界面
            MJSDK_AIHelp_2SDK_aihelp_showRPA showInfo = new MJSDK_AIHelp_2SDK_aihelp_showRPA();
            showInfo.entranceId = "E001";
            SDKMgr.instance.aihelp_showRPA(showInfo, null, null);
        }

        /// <summary>
        /// 设置AIHelp语言信息
        /// </summary>
        public static void setAIHelpLanguage()
        {
            if (!SDKMgr.instance.isUseSDK)
                return;

            SDKMgr.instance.aihelp_updateLan(GameSetting.instance.getCurrentLanguage().toPHPLanguageCode(), null, null);
        }

        /// <summary>
        /// 设置AIHelp用户信息
        /// </summary>
        public static void setAIHelpUserInfo()
        {
            if (!SDKMgr.instance.isUseSDK || 
                NPPlayer.instance == null || 
                NPPlayer.instance.playerInfo == null || 
                NPPlayer.instance.playerInfo.CID <= 0)
                return;

            StringBuilder customData = new StringBuilder();
            customData.Append("{");
            customData.Append($"\"uid\":\"{(Game.instance.uid != null ? Game.instance.uid : "0")}\",");
            customData.Append($"\"udid\":\"{SDKMgr.instance.deviceId}\"");
            customData.Append("}");

            MJSDK_AIHelp_2SDK_aihelp_setUserInfo userInfo = new MJSDK_AIHelp_2SDK_aihelp_setUserInfo();
            userInfo.user_name = NPPlayer.instance.playerInfo.PlayerName;
            userInfo.user_id = NPPlayer.instance.playerInfo.CID.ToString();
            userInfo.server_id = GameInit_SelectServer.instance.loginServerLogicId.ToString();
            userInfo.userTags = $"viplv:{NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL)}";
            userInfo.customData = customData.ToString();

            SDKMgr.instance.aihelp_setUserInfo(userInfo, null, null);
        }
    }
}