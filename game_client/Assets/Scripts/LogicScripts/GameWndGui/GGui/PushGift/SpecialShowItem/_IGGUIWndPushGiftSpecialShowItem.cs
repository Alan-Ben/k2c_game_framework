using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public interface _IGGUIWndPushGiftSpecialShowItem
    {
        NPCommonAssetPathInfo assetPathInfo { get; }

        [NotNull] _AALBasicLoadUIWndBasicClass wndInstance { get; }
     
        void setData(NPCommonCostItem _commonCostItem);
    }
}