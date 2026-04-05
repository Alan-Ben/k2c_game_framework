using ALBasicProtocolPack;
using GS2GC.p016_ChapterOp;

namespace GOE
{
    /// <summary>
    /// 关卡事件变更
    /// </summary>
    public class GSSubDealer_016_053_OnChapterEventChg : NPSubDealer<GS2GC_016_053_OnChapterEventChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_016_053_OnChapterEventChg _createProtocolObj()
        {
            return new GS2GC_016_053_OnChapterEventChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_016_053_OnChapterEventChg _msg)
        {
			NPPlayer.instance.chapterComp.onChapterEventChg(_msg);
        }
    }
}