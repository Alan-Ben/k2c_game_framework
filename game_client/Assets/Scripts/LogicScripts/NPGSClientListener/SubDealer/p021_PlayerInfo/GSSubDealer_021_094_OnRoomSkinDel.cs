using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 房间皮肤删除
    /// </summary>
    public class GSSubDealer_021_094_OnRoomSkinDel : NPSubDealer<GS2GC_021_094_OnRoomSkinDel>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_021_094_OnRoomSkinDel _createProtocolObj()
        {
            return new GS2GC_021_094_OnRoomSkinDel();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_094_OnRoomSkinDel _msg)
        {
            NPPlayer.instance.roomSkinComp.onRoomSkinDel(_msg);
        }
    }
}
