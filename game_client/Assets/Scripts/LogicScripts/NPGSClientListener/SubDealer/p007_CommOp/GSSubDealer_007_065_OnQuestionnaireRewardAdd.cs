using ALBasicProtocolPack;
using GS2GC.p007_CommOp;

namespace GOE
{
    /// <summary>
    /// 问卷调查可以领取奖励推送
    /// </summary>
    public class GSSubDealer_007_065_OnQuestionnaireRewardAdd : NPSubDealer<GS2GC_007_065_OnQuestionnaireRewardAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_007_065_OnQuestionnaireRewardAdd _createProtocolObj()
        {
            return new GS2GC_007_065_OnQuestionnaireRewardAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_007_065_OnQuestionnaireRewardAdd _msg)
        {
			NPPlayer.instance.questionnaireComp.onQuestionnaireRewardAdd(_msg);
        }
    }
}