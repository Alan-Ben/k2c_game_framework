using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoConsortCGDetail : _AALBasicUIWndMono
    {
        [ALHeader("cg图片")]
        public RawImage cgImage;

        [ALHeader("cg视频")]
        public GGUIMonoSimpleVideo monoCgVideo;
        
        [ALHeader("当视频开始播放时的动画名称")]
        public string onVideoStartAniName;
        [ALHeader("当视频播放结束时的动画名称")]
        public string onVideoEndAniName;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1409); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1409);} }
    }
}