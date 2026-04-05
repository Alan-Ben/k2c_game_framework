using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("伙伴的名字和 icon ")]
        public RawImage imgHeroIcon;
        public Text txtHeroName;

        [ALHeader("原建筑名字和对建筑的加成")]
        public Text txtOriginBuilding;
        public Text txtOriginBuildingBonus;
        
        [ALHeader("新建筑名字和对建筑的加成")]
        public Text txtNewBuilding;
        public Text txtNewBuildingBonus;
    }
}