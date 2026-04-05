using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public interface _ITreasureHuntAkeyCaptureResultItemDetailShowWnd
    {
        [NotNull] public _AALBasicLoadUIWndBasicClass wndInstance { get; }
        
        public NPCommonAssetPathInfo assetPathInfo { get; }

        public void setData(TreasureHuntCaptureResultBase _captureResultBase);
    }
}