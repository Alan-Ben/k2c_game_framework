using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 游历博彩押注窗口Mono
    /// </summary>
    public class GGUIMonoTravelGambleAnte : _AALBasicUIWndMono
    {
        [ALHeader("押注道具Mono")]
        public NPGGUIMonoCommonItem monoCommonItem;

        [ALHeader("押注数量计数器")]
        public GGUIMonoBagPopCounter anteNumCounter;

        [ALHeader("押注按钮")]
        public GameObject anteBtn;

        [ALHeader("放弃按钮")]
        public GameObject abandonBtn;

        [ALHeader("稍后按钮")]
        public GameObject laterBtn;

        [ALHeader("可押注时显示")]
        public List<GameObject> canAnteShowList;

        [ALHeader("不可押注时显示")]
        public List<GameObject> cannotAnteShowList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3633); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3633); } }
    }
}