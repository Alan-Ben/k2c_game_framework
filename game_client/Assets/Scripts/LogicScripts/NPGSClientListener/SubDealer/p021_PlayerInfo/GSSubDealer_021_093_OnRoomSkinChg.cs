using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 房间皮肤变更
    /// </summary>
    public class GSSubDealer_021_093_OnRoomSkinChg : NPSubDealer<GS2GC_021_093_OnRoomSkinChg>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_021_093_OnRoomSkinChg _createProtocolObj()
        {
            return new GS2GC_021_093_OnRoomSkinChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_093_OnRoomSkinChg _msg)
        {
            NPPlayer.instance.roomSkinComp.onRoomSkinChg(_msg);
        }
    }
}
