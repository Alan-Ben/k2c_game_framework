
using ALBasicProtocolPack;
using Hotfix.GS2GC.p202_NumMergeOp;

namespace Hotfix
{
    /// <summary>
    /// 棋盘变更推送处理
    /// </summary>
    public class GSSubDealer_202_050_OnNumMergeBoardChg : HotfixSubDealer<GS2GC_202_050_OnNumMergeBoardChg>
    {
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_202_050_OnNumMergeBoardChg _msg)
        {
            if (_msg?.getBoardData() == null)
                return;

            // 更新组件数据
            HotfixNPPlayer.instance.numMergeComponent.updateBoardData(_msg);
        }
    }
}
