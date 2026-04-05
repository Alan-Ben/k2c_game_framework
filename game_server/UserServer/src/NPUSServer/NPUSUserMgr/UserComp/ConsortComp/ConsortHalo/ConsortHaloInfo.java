package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortHalo;

import Common.ConsortObj.Consort_Halo;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.Consort.ConsortHaloSkillObj;
import NPGameRes.Refs.Consort.RefConsortHaloLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

import java.util.ArrayList;

/****
 * 家人星辉数据
 * @author mj
 *
 */
public class ConsortHaloInfo 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//配置数据
	private RefConsortHaloLvl _m_refHaloLvl;
	
	//技能数据列表
	private ArrayList<ConsortHaloSkillInfo> _m_alHaloSkillList;
	
	public ConsortHaloInfo(ConsortInfo _consort)
	{
		_m_ciConsort = _consort;
		
		_m_alHaloSkillList = new ArrayList<>();
		
		_initSkill();
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	//星辉是否解锁
	public boolean isUnlock() {return _m_ciConsort.getBo().getIsHaloUnlock();}
	
	//配置对象
	public RefConsortHaloLvl getRef() {return _m_refHaloLvl;}

	public int getLvl()
	{
		return null == _m_refHaloLvl ? 0 : _m_refHaloLvl.level;
	}

	/**
	 * 增加的亲密度
	 * @return
	 */
	public int getIntimacyAdd()
	{
		return null == _m_refHaloLvl ? 0 : _m_refHaloLvl.add_intimacy;
	}

	/**
	 * 增加的加护力
	 * @return
	 */
	public int getCharmAdd()
	{
		return null == _m_refHaloLvl ? 0 : _m_refHaloLvl.add_charm;
	}

	private void _initSkill()
	{
		//已解锁情况下初始化技能数据
		if (isUnlock())
		{
			//默认0级
			RefConsortHaloLvl ref = _m_ciConsort.getRef().getHaloLevelMapMgr().getLevelData(_m_ciConsort.getBo().getHaloLvl());
			if (null == ref)
			{
				USLog.error(getUSServer(), "player:{} consort:{} init halo lvl:{} fail, not find ref.",
						getUserData().getCid(), _m_ciConsort.getConsortId(), _m_ciConsort.getBo().getHaloLvl());
				return;
			}

			_m_refHaloLvl = ref;

			//初始化对应技能数据
			for(int i = 0; i < _m_refHaloLvl.haloSkillList.size(); i++)
			{
				ConsortHaloSkillObj skillObj = _m_refHaloLvl.haloSkillList.get(i);
				if(null == skillObj)
					continue;
				
				ConsortHaloSkillInfo skillInfo = new ConsortHaloSkillInfo(_m_ciConsort, skillObj.getSkilllRef(), skillObj.getSkillLvlRef());
				_m_alHaloSkillList.add(skillInfo);
	
				if(null != skillInfo.getLvlRef())
				{
					//对伙伴加成
					_m_ciConsort.getPropertyContainer().addModifier(skillInfo.getLvlRef().add_basic_attr);
					//全局属性
					getUserData().getBonusMgr().addBonus(skillInfo.getLvlRef().add_bonus_attr);
				}
			}
		}
	}
	
	/**
	 * 构造协议
	 * @return
	 */
	public Consort_Halo toProto()
	{
		Consort_Halo proto = new Consort_Halo();
		proto.setLvl(getLvl());
		
		return proto;
	}
	
	/**
	 * 解锁星辉
	 * @param _context
	 */
	public void unlock(NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			//已经解锁，不再重复处理
			if (isUnlock())
				return;

			RefConsortHaloLvl ref = _m_ciConsort.getRef().getHaloLevelMapMgr().getLevelData(_m_ciConsort.getBo().getHaloLvl());
			if (ref == null)
			{
				USLog.error(getUSServer(), "player:{} consort:{} haloLvl unlock fail, not find ref.", getUserData().getCid(), getConsortId());
				return;
			}

			_m_refHaloLvl = ref;

			//保存数据
			BM bmObj = getUSServer().getBM();
			_m_ciConsort.getBo().saveIsHaloUnlock(bmObj, true);
			
			//初始化技能
			_updateSkill(_context);

			//重新计算属性
			getConsort().recalIntimacy();
			getConsort().recalCharm();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_060_OnHaloChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/****
	 * 设置等级
	 * @param _lvlRef
	 * @param _context
	 */
	public void setLvl(RefConsortHaloLvl _lvlRef, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			if(null != _m_refHaloLvl && _m_refHaloLvl.level == _lvlRef.level)
				return;

			//记录变更前的值
			int oriLvl = getLvl();

			//更新配表并保存数据
			_m_refHaloLvl = _lvlRef;

			BM bmObj = getUSServer().getBM();
			_m_ciConsort.getBo().setHaloLvl(bmObj, _lvlRef.level);
			_m_ciConsort.getBo().saveAll(bmObj);

			//修改星辉技能
			_updateSkill(_context);

			//重新计算属性
			getConsort().recalIntimacy();
			getConsort().recalCharm();

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_060_OnHaloChg(this));

			//记录MJ日志
			MJEventLog.logConsortAttribute(
				getUserData(),
				getConsortId(),
				5, // 培养类型：5=星辉升级
				_context.getContextId(),
				oriLvl,
				getLvl()
			);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 修改星辉技能
	 * @param _context
	 */
	protected void _updateSkill(NPPlayerContext _context) 
	{
		//移除旧有
		for(int i = 0; i < _m_alHaloSkillList.size(); i++)
		{
			ConsortHaloSkillInfo skillInfo = _m_alHaloSkillList.get(i);
			if(null == skillInfo)
				continue;
			
			if(null != skillInfo.getLvlRef())
			{
				//对伙伴加成
				_m_ciConsort.getPropertyContainer().removeModifier(skillInfo.getLvlRef().add_basic_attr);
				//全局属性
				getUserData().getBonusMgr().removeBonus(skillInfo.getLvlRef().add_bonus_attr);
			}
		}

		//清空原有技能列表数据
		_m_alHaloSkillList.clear();
		
		//初始化对应技能数据
		for(int i = 0; i < _m_refHaloLvl.haloSkillList.size(); i++)
		{
			ConsortHaloSkillObj skillObj = _m_refHaloLvl.haloSkillList.get(i);
			if(null == skillObj)
				continue;
			
			ConsortHaloSkillInfo skillInfo = new ConsortHaloSkillInfo(_m_ciConsort, skillObj.getSkilllRef(), skillObj.getSkillLvlRef());
			_m_alHaloSkillList.add(skillInfo);
			
			if(null != skillInfo.getLvlRef())
			{
				//对伙伴加成
				_m_ciConsort.getPropertyContainer().addModifier(skillInfo.getLvlRef().add_basic_attr);
				//全局属性
				getUserData().getBonusMgr().addBonus(skillInfo.getLvlRef().add_bonus_attr);
			}
		}
	}
	
	/**
	 * 移除星辉技能全局属性加成（GM删除妃子时调用）
	 */
	public void cmdDiscardBonus()
	{
		for (ConsortHaloSkillInfo skillInfo : _m_alHaloSkillList)
		{
			if (null == skillInfo || null == skillInfo.getLvlRef()) continue;
			getUserData().getBonusMgr().removeBonus(skillInfo.getLvlRef().add_bonus_attr);
		}
	}

	/**
	 * 重置星辉（GM命令使用，需要重启客户端）
	 * @param _context
	 */
	public void cmdReset(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			BM bmObj = getUSServer().getBM();
			
			_m_ciConsort.getBo().setHaloLvl(bmObj, 0);
			_m_ciConsort.getBo().setIsHaloUnlock(bmObj, false);
			_m_ciConsort.getBo().saveAll(bmObj);
			
			RefConsortHaloLvl ref = RefConsortHaloLvl.getMgr().get(_m_ciConsort.getBo().getHaloLvl());
			if(null != ref)
			{
				_m_refHaloLvl = ref;
				
				_m_alHaloSkillList.clear();
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
