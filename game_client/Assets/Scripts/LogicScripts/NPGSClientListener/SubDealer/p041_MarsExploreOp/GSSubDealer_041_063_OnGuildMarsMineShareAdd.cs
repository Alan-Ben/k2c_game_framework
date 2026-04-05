using ALBasicProtocolPack;
using GS2GC.p041_MarsExploreOp;
using UnityEngine.Assertions.Must;

namespace GOE
{
    /// <summary>
    /// 火星探索-最新联盟分享火星矿数据ID
    /// </summary>
    public class GSSubDealer_041_063_OnGuildMarsMineShareAdd : NPSubDealer<GS2GC_041_063_OnGuildMarsMineShareAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_041_063_OnGuildMarsMineShareAdd _createProtocolObj()
        {
            return new GS2GC_041_063_OnGuildMarsMineShareAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_041_063_OnGuildMarsMineShareAdd _msg)
        {
            if (_msg == null)
                return;
            
			NPPlayer.instance.marsComp.exploreSubComponent._onSharedMineDataChg(new NewestSharedMineData()
            {
                newestId = _msg.getId(),
                guildId = NPPlayer.instance.guildComp?.guildInfo?.guildId ?? 0
            });
        }
    }
}