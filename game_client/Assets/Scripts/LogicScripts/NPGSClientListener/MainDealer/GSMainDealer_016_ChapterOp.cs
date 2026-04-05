
using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_016_ChapterOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_016_ChapterOp()
        : base(16, 70)
        {
			regDealer(new GSSubDealer_016_051_OnChapterPosChg());
			regDealer(new GSSubDealer_016_052_OnChapterInspireChg());
			regDealer(new GSSubDealer_016_053_OnChapterEventChg());
            regDealer(new GSSubDealer_016_054_OnChapterGmPosChg());
            regDealer(new GSSubDealer_016_055_OnChapterPlotRewardDraw());
        }
    }
}