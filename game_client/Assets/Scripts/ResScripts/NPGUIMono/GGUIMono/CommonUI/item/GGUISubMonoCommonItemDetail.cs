using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUISubMonoCommonItemDetail : _AALBasicUIWndMono
    {
        [ALHeader("物品图标")]
        public RawImage imgItemIcon;

        [ALHeader("物品品质底图")]
        public Image imgQualityBg;

        [ALHeader("物品名称")]
        public Text txtItemName;

        [ALHeader("物品持有数量文本")]
        public Text txtNum;
        [ALHeader("数量描述时使用的key(一个参数, 数量)")]
        public string txtNumKey = TransKeyConst.common_resource_haveNum;
        
        [ALHeader("物品产出文本")] 
        public Text txtAccess;

        [ALHeader("物品详细描述文本")] 
        public Text txtItemDesc;
    }
}