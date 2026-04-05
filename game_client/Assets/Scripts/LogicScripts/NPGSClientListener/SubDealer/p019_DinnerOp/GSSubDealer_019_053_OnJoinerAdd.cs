using ALBasicProtocolPack;
using GS2GC.p019_DinnerOp;

namespace GOE
{
    /// <summary>
    /// 参加宴会玩家推送
    /// </summary>
    public class GSSubDealer_019_053_OnJoinerAdd : NPSubDealer<GS2GC_019_053_OnJoinerAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_019_053_OnJoinerAdd _createProtocolObj()
        {
            return new GS2GC_019_053_OnJoinerAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_019_053_OnJoinerAdd _msg)
        {
            NPPlayer.instance.dinnerComp.OnJoinerAdd(_msg);
        }
    }
}