using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{

    /// <summary>
    /// 火星任务类型
    /// </summary>
    public enum EMarsMissionType
    {
        [InspectorName("GO_TO（前往）")]
        GO_TO,
        [InspectorName("BASE（基地）")]
        BASE,
        [InspectorName("DEVELOP（发展）")]
        DEVELOP,
        [InspectorName("EXPLORE（探索）")]
        EXPLORE,
    }

    /// <summary>
    /// 火星任务预览页签
    /// </summary>
    [System.Serializable]
    public class GGUIMarsMissionPreviewTabMono
    {
        [ALHeader("页签类型")]
        public EMarsMissionType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("需要显示的GO列表")]
        public List<GameObject> goShowList;
    }

    /// <summary>
    /// 火星任务预览界面
    /// </summary>
    public class GGUIMonoMarsMissionPreview : _AALBasicUIWndMono
    {
        [ALHeader("点击关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public List<GGUIMarsMissionPreviewTabMono> monoTabList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7000); } }
    }
}
