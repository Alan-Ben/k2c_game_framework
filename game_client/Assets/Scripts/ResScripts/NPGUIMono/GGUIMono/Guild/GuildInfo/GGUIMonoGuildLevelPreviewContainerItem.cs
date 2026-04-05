using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟等级预览信息列表item
    /// </summary>
    public class GGUIMonoGuildLevelPreviewContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("描述值")]
        public Text txtValue;
    }
}