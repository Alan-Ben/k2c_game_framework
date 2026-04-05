
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingUpgradeSuccess : _AALBasicUIWndMono
    {
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("升级建筑样式变化")]
        public CommonUpgradePropertyShow<RawImage> monoPreviewTex;
        [ALHeader("等级提升的描述")]
        public Text txtUpgradeDesc;
        [ALHeader("升级建筑等级变化")]
        public CommonUpgradePropertyShow<Text> monoLevel;
        [ALHeader("升级建筑员工上限变化")]
        public CommonUpgradePropertyShow<Text> monoEmployeeLimit;
        [ALHeader("升级建筑收益倍率变化")]
        public CommonUpgradePropertyShow<Text> monoEmployeeEarningsRate;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("解锁的视频描述和缩略图")]
        public Text txtVideoDesc;
        public RawImage imgVideoPreview;
        [ALHeader("有解锁新视频时展示的对象")]
        public List<GameObject> listVideoUnlockShow;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1105); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1105); } }
    }
}