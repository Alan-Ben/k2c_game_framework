using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 弹幕容器
    /// </summary>
    public class GGUIMonoCommentContainer : _AALBasicUIWndMono
    {
        [ALHeader("ScrollRect")]
        public ScrollRect scrollRect;
        [ALHeader("容器layout")] 
        public LayoutGroup itemContainer;
        [ALHeader("保存最大数量")]
        public int maxCount = 10;
    }
}