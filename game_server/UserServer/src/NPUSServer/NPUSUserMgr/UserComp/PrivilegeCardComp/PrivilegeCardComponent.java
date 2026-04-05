package NPUSServer.NPUSUserMgr.UserComp.PrivilegeCardComp;

import Common.PrivilegeCardEnum.EPrivilegeCardType;
import Common.PrivilegeCardObj.PrivilegeCardObj_Info;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUSUserMgr.UserComp._ITickableComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerPrivilegeCardBO;

import java.util.ArrayList;
import java.util.List;

public class PrivilegeCardComponent extends _ANPUserComponent implements _IUserItemBasicDealer, _ITickableComponent, _IHandlerHolder
{
	//权益卡数据数组
	private PrivilegeCardInfo[] _m_arrPrivilegeCardArr;
    //玩家属性加成
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;
	
	public PrivilegeCardComponent(NPUSUserData _userData)
    {
        super(_userData, NPCommonEnum.ENPPlayerCompType.PRIVILEGE_CARD);
        
        _m_arrPrivilegeCardArr = new PrivilegeCardInfo[EPrivilegeCardType.EPrivilegeCardType_Length];

        _m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer();

        _initAll();
    }
	
	public PrivilegeCardInfo getPrivilegeCard(EPrivilegeCardType _type) {return _m_arrPrivilegeCardArr[_type.ordinal()];}
	public PrivilegeCardInfo getPrivilegeCard(int _type) {return _m_arrPrivilegeCardArr[_type];}

    public NPPlayerPropertyContainer getPlayerPropertyContainer() {return _m_pcPlayerPropertyContainer;}
	
	/**
	 * 预加载初始化所有权益卡数据
	 * EPrivilegeCardType_Length
	 		MONTH, //1 ==== 月卡
			YERA, //2 ==== 年卡
	 */
	private void _initAll()
	{
		for(int i = 0; i < EPrivilegeCardType.EPrivilegeCardType_Length; i++)
		{
			EPrivilegeCardType type = EPrivilegeCardType.EPrivilegeCardType_FromInt(i);
			if(EPrivilegeCardType.NONE == type)
				continue;
			
			PrivilegeCardInfo info = new PrivilegeCardInfo(getUserData(), type);
			_m_arrPrivilegeCardArr[i] = info;
		}
	}
	
	@Override
	protected void _init() 
	{
		getUserData().getUSServer().getBM().getBM(PlayerPrivilegeCardBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerPrivilegeCardBO>>()
                {
		            @Override
		            public void dealFail()
		            {
		                USLog.error(getUserData().getUSServer(), "Can not load PrivilegeCard Data[cid:" + getUserData().getCid() + "]");
		                getUserData().setDataLoadFail();
		            }
            
                    @Override
                    public void dealSuc(List<PlayerPrivilegeCardBO> _boList)
                    {
                    	_initFromBoList(_boList);
                    }
                });
	}
	private void _initFromBoList(List<PlayerPrivilegeCardBO> _boList)
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerPrivilegeCardBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			PrivilegeCardInfo info = getPrivilegeCard(bo.getCardType());
			if(null == info)
			{
				USLog.error(getUSServer(), "player:{} cardType:{} load bo fail, not find obj.", getCid(), bo.getCardType());
				continue;
			}
			
			info._loadBo(bo);
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
		//初始化加载
		for(int i = 0; i < _m_arrPrivilegeCardArr.length; i++)
		{
			PrivilegeCardInfo info = _m_arrPrivilegeCardArr[i];
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
		return ENPItemType.PRIVILEGE_CARD;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		return hasItem(_itemId, 1) ? 1 : 0;
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		PrivilegeCardInfo info = getPrivilegeCard((int) _itemId);
		return null != info && info.isEffect();
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
		gainCard(EPrivilegeCardType.EPrivilegeCardType_FromInt((int) _itemId), _count, _context);
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		return false;
	}

	@Override
	public void tick1Sec() 
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_arrPrivilegeCardArr.length; i++)
			{
				PrivilegeCardInfo info = _m_arrPrivilegeCardArr[i];
				if(null == info)
					continue;
			
				//检查权益卡失效情况
				info.checkInvalid();
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 自动结算玩家非手动的结算（需要在产速计算完成后再调用）
	 */
	public void autoSettleAll()
	{
		for(int i = 0; i < _m_arrPrivilegeCardArr.length; i++)
		{
			PrivilegeCardInfo info = _m_arrPrivilegeCardArr[i];
			if(null == info)
				continue;
			
			//自动结算收益
			info.autoSettle(NPPlayerContext.createNew(ENPGameEvent.PRIVILEGE_CARD_AUTO_SETTLE), null);
		}
		
        // 注册跨天事件监听器，每日0点检查是否有新的邮件计划需要发送
        getUserData().OnCrossDay.addHandler(this, new HandlerOne<Integer>()
        {
            @Override
            public void handle(Integer _nowTag)
            {
        		for(int i = 0; i < _m_arrPrivilegeCardArr.length; i++)
        		{
        			PrivilegeCardInfo info = _m_arrPrivilegeCardArr[i];
        			if(null == info)
        				continue;
        			
        			//自动结算收益
        			info.autoSettle(NPPlayerContext.createNew(ENPGameEvent.SERVER_CROSS_DAY), null);
        		}
            }
        });
	}
	
	/**
	 * 构造数据协议
	 * @param _list
	 */
	public void makeProto(ArrayList<PrivilegeCardObj_Info> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_arrPrivilegeCardArr.length; i++)
			{
				PrivilegeCardInfo info = _m_arrPrivilegeCardArr[i];
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
	 * 获取卡片
	 * @param _type
	 * @param _count
	 * @param _context
	 */
	public void gainCard(EPrivilegeCardType _type, long _count, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			PrivilegeCardInfo info = getPrivilegeCard(_type);
			if(null == info)
			{
				USLog.error(getUSServer(), "player:{} cardType:{} gain card fail, not find obj.", getCid(), _type);
				return;
			}
			
			if(null == info.getRef())
			{
				USLog.error(getUSServer(), "player:{} cardType:{} gain card fail, not find ref.", getCid(), _type);
				return;
			}
			
			//增加次数
			info._addCount(_count, _context);
			
			//检查生效
			info.checkEffective(true);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_054_OnPrivilegeCardChg(info));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	@Override
	public String toString()
	{
		getUserData().lockUser();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < _m_arrPrivilegeCardArr.length; i++)
			{
				PrivilegeCardInfo info = _m_arrPrivilegeCardArr[i];
				if(null == info)
					continue;
				
				sb.append("\n---------- ").append(info.getCardType()).append(" ----------");
				sb.append(info.toString());
			}
			
			return sb.toString();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
