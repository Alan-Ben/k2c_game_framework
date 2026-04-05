using ALBasicProtocolPack;
using GS2GC.p007_CommOp;

namespace GOE
{
    /// <summary>
    /// 问卷调查活动开始
    /// </summary>
    public class GSSubDealer_007_063_OnQuestionnaireStart : NPSubDealer<GS2GC_007_063_OnQuestionnaireStart>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_007_063_OnQuestionnaireStart _createProtocolObj()
        {
            return new GS2GC_007_063_OnQuestionnaireStart();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_007_063_OnQuestionnaireStart _msg)
        {
			NPPlayer.instance.questionnaireComp.onQuestionnaireStart(_msg);
        }
    }
}