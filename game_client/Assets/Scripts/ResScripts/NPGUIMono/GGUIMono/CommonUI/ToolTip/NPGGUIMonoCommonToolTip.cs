using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用的提示跟随子窗口基类
    /// </summary>
    public class NPGGUIMonoCommonToolTip : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("需要紧紧跟随的角标的go")]
        public GameObject goFollow;
        [ALHeader("是否横向跟随")]
        public bool isHorizontalFollow = false;

        [ALHeader("是否默认向下/左弹出")]
        public bool isDefaultDownLeft = false;

        [ALHeader("是否无视点击载体的RectTransform大小")]
        public bool ignoreClickRectSize = false;
    }
}