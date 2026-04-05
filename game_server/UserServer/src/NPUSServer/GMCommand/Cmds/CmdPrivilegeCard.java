package NPUSServer.GMCommand.Cmds;


import Common.PrivilegeCardEnum.EPrivilegeCardType;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.PrivilegeCardComp.PrivilegeCardInfo;

@ACommander(comment = "权益卡相关命令", name = "pCard")
public class CmdPrivilegeCard extends UsCmdBase
{
    @ACommand(comment = "权益卡信息")
    public String info()
    {
    	return getOwner().getPrivilegeCardComponent().toString();
    }
    
    @ACommand(comment = "设置权益卡[权益卡类型，开启时间（秒），结束时间（秒），上次领取时间（秒）]")
    public String set(EPrivilegeCardType _card, int _startS, int _endS, int _lastGainS)
    {
    	PrivilegeCardInfo info = getOwner().getPrivilegeCardComponent().getPrivilegeCard(_card);
    	if(null == info)
    		return "fail, not find card.";
    	
    	info.cmdSet(_startS, _endS, _lastGainS);
    	
    	return "ok";
    }

    @ACommand(comment = "自动结算权益卡数据[卡类型]")
    public String autoSettle(EPrivilegeCardType _type)
    {
    	PrivilegeCardInfo card = getOwner().getPrivilegeCardComponent().getPrivilegeCard(_type);
    	if(null == card)
    		return "fail, not find card.";
    	
    	StringBuilder sb = new StringBuilder();
    	card.autoSettle(getContext(), sb);
    	
    	return sb.toString();
    }
}
