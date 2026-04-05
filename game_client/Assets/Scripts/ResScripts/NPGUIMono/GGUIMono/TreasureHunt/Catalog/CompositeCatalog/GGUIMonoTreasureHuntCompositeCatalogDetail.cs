using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntCompositeCatalogDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("品质显示物体")]
        public GGUISubMonoQualityShowGo qualityShowGoMono;

        [ALHeader("组合名")]
        public TextEx txtCompositeName;
     
        [ALHeader("矿石列表")]
        public GGUIMonoTreasureHuntOreItemContainer monoOreItemContainer;
        
        [ALHeader("图鉴描述")]
        public TextEx txtCatalogDesc;

        [ALHeader("图鉴组合完成时间")]
        public TextEx txtConstituteTime;
     
        [ALHeader("不同组合图鉴状态显示的列表")]
        public List<NPCommonEnumStatInfo<ETreasureHuntCompositeCatalogState>> compositeCatalogStateShowList;
        
        [ALHeader("普通技能信息")]
        public GGUIMonoTreasureHuntSkillInfo monoNormalSkillInfo;
        [ALHeader("高级技能信息")]
        public GGUIMonoTreasureHuntSkillInfo monoAdvancedSkillInfo;

        [ALHeader("上一个图鉴按钮")]
        public GameObject btnPre;
        [ALHeader("下一个图鉴按钮")]
        public GameObject btnNext;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6822); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6822); } }
    }
}