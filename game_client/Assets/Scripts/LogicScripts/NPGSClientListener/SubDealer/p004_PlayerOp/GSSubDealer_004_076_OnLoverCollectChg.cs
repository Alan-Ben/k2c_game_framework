using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 情人收集数据变更
    /// </summary>
    public class GSSubDealer_004_076_OnLoverCollectChg : NPSubDealer<GS2GC_004_076_OnLoverCollectChg>
    {
        protected override GS2GC_004_076_OnLoverCollectChg _createProtocolObj()
        {
            return new GS2GC_004_076_OnLoverCollectChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_076_OnLoverCollectChg _msg)
        {
            NPPlayer.instance.loverCollectComp.onLoverCollectChg(_msg);
        }
    }
}
