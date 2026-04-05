using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场贸易站信息弹窗
    /// </summary>
    public class GGUIMonoArenaStationInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("当前等级")]
        public Text txtLevel;
        [ALHeader("下个等级")]
        public Text txtNextLevel;
        [ALHeader("当前收益速度")]
        public Text txtCurCollectSpeed;
        [ALHeader("下个收益速度")]
        public Text txtNextCollectSpeed;
        [ALHeader("当前上限")]
        public Text txtCurLimit;
        [ALHeader("下个上限")]
        public Text txtNextLimit;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("收益详情按钮")]
        public GameObject btnSpeedDetail;
        [ALHeader("上限详情按钮")]
        public GameObject btnLimitDetail;
        [ALHeader("满级时需要显示的GO列表")]
        public List<GameObject> goMaxLevelShowList;
        [ALHeader("满级时需要隐藏的GO列表")]
        public List<GameObject> goMaxLevelHideList;
        [ALHeader("收益详情tip的偏移量")]
        public float speedTipInterval;
        [ALHeader("上限详情tip的X偏移量")]
        public float limitTipIntervalX;
        [ALHeader("上限详情tip的Y偏移量")]
        public float limitTipIntervalY;
        [ALHeader("有特权无上限时需要显示的GO列表")]
        public List<GameObject> goHavePrivilegeShowList;
        [ALHeader("有特权无上限时需要隐藏的GO列表")]
        public List<GameObject> goHavePrivilegeHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5201); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5201); } }
    }
}