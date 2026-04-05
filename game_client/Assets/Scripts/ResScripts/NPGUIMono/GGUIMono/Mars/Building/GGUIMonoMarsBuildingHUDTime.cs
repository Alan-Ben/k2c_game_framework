using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingHUDTime : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("剩余时间")]
        public Text txtTime;
        public TextMeshProUGUI tmpTime;

        [ALHeader("联盟求助按钮")]
        public GameObject btnAssist;
        [ALHeader("可联盟求助需要显隐藏的物体列表")]
        public List<GameObject> canAssistShowGos;
        public List<GameObject> canAssistHideGos;
    }
}