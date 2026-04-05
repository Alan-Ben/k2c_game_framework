package NPUSServer.Guild.GuildDungeon;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.GuildDungeonObj.GuildDungeon_DungeonMonster;
import Common.GuildDungeonObj.GuildDungeon_InstanceInfo;
import Common.GuildDungeonObj.GuildDungeon_Monster;
import Common.MailObj.Mail_Data;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeon;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonLvl;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUserServer;
import USDB.Bo.GuildDungeonInstanceBO;
import USDB.Bo.GuildDungeonLogBO;
import USDB.Bo.GuildDungeonMonsterBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 公会副本管理对象
 * @author mj
 *
 */
public class GuildDungeonInstanceMgr 
{
	//公会数据
	private GuildInfo _m_giGuildInfo;
	//副本实例数据列表
	private ArrayList<GuildDungeonInstanceInfo> _m_alDungeonInstanceList;
	//锁对象
    private MutexObject _m_mutex;
	
	public GuildDungeonInstanceMgr(GuildInfo _guildInfo)
	{
		_m_giGuildInfo = _guildInfo;
		
		_m_alDungeonInstanceList = new ArrayList<>();

		_m_mutex = new MutexObject();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public GuildInfo getGuild() {return _m_giGuildInfo;}
	public NPUserServer getUSServer() {return _m_giGuildInfo.getGuildMgr().getServer();}
	
	public void _initBo(GuildDungeonInstanceBO _bo)
	{
		GuildDungeonInstanceInfo info = new GuildDungeonInstanceInfo(_m_giGuildInfo, _bo);
		_m_alDungeonInstanceList.add(info);
	}
	
	/**
	 * 加载完成后处理
	 */
	public void _onInited()
	{
		for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
		{
			GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
			if(null == info)
				continue;
			
			info._onInited();
		}
	}
	
	/**
	 * 获取正在运行的实例数量
	 * @return
	 */
	public int getCount()
	{
		_lock();
		
		try
		{
			return _m_alDungeonInstanceList.size();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查是否有正在运行的指定的公会副本
	 * @param _dungeonId
	 * @return
	 */
	public boolean hasDungeon(long _dungeonId)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
			{
				GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
				if(null == info)
					continue;
				
				if(info.getDungeonId() == _dungeonId)
					return true;
			}
			
			return false;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 查找指定公会副本实例数据
	 * @param _dungeonId
	 * @return
	 */
	public GuildDungeonInstanceInfo lookup(long _id)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
			{
				GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
				if(null == info)
					continue;
				
				if(info.getId() == _id)
					return info;
			}
			
			return null;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造实例数据列表
	 * @param _list
	 */
	public void makeProto(ArrayList<GuildDungeon_InstanceInfo> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
			{
				GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toProto());
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取指定玩家已领取怪物奖励数据
	 * @param _cid
	 * @param _list
	 */
	public void makeGainedRewardList(long _cid, ArrayList<GuildDungeon_DungeonMonster> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
			{
				GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
				if(null == info)
					continue;
			
				info.getMonsterMgr().makeGainedRewardList(_cid, _list);
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 创建副本实例
	 * @param _cid
	 * @param _dungeonRef
	 * @param _dungeonLvlRef
	 * @param _monsterList
	 * @param _context
	 * @return
	 */
	public GuildDungeonInstanceInfo start(RefGuildDungeon _dungeonRef, RefGuildDungeonLvl _dungeonLvlRef, ArrayList<GuildDungeon_Monster> _monsterList, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//公会已经销毁
			if(_m_giGuildInfo.isDissolve())
				return null;
			
			//相同公会副本已经开启
			if(hasDungeon(_dungeonRef.id))
				return null;
			
			BM bmObj = getUSServer().getBM();
			
			//创建公会副本实例数据
			GuildDungeonInstanceBO bo = new GuildDungeonInstanceBO();
			bo.setGuildId(bmObj, getGuild().getGuildId());
			bo.setDungeonId(bmObj, _dungeonRef.id);
			bo.setDungeonLvl(bmObj, _dungeonLvlRef.lvl);
			bo.setStartMs(bmObj, CommonFunc.getNowTimeMS());
			bo.insert(bmObj);
			
			GuildDungeonInstanceInfo info = new GuildDungeonInstanceInfo(_m_giGuildInfo, bo, _dungeonRef, _dungeonLvlRef);
			_m_alDungeonInstanceList.add(info);
			
			//创建怪物数据
			for(int i = 0; i < _monsterList.size(); i++)
			{
				GuildDungeon_Monster monster = _monsterList.get(i);
				if(null == monster)
					continue;
				
				GuildDungeonMonsterBO monsterBo = new GuildDungeonMonsterBO();
				monsterBo.setGuildId(bmObj, getGuild().getGuildId());
				monsterBo.setInstaceId(bmObj, info.getId());
				monsterBo.setMonsterId(bmObj, monster.getMonsterId());
				monsterBo.setHp(bmObj, monster.getHp());
				monsterBo.setIsReward(bmObj, monster.getIsReward());
				monsterBo.insert(bmObj);
				
				GuildDungeonMonsterInfo monsterInfo = new GuildDungeonMonsterInfo(info, monsterBo);
				info.getMonsterMgr()._initAddMonster(monsterInfo);
			}
			
			return info;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 结算数据
	 * @param _memberCidList
	 * @param _context
	 */
	protected void _settle(List<Long> _memberCidList, NPPlayerContext _context) 
	{
		_lock();
		
		try
		{
			//更新实例状态
			for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
			{
				GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
				if(null == info)
					continue;
				
				info.setSettled();
			}
			
			//对所有玩家进行结算处理
			for(int i = 0; i < _memberCidList.size(); i++)
			{
				_settlePlayerMonsterReward(_memberCidList.get(i), _context);
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 指定玩家结算
	 * @param _memberCid
	 * @param _context
	 */
	private void _settlePlayerMonsterReward(long _memberCid, NPPlayerContext _context) 
	{
		//计算结算获得的物品列表
		NPItemCollector collector = new NPItemCollector(0);
		for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
		{
			GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
			if(null == info)
				continue;
			
			if(info.isSettled())
			{
				info.getMonsterMgr()._settlePlayerMonsterReward(_memberCid, collector);
			}
		}
		
		//发送邮件
		if(!collector.isEmpty())
		{
			ALSynTaskManager.getInstance().regTask(()->
			{
				NPPlayerContext context = NPPlayerContext.createNew(_context);
				
				//【BUG-0】公会副本-奖励邮件里面显示找不到对应奖励 https://www.teambition.com/task/6905b0fe8ceedda58faf3c4e
				//解析物品，如果是reward类型，需要先随机出具体的物品再放入邮件
				NPItemCollector mailCollector = new NPItemCollector(0);
				for(int i = 0; i < collector.getAllItemList().size(); i++)
				{
					NPCommonCostItem item = collector.getAllItemList().get(i);
					if(null == item)
						continue;
					
					if(item.getItemType() == ENPItemType.REWARD)
					{
						for(int j = 0; j < item.getCount(); j++)
						{
							RewardObj rewardObj = RewardMgr.getInstance().lookupReward(item.getItemId());
							if(null == rewardObj)
								continue;
							
							mailCollector.addItemList(rewardObj.getItemList());
						}
					}
					else
					{
						mailCollector.addItem(item);
					}
				}
				
				Mail_Data mailData = new Mail_Data();
	            mailData.setMailRefId(RefGeneral.Ref().guild_dungeon_reward_mail_id);
	            mailCollector.fillProtoList(mailData.getItemList().getItemList());
	            MailSystem.addMail(getUSServer(), _memberCid, mailData, context);
			});
		}
	}
	
	/**
	 * 销毁实例数据
	 */
	protected void _discard() 
	{
		_lock();
		
		try
		{
			//实例数据
			getUSServer().getBM().getBM(GuildDungeonInstanceBO.class).delAll("guildId", getGuild().getGuildId());
			//怪物数据
			getUSServer().getBM().getBM(GuildDungeonMonsterBO.class).delAll("guildId", getGuild().getGuildId());
			//日志数据
			getUSServer().getBM().getBM(GuildDungeonLogBO.class).delAll("guildId", getGuild().getGuildId());
			
			_m_alDungeonInstanceList.clear();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 计算怪物奖励
	 * @param _cid
	 * @param _collector
	 */
	public void setGainAllMonsterReward(long _cid, NPItemCollector _collector)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
			{
				GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
				if(null == info)
					continue;
			
				info.getMonsterMgr().setGainAllMonsterReward(_cid, _collector);
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	@Override
	public String toString()
	{
		_lock();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("\nsize:").append(_m_alDungeonInstanceList.size());
			for(int i = 0; i < _m_alDungeonInstanceList.size(); i++)
			{
				GuildDungeonInstanceInfo info = _m_alDungeonInstanceList.get(i);
				if(null == info)
					continue;
				
				sb.append("\n==========================================")
					.append(info.toString());
			}
			
			sb.append("\n");
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
