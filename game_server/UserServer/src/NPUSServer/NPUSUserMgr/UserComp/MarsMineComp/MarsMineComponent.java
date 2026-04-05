package NPUSServer.NPUSUserMgr.UserComp.MarsMineComp;

import AllRpcData.US_Service.Guild.GuildAddMarsMine;
import Common.MarsObj.Mars_MineIdx;
import Common.ServerObj.ServerObj_MarsMine;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Mars.RefMarsExploreLvl;
import NPGameRes.Refs.Mars.RefMarsExploreMine;
import NPGameRes.Refs.Mars.RefMarsExplorePos;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsMineBO;
import USLOGDB.Bo.LogMarsMineAddBO;
import USLOGDB.Bo.LogMarsMineDelBO;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

public class MarsMineComponent extends _ANPUserComponent
{
	//当前玩家刷出的矿列表
	private ArrayList<MarsMineInfo> _m_alMineList;
	//当前已经占据的位置
	private HashSet<Long> _m_hmUsedPosIdSet;
	
	public MarsMineComponent(NPUSUserData _userData) 
	{
		super(_userData, ENPPlayerCompType.MARS_MINE);
		
		_m_alMineList = new ArrayList<>();
		_m_hmUsedPosIdSet = new HashSet<>();
	}

	@Override
	protected void _init() 
	{
        getUSServer().getBM().getBM(PlayerMarsMineBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsMineBO>>()
        {
            @Override
            public void dealFail()
            {
            	getUserData().setDataLoadFail();
            }
            
            @Override
            public void dealSuc(List<PlayerMarsMineBO> _list)
            {
            	_initFromBo(_list);
            	
            	setInited();
            }
        });
    }
	private void _initFromBo(List<PlayerMarsMineBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerMarsMineBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			MarsMineInfo info = new MarsMineInfo(getUserData(), bo);
			_m_alMineList.add(info);
			
			_m_hmUsedPosIdSet.add(info.getPos());
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
	}

	@Override
	public void dispose() 
	{
	}
	
	/**
	 * 获取当前全部火星矿实例ID列表
	 * @return
	 */
	public ArrayList<Long> getMineIdList()
	{
		getUserData().lockUser();
		
		try
		{
			ArrayList<Long> idList = new ArrayList<>();
			for(int i = 0; i < _m_alMineList.size(); i++)
			{
				MarsMineInfo info = _m_alMineList.get(i);
				if(null == info)
					continue;
				
				idList.add(info.getInstanceId());
			}
			
			return idList;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定的矿资源数据
	 * @param _id
	 * @return
	 */
	public MarsMineInfo lookup(long _id)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alMineList.size(); i++)
			{
				MarsMineInfo info = _m_alMineList.get(i);
				if(null == info)
					continue;
				
				if(info.getInstanceId() == _id)
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
	 * 构造数据协议
	 * @param _list
	 */
	public void makeProto(ArrayList<Mars_MineIdx> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alMineList.size(); i++)
			{
				MarsMineInfo info = _m_alMineList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toIdxProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 根据位置查找指定的矿资源数据
	 * @param _pos
	 * @return
	 */
	public MarsMineInfo lookupByPos(long _pos)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alMineList.size(); i++)
			{
				MarsMineInfo info = _m_alMineList.get(i);
				if(null == info)
					continue;
				
				if(info.getPos() == _pos)
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
	 * 创建随机矿
	 * 
	 * 随机规则：
	 		1.
	 		2.
	 * 
	 * @param _context
	 */
	public void buildRandMine(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//随机获取矿配置
			RefMarsExploreLvl exploreLvlRef = getUserData().getMarsExploreComponent().getExploreInfo().getLvlRef();
			if(null == exploreLvlRef)
			{
				USLog.error(getUSServer(), "player:{} mars explore build rand mine fail, not find explore lvl.", getCid());
				return;
			}
			
			//根据权重刷新矿等级
			RefMarsExploreLvl.MarsExploreMineRndInfo mineLvl = exploreLvlRef.mineIdList.random();
			if(mineLvl.mineLvl <= 0)
			{
				USLog.error(getUSServer(), "player:{} explore lvl:{} mars explore not find mine lvl.", getCid(), exploreLvlRef.explore_level);
				return;
			}

			RefMarsExploreMine mineRef = RefGeneral.Ref().randMarsLvlMineMap(mineLvl.mineLvl);
			if(null == mineRef)
			{
				USLog.error(getUSServer(), "player:{} explore lvl:{} mineLvl:{} mars explore not find mine ref."
						, getCid(), exploreLvlRef.explore_level, mineLvl);
				return;
			}
			 
			 //是否创建新矿，纯随机
			int rand = CommonFunc.randomInt(10000);
			//判断是否出新矿
			boolean canBuild = rand < mineLvl.newPer;
			//如数据有问题，以50%为默认标准
			if(mineLvl.newPer <= 0)
				canBuild = rand < 5000;

			//构造矿回调处理
			_ICallBackIntT<ServerObj_MarsMine> mineGetCallback = new _ICallBackIntT<ServerObj_MarsMine>() {
				@Override
				public void onRunOver(int _err, ServerObj_MarsMine _mineObj)
				{
					//调用事件函数
					onMineCreated(_mineObj, _context);
				}
			};

			if(canBuild)
			{
				//尝试创建新矿
				MarsMineSystem.CreateNewMine(getUSServer(), mineRef, mineGetCallback);
			}
			else
			{
				//尝试获取旧矿
				MarsMineSystem.RandOtherPlayerMine(getUSServer(), mineRef, mineGetCallback);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 移除指定火星矿
	 * @param _context
	 */
	public void removeMine(long _id, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alMineList.size(); i++)
			{
				MarsMineInfo info = _m_alMineList.get(i);
				if(null == info)
					continue;
				
				if(info.getInstanceId() == _id)
				{
					//移除容器数据
					_m_alMineList.remove(i);
					_m_hmUsedPosIdSet.remove(info.getPos());
					
					//移除bo数据
					info._del();
					
					//推送
					getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_060_OnMineDel(_id));
					
					//日志数据
					LogMarsMineDelBO logBo = new LogMarsMineDelBO();
					 logBo.setCid(getBM(), getCid());
					logBo.setMineInstanceId(getBM(), _id);
					CommLogDB.log(getBM(), logBo, _context);
					
					break;
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
     * 矿创建完成之后调用的事件函数
     * 对外开放是因为作弊命令可能用到
     *
     * @param _mineObj
     */
	public void onMineCreated(ServerObj_MarsMine _mineObj, NPPlayerContext _context)
	{
        long posId;

		getUserData().lockUser();
		try
		{
			//判断矿是否在本数据集已经存在，如存在则不做处理，视为未刷新
			if(null != lookup(_mineObj.getId()))
				return ;

			//随机获取矿配置
			RefMarsExploreLvl exploreLvlRef = getUserData().getMarsExploreComponent().getExploreInfo().getLvlRef();
			if(null == exploreLvlRef)
			{
				USLog.error(getUSServer(), "player:{} buildMineFromPool fail, not find explore lvl.", getCid());
				return ;
			}

			//获取随机地址
			RefMarsExplorePos posRef = exploreLvlRef.randExploreMinePos(_m_hmUsedPosIdSet);
			if(null == posRef)
			{
				USLog.error(getUSServer(), "player:{} exploreLvl:{} buildMineFromPool fail, not refresh pos.", getCid(), exploreLvlRef.explore_level);
				return ;
			}

			//关联数据
			PlayerMarsMineBO bo = new PlayerMarsMineBO();
			bo.setCid(getBM(), getUserData().getCid());
			bo.setInstanceId(getBM(), _mineObj.getId());
			bo.setRefId(getBM(), _mineObj.getRefId());
			bo.setStartShowMs(getBM(), CommonFunc.getNowTimeMS());
			bo.setEndShowMs(getBM(), _mineObj.getEndShowMs());
			bo.setPos(getBM(), posRef.id);
			bo.insert(getBM());

            posId = posRef.Id();
			MarsMineInfo info = new MarsMineInfo(getUserData(), bo);
			_m_alMineList.add(info);

			_m_hmUsedPosIdSet.add(info.getPos());

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_059_OnMineAdd(info));

			//日志数据
			LogMarsMineAddBO logBo = new LogMarsMineAddBO();
			logBo.setCid(getBM(), getCid());
			logBo.setMineInstanceId(getBM(), info.getInstanceId());
			logBo.setMineRefId(getBM(), info.getRefId());
			logBo.setStartShowMs(getBM(), info.getStartShowMs());
			logBo.setEndShowMs(getBM(), info.getEndShowMs());
			logBo.setPos(getBM(), info.getPos());
			CommLogDB.log(getBM(), logBo, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}

        //分享到联盟
		GuildAddMarsMine rpc = new GuildAddMarsMine();
		rpc.req().setCid(getCid());
		rpc.req().setGuildId(getUserData().getGuildComponent().getGuildId());
		rpc.req().setMineId(_mineObj.getId());
		rpc.req().setPosId(posId);
		rpc.req().setEndShowTimeMS(_mineObj.getEndShowMs());

		int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(getUserData().getGuildComponent().getGuildId());

		getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
				, ()-> {
					USLog.error(getUSServer(), "player:{} guild:{} send rpc addMarsMine fail.", getCid(), getUserData().getGuildComponent().getGuildId());
				});
	}
}
