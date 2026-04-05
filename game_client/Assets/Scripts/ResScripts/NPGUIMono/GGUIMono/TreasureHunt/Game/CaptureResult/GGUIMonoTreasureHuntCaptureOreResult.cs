using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取矿石窗口
    /// </summary>
    public class GGUIMonoTreasureHuntCaptureOreResult : _AALBasicUIWndMono
    {
        [ALHeader("矿石信息")]
        public GGUIMonoTreasureHuntOreInfo monoOreInfo;

        [ALHeader("首次获得显示列表")]
        public List<GameObject> firstGotShowList;
        [ALHeader("首次获取隐藏列表")]
        public List<GameObject> firstGotHideList;

        [ALHeader("本服矿石质量记录")]
        public TextEx txtServerOreMassRecord;
        [ALHeader("本服矿石质量记录所有者名称")]
        public TextEx txtServerOreMassRecordOwnerName;

        [ALHeader("可激活技能信息")]
        public GGUIMonoTreasureHuntSkillInfo monoOreSkillInfo;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6812); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6812); } }
    }
}