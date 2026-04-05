using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingProductGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("业务图标")]
        public RawImage imgProductIcon;
        public RawImage imgProductIcon2;
        [ALHeader("产出进度")]
        public Slider sldOutputProgress;
        [ALHeader("上飘点的位置")]
        public Transform transOutputPoint;
        public long outputTipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID;
    }
}