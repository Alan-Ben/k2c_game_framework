using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 房间皮肤添加
    /// </summary>
    public class GSSubDealer_021_092_OnRoomSkinAdd : NPSubDealer<GS2GC_021_092_OnRoomSkinAdd>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_021_092_OnRoomSkinAdd _createProtocolObj()
        {
            return new GS2GC_021_092_OnRoomSkinAdd();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_092_OnRoomSkinAdd _msg)
        {
            NPPlayer.instance.roomSkinComp.onRoomSkinAdd(_msg);
        }
    }
}
