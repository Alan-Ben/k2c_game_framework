package MGClient.Cmd.Cmds;

import Common.Common_AiChatMessage;
import CommonEnum.EAiChatRoleType;
import GC2GS.p004_PlayerOp.GC2GS_004_010_ReqSomeOnePlayerInfo;
import GC2GS.p004_PlayerOp.GC2GS_004_015_ReqWeekCardSettleInfo;
import GC2GS.p011_ClientDataOp.GC2GS_011_003_ReqWebEncryptedData;
import GC2GS.p015_ConsortOp.GC2GS_015_022_ReqConsortAiChat;
import GC2GS.p021_PlayerInfo.GC2GS_021_027_ReqDailyCheckRefresh;
import GC2GS.p021_PlayerInfo.GC2GS_021_028_ReqDailyCheck;
import GC2GS.p021_PlayerInfo.GC2GS_021_029_ReqDailyCheckDrawReward;
import GS2GC.p004_PlayerOp.GS2GC_004_010_RetSomeOnePlayerInfo;
import GS2GC.p004_PlayerOp.GS2GC_004_015_RetWeekCardSettleInfo;
import GS2GC.p011_ClientDataOp.GS2GC_011_003_RetWebEncryptedData;
import GS2GC.p015_ConsortOp.GS2GC_015_022_RetConsortAiChat;
import GS2GC.p021_PlayerInfo.GS2GC_021_027_RetDailyCheckRefresh;
import GS2GC.p021_PlayerInfo.GS2GC_021_028_RetDailyCheck;
import GS2GC.p021_PlayerInfo.GS2GC_021_029_RetDailyCheckDrawReward;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;
import NPGC2GS.p001_BasicOp.NPGC2GS_001_011_ReqPlayerJoinedUSList;
import NPGC2GS.p001_BasicOp.NPGC2GS_001_012_ReqMostRecommendedUSInfo;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "玩家", name = "player")
public class CmdPlayer extends CmdBase
{
    @Command(comment = "查询玩家展示数据")
    public void showInfo(long _cid)
    {
        GC2GS_004_010_ReqSomeOnePlayerInfo proto = new GC2GS_004_010_ReqSomeOnePlayerInfo();
        proto.setCid(_cid);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_004_010_RetSomeOnePlayerInfo>(GS2GC_004_010_RetSomeOnePlayerInfo.class)
        {
            @Override
            public void handle(GS2GC_004_010_RetSomeOnePlayerInfo _response)
            {
            }
        });
    }

    @Command(comment = "查询服务器列表")
    public void showUsList()
    {
        NPGC2GS_001_011_ReqPlayerJoinedUSList proto = new NPGC2GS_001_011_ReqPlayerJoinedUSList();
//        getOwner().request(proto, new _AClientRequestHandler<NPGS2GC_001_011_RetPlayerJoinedUSList>(NPGS2GC_001_011_RetPlayerJoinedUSList.class) {
//            @Override
//            public void handle(NPGS2GC_001_011_RetPlayerJoinedUSList _response) {
//
//            }
//        });
        getOwner().getGateListener().send(proto);
    }

    @Command(comment = "最推荐服务器id")
    public void mostRecommendedUSLogicId()
    {
        NPGC2GS_001_012_ReqMostRecommendedUSInfo proto = new NPGC2GS_001_012_ReqMostRecommendedUSInfo();
//        getOwner().request(proto, new _AClientRequestHandler<NPGS2GC_001_012_RetUSInfoList>(NPGS2GC_001_012_RetUSInfoList.class) {
//            @Override
//            public void handle(NPGS2GC_001_012_RetUSInfoList _response) {
//
//            }
//        });
        getOwner().getGateListener().send(proto);
    }

    @Command(comment = "签到")
    public void dailyCheck(long _dessertId)
    {
        GC2GS_021_028_ReqDailyCheck proto = new GC2GS_021_028_ReqDailyCheck();
        proto.setDessertId(_dessertId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_021_028_RetDailyCheck>(GS2GC_021_028_RetDailyCheck.class) {
            @Override
            public void handle(GS2GC_021_028_RetDailyCheck _response) {

            }
        });
    }

    @Command(comment = "刷新")
    public void refreshDailyCheck()
    {
        GC2GS_021_027_ReqDailyCheckRefresh proto = new GC2GS_021_027_ReqDailyCheckRefresh();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_021_027_RetDailyCheckRefresh>(GS2GC_021_027_RetDailyCheckRefresh.class) {
            @Override
            public void handle(GS2GC_021_027_RetDailyCheckRefresh _response) {

            }
        });
    }

    @Command(comment = "领每日签到奖励")
    public void drawDailyCheck(int _days)
    {
        GC2GS_021_029_ReqDailyCheckDrawReward proto = new GC2GS_021_029_ReqDailyCheckDrawReward();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_021_029_RetDailyCheckDrawReward>(GS2GC_021_029_RetDailyCheckDrawReward.class) {
            @Override
            public void handle(GS2GC_021_029_RetDailyCheckDrawReward _response) {

            }
        });
    }

    @Command(comment = "查看周卡结算信息")
    public void weekCardSettleInfo()
    {
        GC2GS_004_015_ReqWeekCardSettleInfo proto = new GC2GS_004_015_ReqWeekCardSettleInfo();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_004_015_RetWeekCardSettleInfo>(GS2GC_004_015_RetWeekCardSettleInfo.class) {
            @Override
            public void handle(GS2GC_004_015_RetWeekCardSettleInfo _response) {

            }
        });
    }

    @Command(comment = "请求web透传参数")
    public void testWebToken()
    {
        GC2GS_011_003_ReqWebEncryptedData proto = new GC2GS_011_003_ReqWebEncryptedData();
        proto.setProject(String.valueOf(16));
        proto.setLangCode("zh-CN");
        proto.setPackageName("xxx");
        proto.setUserPay(99.99F);
        proto.setProducts("{\"6002\":1,\"6003\":1}");
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_011_003_RetWebEncryptedData>(GS2GC_011_003_RetWebEncryptedData.class) {
            @Override
            public void handle(GS2GC_011_003_RetWebEncryptedData _response) {

            }
        });
    }

    @Command(comment = "ai聊天")
    public void aiChat()
    {
        GC2GS_015_022_ReqConsortAiChat msg = new GC2GS_015_022_ReqConsortAiChat();
        msg.setConsortId(2101);
        msg.getMsgList().add(new Common_AiChatMessage(EAiChatRoleType.USER, "你好"));
        getOwner().request(msg, new _AClientRequestHandler<GS2GC_015_022_RetConsortAiChat>(GS2GC_015_022_RetConsortAiChat.class)
        {
            @Override
            public void handle(GS2GC_015_022_RetConsortAiChat _response)
            {
            }
        });
    }
}
