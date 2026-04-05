using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 小游戏结算弹窗
    /// </summary>
    public class NPGGUIMonoMiniGameEnd_NoRecord : _AALBasicUIWndMono
    {
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        [ALHeader("确认按钮文本")]
        public Text txtBtnConfirm;
        [ALHeader("本局得分文本")]
        public Text txtScore;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(10002); } }
        public static string objName { get { return UIResPathAssistant.getObjName(10002); } }
    }
}
