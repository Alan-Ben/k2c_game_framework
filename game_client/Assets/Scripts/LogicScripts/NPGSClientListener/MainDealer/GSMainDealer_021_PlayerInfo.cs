
using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_021_PlayerInfo : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_021_PlayerInfo() :
            base(21, 99)
        {
            //头像
            regDealer(new GSSubDealer_021_006_PlayerIconAdd());
            regDealer(new GSSubDealer_021_007_PlayerIconChg());
            regDealer(new GSSubDealer_021_008_PlayerIconDel());
            //头像框
            regDealer(new GSSubDealer_021_011_PlayerIconBgkAdd());
            regDealer(new GSSubDealer_021_012_PlayerIconBgkChg());
            regDealer(new GSSubDealer_021_014_PlayerIconBgkDel());
            //气泡框
            regDealer(new GSSubDealer_021_016_PlayerBubbleAdd());
            regDealer(new GSSubDealer_021_017_PlayerBubbleChg());
            regDealer(new GSSubDealer_021_018_PlayerBubbleDel());
			regDealer(new GSSubDealer_021_020_RetDealAnecdoteRewardEvent());
			regDealer(new GSSubDealer_021_021_RetDrawAnecdoteEarningsProcessReward());
			regDealer(new GSSubDealer_021_022_RetDrawAnecdoteEarningsFinalReward());
			regDealer(new GSSubDealer_021_040_RetDealAnecdoteChoiceEvent());
			regDealer(new GSSubDealer_021_041_RetDealCommonRefresh());

            //CD
            regDealer(new GSSubDealer_021_050_OnPlayerLazyCdChged());
            regDealer(new GSSubDealer_021_051_OnAddNewLazyCd());
            regDealer(new GSSubDealer_021_052_OnFixedCDChged());
			regDealer(new GSSubDealer_021_053_OnFuncUnlockDone());
			regDealer(new GSSubDealer_021_054_OnPrivilegeCardChg());
			regDealer(new GSSubDealer_021_055_OnFriendGroupOrderListChg());
			regDealer(new GSSubDealer_021_056_OnFriendGroupCreate());
			regDealer(new GSSubDealer_021_057_OnFriendGroupDelete());
			regDealer(new GSSubDealer_021_058_OnFriendBelongGroupChg());
			regDealer(new GSSubDealer_021_059_OnFriendGroupNameChg());

            //好友
            regDealer(new GSSubDealer_021_060_OnFriendApplyChg());
            regDealer(new GSSubDealer_021_061_OnFriendApplyRemove());
			regDealer(new GSSubDealer_021_062_OnPlayerPermissionsChg());
            regDealer(new GSSubDealer_021_063_OnFriendChg());
            regDealer(new GSSubDealer_021_064_OnFriendRemove());

            //成就相关
            regDealer(new GSSubDealer_021_066_OnAchieveChg());
            regDealer(new GSSubDealer_021_067_OnAchievePointChg());

            //政务相关
            regDealer(new GSSubDealer_021_070_OnAnecdoteChg());
			regDealer(new GSSubDealer_021_071_OnAnecdoteEventAdd());
			regDealer(new GSSubDealer_021_072_OnAnecdoteEventRemove());
			regDealer(new GSSubDealer_021_073_OnCommonRefreshChg());
           
			regDealer(new GSSubDealer_021_075_OnForeverAddChg());

			
            //游历相关
			regDealer(new GSSubDealer_021_083_OnChatEmoteGroupDel());
			regDealer(new GSSubDealer_021_084_OnChatEmoteGroupChg());
			regDealer(new GSSubDealer_021_087_OnChatEmoteGroupAdd());
			regDealer(new GSSubDealer_021_088_OnCuteActorDel());
			regDealer(new GSSubDealer_021_089_OnCuteActorChg());
			regDealer(new GSSubDealer_021_090_OnCuteActorAdd());

            //房间皮肤
            regDealer(new GSSubDealer_021_092_OnRoomSkinAdd());
            regDealer(new GSSubDealer_021_093_OnRoomSkinChg());
            regDealer(new GSSubDealer_021_094_OnRoomSkinDel());
        }
    }
}
