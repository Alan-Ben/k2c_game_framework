using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 招聘简历移动时动画类型
    /// </summary>
    public enum EHireResumeMoveAniType
    {
        [InspectorName("START_MOVE_RIGHT（开始右滑）")]
        START_MOVE_RIGHT,
        [InspectorName("END_MOVE_RIGHT（结束右滑）")]
        END_MOVE_RIGHT,
        [InspectorName("START_MOVE_LEFT（开始左滑）")]
        START_MOVE_LEFT,
        [InspectorName("END_MOVE_LEFT（结束左滑）")]
        END_MOVE_LEFT,
    }

    /// <summary>
    /// 招聘体验界面
    /// </summary>
    public class GGUIMonoHireMain : _AALBasicUIWndMono
    {
        [ALHeader("跳过按钮")]
        public GameObject btnSkip;
        [ALHeader("同意按钮")]
        public GameObject btnAgree;
        [ALHeader("拒绝按钮")]
        public GameObject btnRefuse;
        [ALHeader("拖拽按钮")]
        public GameObject btnDrag;
        [ALHeader("展示跳过按钮的操作次数，大于等于该次数则展示跳过按钮")]
        public int showSkipAfterOpNum = 3;
        [ALHeader("跳过GO")]
        public GameObject goSkip;
        [ALHeader("简历数量")]
        public Text txtResumeNum;
        [ALHeader("上面的招聘简历item")]
        public GGUIMonoHireResumeItem monoFirstHireResumeItem;
        [ALHeader("下面的招聘简历item")]
        public GGUIMonoHireResumeItem monoSecondHireResumeItem;
        [ALHeader("只剩下一个时需要显示的GO列表")]
        public List<GameObject> goOneLeftShowList;
        [ALHeader("只剩下一个时需要隐藏的GO列表")]
        public List<GameObject> goOneLeftHideList;
        [ALHeader("新简历摆正动画")]
        public CommonAnimationSingleInfo aniStraighten;

        [ALInfo("====滑动表现相关配置====")]
        [ALHeader("右滑目标终点")]
        public RectTransform rightTargetPoint;
        [ALHeader("左滑目标终点")]
        public RectTransform leftTargetPoint;
        [ALHeader("右滑临界距离，超过这个距离会自动同意并划出item")]
        public float moveRightCriticalDistance = 200;
        [ALHeader("左滑临界距离，超过这个距离会自动拒绝并划出item")]
        public float moveLeftCriticalDistance = 200;
        [ALHeader("点击按钮自动移动时间秒")]
        public float clickAutoMoveTime = 0.5f;
        [ALHeader("松手自动回弹移动总时间秒，会按距离比例算实际时间")]
        public float autoMoveBackTime = 0.5f;
        [ALHeader("超出临界自动移动时间秒")]
        public float overCriticalAutoMoveTime = 0.5f;
        [ALHeader("招聘简历移动时需要播放的动画配置")]
        public CommonAnimationShowTypeInfo<EHireResumeMoveAniType> moveAniTypeInfo;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6500); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6500); } }
    }
}