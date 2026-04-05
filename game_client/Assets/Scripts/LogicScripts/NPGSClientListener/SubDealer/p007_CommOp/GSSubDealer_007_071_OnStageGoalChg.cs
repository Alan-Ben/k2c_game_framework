using ALBasicProtocolPack;
using GS2GC.p007_CommOp;

namespace GOE
{
    /// <summary>
    /// 阶段目标变动
    /// </summary>
    public class GSSubDealer_007_071_OnStageGoalChg : NPSubDealer<GS2GC_007_071_OnStageGoalChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_007_071_OnStageGoalChg _createProtocolObj()
        {
            return new GS2GC_007_071_OnStageGoalChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_007_071_OnStageGoalChg _msg)
        {
            if (null == _msg)
                return;

            NPPlayer.instance.stageGoalComp.onStageGoalChg(_msg.getStageGoal());
        }
    }
}