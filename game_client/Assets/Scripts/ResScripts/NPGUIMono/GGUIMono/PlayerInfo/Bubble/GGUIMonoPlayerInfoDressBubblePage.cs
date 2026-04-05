using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家装扮-  气泡框
    /// </summary>
    public class GGUIMonoPlayerInfoDressBubblePage : _ANPGGUIMonoPlayerInfoDressBasePage
    {
        [ALHeader("气泡框列表")]
        public GGUIMonoPlayerBubbleListItemGrid monoListGrid;
        [ALHeader("气泡框挂载父节点")]
        public Transform bubblePosPar;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("获取途径、有效期")]
        public Text txtSource;
    }
}
