using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟派遣item
    /// </summary>
    public class GGUIMonoGuildDispatchItem : _AALBasicUIWndMono
    {
        [ALHeader("属性名")]
        public TextEx txtAttrName;

        [ALHeader("属性图标")]
        public RawImage attrImg;

        [ALHeader("加成百分比")]
        public TextEx txtAddPro;

        [ALHeader("派遣大臣列表")]
        public GGUIMonoGuildDispatchHeroIconContainer monoDispatchHeroContainer;
    }
}