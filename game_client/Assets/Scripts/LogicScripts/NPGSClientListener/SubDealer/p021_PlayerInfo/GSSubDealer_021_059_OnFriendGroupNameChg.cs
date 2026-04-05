using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 好友分组名变更
    /// </summary>
    public class GSSubDealer_021_059_OnFriendGroupNameChg : NPSubDealer<GS2GC_021_059_OnFriendGroupNameChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_021_059_OnFriendGroupNameChg _createProtocolObj()
        {
            return new GS2GC_021_059_OnFriendGroupNameChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_059_OnFriendGroupNameChg _msg)
        {
            NPPlayer.instance.friendsComp.onFriendGroupNameChg(_msg);
        }
    }
}