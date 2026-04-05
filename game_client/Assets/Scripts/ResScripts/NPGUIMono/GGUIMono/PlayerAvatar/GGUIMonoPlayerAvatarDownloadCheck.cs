
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum GGUIMonoPlayerAvatarDownloadCheckBtnState
    {
        [InspectorName("需要下载")]
        NEED_DOWNLOAD,
        [InspectorName("下载中")]
        DOWNLOADING,
        [InspectorName("已下载")]
        DOWNLOADED,
    }
    public class GGUIMonoPlayerAvatarDownloadCheck : _AALBasicUIWndMono
    {
        [ALHeader("下载未下载的内容")]
        public GameObject btnDownload;
        [ALHeader("下载进度")]
        public Text txtDownloadProgress;
        public Slider sliderDownloadProgress;

        [ALHeader("各种下载状态下的显示情况")]
        public MultiStateShow<GGUIMonoPlayerAvatarDownloadCheckBtnState> stateShow;

        /************
        * 资源加载路径
        */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_PLAYER_AVATAR_DOWNLOAD_TIP); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_PLAYER_AVATAR_DOWNLOAD_TIP);} }
    }
}