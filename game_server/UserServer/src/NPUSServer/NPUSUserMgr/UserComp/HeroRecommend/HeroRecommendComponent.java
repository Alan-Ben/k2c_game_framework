package NPUSServer.NPUSUserMgr.UserComp.HeroRecommend;

import Common.HeroRecommendObj.HeroRecommend_Info;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.HeroRecommandErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.Random;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Hero.RefHeroRecommend;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerHeroRecommendBO;

import java.util.ArrayList;
import java.util.List;

/*****
 * 大臣推荐组件
 * @author mj
 *
 */
public class HeroRecommendComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
	//大臣推荐事件数据列表
	private ArrayList<HeroRecommendInfo> _m_alHeroRecommendInfoList;
	
	public HeroRecommendComponent(NPUSUserData _userData) 
	{
		super(_userData, ENPPlayerCompType.HERO_RECOMMEND);
		
		_m_alHeroRecommendInfoList = new ArrayList<>();
	}

	@Override
	protected void _init() 
	{
		getUSServer().getBM().getBM(PlayerHeroRecommendBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerHeroRecommendBO>>()
		{
			@Override
			public void dealFail() 
			{
				USLog.error(getUSServer(), "player:{} load hero recommend bo fail.", getUserData().getCid());
				getUserData().setDataLoadFail();
			}
			
			@Override
			public void dealSuc(List<PlayerHeroRecommendBO> _boList) 
			{
				_initFromBo(_boList);
			}
		});
	}
	private void _initFromBo(List<PlayerHeroRecommendBO> _boList)
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerHeroRecommendBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			RefHeroRecommend ref = RefHeroRecommend.getMgr().get(bo.getRefId());
			if(null == ref)
			{
				USLog.error(getUSServer(), "player:{} init hero recommend ref:{} fail.", getUserData().getCid(), bo.getRefId());
				continue;
			}
			
			HeroRecommendInfo info = new HeroRecommendInfo(getUserData(), bo, ref);
			_m_alHeroRecommendInfoList.add(info);
		}
		
		setInited();
	}

	@Override
	public ENPPlayerCompType[] getDependCompList() 
	{
		return null;
	}

	@Override
	public void onInited() 
	{
	}

	@Override
	public void dispose() 
	{
	}

	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.HERO_RECOMMEND;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		return 0;
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		return false;
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		for(int i = 0; i < _count; i++)
		{
			addHeroRecommend(_itemId, _context);
		}
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
	{
		for(int i = 0; i < _count; i++)
		{
			addHeroRecommend(_itemId, _context);
		}
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
	{
		return false;
	}
	
	/**
	 * 构造数据列表协议对象
	 * @param _list
	 */
	public void makeProto(ArrayList<HeroRecommend_Info> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alHeroRecommendInfoList.size(); i++)
			{
				HeroRecommendInfo info = _m_alHeroRecommendInfoList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定大臣推荐事件
	 * @param _instanceId
	 * @return
	 */
	public HeroRecommendInfo lookupHeroRecommend(long _instanceId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alHeroRecommendInfoList.size(); i++)
			{
				HeroRecommendInfo info = _m_alHeroRecommendInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.getInstanceId() == _instanceId)
					return info;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加大臣推荐事件
	 * @param _refId
	 * @param _context
	 */
	public void addHeroRecommend(long _refId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			RefHeroRecommend ref = RefHeroRecommend.getMgr().get(_refId);
			if(null == ref)
			{
				USLog.error(getUSServer(), "player:{} add hero recommend ref:{} fail, not find ref.", getUserData().getCid(), _refId);
				return;
			}
			
			PlayerHeroRecommendBO bo = new PlayerHeroRecommendBO();
			bo.setCid(getUSServer().getBM(), getUserData().getCid());
			bo.setRefId(getUSServer().getBM(), _refId);
			bo.setRndSeed(getUSServer().getBM(), Random.popBattleRandomSeed());
			bo.insert(getUSServer().getBM());
			
			HeroRecommendInfo info = new HeroRecommendInfo(getUserData(), bo, ref);
			_m_alHeroRecommendInfoList.add(info);
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_085_OnHeroRecommendAdd(info));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 处理大臣推荐事件
	 * @param _instanceId
	 * @param _heroId
	 * @param _context
	 * @return
	 */
	public ResultOne<Long> dealHeroRecommend(long _instanceId, long _heroId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		try
		{
			HeroRecommendInfo info = lookupHeroRecommend(_instanceId);
			if(null == info)
				return ResultOne.failed(HeroRecommandErr.HERO_RECOMMEND_NOT_FIND);
			
			//检查推荐大臣池子
			if(!info.isHeroInPool(_heroId))
				return ResultOne.failed(HeroRecommandErr.HERO_RECOMMEND_NOT_IN_POOL);
			
			//检查大臣获取条件
			if(!NPPlayerConditionDealerMgr.IsEnable(info.getRef().choose_condition, getUserData(), null))
				return ResultOne.failed(HeroRecommandErr.HERO_RECOMMEND_COND_ERROR);
			
			//移除推荐事件
			_m_alHeroRecommendInfoList.remove(info);
			info.del();
			getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_086_OnHeroRecommendDel(_instanceId));
			
			//获取大臣
			getUserData().gainItem(ENPItemType.HERO, _heroId, _context);

			return ResultOne.succ(_heroId);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 移除大臣推荐事件
	 * @param _instanceId
	 * @param _context
	 */
	public void delHeroRecommend(long _instanceId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alHeroRecommendInfoList.size(); i++)
			{
				HeroRecommendInfo info = _m_alHeroRecommendInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.getInstanceId() == _instanceId)
				{
					_m_alHeroRecommendInfoList.remove(info);
					info.del();
					getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_086_OnHeroRecommendDel(_instanceId));
					
					break;
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
