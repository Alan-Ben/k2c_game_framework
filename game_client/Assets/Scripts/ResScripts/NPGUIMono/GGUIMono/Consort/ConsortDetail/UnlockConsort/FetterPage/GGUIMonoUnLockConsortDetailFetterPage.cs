using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面羁绊page
    /// </summary>
    public class GGUIMonoUnLockConsortDetailFetterPage : _AGGUIMonoUnLockConsortDetailTabPage
    {
        [ALHeader("羁绊信息子窗口")]
        public GGUISubMonoConsortFetterInfo monoFetterInfo;
        [ALHeader("羁绊技能详情按钮")]
        public GameObject btnFetterSkillDetail;
        [ALHeader("羁绊详情弹窗ui路径id")]
        public long fetterSkillDetailToolTipUIResId;
        
        [ALHeader("羁绊等级详情按钮")]
        public GameObject btnFetterLvlDetail;
        [ALHeader("羁绊等级详情弹窗ui路径id")]
        public long fetterLvlDetailToolTipUIResId;
        
        [ALHeader("玩家等级进度条Slider")]
        public Slider playerLvlSlider;
        [ALHeader("玩家等级进度条Slider的值显示")]
        public TextEx txtPlayerLvlSliderValue;
        
        [ALHeader("妃子亲密度Slider")]
        public Slider consortIntimacySlider;
        [ALHeader("妃子亲密度进度条Slider的值显示")]
        public TextEx txtConsortIntimacySliderValue;
        
        [ALHeader("妃子加护力进度条Slider")]
        public Slider consortCharmSlider;
        [ALHeader("妃子加护力Slider的值显示")]
        public TextEx txtConsortCharmSliderValue;

        [ALHeader("达到最高级时显示")]
        public List<GameObject> maxLevelShow;
        [ALHeader("达到最高级时隐藏")]
        public List<GameObject> maxLevelHide;
        
        [ALHeader("升级按钮")]
        public GameObject btnLevelUp;
    }
}