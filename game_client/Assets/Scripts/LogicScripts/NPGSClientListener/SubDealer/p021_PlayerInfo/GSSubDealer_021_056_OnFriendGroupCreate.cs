using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 创建分组
    /// </summary>
    public class GSSubDealer_021_056_OnFriendGroupCreate : NPSubDealer<GS2GC_021_056_OnFriendGroupCreate>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_021_056_OnFriendGroupCreate _createProtocolObj()
        {
            return new GS2GC_021_056_OnFriendGroupCreate();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_056_OnFriendGroupCreate _msg)
        {
            NPPlayer.instance.friendsComp.onFriendGroupCreate(_msg);
        }
    }
}