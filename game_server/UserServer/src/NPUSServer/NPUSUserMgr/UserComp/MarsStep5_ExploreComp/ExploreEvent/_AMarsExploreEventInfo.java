package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.MarsEnum.EMarsExploreEventType;
import Common.MarsObj.Mars_ExploreEvent;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MARS_EVENT_DONE;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerMarsExploreEventBO;

public abstract class _AMarsExploreEventInfo 
{
	/**
	 * 事件类型
	 * @return
	 */
	abstract public EMarsExploreEventType getEventType();
	/**
	 * 开启处理
	 * @param _team
	 * @param _context
	 * @return
	 */
	abstract public Result dealStart(MarsExploreTeam _team, NPPlayerContext _context);

	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//事件数据
	private PlayerMarsExploreEventBO _m_bo;
	
	public _AMarsExploreEventInfo(NPUSUserData _userData, PlayerMarsExploreEventBO _bo)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;
	}

	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public PlayerMarsExploreEventBO getBo() {return _m_bo;}
    public long getId() {return getBo().getId();}
    public int getExploreLvl() {return getBo().getExploreLvl();}
    public long getEventId() {return getBo().getEventId();}
    public int getQuality() {return getBo().getQuality();}
    public long getPos() {return getBo().getPos();}
    public long getCreatedMs() {return getBo().getCreatedMs();}
    public boolean isDone() {return getBo().getIsDone();}

    public void setDone()
	{
		getBo().saveIsDone(getBM(), true);

		//推送事件数据
		getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_055_OnExploreEventDone(getId()));
	}
    public void setExtData(_IALProtocolStructure _value) {getBo().saveExtData(getBM(), CommonFunc.ByteBfferToBytes(_value.makePackage()));}

    public Mars_ExploreEvent toProto()
    {
    	Mars_ExploreEvent proto = new Mars_ExploreEvent();
    	proto.setId(getId());
    	proto.setEventType(getEventType());
    	proto.setEventId(getEventId());
    	proto.setPos(getPos());
    	proto.setIsDone(isDone());
    	proto.setExploreLvl(getExploreLvl());
    	proto.setCreatedMs(getCreatedMs());
    	
    	return proto;
    }
    
    /**
     * 完成事件触发
     * @param _context
     */
    protected void _onDone(NPPlayerContext _context) 
    {
		Event_P_MARS_EVENT_DONE evt = new Event_P_MARS_EVENT_DONE(_context);
		getUserData().onLogicEvent(evt);
	}
    
    protected void _del()
    {
    	getBo().del(getBM());
    	getUserData().getMarsExploreComponent().getEventMgr()._removeEvent(this);
    	//推送数据
    	getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_052_OnExploreEventDel(getId()));
    }
}
