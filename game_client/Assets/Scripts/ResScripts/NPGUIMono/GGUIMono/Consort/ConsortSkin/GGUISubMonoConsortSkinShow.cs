using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUISubMonoConsortSkinShow : _AALBasicUIWndMono
    {
        [ALHeader("所属妃子品质展示")]
        public GGUIMonoConsortQualityShow monoConsortQualityShow;
        [ALHeader("所属妃子名称文本")]
        public TextEx txtConsortName;
        [ALHeader("所属妃子名TextMesh")]
        public TMP_Text txtConsortNameMesh;
        [ALHeader("所属妃子称号文本")]
        public TextEx txtConsortTitle;
        
        [ALHeader("皮肤名")]
        public TextEx txtConsortSkinName;
        [ALHeader("皮肤描述")]
        public TextEx txtConsortSkinDesc;
        [ALHeader("是默认皮肤时显示的物体列表")]
        public List<GameObject> isDefaultSkinShow;
        [ALHeader("是默认皮肤时隐藏的物体列表")]
        public List<GameObject> isDefaultSkinHide;

        [ALHeader("星级")]
        public GGUIMonoHeroCommonStar monoStar;
        
        [ALHeader("头像")]
        public RawImage headIcon;
        [ALHeader("头像背景")]
        public Image headBg;
        
        [ALHeader("半身像")]
        public RawImage cardRawImage;
        [ALHeader("半身像背景")]
        public RawImage cardBg;
        
        [ALHeader("妃子td showcase")]
        public GGUIMonoCommonShowCase monoShowcase;
        [ALHeader("妃子形象在td showcase中的index(配置小于0的值时不显示)")]
        public int consortActorInShowCaseIndex = 0;
        [ALHeader("背景在td showcase中的index(配置小于0的值时不显示)")]
        public int bgInShowCaseIndex = 3;
        [ALHeader("进入时候妃子播放的动画名")]
        public string enterAniName; 
    }
}