using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreBattleInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("横幅图片")]
        public RawImage imgBanner;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("类型图标")]
        public RawImage imgIcon;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("类型描述")]
        public Text txtTypeDesc;
        [ALHeader("距离")]
        public Text txtDistance;
        [ALHeader("所需时间")]
        public Text txtTimeTakes;
        [ALHeader("探索队伍实力")]
        public Text txtPower;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        [ALHeader("前往探索按钮")]
        public GameObject btnGoExplore;
        [ALHeader("选择队伍时相关的动画")]
        public Animation anim;
        public string teamSelectStartAnimName;
        public string teamSelectCancelAnimName;
        
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7412); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7412); } }
    }
}