using ALBasicProtocolPack;
using GS2GC.p016_ChapterOp;

namespace GOE
{
    /// <summary>
    /// 关卡剧情奖励领取推送
    /// </summary>
    public class GSSubDealer_016_055_OnChapterPlotRewardDraw : NPSubDealer<GS2GC_016_055_OnChapterPlotRewardDraw>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_016_055_OnChapterPlotRewardDraw _createProtocolObj()
        {
            return new GS2GC_016_055_OnChapterPlotRewardDraw();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_016_055_OnChapterPlotRewardDraw _msg)
        {
            NPPlayer.instance.chapterComp.OnChapterPlotRewardDraw(_msg);

        }
    }
}