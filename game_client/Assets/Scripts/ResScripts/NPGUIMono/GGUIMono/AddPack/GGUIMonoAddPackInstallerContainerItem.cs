using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum GGUIMonoAddPackInstallerContainerItemState
    {
        [InspectorName("等待下载")]
        NEED_DOWNLOAD,
        [InspectorName("下载中")]
        DOWNLOADING,
        [InspectorName("下载完成")]
        DOWNLOAD_DONE,
    }
    public class GGUIMonoAddPackInstallerContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("下载器的名字")]
        public Text txtName;
        [ALHeader("下载进度百分比")]
        public Text txtPercentProgress;
        [ALHeader("下载进度文件大小")]
        public Text txtSizeProgress;
        [ALHeader("下载进度条")]
        public Slider sldProgress;

        [ALHeader("开始按钮")]
        public GameObject btnStart;
        [ALHeader("停止按钮")]
        public GameObject btnStop;
        
        public MultiStateShow<GGUIMonoAddPackInstallerContainerItemState> stateShow;
    }
}