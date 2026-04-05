using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家升级成功弹窗属性变化item
    /// </summary>
    public class GGUIMonoPlayerLvlUpSucContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("属性名称")]
        public Text txtName;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("旧的值")]
        public Text txtLastNum;
        [ALHeader("新的值")]
        public Text txtCurNum;
        [ALHeader("增加的数值")]
        public Text txtAddNum;
        // [ALHeader("特殊条目显示的文本颜色")]
        // public Color specialColor = Color.white;
        // [ALHeader("新增条目显示的文本颜色")]
        // public Color addColor = Color.white;
        // [ALHeader("未变化条目显示的文本颜色")]
        // public Color normalColor = Color.white;
        [ALHeader("是特殊条目时需要展示的GO列表")]
        public List<GameObject> goSpecialShowList;
        [ALHeader("是新增条目时需要显示的GO列表")]
        public List<GameObject> goAddShowList;
        [ALHeader("是未变化条目时需要显示的GO列表")]
        public List<GameObject> goNormalShowList;
        [ALHeader("延时开始展示数字跳动时间(秒)")]
        public float delayShowTextChgSec;
        [ALHeader("数字跳动时间(秒)")]
        public float showTextChgDurationSec;
        [ALHeader("数字开始跳动时需要播放的动画")]
        public CommonAnimationSingleInfo aniShowTextChg;
    }
}