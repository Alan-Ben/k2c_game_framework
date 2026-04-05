package NPUSServer.GMCommand.Cmds;


import Common.DinnerEnum.EDinnerPermitType;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Dinner.RefDinnerPermit;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSMain;
import NPUSServer.NPUserServer;

@ACommander(comment = "宴会命令", name = "dinner")
public class CmdDinner extends UsCmdBase
{
	@ACommand(comment = "宴会系统[US服务器ID]")
	public String info(int _usId)
	{
		NPUserServer usServer = NPUSMain.GetServerByTypeId(_usId);
		if(null == usServer)
			return "fail, not find us[" + _usId + "]";
		
		StringBuilder sb = new StringBuilder();
		sb.append("\ngroup:").append(usServer.getDinnerPool().getGroupId());
		
		return sb.toString();
	}

	@ACommand(comment = "设置宴会服务器分组IDD[US服务器ID，分组ID]")
	public String setGroup(int _usId, long _groupId)
	{
		NPUserServer usServer = NPUSMain.GetServerByTypeId(_usId);
		if(null == usServer)
			return "fail, not find us[" + _usId + "]";
		
		usServer.getDinnerPool().chgGroupId(_groupId);
		
		return "ok";
	}
	
	@ACommand(comment = "结算玩家自身宴会")
    public String settleOwn()
    {
		DinnerInfo info = getUserServer().getDinnerPool().lookupByOwnerCid(getOwner().getCid());
		if(null == info)
			return "fail, not find dinner.";
		
		info.getBo().setEndTs(getUserServer().getBM(), CommonFunc.getNowTimeSec());
		info.getBo().saveAll(getUserServer().getBM());
		
		return "ok";
    }
	
	@ACommand(comment = "增加宴会凭证[凭证类型，凭证类型ID]")
    public String addPermit(EDinnerPermitType _permitType, long _typeId)
    {
		RefDinnerPermit ref = RefDinnerPermit.getMgr().get(_permitType.ordinal());
		if(null == ref)
			return "fail, not find ref, type:" + _permitType;
		
		getOwner().getDinnerComponent().addPermit(ref, _typeId, getContext());
		
		return "ok";
    }
}
