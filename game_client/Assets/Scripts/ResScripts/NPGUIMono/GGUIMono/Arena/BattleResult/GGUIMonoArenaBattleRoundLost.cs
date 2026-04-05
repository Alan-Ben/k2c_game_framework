using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场回合失败弹窗
    /// </summary>
    public class GGUIMonoArenaBattleRoundLost : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("失败描述")]
        public Text txtDesc;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5215); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5215); } }
    }
}