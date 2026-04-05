using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 新晋杰出者奖励可以领取
    /// </summary>
    public class GSSubDealer_004_062_OnGraveNewReward : NPSubDealer<GS2GC_004_062_OnGraveNewReward>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_004_062_OnGraveNewReward _createProtocolObj()
        {
            return new GS2GC_004_062_OnGraveNewReward();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_062_OnGraveNewReward _msg)
        {
			NPPlayer.instance.graveComp.onGraveNewReward(_msg);
        }
    }
}