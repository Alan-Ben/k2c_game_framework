using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 其他联盟信息界面
    /// </summary>
    public class GGUIMonoGuildOtherInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("联盟基础信息脚本")]
        public GGUIMonoGuildSubBaseInfo monoGuildBaseInfo;
        
        [ALHeader("成员列表")]
        public GGUIMonoGuildOtherInfoMemberGrid monoMemberGrid;

        [ALHeader("加入联盟按钮")]
        public GGUIMonoJoinGuildBtn monoJoinBtn;

        [ALHeader("联盟解锁描述文本")]
        public Text txtUnlockDesc;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4910); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4910); } }
    }
}
