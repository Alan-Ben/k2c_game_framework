
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChildGraduatedDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("基础村庄收益")] 
        public TextWithCustomKey txtBaseEarnings;
        [ALHeader("监护者加成")]
        public TextWithCustomKey txtGuardianBonus;
        [ALHeader("天资加成")]
        public TextWithCustomKey txtTalentBonus;
        [ALHeader("职业加成")]
        public TextWithCustomKey txtCareerBonus;
        [ALHeader("评级加成")]
        public TextWithCustomKey txtQualityBonus;
        [ALHeader("其它加成")]
        public GGUIMonoCommonPropertyDetail monoOtherBonus;
        [ALHeader("是卷王或是普通人时展示的对象")]
        public List<GameObject> listSuperShow;
        public List<GameObject> listNormalShow;


        public void setIsSuper(bool _isSuper)
        {
            ALUGUICommon.setGameObjEnable(listSuperShow, false);
            ALUGUICommon.setGameObjEnable(listNormalShow, false);
            ALUGUICommon.setGameObjEnable(_isSuper ? listSuperShow : listNormalShow, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1212); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1212); } }
    }
}