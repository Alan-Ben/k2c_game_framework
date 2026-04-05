using ALPackage;

namespace GOE
{
    public class GGUIMonoDinnerCreateItem_GiftdeChildCele : _AGGUIMonoDinnerCreateItemBase
    {
        [ALHeader("宴会名字")]    public TextEx txtName;
        [ALHeader("宴会描述")]    public TextEx txtDesc;
        [ALHeader("凭证倒计时")] public TextEx txtLifeTime;
        [ALHeader("子嗣形象")]   public GGUIMonoChildInfo childCardItem;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2928); } }
        public static string objName   { get { return UIResPathAssistant.getObjName(2928); } }
    }
}
