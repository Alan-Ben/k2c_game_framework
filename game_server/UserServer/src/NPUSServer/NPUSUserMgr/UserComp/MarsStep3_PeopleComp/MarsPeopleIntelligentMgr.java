package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import Common.MarsObj.Mars_Intelligent;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Mars.RefMarsIntelligentControl;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsPeopleIntelligentBO;

import java.util.ArrayList;
import java.util.List;

public class MarsPeopleIntelligentMgr 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//决策数据列表
	private ArrayList<MarsPeopleIntelligentInfo> _m_alleIntelligentList;
	
	public MarsPeopleIntelligentMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alleIntelligentList = new ArrayList<>();
		//加载配表里所有数据
		_initAllFromRef();
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;} 
	
	//加载配表里所有数据
	private void _initAllFromRef()
	{
		List<RefMarsIntelligentControl> refList = RefMarsIntelligentControl.getMgr().getList();
		for(int i = 0; i < refList.size(); i++)
		{
			RefMarsIntelligentControl ref = refList.get(i);
			if(null == ref)
				continue;
			
			MarsPeopleIntelligentInfo info = new MarsPeopleIntelligentInfo(getUserData(), ref);
			_m_alleIntelligentList.add(info);
		}
	}
	
	protected void _initFromDB(_ICallBackBool _handler)
    {
		getUserData().getUSServer().getBM().getBM(PlayerMarsPeopleIntelligentBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerMarsPeopleIntelligentBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerMarsPeopleIntelligentBO> _boList)
                    {
                    	_initFromBoList(_boList);
                    	
                    	_handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                    	_handler.onRunOver(false);
                    }
                });
    }
	
	/**
	 * 加载bo数据
	 * @param _boList
	 */
	private void _initFromBoList(List<PlayerMarsPeopleIntelligentBO> _boList) 
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerMarsPeopleIntelligentBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			MarsPeopleIntelligentInfo info = lookup(bo.getRefId());
			if(null == info)
			{
				USLog.error(getUserData().getUSServer(), "player:{} mars intelligent:{} init bo not find obj.", getUserData().getCid(), bo.getRefId());
				continue;
			}
			
			info._loadBo(bo);
		}
	}
	
	/**
	 * 查找知道决策数据
	 * @param _refId
	 * @return
	 */
	public MarsPeopleIntelligentInfo lookup(long _refId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alleIntelligentList.size(); i++)
			{
				MarsPeopleIntelligentInfo info = _m_alleIntelligentList.get(i);
				if(null == info)
					continue;
				
				if(info.getRefId() == _refId)
				{
					return info;
				}
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造数据列表，只放入有数据变化的决策
	 * @param _list
	 */
	public void makeProto(ArrayList<Mars_Intelligent> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alleIntelligentList.size(); i++)
			{
				MarsPeopleIntelligentInfo info = _m_alleIntelligentList.get(i);
				if(null == info)
					continue;
				
				if(info.getId() > 0)
				{
					_list.add(info.toProto());
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
