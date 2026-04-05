using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 商店评价吐槽界面
    /// </summary>
    public class GGUIMonoStoreReviewsRoast : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("发送按钮")]
        public GameObject btnSend;
        [ALHeader("输入框")]
        public InputField inputText;
        [ALHeader("输入限制字数")]
        public int limitTextLength = 300;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8011); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8011); } }
    }
}
