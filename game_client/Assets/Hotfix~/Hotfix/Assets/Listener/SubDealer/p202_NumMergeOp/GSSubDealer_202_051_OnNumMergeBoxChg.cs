
using ALBasicProtocolPack;
using Hotfix.GS2GC.p202_NumMergeOp;

namespace Hotfix
{
    /// <summary>
    /// 宝箱变更推送处理
    /// </summary>
    public class GSSubDealer_202_051_OnNumMergeBoxChg : HotfixSubDealer<GS2GC_202_051_OnNumMergeBoxChg>
    {
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_202_051_OnNumMergeBoxChg _msg)
        {
            if (_msg == null)
                return;

            // 更新组件数据
            HotfixNPPlayer.instance.numMergeComponent.updateBoxData(_msg);
        }
    }
}
