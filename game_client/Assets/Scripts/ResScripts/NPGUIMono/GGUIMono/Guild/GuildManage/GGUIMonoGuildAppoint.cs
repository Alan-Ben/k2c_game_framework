using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟成员任命界面
    /// </summary>
    public class GGUIMonoGuildAppoint : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("转让盟主按钮")]
        public GameObject btnTransferLeader;
        [ALHeader("踢出按钮")]
        public GameObject btnKickOut;
        [ALHeader("任命类型按钮列表")]
        public List<GGUIMonoGuildAppointItem> monoAppointTypeItemList;
        [ALHeader("任命条件描述")]
        public Text txtAppointDesc;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4914); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4914); } }
    }
}
