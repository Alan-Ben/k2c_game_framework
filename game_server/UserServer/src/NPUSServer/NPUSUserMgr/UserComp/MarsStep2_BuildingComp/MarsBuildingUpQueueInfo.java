package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.GuildEnum.EGuildMarsHelpObjType;
import Common.GuildObj.Guild_MarsHelp_BuildingQueue;
import Common.MarsObj.Mars_BuildingUpQueue;
import NPCommon.DB.BM.BM;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp._IGuildMarsHelp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerMarsBuildingUpQueueBO;

import java.nio.ByteBuffer;

public class MarsBuildingUpQueueInfo implements _IGuildMarsHelp
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//队列数据
	private PlayerMarsBuildingUpQueueBO _m_bo;
	//升级消耗道具列表
	private NPCommon_ItemList _m_ilUpgradeCostItemList;

	public MarsBuildingUpQueueInfo(NPUSUserData _userData, PlayerMarsBuildingUpQueueBO _bo)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;

		_m_ilUpgradeCostItemList = new NPCommon_ItemList();
		_initUpgradeCostItemList();
	}

	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public PlayerMarsBuildingUpQueueBO getBo() {return _m_bo;}
    public long getId() {return getBo().getId();}
    public long getBuildingId() {return getBo().getBuildingId();}
    public int getTargetLvl() {return getBo().getTargetLvl();}
    public long getStartUpgradeLvlMs() {return getBo().getStartUpgradeLvlMs();}
    public long getEndUpgradeLvlMs() {return getBo().getEndUpgradeLvlMs();}
    public long getGuildHelpId() {return getBo().getGuildHelpId();}
    public int getGuildHelpSecs() {return getBo().getGuildHelpSecs();}
    public int getItemHelpSecs() {return getBo().getItemHelpSecs();}
    
    public NPCommon_ItemList getUpgradeCostItemList() {return _m_ilUpgradeCostItemList;}

    /**
     * 最终截至时间，包含互助时间
     * @return
     */
    public long getFinalEndUpgradeLvlMs() 
    {
    	return getBo().getEndUpgradeLvlMs() - getGuildHelpSecs() * 1000 - getItemHelpSecs() * 1000;
    }
    
    protected void _initUpgradeCostItemList() 
	{
		if(null != _m_bo.getUpgradeCost())
		{
			ByteBuffer buff = ByteBuffer.wrap(_m_bo.getUpgradeCost());
			_m_ilUpgradeCostItemList.readPackage(buff);
		}
	}
    
    public Mars_BuildingUpQueue toProto()
    {
    	Mars_BuildingUpQueue proto = new Mars_BuildingUpQueue();
    	proto.setId(getId());
    	proto.setBuildingId(getBuildingId());
    	proto.setStartUpgradeLvlMs(getStartUpgradeLvlMs());
    	proto.setEndUpgradeLvlMs(getEndUpgradeLvlMs());
    	proto.setGuildHelpId(getGuildHelpId());
    	proto.setGuildHelpSecs(getGuildHelpSecs());
    	proto.setItemHelpSecs(getItemHelpSecs());
    	
    	return proto;
    }

	/**
	 * 加速升级（减少截至时间）
	 * @param _reduceSecs
	 * @param _context
	 * @return 实际使用的时长（秒）
	 */
	public int reduceSecs(int _reduceSecs, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long endUpgradeLvlMs = getEndUpgradeLvlMs();
			//当前无升级
			if(endUpgradeLvlMs == 0)
				return 0;
			
			//计算并更新数据
			long remainMs = getFinalEndUpgradeLvlMs() - CommonFunc.getNowTimeMS();
			//无需加速
			if(remainMs <= 0)
				return 0;
			
			//计算加速时长
			long realReduceMs = Math.min((_reduceSecs * 1000), remainMs);
			int realReduceSecs = (int) Math.ceil(realReduceMs / 1000f);
			realReduceSecs = Math.max(realReduceSecs, 0);
			//更新加速时长（秒）
			if(realReduceSecs > 0)
			{
				getBo().saveItemHelpSecs(getBM(), getItemHelpSecs() + realReduceSecs);
			}
			
			return realReduceSecs;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 移除数据
	 */
	protected void _del() 
	{
		getBo().del(getBM());
	}

	//--------------------------- 求助对象接口方法 ---------------------------//
	@Override
	public EGuildMarsHelpObjType getObjType() 
	{
		return EGuildMarsHelpObjType.BUILDING_QUEUE;
	}
	
	@Override
	public long getObjId() 
	{
		return getId();
	}

	@Override
	public _IALProtocolStructure makeExt() 
	{
		Guild_MarsHelp_BuildingQueue proto = new Guild_MarsHelp_BuildingQueue();
		proto.setBuildingId(getBuildingId());
		proto.setLvl(getTargetLvl());
		
		return proto;
	}

	@Override
	public long getHelpId() 
	{
		return getGuildHelpId();
	}

	@Override
	public int getHelpSecs() 
	{
		return getGuildHelpSecs();
	}

	@Override
	public boolean canSendHelp(EGuildMarsHelpObjType _objEGuildMarsHelpObjType)
	{
		return getGuildHelpId() <= 0;
	}

	@Override
	public void setSendHelp(long _helpId) 
	{
		getUserData().lockUser();
		
		try
		{
			getBo().setGuildHelpId(getBM(), _helpId);
			getBo().saveAll(getBM());
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public void updateHelpSecs(long _helpId, int _helpSecs) 
	{
		getUserData().lockUser();
		
		try
		{
			//检查请求ID
			if(getGuildHelpId() != _helpId)
				return;
			
			//检查互助时长（只大不小）
			if(_helpSecs <= getGuildHelpSecs())
				return;
			
			//帮助时间增大，不会变小		
			getBo().setGuildHelpSecs(getBM(), _helpSecs);
			getBo().saveAll(getBM());
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
