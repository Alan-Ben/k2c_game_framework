using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 属性据点item
    /// </summary>
    public class GGUIMonoGuildCooperateAttrPosItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("图标2")]
        public RawImage imgIcon2;
        [ALHeader("进度条")]
        public NPGGUIMonoProgress monoProgress;
    }
}