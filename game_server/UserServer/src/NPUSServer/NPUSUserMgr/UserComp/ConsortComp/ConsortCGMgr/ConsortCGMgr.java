package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortCGMgr;

import Common.ConsortObj.Consort_CGInfo;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Consort.RefConsortCg;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortCgBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 家人CG数据管理
 * @author mj
 *
 */
public class ConsortCGMgr implements _IUserItemBasicDealer
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	
	//玩家已解锁的CG数据
	private ArrayList<ConsortCGInfo> _m_alConsortCgList;
	
	public ConsortCGMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alConsortCgList = new ArrayList<>();
	}

	//玩家数据对象
	public NPUSUserData getUserData() {return _m_udUserData;}
	//服务器数据对象
	public NPUserServer getUSServer() {return _m_udUserData.getUSServer();}
	
	/**
	 * 从PlayerBo数据中进行解析
	 * @param _handler
	 */
	public void _initFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerConsortCgBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerConsortCgBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load consort_cg bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerConsortCgBO> _boList)
            {
            	for(int i = 0; i < _boList.size(); i++)
            	{
            		PlayerConsortCgBO bo = _boList.get(i);
            		if(null == bo)
            			continue;
            		
            		ConsortCGInfo info = new ConsortCGInfo(_m_udUserData, bo);
            		_m_alConsortCgList.add(info);
            	}
            	
                _handler.onRunOver(true);
            }
        });
	}
	
	/**
	 * 获取解锁CG的数量
	 * @return
	 */
	public int getCount()
	{
		getUserData().lockUser();
		
		try
		{
			return _m_alConsortCgList.size();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造数据协议列表
	 * @param _list
	 */
	public void makeProto(ArrayList<Consort_CGInfo> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alConsortCgList.size(); i++)
			{
				ConsortCGInfo cg = _m_alConsortCgList.get(i);
				if(null == cg)
					continue;
				
				_list.add(cg.toProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取指定家人的加护点收益加成总和
	 * @param _consortId
	 * @return
	 */
	public int getConsortCharmPointPerSum(long _consortId)
	{
		getUserData().lockUser();
		
		try
		{
			int sum = 0;
			for(int i = 0; i < _m_alConsortCgList.size(); i++)
			{
				ConsortCGInfo cg = _m_alConsortCgList.get(i);
				if(null == cg)
					continue;
				
				if(_consortId == cg.getRelationConsortId())
					sum += cg.getAdd();
			}
			
			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找对应的cg
	 * @param _cgId
	 * @return
	 */
	public ConsortCGInfo lookup(long _cgId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alConsortCgList.size(); i++)
			{
				ConsortCGInfo cg = _m_alConsortCgList.get(i);
				if(null == cg)
					continue;
				
				if(cg.getCgId() == _cgId)
					return cg;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/***
	 * 确认CG是否存在
	 * @param _cgId
	 * @return
	 */
	public boolean hasCg(long _cgId)
	{
		return null != lookup(_cgId);
	}
	
	/**
	 * 增加一个CG
	 * @param _cgId
	 * @param _context
	 */
	public int gainCG(long _cgId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//已经存在
			if(hasCg(_cgId))
				return 0;
			
			//检查配表
			RefConsortCg ref = RefConsortCg.getMgr().get(_cgId);
			if(null == ref)
			{
				USLog.error(getUSServer(), "player:{} gain consort cg:{} fail, not find ref.", getUserData().getCid(), _cgId);
				return 0;
			}
			
			//增加数据
			BM bmObj = getUSServer().getBM();
			
			PlayerConsortCgBO bo = new PlayerConsortCgBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setCgId(bmObj, _cgId);
			bo.setRewarded(bmObj, false);
			bo.insert(bmObj);
			
			ConsortCGInfo info = new ConsortCGInfo(_m_udUserData, bo, ref);
			_m_alConsortCgList.add(info);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_061_OnCgChg(info));
			
			return 1;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.CONSORT_CG;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		getUserData().lockUser();
		
		try
		{
			return hasCg(_itemId) ? 1 : 0;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		getUserData().lockUser();
		
		try
		{
			return hasCg(_itemId);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		//数量不为1，需要报错
		if(_count != 1)
		{
			USLog.error(getUSServer(), "player:{} init gain consort_cg:{} count:{} error.", getUserData().getCid(), _itemId, _count);
			return;
		}
		
		gainCG(_itemId, _context);
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
	{
		//数量不为1，需要报错
		if(_count != 1)
		{
			USLog.error(getUSServer(), "player:{} gain consort_cg:{} count:{} error.", getUserData().getCid(), _itemId, _count);
			return;
		}
		
		gainCG(_itemId, _context);
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
	{
		return false;
	}
}
