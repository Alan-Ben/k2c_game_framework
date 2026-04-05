using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChildInfo : _AALBasicUIWndMono
    {
        [ALHeader("子嗣头像")]
        public RawImage imgIcon;
        [ALHeader("半身像")]
        public RawImage imgCardIcon;
        [ALHeader("展示 3d 形象的 showcase ")]
        public GGUIMonoCommonShowCase scTdShow;
        [ALHeader("展示迷你 3d 形象的 showcase ")]
        public GGUIMonoCommonShowCase scTdMiniShow;
        [ALHeader("相性图片")]
        public RawImage imgAttrIcon;
        [ALHeader("监护人头像")]
        public RawImage imgGuardianIcon;
        [ALHeader("监护人头像品质背景")]
        public Image imgGuardianIconBg;
        [ALHeader("子嗣名字和其语言 key")]
        public Text txtName;
        public string nameKey;
        [ALHeader("监护人和其语言 key")]
        public Text txtGuardian;
        public string guardianKey;
        [ALHeader("职业和其语言 key")]
        public Text txtCareer;
        public string careerKey;
        [ALHeader("天资和其语言 key")] 
        public Text txtQuality;
        public string qualityKey;
        [ALHeader("初始亲密度和其语言 key")]
        public Text txtIntimacy;
        public string intimacyKey;
        [ALHeader("子嗣的所属的玩家名和其语言 key ")]
        public Text txtPlayerName;
        public string playerNameKey;
        [ALHeader("资质图片")]
        public RawImage imgQuality;
        [ALHeader("资质的背景")]
        public RawImage imgQualityBg;
        [ALHeader("如果是卷王展示什么")]
        public List<GameObject> listSuperShow;
        [ALHeader("村庄收益")]
        public Text txtEarnings;
        public string earningsKey;
        [ALHeader("性别展示")] 
        public List<GameObject> listMaleShow;
        public List<GameObject> listFemaleShow;
        
        
        public void setSuperShow(bool _isSuper)
        {
            ALUGUICommon.setGameObjEnable(listSuperShow, _isSuper);
        }
        public void setSexShow(bool _isMale)
        {
            ALUGUICommon.setGameObjEnable(listMaleShow, false);
            ALUGUICommon.setGameObjEnable(listFemaleShow, false);
            ALUGUICommon.setGameObjEnable(_isMale ? listMaleShow : listFemaleShow, true);
        }
    }
}