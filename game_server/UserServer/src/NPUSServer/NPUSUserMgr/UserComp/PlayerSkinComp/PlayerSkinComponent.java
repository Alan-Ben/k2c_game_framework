package NPUSServer.NPUSUserMgr.UserComp.PlayerSkinComp;

import Common.NpPlayerInfoObj.PlayerInfo_Skin;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPEnum.ENPItemType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerSkinBO;

import java.util.ArrayList;
import java.util.List;

public class PlayerSkinComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
	//玩家皮肤列表
	private ArrayList<PlayerSkinInfo> _m_alPlayerSkinList;
	
	public PlayerSkinComponent(NPUSUserData _userData)
    {
        super(_userData, NPCommonEnum.ENPPlayerCompType.PLAYER_SKIN);
        
        _m_alPlayerSkinList = new ArrayList<>();
    }
	
	@Override
	protected void _init() 
	{	
        getUSServer().getBM().getBM(PlayerSkinBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerSkinBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Player Skin Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerSkinBO> _list)
            {
                _initBo(_list);
            }
        });
    }
    //初始化数据
    private void _initBo(List<PlayerSkinBO> _list)
    {
        //获取当前时间
        for (int i = 0; i < _list.size(); i++)
        {
        	PlayerSkinBO bo = _list.get(i);
            if (null == bo)
                continue;

            PlayerSkinInfo info = new PlayerSkinInfo(getUserData(), bo);
            _m_alPlayerSkinList.add(info);
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
		return ENPItemType.PLAYER_SKIN;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alPlayerSkinList.size(); i++)
			{
				PlayerSkinInfo info = _m_alPlayerSkinList.get(i);
				if(null == info)
					continue;
				
				if(info.getSkinId() == _itemId)
					return 1;
			}
			
			return 0;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		return getItemCount(_itemId) > 0;
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		gainItem(_itemId, _count, isInited(), _context);
	}

	//count-该字段没有使用
	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context) 
	{
		getUserData().lockUser();
		
		try
		{
			//已经存在
			if(hasItem(_itemId, _count))
				return;
			
			PlayerSkinBO bo = new PlayerSkinBO();
			bo.setCid(getUSServer().getBM(), getUserData().getCid());
			bo.setSkinId(getUSServer().getBM(), _itemId);
			bo.setSkinLvl(getUSServer().getBM(), 1);
			bo.insert(getUSServer().getBM());
			
			PlayerSkinInfo info = new PlayerSkinInfo(getUserData(), bo);
			_m_alPlayerSkinList.add(info);
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_060_OnPlayerSkinChg(info));
			
			//放入数据
			_context.collectItem(getItemType(), _itemId, 1, false);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		return false;
	}
	
	public void makeProto(ArrayList<PlayerInfo_Skin> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alPlayerSkinList.size(); i++)
			{
				PlayerSkinInfo info = _m_alPlayerSkinList.get(i);
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
	 * 查找指定的玩家皮肤
	 * @param _skinId
	 * @return
	 */
	public PlayerSkinInfo lookup(long _skinId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alPlayerSkinList.size(); i++)
			{
				PlayerSkinInfo skin = _m_alPlayerSkinList.get(i);
				if(null == skin)
					continue;
				
				if(skin.getSkinId() == _skinId)
					return skin;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
