package NPUSServer.NPUSUserMgr.UserComp.ForeverAddComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPCommon_ForeverAddInfo;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.Refs.RefPlayerForeverAdd;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerForeverAddBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 玩家永久加成
 * @author mj
 *
 */
public class ForeverAddComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
	//永久加成数据
	private ArrayList<ForeverAddInfo> _m_alForeverAddList;
	
    //玩家属性加成容器
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;
    
	public ForeverAddComponent(NPUSUserData _userData) 
	{
		super(_userData, ENPPlayerCompType.FOREVER_ADD);
		
		_m_alForeverAddList = new ArrayList<>();

		_m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer();
	}
    
    public NPPlayerPropertyContainer getPlayerPropertyContainer() {return _m_pcPlayerPropertyContainer;}

	@Override
	protected void _init() 
	{
        getUSServer().getBM().getBM(PlayerForeverAddBO.class).findAll("cid", getUserData().getCid(), 
        		new _ASelectCallback<List<PlayerForeverAddBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Forever Add Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerForeverAddBO> _list)
            {
            	_initFromBoList(_list);
                
                setInited();
            }
        });
    }
	private void _initFromBoList(List<PlayerForeverAddBO> _boList)
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerForeverAddBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			ForeverAddInfo info = new ForeverAddInfo(getUserData(), bo);
			_m_alForeverAddList.add(info);
		}
	}

	@Override
	public ENPPlayerCompType[] getDependCompList() 
	{
		return null;
	}

	@Override
	public void onInited() 
	{
		for(int i = 0; i < _m_alForeverAddList.size(); i++)
		{
			ForeverAddInfo info = _m_alForeverAddList.get(i);
			if(null == info)
				continue;
			
			info._onInited();
		}
	}

	@Override
	public void dispose() 
	{
	}

	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.FOREVER_ADD;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		ForeverAddInfo info = lookup(_itemId);
		
		return null != info ? info.getItemCount() : 0;
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		ForeverAddInfo info = lookup(_itemId);
		
		return null != info && info.getItemCount() >= _count;
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		gainForeverAdd(_itemId, (int) _count, false, _context);
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context) 
	{
		gainForeverAdd(_itemId, (int) _count, _isNotMerge, _context);
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		return spendForeverAdd(_itemId, (int) _count, _context);
	}
	
	public ForeverAddInfo lookup(long _id)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alForeverAddList.size(); i++)
			{
				ForeverAddInfo info = _m_alForeverAddList.get(i);
				if(null == info)
					continue;
				
				if(info.getItemId() == _id)
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
	 * 获取新的永久加成数据，需要检查允许加成次数上限
	 * @param _itemId
	 * @param _count
	 * @param _isNotMerge
	 * @param _context
	 */
	public void gainForeverAdd(long _itemId, int _count, boolean _isNotMerge, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			int preCount = 0;
			
			ForeverAddInfo info = lookup(_itemId);
			if(null == info) //数据不存在
			{
				RefPlayerForeverAdd ref = RefPlayerForeverAdd.getMgr().get(_itemId);
				if(null == ref)
				{
					USLog.error(getUSServer(), "player:{} id:{} can not gain forever add, not find ref.", getUserData().getCid(), _itemId);
					return;
				}
				
				//计算最大值
				if(ref.add_limit > 0)
					_count = Math.min(_count, ref.add_limit);
				
				//增加数据
				PlayerForeverAddBO bo = new PlayerForeverAddBO();
				bo.setCid(getBM(), getUserData().getCid());
				bo.setItemId(getBM(), _itemId);
				bo.setItemCount(getBM(), _count);
				bo.insert(getBM());
				
				info = new ForeverAddInfo(getUserData(), bo, ref);
				_m_alForeverAddList.add(info);
				
				//更新玩家属性
				getPlayerPropertyContainer().addModifier(ref.player_pro_add, _count);
				
				//推送数据
				getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_075_OnForeverAddChg(info));
			}
			else //数据已存在
			{
				info.setCount((_count + info.getItemCount()), _context);
			}

            //放入数据
			if(info.getItemCount() > preCount)
            {
				_context.collectItem(getItemType(), _itemId, (info.getItemCount() - preCount), _isNotMerge);
            }
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 扣除永久加成数据
	 * @param _itemId
	 * @param _count
	 * @param _context
	 * @return
	 */
	public boolean spendForeverAdd(long _itemId, int _count, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			ForeverAddInfo info = lookup(_itemId);
			if(null == info)
				return false;
			
			if(info.getItemCount() < _count)
				return false;
			
			info.setCount(_count, _context);
			
			return true;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造数据列表
	 * @param _list
	 */
	public void makeProto(ArrayList<NPCommon_ForeverAddInfo> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alForeverAddList.size(); i++)
			{
				ForeverAddInfo info = _m_alForeverAddList.get(i);
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
}
