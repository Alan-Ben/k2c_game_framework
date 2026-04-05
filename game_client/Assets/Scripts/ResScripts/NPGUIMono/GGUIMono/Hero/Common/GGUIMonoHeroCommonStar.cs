using UnityEngine;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    [System.Serializable]
    public class GGUIMonoHeroStarShowAndHide
    {
        [ALHeader("有星级时显示GO")]
        public GameObject haveStarShowGo;
        [ALHeader("有星级时隐藏GO")]
        public GameObject haveStarHideGo;
        [ALHeader("星星显示动画")]
        public CommonAnimationSingleInfo starShowAni;
    }

    /// <summary>
    /// 通用伙伴星级附加窗口
    /// </summary>
    public class GGUIMonoHeroCommonStar : _AALBasicUIWndMono
    {
        [ALHeader("星级展示列表")]
        public List<GGUIMonoHeroStarShowAndHide> starList;
    }
}