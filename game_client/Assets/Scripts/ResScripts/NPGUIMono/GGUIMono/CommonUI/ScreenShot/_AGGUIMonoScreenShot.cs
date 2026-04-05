using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家穿搭分享界面
    /// </summary>
    public class _AGGUIMonoScreenShot : _AALBasicUIWndMono
    {
        [ALHeader("返回按钮")]
        public GameObject btnBack;
        [ALHeader("截屏按钮")]
        public GameObject btnShot;
        [ALHeader("截屏用于界面上显示的截图大小")]
        public Vector2 showClothesImageSize = new Vector2(540f,960f);
        [ALHeader("实际分享出去的截图大小")]
        public Vector2 shareClothesImageSize = new Vector2(135f,240f);
        [ALHeader("切图的区域显示")]
        public RectTransform showRectTransform;
        [ALHeader("距离边框的最小比例")]
        public float showRectSpaceScale = 0.05f;
    }
}