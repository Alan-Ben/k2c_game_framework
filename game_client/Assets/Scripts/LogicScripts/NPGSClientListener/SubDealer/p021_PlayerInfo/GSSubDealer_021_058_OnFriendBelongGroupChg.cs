using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 好友归属分组变更
    /// </summary>
    public class GSSubDealer_021_058_OnFriendBelongGroupChg : NPSubDealer<GS2GC_021_058_OnFriendBelongGroupChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_021_058_OnFriendBelongGroupChg _createProtocolObj()
        {
            return new GS2GC_021_058_OnFriendBelongGroupChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_058_OnFriendBelongGroupChg _msg)
        {
            NPPlayer.instance.friendsComp.onFriendBelongGroupChg(_msg);
        }
    }
}