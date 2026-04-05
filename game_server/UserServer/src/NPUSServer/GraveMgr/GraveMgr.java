package NPUSServer.GraveMgr;

import Common.GraveObj.GraveObj_NewInfo;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUserServer;

/**
 * 杰出者数据管理
 * @author mj
 *
 */
public class GraveMgr 
{
	//归属US服务器对象
	private NPUserServer _m_usUSServer;
	//新晋数据管理
	private GraveNewMgr _m_mgrGraveNewMgr;
	//记录数据管理
	private GraveRecordMgr _m_mgrGraveRecordMgr;
	//称号记录数据管理（只记录本服玩家数据）
	private GravePlayerTitleRecordMgr _m_mgrGravePlayerTitleRecordMgr;
	
	public GraveMgr(NPUserServer _usServer)
	{
		_m_usUSServer = _usServer;
		
		_m_mgrGraveNewMgr = new GraveNewMgr(_m_usUSServer);
		_m_mgrGraveRecordMgr = new GraveRecordMgr(_m_usUSServer);
		_m_mgrGravePlayerTitleRecordMgr = new GravePlayerTitleRecordMgr(_m_usUSServer);
	}
	
	public NPUserServer getUSServer() {return _m_usUSServer;}
	public GraveNewMgr getGraveNewMgr() {return _m_mgrGraveNewMgr;}
	public GraveRecordMgr getGraveRecordMgr() {return _m_mgrGraveRecordMgr;}
	public GravePlayerTitleRecordMgr getGravePlayerTitleRecordMgr() {return _m_mgrGravePlayerTitleRecordMgr;}
	
	public boolean s_init()
	{
		return getGraveNewMgr().s_init() 
				&& getGraveRecordMgr().s_init() 
				&& getGravePlayerTitleRecordMgr().s_init();
	}
	
	/**
	 * 增加杰出者数据
	 * @param _newInfo
	 * @param _endMs
	 */
	public void addGraveNewInfo(GraveObj_NewInfo _newInfo, long _endMs)
	{
		//新晋杰出者数据
		getGraveNewMgr().addGraveNewInfo(_newInfo, _endMs);
		
		//杰出者数据
		getGraveRecordMgr().addRecord(_newInfo);
		
		//杰出者（本服）的称号历史记录
		if(getUSServer().getServerTypeId() == CommonFunc.parseServerTypeIdFromCid(_newInfo.getCid()))
		{
			getGravePlayerTitleRecordMgr().addRecord(_newInfo);
		}
	}
	public void addGraveNewInfo(GraveObj_NewInfo _newInfo)
	{
		long endMs = CommonFunc.getNowTimeMS() + RefGeneral.Ref().grave_congratulate_show_time_ts * 1000;
		
		addGraveNewInfo(_newInfo, endMs);
	}
}
