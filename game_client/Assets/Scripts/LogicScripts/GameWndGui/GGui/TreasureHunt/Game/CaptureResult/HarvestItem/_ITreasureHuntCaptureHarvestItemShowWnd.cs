using ALPackage;
using Common.TreasureHuntEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 太空寻宝-打捞收获物item展示窗口
    /// </summary>
    public interface _ITreasureHuntCaptureHarvestItemShowWnd
    {
        [NotNull] public _IALBasicUIWndInterface wndInstance { get; }

        /// <summary>
        /// 收获物类型
        /// </summary>
        public ETreasureHuntGainType havestType { get; }

        public void setData(_ATreasureHuntCaptureHarvestItemInfo _harvestItemInfo);
    }
}