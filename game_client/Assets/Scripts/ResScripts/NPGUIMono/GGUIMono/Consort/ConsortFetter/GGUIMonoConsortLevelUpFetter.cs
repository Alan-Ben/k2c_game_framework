using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 羁绊升级弹窗
    /// </summary>
    public class GGUIMonoConsortLevelUpFetter : _AALBasicUIWndMono
    {
        [ALHeader("当前羁绊等级名")]
        public TextEx txtNowFetterLvlName;
        [ALHeader("当前羁绊等级称号图标")]
        public RawImage nowFetterLvlSign;
        
        [ALHeader("下一羁绊等级名")]
        public TextEx txtNextFetterLvlName;
        [ALHeader("下一羁绊等级称号图标")]
        public RawImage nextFetterLvlSign;

        [ALHeader("子嗣品质有提升时显示")]
        public List<GameObject> childQualityHasImprovedShow;
        [ALHeader("当前羁绊等级子嗣品质")] 
        public TextEx txtNowFetterLvlChildQuality;
        [ALHeader("下一羁绊等级子嗣品质")]
        public TextEx txtNextFetterLvlChildQuality;
        [ALHeader("查看羁绊等级效果详情按钮")]
        public GameObject btnFetterLvlEffectDetail;
        [ALHeader("羁绊等级效果详情弹窗ui路径id")]
        public long fetterLvlDetailToolTipUIResId;
        
        [ALHeader("羁绊技能等级有提升时显示")]
        public List<GameObject> fetterSkillLvlHasImprovedShow;

        [ALHeader("羁绊技能名")]
        public TextEx txtFetterSkillName;
        [ALHeader("羁绊技能图标")]
        public RawImage imgFetterSkillIcon;
        
        [ALHeader("当前羁绊技能等级")]
        public TextEx txtNowFetterSkillLvl;
        [ALHeader("下一羁绊技能等级")]
        public TextEx txtNextFetterSkillLvl;

        [ALHeader("当前羁绊技能等级效果描述")]
        public TextEx txtNowFetterSkillLvlEffectDesc;
        
        [ALHeader("下一羁绊技能等级效果描述")]
        public TextEx txtNextFetterSkillLvlEffectDesc;
        [ALHeader("下一羁绊技能等级效果描述文本(一个参数, 加成值)")]
        public string nextFetterSkillLvlEffectDescKey;

        [ALHeader("提升需要玩家等级")]
        public TextEx txtNeedPlayerLvl;
        [ALHeader("需要玩家等级描述key")]
        public string txtNeedPlayerLvlKey;
        
        [ALHeader("需要亲密度")]
        public TextEx txtNeedIntimacy;
        [ALHeader("需要的加护力")]
        public TextEx txtNeedCharm;
        [ALHeader("需要值足够时显示颜色")]
        public Color needEnoughColor;
        [ALHeader("需要值不足够时显示颜色")]
        public Color needNotEnoughColor;

        [ALHeader("达到最高级时显示")]
        public List<GameObject> maxLevelShow;
        [ALHeader("达到最高级时隐藏")]
        public List<GameObject> maxLevelHide;
        
        [ALHeader("等级提升按钮")]
        public GameObject btnLevelUp;
        [ALHeader("升级特效父节点")]
        public Transform levelUpSfxParent;
        [ALHeader("升级特效id")]
        public long levelUpSfxId;
        
        [ALHeader("关闭弹窗")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1416); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1416);} }
    }
}