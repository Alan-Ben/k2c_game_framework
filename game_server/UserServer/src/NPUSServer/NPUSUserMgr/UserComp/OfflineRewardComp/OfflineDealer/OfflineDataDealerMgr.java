package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPCommon.Log.CommLog;

/**
 * 离线数据处理对象
 * @author mj
 *
 */
public class OfflineDataDealerMgr
{
    private static OfflineDataDealerMgr _g_instance = new OfflineDataDealerMgr();
    public static OfflineDataDealerMgr getInstance()
    {
        return _g_instance;
    }

    //处理对象数组
    public _AOfflineDataDealer[] _m_arrDealer;

    public OfflineDataDealerMgr()
    {
        _m_arrDealer = new _AOfflineDataDealer[EOfflineRewardEnum.EOfflineRewardEnum_Length];


        //注册处理对象
        regDealer(new OfflineDealer_GM());//玩家GM命令

        regDealer(new OfflineDealer_DinnerOwnerResult());//宴会主人结算数据
        regDealer(new OfflineDealer_DinnerBeJoined());//开宴玩家增加宴会交互记录

        regDealer(new OfflineDealer_FriendAcceptApply());//好友收到邀请
        regDealer(new OfflineDealer_FriendAgreeApply());//同意好友邀请
        regDealer(new OfflineDealer_FriendRemove());//移除好友

        regDealer(new OfflineDealer_AdultMarryAcceptPersonApply());//子嗣联姻收到个人邀请
        regDealer(new OfflineDealer_AdultMarryAgreePersonApply());//子嗣联姻同意个人申请
        regDealer(new OfflineDealer_AdultMarryCancelPersonApply());//子嗣联姻取消个人邀请
        regDealer(new OfflineDealer_AdultMarryRefusePersonApply());//子嗣联姻拒绝个人邀请
        regDealer(new OfflineDealer_AdultMarryAgreeServerApply());//子嗣联姻同意全服联姻
        regDealer(new OfflineDealer_AddultMarryReward());//子嗣联姻奖励

        regDealer(new OfflineDealer_JoinGuild());//玩家加入联盟
        regDealer(new OfflineDealer_QuitGuild());//玩家退出联盟
        
        regDealer(new OfflineDealer_MarsTeamOccupyResult());//火星系统-占领结果
        regDealer(new OfflineDealer_MarsExplorePVPLog());//火星系统-PVP日志
        regDealer(new OfflineDealer_MarsMineTeamSettle());//火星系统-火星矿结算
        regDealer(new OfflineDealer_MarsRallyJoinToWait());//火星系统-集结加入到达切换等待
        regDealer(new OfflineDealer_MarsRallyJoinFailBack());//火星系统-集结加入失败遣返

        regDealer(new OfflineDealer_ChgPlayerRecord());//变更玩家记录
        regDealer(new OfflineDealer_OrderPay());//订单支付
        regDealer(new OfflineDealer_OrderDelivery());//订单支付
        
        regDealer(new OfflineDealer_GuildMarsHelpSuc());//公会-火星互助

        regDealer(new OfflineDealer_GuildBoxDispatch());//联盟宝箱-发送奖励邮件
        
        regDealer(new OfflineDealer_GuildMarsHelpBeAutoDealed());//联盟-火星互助自动-被帮助玩家处理
        regDealer(new OfflineDealer_GuildMarsHelpAutoDeal());//联盟-火星互助自动-帮助玩家处理

        regDealer(new OfflineDealer_ForbidChat());//禁言
        regDealer(new OfflineDealer_LiftForbidChat());//解除禁言
    }

    public void regDealer(_AOfflineDataDealer _dealer)
    {
        _m_arrDealer[_dealer.getEnum().ordinal()] = _dealer;
    }

    public _AOfflineDataDealer getDealer(EOfflineRewardEnum _enum)
    {
    	_AOfflineDataDealer dealer = _m_arrDealer[_enum.ordinal()];
    	if(null == dealer)
    	{
    		CommLog.error("OfflineDataDealer:{} lost.", _enum);
    	}
    	
    	return dealer;
    }
}
