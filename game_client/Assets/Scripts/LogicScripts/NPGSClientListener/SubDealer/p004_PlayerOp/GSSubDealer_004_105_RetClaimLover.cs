using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 情人收集-领取情人返回
    /// </summary>
    public class GSSubDealer_004_105_RetClaimLover : NPSubDealer<GS2GC_004_105_RetClaimLover>
    {
        protected override GS2GC_004_105_RetClaimLover _createProtocolObj()
        {
            return new GS2GC_004_105_RetClaimLover();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_105_RetClaimLover _msg)
        {
        }
    }
}
