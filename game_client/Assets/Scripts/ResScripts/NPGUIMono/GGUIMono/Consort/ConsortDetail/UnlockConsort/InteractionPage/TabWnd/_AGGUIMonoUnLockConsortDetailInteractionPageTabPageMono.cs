using ALPackage;
using UnityEngine;

namespace GOE
{
    public class _AGGUIMonoUnLockConsortDetailInteractionPageTabPageMono : _AALBasicUIWndMono
    {
        [ALHeader("详细信息窗口")]
        public GGUISubMonoUnlockConsortDetailInfo monoConsortDetailInfo;
        
        [ALHeader("仅显示形象动画")]
        public Animation onlyShowActorAnim;
        [ALHeader("仅显示形象功能开启时展示动画名")]
        public string onlyShowActorFuncOnAnimName;
        [ALHeader("仅显示形象功能关闭时展示动画名")]
        public string onlyShowActorFuncOffAnimName;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
    }
}