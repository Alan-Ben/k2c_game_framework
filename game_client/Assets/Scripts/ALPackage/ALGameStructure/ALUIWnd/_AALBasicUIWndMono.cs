using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/******************
 * 基本的窗口脚本对象
 **/
namespace ALPackage
{
    public abstract class _AALBasicUIWndMono : MonoBehaviour
    {
        public Animation wndAnimation;
        [ALHeader("显示动画延迟时间，在时间结束之后会调用showWnd函数的延迟回调")]
        public string showAniName = "show";
        [ALHeader("隐藏动画延迟时间，在时间结束之后会调用hideWnd函数的延迟回调")]
        public string hideAniName = "hide";
        public float showAniTime;
        public float hideAniTime;

        //iphoneX下的缩放尺寸
        public List<RectTransform> iphoneXSetUIObj;
        public Vector2 iphoneXOffsetMax;
        public Vector2 iphoneXOffsetMin;

        /** ui控制相关对象 */
        public List<GraphicRaycaster> uiGraphicRaycasterList;

#if AL_PUERTS
        [ALHeader("puerts脚本路径，不存在则留空")]
        public string puertsScriptsPath;
#endif

#if AL_XLUA
        [ALHeader("lua脚本路径，不存在则留空")]
        public string luaScriptsPath;
#endif

        public static string GetAssetPath(string _localPath, string _bundlePath) {
#if UNITY_EDITOR
            //if (ALLocalResLoaderMgr.instance.isLoadGameObjectFromLocal) {
            //    return _localPath;
            //}
            //else {
                return _bundlePath;
            //}
#else
                return _bundlePath;
#endif
        }
    }
}
