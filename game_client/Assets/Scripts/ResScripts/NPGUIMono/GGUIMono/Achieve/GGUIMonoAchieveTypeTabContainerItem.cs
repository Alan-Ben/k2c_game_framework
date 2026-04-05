using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 成就页签item
    /// </summary>
    public class GGUIMonoAchieveTypeTabContainerItem : _ANPGGUIMonoSingleChoiceItem
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("红点")]
        public GameObject goRedTip;
    }
}