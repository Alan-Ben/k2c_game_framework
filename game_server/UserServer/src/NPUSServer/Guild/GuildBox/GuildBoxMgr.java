package NPUSServer.Guild.GuildBox;

import AllRpcData.US_Service.Guild.GuildMemberSettleGuildBox_2C;
import Common.GuildEnum.EGuildBoxType;
import Common.ServerObj.ServerObj_GuildBoxSettleList;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Guild.RefGuildBox;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.USLog;
import USDB.Bo.GuildBoxBO;

/**
 * 公会宝箱数据管理
 * @author mj
 *
 */
public class GuildBoxMgr 
{
	//公会数据
    private GuildInfo _m_guildInfo;
    //公会宝箱数据列表
    private GuildBoxTypeMgr[] _m_arrTypeMgrArr;
    
    public GuildBoxMgr(GuildInfo _guild)
    {
    	_m_guildInfo = _guild;
    	_m_arrTypeMgrArr = new GuildBoxTypeMgr[EGuildBoxType.EGuildBoxType_Length];
    	
    	_initTypeMgr();
    }
    
    public GuildInfo getGuild() {return _m_guildInfo;}
    public GuildBoxTypeMgr getTypeMgr(EGuildBoxType _type) {return _m_arrTypeMgrArr[_type.ordinal()];}
    
    private void _initTypeMgr()
    {
    	for(int i = 0; i < EGuildBoxType.EGuildBoxType_Length; i++)
    	{
    		EGuildBoxType type = EGuildBoxType.EGuildBoxType_FromInt(i);
    		if(null == type)
    			continue;
    		
    		if(EGuildBoxType.NONE == type)
    			continue;
    		
    		_m_arrTypeMgrArr[type.ordinal()] = new GuildBoxTypeMgr(type, _m_guildInfo);
    	}
    }
    
    public void _initFromBo(GuildBoxBO _bo) 
    {
    	RefGuildBox typeRef = RefGuildBox.getMgr().get(_bo.getBoxId());
    	if(null == typeRef)
    		return;
    	
    	GuildBoxTypeMgr typeMgr = getTypeMgr(typeRef.type);
    	if(null == typeMgr)
    		return;
    	
    	typeMgr._initFromBo(_bo);
    }

    public void _onInited()
    {
    	for(int i = 0; i < _m_arrTypeMgrArr.length; i++)
    	{
    		GuildBoxTypeMgr typeMgr = _m_arrTypeMgrArr[i];
    		if(null == typeMgr)
    			continue;
    	
    		typeMgr._onInited();
    	}
    }
    
    /**
     * 新玩家加入联盟时处理
     * @param _member
     */
    public void _updateMemberMaxInstanceId(GuildMemberInfo _member)
    {
    	for(int i = 0; i < _m_arrTypeMgrArr.length; i++)
    	{
    		GuildBoxTypeMgr typeMgr = _m_arrTypeMgrArr[i];
    		if(null == typeMgr)
    			continue;
    	
    		_member.getMemberGuildBoxInfo().updateLastGuildBox(typeMgr.getType(), typeMgr.getMaxBoxInstanceId());
    	}
    }
    
    /**
     * 增加联盟宝箱，根据宝箱的配置找到对应的处理对象
     * @param _cid
     * @param _boxId
     * @param _count
     * @param _context
     */
    public void addBox(long _cid, long _boxId, long _count, NPPlayerContext _context)
    {
    	RefGuildBox ref = RefGuildBox.getMgr().get(_boxId);
    	if(null == ref)
    		return;
    	
    	GuildBoxTypeMgr typeMgr = getTypeMgr(ref.type);
    	if(null == typeMgr)
    		return;
    		
    	typeMgr.addBox(_cid, _boxId, _count, _context);
    }
    
    /**
     * 所有宝箱都发起结算
     * @param _member
     */
    public void settlePlayerAllBox(GuildMemberInfo _member)
    {
    	ServerObj_GuildBoxSettleList obj = new ServerObj_GuildBoxSettleList();
    	
    	for(int i = 0; i < _m_arrTypeMgrArr.length; i++)
    	{
    		GuildBoxTypeMgr typeMgr = _m_arrTypeMgrArr[i];
    		if(null == typeMgr)
    			continue;
    		
    		typeMgr.settlePlayerBox(_member, obj);
    	}
    	
    	//结算玩家数据
		//统一走Rpc走offlineReward处理机制
		GuildMemberSettleGuildBox_2C rpc = new GuildMemberSettleGuildBox_2C();
		rpc.req().setCid(_member.getCid());
		rpc.req().setBoxList(obj);

		int usId = CommonFunc.parseServerTypeIdFromCid(_member.getCid());
		long guildId = getGuild().getGuildId();

		getGuild().getGuildMgr().getServer().rpc2us().requestToRepeat(usId, rpc, null, 3
				, ()-> {
					USLog.error(getGuild().getGuildMgr().getServer(), "player:{} guild:{} send rpc guildMemberSettleGuildBox fail.", _member.getCid(), guildId);
				});
    }
    
    /**
     * 销毁数据
     */
    public void discard()
    {
    	for(int i = 0; i < _m_arrTypeMgrArr.length; i++)
    	{
    		GuildBoxTypeMgr typeMgr = _m_arrTypeMgrArr[i];
    		if(null == typeMgr)
    			continue;
    		
    		typeMgr.discard();
    	}
    }
}
