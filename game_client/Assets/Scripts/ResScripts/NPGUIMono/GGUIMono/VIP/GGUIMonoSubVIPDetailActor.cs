using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP详情加载页面形象
    /// </summary>
    public class GGUIMonoSubVIPDetailActor : _AALBasicUIWndMono
    {
        [ALHeader("顾问情人形象")]
        public GGUIMonoCommonShowCase monoShowCase;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("称号")]
        public Text txtTitle;
        [ALHeader("预览按钮")]
        public GameObject btnPreview;
        [ALHeader("形象在td showcase中的index(配置小于0的值时不显示)")]
        public int actorInShowCaseIndex = 0;
        [ALHeader("在td showcase中的index(配置小于0的值时不显示)")]
        public int bgInShowCaseIndex = 3;
        [ALHeader("是顾问时需要显示的GO列表")]
        public List<GameObject> goHeroShowList;
        [ALHeader("是家人时需要显示的GO列表")]
        public List<GameObject> goConsortShowList;
    }
}
