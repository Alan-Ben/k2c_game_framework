using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子故事Grid
    /// </summary>
    public class GGUIMonoConsortStoryGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoConsortStoryGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;

        [ALHeader("故事类型bar的UI资源id")]
        public long storyTypeBarUiResId;
    }
}