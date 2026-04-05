using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoDialogueOptionItemPrefab: _AALBasicUIWndMono
    {
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
        [ALHeader("回应文本")]
        public Text txtResponse;
    }
}