using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    // 征收弹窗基类
    public abstract class _AGGUIMonoLevyBase: _AALBasicUIWndMono
    {
        [ALHeader("每秒产量")]
        public TextEx productSecTxt;

        [ALHeader("每秒产量使用的key")]
        public string productSecTxtKey;

        [ALHeader("每秒产量显示图片")]
        public RawImage productSecImg;

        [ALHeader("tick 周期 /秒")]
        public float tickSecs;

        [ALHeader("属性展示")]
        public GGUIMonoCommonAttrItem attrItemMono;

        [ALHeader("对应的属性类型")]
        public EBasicAttrType attrType;

        [ALHeader("说明弹窗跟随偏移量")]
        public float infoToolTipInterval;

        [ALHeader("说明按钮")]
        public GameObject infoBtn;

        [ALHeader("获取按钮")]
        public GameObject getBtn;

        [ALHeader("关闭按钮")]
        public GameObject closeBtn;
    }
}
