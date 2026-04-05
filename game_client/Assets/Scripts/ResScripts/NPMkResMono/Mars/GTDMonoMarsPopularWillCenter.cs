using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 民意中心
    /// </summary>
    public class GTDMonoMarsPopularWillCenter : MonoBehaviour
    {
        [ALHeader("UI跟随的目标")]
        public Transform uiFollowTarget;
        [ALHeader("UI跟随的资源ID")]
        public int followUiAssetPathId;
        
        [ALHeader("点击区域")]
        public GTDCommonPosClickMono monoClick;
        [ALHeader("点击聚焦位置")]
        public Transform clickFocusPosition;
        [ALHeader("建筑聚焦的设置")]
        public Vector2 focusViewportPos = new Vector2(0.5f, 0.7f);
        public float focusScale = 1.2f;
        public float focusTime = 0.5f;
    }
}