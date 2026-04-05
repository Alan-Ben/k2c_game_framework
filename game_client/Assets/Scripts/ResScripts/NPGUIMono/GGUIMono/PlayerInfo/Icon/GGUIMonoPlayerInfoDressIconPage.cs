using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 头像类型
    /// </summary>
    public enum EPlayerInfoIconType
    {
        NONE,
        Normal,//默认
        Consort,//恋人
        Hero,//骑士
    }

    /// <summary>
    /// 玩家装扮-头像
    /// </summary>
    ///
    public class GGUIMonoPlayerInfoDressIconPage : _ANPGGUIMonoPlayerInfoDressBasePage
    {
        [ALHeader("头像列表")]
        public GGUIMonoPlayerIconListItemGrid monoListGrid;
        [ALHeader("来源描述")]
        public Text txtSource;
    }
}

