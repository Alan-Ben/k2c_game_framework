using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsBuildQueue : _AALBasicUIWndMono
    {
        [ALHeader("动画组件")]
        public Animation anim;
        [ALHeader("展示按钮和展示动画名")]
        public GameObject btnShow;
        public string showAnimName;
        [ALHeader("隐藏按钮和隐藏动画名")]
        public GameObject btnHide;
        public string hideAnimName;
        [ALHeader("建造队列容器")]
        public GGUIMonoMarsBuildQueueContainer monoBuildQueueContainer;
    }
}