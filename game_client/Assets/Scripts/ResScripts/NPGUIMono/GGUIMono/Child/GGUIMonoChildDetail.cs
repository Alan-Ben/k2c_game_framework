
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChildDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("子嗣的信息")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("单次教学的奖励数")]
        public Text txtEducatingAwards;
        [ALHeader("基础奖励")]
        public Text txtEducatingBaseAwards;
        [ALHeader("学生天资的加成和语言 key ")]
        public Text txtChildQualityBonus;
        public string childQualityBonusKey;
        [ALHeader("来自其它系统的加成")]
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
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1203); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1203); } }
    }
}