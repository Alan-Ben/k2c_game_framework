using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 骑士推荐跟随入口item
    /// </summary>
    public class GGUIMonoHeroRecommendPointItem : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}