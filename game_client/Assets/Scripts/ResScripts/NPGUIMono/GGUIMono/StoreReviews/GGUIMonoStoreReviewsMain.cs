using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 商店评价主界面
    /// </summary>
    public class GGUIMonoStoreReviewsMain : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("赞扬按钮")]
        public GameObject btnPraise;
        [ALHeader("吐槽按钮")]
        public GameObject btnRoast;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8010); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8010); } }
    }

}
