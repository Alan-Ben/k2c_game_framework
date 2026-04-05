
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家装扮-头像框
    /// </summary>
    ///
    public class GGUIMonoPlayerInfoDressIconBgkPage : _ANPGGUIMonoPlayerInfoDressBasePage
    {
        [ALHeader("头像框列表")]
        public GGUIMonoPlayerIconBgkListItemGrid monoListGrid;
        [ALHeader("头像挂载父节点")]
        public Transform iconPosPar;
        [ALHeader("头像框挂载父节点")]
        public Transform iconBgkPosPar;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("获取途径、有效期")]
        public Text txtSource;
    }
}
