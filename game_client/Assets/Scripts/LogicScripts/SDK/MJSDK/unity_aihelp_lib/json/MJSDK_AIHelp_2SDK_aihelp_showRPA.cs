using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(aihelp-showRPA：机器人流程自动化)消息结构体 
    /// </summary>
    public class MJSDK_AIHelp_2SDK_aihelp_showRPA : MJSDK_2SDK_Base
    {
        //在 AIHelp 后台配置的自定义入口 ID（人口客服、机器人客服、常见问题等）
        public string entranceId;
        // 参数说明：人工客服自定义欢迎语
        public string welcomeMsg;
    }
}