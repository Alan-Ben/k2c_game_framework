using ALBasicProtocolPack;
using GS2GC.p016_ChapterOp;

namespace GOE
{
    /// <summary>
    /// 玩家关卡位置变化
    /// 作弊命令单独处理协议
    /// </summary>
    public class GSSubDealer_016_054_OnChapterGmPosChg : NPSubDealer<GS2GC_016_054_OnChapterGmPosChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_016_054_OnChapterGmPosChg _createProtocolObj()
        {
            return new GS2GC_016_054_OnChapterGmPosChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_016_054_OnChapterGmPosChg _msg)
        {
            NPPlayer.instance.chapterComp.onGMChapterPosChg(_msg);

        }
    }
}