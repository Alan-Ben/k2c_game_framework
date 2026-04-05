using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用伙伴卡牌item
    /// </summary>
    public class GGUIMonoHeroCommonCardItem : _AALBasicUIWndMono
    {
        [ALHeader("名字")] 
        public Text txtName;
        [ALHeader("皮肤名字(称号)")]
        public TextEx txtSkinName;
        [ALHeader("等级")] 
        public Text txtLvl;
        [ALHeader("等级翻译KEY，没填默认只显示数值")]
        public string lvlTransKey;
        [ALHeader("实力")]
        public Text txtPower;
        [ALHeader("实力翻译KEY，没填默认只显示数值")]
        public string powerTransKey;
        [ALHeader("总资质")]
        public Text txtTalent;
        [ALHeader("资质翻译KEY，没填默认只显示数值")]
        public string talentTransKey;
        [ALHeader("卡牌形象")] 
        public RawImage texBody;
        [ALHeader("背景框")] 
        public RawImage imgBg;
        [ALHeader("相性图标")]
        public RawImage texSpecAttr;
        [ALHeader("名称背景图")]
        public RawImage texNameBg;
        [ALHeader("品质展示go")]
        public GGUISubMonoQualityShowGo monoQualityShowGo;
        [ALHeader("星级")]
        public GGUIMonoHeroCommonStar monoStar;
        [ALHeader("未解锁时显示的go列表")] 
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁时隐藏的go列表")] 
        public List<GameObject> goLockHideList;
        [ALHeader("点击按钮")] 
        public GameObject btnClick;
        [ALHeader("是否根据解锁状态设置形象颜色")]
        public bool isSetBodyColor = false;
        [ALHeader("未解锁时形象颜色")]
        public Color lockBodyColor = Color.gray;
        [ALHeader("已解锁时形象颜色")]
        public Color unLockBodyColor = Color.white;
    }
}