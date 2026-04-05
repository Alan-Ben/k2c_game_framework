
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBuildingBuildSuccess : _AALBasicUIWndMono
    {
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("建筑的预览图")]
        public RawImage imgPreviewTex;
        [ALHeader("建造成功的描述")]
        public Text txtBuildDesc;
        [ALHeader("0 收益每人的文本")]
        public Text txtZeroEarnings;
        [ALHeader("员工收益值")]
        public Text txtEarningsPerPerson;
        [ALHeader("0 容量的文本")]
        public Text txtZeroCapacity;
        [ALHeader("员工上限值")]
        public Text txtEmployeeCapacity;
        [ALHeader("如果是经营建筑才要展示的内容")]
        public List<GameObject> businessBuildingShow;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1109); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1109); } }
    }
}