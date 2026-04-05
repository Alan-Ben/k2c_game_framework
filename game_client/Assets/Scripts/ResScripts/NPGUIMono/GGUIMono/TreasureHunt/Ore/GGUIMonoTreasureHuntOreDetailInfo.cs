using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石详情信息窗口
    /// </summary>
    public class GGUIMonoTreasureHuntOreDetailInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("矿石信息")]
        public GGUIMonoTreasureHuntOreInfo monoOreInfo;

        [ALHeader("最大质量记录详情按钮")]
        public GameObject btnMaxMassRecordDetail;
        [ALHeader("有最大质量记录可以领取显示对象列表")]
        public List<GameObject> hasMaxMassRecordCanDrawShow;

        [ALHeader("首次获取时间")]
        public TextEx txtFirstGetTime;

        [ALHeader("普通技能信息")]
        public GGUIMonoTreasureHuntSkillInfo monoNormalSkillInfo;
        [ALHeader("高级技能信息")]
        public GGUIMonoTreasureHuntSkillInfo monoAdvancedSkillInfo;
        
        [ALHeader("上一个矿石按钮")]
        public GameObject btnPre;
        [ALHeader("下一个矿石按钮")]
        public GameObject btnNext;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6819); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6819); } }
    }
}