package NPUSServer.Guild.Member;

import Common.GuildEnum.EGuildBoxType;
import NPCommon.DB.BM.BM;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GuildMemberBO;

/**
 * 联盟玩家的联盟宝箱相关数据的修改更新
 * @author mj
 *
 */
public class GuildMemberGuildBoxInfo 
{
	//对应联盟玩家数据
	private GuildMemberInfo _m_member;
	
	//对应宝箱的可刷新数量，用于通知客户端，在领取后重置
	private int _m_iClientActiveAddCount;
	private int _m_iClientFreeAddCount;
	private int _m_iClientGiftAddCount;
	
	public GuildMemberGuildBoxInfo(GuildMemberInfo _member)
	{
		_m_member = _member;
	}
	
	public GuildMemberInfo getMember() {return _m_member;}
	
	public GuildInfo getGuild() {return _m_member.getMemberMgr().getGuildInfo();}
	public long getGuildId() {return getGuild().getGuildId();}
	public NPUserServer getServer() {return getGuild().getGuildMgr().getServer();}
	public BM getBM() {return getServer().getBM();}

	public GuildMemberBO getBo() {return getMember().getBo();}
	public long getCid() {return getBo().getCid();} 
    
    /**
     * 玩家上次获取的联盟宝箱数据的实例ID
     * @return
     */
    public long getLastActiveGuildBox() {return getBo().getLastActiveGuildBox();}
    public void saveLastActiveGuildBox(long _value) {getBo().saveLastActiveGuildBox(getBM(), _value);}
    
    public long getLastFreeGuildBox() {return getBo().getLastFreeGuildBox();}
    public void saveLastFreeGuildBox(long _value) {getBo().saveLastFreeGuildBox(getBM(), _value);}
    
    public long getLastGiftGuildBox() {return getBo().getLastGiftGuildBox();}
    public void saveLastGiftGuildBox(long _value) {getBo().saveLastGiftGuildBox(getBM(), _value);}
    
    /**
     * 获取对应宝箱类型的最后标记
     * @param _boxType
     * @return
     */
    public long getLastGuildBox(EGuildBoxType _boxType)
    {
    	if(EGuildBoxType.GUILD_ACTIVE_BOX == _boxType)
    		return getLastActiveGuildBox();
    	else if(EGuildBoxType.GUILD_FREE_BOX == _boxType)
    		return getLastFreeGuildBox();
    	else if(EGuildBoxType.GUILD_GIFT_BOX == _boxType)
    		return getLastGiftGuildBox();

		USLog.error(getServer(), "guild:{} cid:{} boxType:{} get last guild box tag fail."
				, getGuildId(), getCid(), _boxType);
    	
    	return -1;
    }
    
    /**
     * 更新对应宝箱类型的标记
     * @param _boxType
     * @param _value
     */
    public void updateLastGuildBox(EGuildBoxType _boxType, long _value)
    {
    	if(EGuildBoxType.GUILD_ACTIVE_BOX == _boxType)
    		saveLastActiveGuildBox(_value);
    	else if(EGuildBoxType.GUILD_FREE_BOX == _boxType)
    		saveLastFreeGuildBox(_value);
    	else if(EGuildBoxType.GUILD_GIFT_BOX == _boxType)
    		saveLastGiftGuildBox(_value);
    	else
    		USLog.error(getServer(), "guild:{} cid:{} boxType:{} save last guild box tag fail."
    				, getGuildId(), getCid(), _boxType);
    }
    
    /**
     * 新增宝箱时需要推送客户端
     * @param _boxType
     * @param _addValue
     */
    public void incrClientAddCount(EGuildBoxType _boxType, int _addValue)
    {
    	int clientAddCount = 0;
    	if(EGuildBoxType.GUILD_ACTIVE_BOX == _boxType)
    	{
    		_m_iClientActiveAddCount += _addValue;
    		clientAddCount = _m_iClientActiveAddCount;
    	}
    	else if(EGuildBoxType.GUILD_FREE_BOX == _boxType)
    	{
    		_m_iClientFreeAddCount += _addValue;
    		clientAddCount = _m_iClientFreeAddCount;
    	}
    	else if(EGuildBoxType.GUILD_GIFT_BOX == _boxType)
    	{
    		_m_iClientGiftAddCount += _addValue;
    		clientAddCount = _m_iClientGiftAddCount;
    	}
    	else
    	{
    		USLog.error(getServer(), "guild:{} cid:{} boxType:{} incr client add value fail."
    				, getGuildId(), getCid(), _boxType);
    		return;
    	}
    	
    	getMember().sendMsg(US2GCWriter_042_GuildRelatedOp.make_055_OnGuildBoxAddCountChg(_boxType, clientAddCount));
    }
    
    /**
     * 重置客户端展示增量
     * @param _boxType
     */
    public void resetClientAddCount(EGuildBoxType _boxType)
	{
    	if(EGuildBoxType.GUILD_ACTIVE_BOX == _boxType)
    	{
    		_m_iClientActiveAddCount = 0;
    	}
    	else if(EGuildBoxType.GUILD_FREE_BOX == _boxType)
    	{
    		_m_iClientFreeAddCount = 0;
    	}
    	else if(EGuildBoxType.GUILD_GIFT_BOX == _boxType)
    	{
    		_m_iClientGiftAddCount = 0;
    	}
    }
}
