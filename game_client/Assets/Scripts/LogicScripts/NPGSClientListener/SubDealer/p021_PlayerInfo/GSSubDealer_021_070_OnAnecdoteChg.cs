using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 推送政务数据变更消息
    /// </summary>
    public class GSSubDealer_021_070_OnAnecdoteChg : NPSubDealer<GS2GC_021_070_OnAnecdoteChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_021_070_OnAnecdoteChg _createProtocolObj()
        {
            return new GS2GC_021_070_OnAnecdoteChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_070_OnAnecdoteChg _msg)
        {
            NPPlayer.instance.anecdoteComp._onAnecdoteChg(_msg);
        }
    }
}