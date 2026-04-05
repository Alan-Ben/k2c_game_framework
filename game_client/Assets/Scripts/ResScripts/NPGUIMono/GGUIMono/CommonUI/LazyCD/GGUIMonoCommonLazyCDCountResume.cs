using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// lazyCD数量显示以及恢复通用脚本
    /// </summary>
    public class GGUIMonoCommonLazyCDCountResume : _AALBasicUIWndMono
    {
        [ALHeader("倒计时文本")] 
        public TextEx txtTimeDown;
        [ALHeader("数量文本")] 
        public TextEx txtCount;
        [ALHeader("数量不足的时候显示，充足就隐藏")] 
        public List<GameObject> goListCDingShow;
        [ALHeader("数量不足的时候隐藏，充足就显示")] 
        public List<GameObject> goListCDingHide;
        [ALHeader("数量满的时候显示，不满就隐藏")] 
        public List<GameObject> goListMaxShow;
        [ALHeader("特效父节点")] 
        public Transform sfxParent;
        [ALHeader("数量增加显示的特效id")] 
        public long addCountSfxId;

        [ALHeader("当数量满时是否需要设置当前数量文本颜色，这个勾选下面两个颜色才有效")]
        public bool isSetCurTextColorWhenFull;
        [ALHeader("当数量满时当前数量文本颜色")]
        public Color curTextFullColor = Color.white;
        [ALHeader("当数量未满时当前数量文本颜色")]
        public Color curTextNotFullColor = Color.white;
        
        [ALHeader("恢复按钮")]
        public GameObject btnResume;
    }
}