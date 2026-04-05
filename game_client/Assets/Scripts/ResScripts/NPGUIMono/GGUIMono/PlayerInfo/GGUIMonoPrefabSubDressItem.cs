using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 穿戴道具预制体item
    /// </summary>
    public class GGUIMonoPrefabSubDressItem : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("spt图标")]
        public Image imgSptIcon;
        [ALHeader("名称")]
        public Text txtName;
    }
}
