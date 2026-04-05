package NPUSServer.NPUSUserMgr.UserComp.GuildDungeonComp;

import Common.GuildDungeonObj.GuildDungeon_FightHero;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerGuildDungeonHeroBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 公会副本 - 玩家自身数据
 * @author mj
 *
 */
public class GuildDungeonComponent extends _ANPUserComponent
{
	//出战大臣数据列表
	private ArrayList<GuildDungeonHeroFightInfo> _m_alHeroFightInfoList;
	
    public GuildDungeonComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.GUILD_DUNGEON);
        
        _m_alHeroFightInfoList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerGuildDungeonHeroBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerGuildDungeonHeroBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load DungeonHero Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerGuildDungeonHeroBO> _list)
            {
            	_initFromBo(_list);
            }
        });
    }
    private void _initFromBo(List<PlayerGuildDungeonHeroBO> _boList)
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerGuildDungeonHeroBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			GuildDungeonHeroFightInfo info = new GuildDungeonHeroFightInfo(getUserData(), bo);
			_m_alHeroFightInfoList.add(info);
		}
		
		setInited();
	}

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    }
    
    /**
     * 刷新全部大臣数据
     * @param _push
     */
    public void refreshAll(boolean _push)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alHeroFightInfoList.size(); i++)
    		{
    			GuildDungeonHeroFightInfo info = _m_alHeroFightInfoList.get(i);
    			if(null == info)
    				continue;
    			
    			info._refresh(_push);
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 构造出战大臣数据
     * @param _list
     */
    public void makeFightHeroList(ArrayList<GuildDungeon_FightHero> _list)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		refreshAll(false);
    		
    		for(int i = 0; i < _m_alHeroFightInfoList.size(); i++)
    		{
    			GuildDungeonHeroFightInfo info = _m_alHeroFightInfoList.get(i);
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
     * 查找指定大臣出战数据
     * @param _heroId
     * @return
     */
    public GuildDungeonHeroFightInfo lookupHeroFight(long _heroId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alHeroFightInfoList.size(); i++)
    		{
    			GuildDungeonHeroFightInfo info = _m_alHeroFightInfoList.get(i);
    			if(null == info)
    				continue;
    		
    			if(info.getHeroId() == _heroId)
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
     * 初始化大臣出战数据
     * @param _heroId
     * @return
     */
    public GuildDungeonHeroFightInfo ensureHeroFight(long _heroId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		GuildDungeonHeroFightInfo info = lookupHeroFight(_heroId);
    		if(null == info)
    		{
    			BM bmObj = getUSServer().getBM();
    			
    			PlayerGuildDungeonHeroBO bo = new PlayerGuildDungeonHeroBO();
    			bo.setCid(bmObj, getUserData().getCid());
    			bo.setHeroId(bmObj, _heroId);
    			bo.insert(bmObj);
    			
    			info = new GuildDungeonHeroFightInfo(getUserData(), bo);
    			_m_alHeroFightInfoList.add(info);
    		}
    		else
    		{
    			info._refresh(false);
    		}
    		
    		return info;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 检查指定大臣是否可以出战
     * @param _hero
     * @return
     */
    public boolean canHeroAttack(HeroInfo _hero)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		GuildDungeonHeroFightInfo info = lookupHeroFight(_hero.getHeroId());
    		if(null == info)
    			return true;
    		
    		return info._canAttack();
    	}
    	finally
    	{
    		getUserData().unlockUser();
        }
    }
    
    /**
     * 计入大臣出战次数
     * @param _hero
     * @param _context
     * @return
     */
    public Result heroAttack(HeroInfo _hero, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//检查是否可以出战
    		if(!canHeroAttack(_hero))
    			return GuildErr.GUILD_DUNGEON_FIGHT_HERO_FAIL;
    		
    		//更新出战计数
    		GuildDungeonHeroFightInfo info = ensureHeroFight(_hero.getHeroId());
    		info._incrFightedCount();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_037_GuildDungeonOp.make_054_OnDungeonHeroFightChg(info));
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
        }
    }
    
    /**
     * 对指定大臣出战次数进行返回补偿
     * @param _hero
     * @param _context
     * @return
     */
    public void heroAttackReturn(HeroInfo _hero, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		GuildDungeonHeroFightInfo info = lookupHeroFight(_hero.getHeroId());
    		if(null == info)
    			return;
    		
    		info._refresh(false);
    		
    		int fightCount = info.getFightedCount();
    		if(fightCount > 0)
    		{
    			info._setFightedCount(fightCount - 1);
    		}
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_037_GuildDungeonOp.make_054_OnDungeonHeroFightChg(info));
    	}
    	finally
    	{
    		getUserData().unlockUser();
        }
    }
    
    /**
     * 计入大臣恢复次数
     * @param _hero
     * @param _context
     * @return
     */
    public Result heroRecover(HeroInfo _hero, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		GuildDungeonHeroFightInfo info = lookupHeroFight(_hero.getHeroId());
    		if(null == info)
    			return GuildErr.GUILD_DUNGEON_NOT_RECOVER_HERO;
    		
    		info._refresh(false);
    		
    		//无战斗次数，无需恢复
    		if(info.getFightedCount() <= 0)
    			return GuildErr.GUILD_DUNGEON_NOT_RECOVER_HERO;

    		//检查恢复次数上限
    		if(info.getRecoveredCount() >= RefGeneral.Ref().guild_dungeon_recover_hero_limit)
    			return GuildErr.GUILD_DUNGEON_RECOVER_LIMIT;
    		
    		//检查消耗道具
    		if(!getUserData().hasItem(RefGeneral.Ref().guild_dungeon_recover_hero_cost))
    			return CommErr.ITEM_NOT_ENOUGH;
    		
    		if(!getUserData().spendItem(RefGeneral.Ref().guild_dungeon_recover_hero_cost, _context))
    			return CommErr.CONSUME_FAIL;
    		
    		//执行恢复
    		info._setRecoveredCount(info.getRecoveredCount() + 1);
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_037_GuildDungeonOp.make_054_OnDungeonHeroFightChg(info));
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
        }
    }
}
