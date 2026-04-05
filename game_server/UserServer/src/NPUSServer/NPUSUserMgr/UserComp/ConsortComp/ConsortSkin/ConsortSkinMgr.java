package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortSkin;

import Common.ConsortObj.Consort_SkinInfo;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.Consort.RefConsortSkin;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortSkinBO;

import java.util.ArrayList;

/**
 * 家人时装数据管理
 * @author mj
 *
 */
public class ConsortSkinMgr 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//皮肤列表数据
	private ArrayList<ConsortSkinInfo> _m_alSkinList;
	
	public ConsortSkinMgr(ConsortInfo _consort)
	{
		_m_ciConsort = _consort;
		
		_m_alSkinList = new ArrayList<>();
		
		_initDefault();
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	/**
	 * 初始化默认皮肤
	 */
	private void _initDefault()
	{
		if(_m_ciConsort.getRef().default_skin_id > 0)
		{
			RefConsortSkin skinRef = RefConsortSkin.getMgr().get(_m_ciConsort.getRef().default_skin_id);
			if(null == skinRef)
			{
				USLog.error(getUSServer(), "player:{} consort:{} init consort:{} default skin:{} fail, not find ref."
						, getUserData().getCid(), _m_ciConsort.getConsortId(), _m_ciConsort.getConsortId(), _m_ciConsort.getRef().default_skin_id);
			}
			else
			{
				ConsortSkinInfo skin = new ConsortSkinInfo(_m_ciConsort, skinRef);
				_m_alSkinList.add(skin);
			}
		}
	}
	
	/**
	 * 初始化Bo
	 * @param _bo
	 */
	public void _initBo(PlayerConsortSkinBO _bo)
	{
		ConsortSkinInfo skin = lookup(_bo.getSkinId());
		if(null == skin)
		{
			RefConsortSkin skinRef = RefConsortSkin.getMgr().get(_bo.getSkinId());
			if(null == skinRef)
			{
				USLog.error(getUSServer(), "player:{} consort:{} skin:{} init bo fail, not find ref."
						, getUserData().getCid(), _m_ciConsort.getConsortId(), _bo.getSkinId());
				
				return;
			}
			
			skin = new ConsortSkinInfo(_m_ciConsort, skinRef);
			_m_alSkinList.add(skin);
		}
		
		skin._setBo(_bo);
	}
	
	/**
	 * 构造数据列表协议
	 * @param _list
	 */
	public void makeProto(ArrayList<Consort_SkinInfo> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSkinList.size(); i++)
			{
				ConsortSkinInfo skin = _m_alSkinList.get(i);
				if(null == skin)
					continue;
				
				_list.add(skin.toProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 
	 * @param _skinId
	 * @return
	 */
	public ConsortSkinInfo lookup(long _skinId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSkinList.size(); i++)
			{
				ConsortSkinInfo skin = _m_alSkinList.get(i);
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
	
	/**
	 * 查找是否有对应皮肤
	 * @param _skinId
	 * @return
	 */
	public boolean hasSkin(long _skinId)
	{
		return null != lookup(_skinId);
	}
	
	/**
	 * 解锁皮肤
	 * @param _skinId
	 * @param _context
	 */
	public void unlockSkin(long _skinId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//已经获得皮肤
			if(hasSkin(_skinId))
				return;
			
			//检查配表数据
			RefConsortSkin skinRef = RefConsortSkin.getMgr().get(_skinId);
			if(null == skinRef)
			{
				USLog.error(getUSServer(), "player:{} consort:{} unlock skin:{} fail, not find ref.", getUserData().getCid(), _skinId);
				return;
			}
			
			//构造数据
			BM bmObj = getUSServer().getBM();
			
			PlayerConsortSkinBO bo = new PlayerConsortSkinBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setConsortId(bmObj, getConsortId());
			bo.setSkinId(bmObj, _skinId);
			bo.insert(bmObj);
			
			ConsortSkinInfo skin = new ConsortSkinInfo(_m_ciConsort, skinRef);
			_m_alSkinList.add(skin);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_055_OnSkinAdd(skin));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 移除家人皮肤（GM命令）
	 * @param _skinId
	 * @param _context
	 */
	public void cmdDelSkin(long _skinId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSkinList.size(); i++)
			{
				ConsortSkinInfo skin = _m_alSkinList.get(i);
				if(null == skin)
					continue;
				
				if(skin.getSkinId() == _skinId)
				{
					_m_alSkinList.remove(i);
					skin._discard();
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
