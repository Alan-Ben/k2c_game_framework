package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortSkin;

import Common.ConsortObj.Consort_SkinInfo;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.Consort.RefConsortSkin;
import NPGameRes.Refs.Consort.RefConsortSkinLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortSkinBO;

/**
 * 家人时装
 * @author mj
 *
 */
public class ConsortSkinInfo 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//配置数据
	private RefConsortSkin _m_refSkin;
	private RefConsortSkinLvl _m_refSkinLvl;
	
	//Bo数据
	private PlayerConsortSkinBO _m_boSkin;
	
	public ConsortSkinInfo(ConsortInfo _consort, RefConsortSkin _ref)
	{
		_m_ciConsort = _consort;
		
		_m_refSkin = _ref;
	}
	public ConsortSkinInfo(ConsortInfo _consort, RefConsortSkin _ref, PlayerConsortSkinBO _bo)
	{
		_m_ciConsort = _consort;
		
		_m_refSkin = _ref;
		
		_setBo(_bo);
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	//配置数据
	public RefConsortSkin getRef() {return _m_refSkin;}
	public long getSkinId() {return _m_refSkin.id;}
	
	public RefConsortSkinLvl getLvlRef() {return _m_refSkinLvl;}
	public int getLvl() {return null == _m_refSkinLvl ? 0 : _m_refSkinLvl.lvl;}
	
	//Bo数据
	public PlayerConsortSkinBO getBo() {return _m_boSkin;}
	
	protected void _setBo(PlayerConsortSkinBO _bo) 
	{
		_m_boSkin = _bo;
		
		if(_m_boSkin.getSkinLvl() > 0)
		{
			_m_refSkinLvl = _m_refSkin.getLevelMapMgr().getLevelData(_m_boSkin.getSkinLvl());
			if(null == _m_refSkinLvl)
			{
				USLog.error(getUSServer(), "player:{} init consort:{} skin:{} lvl:{} fail, not find ref."
						, getUserData().getCid(), getConsortId(), getSkinId(), _m_boSkin.getSkinLvl());
			}
		}
	}
	
	/****
	 * 构造数据对象
	 * @return
	 */
	public Consort_SkinInfo toProto()
	{
		Consort_SkinInfo proto = new Consort_SkinInfo();
		proto.setSkinId(getSkinId());
		proto.setLvl(getLvl());
		
		return proto;
	}
	
	/**
	 * 设置皮肤等级
	 * @param _lvlRef
	 * @param _context
	 */
	public void setLvl(RefConsortSkinLvl _lvlRef, NPPlayerContext _context)
	{
		if(getLvl() == _lvlRef.lvl)
			return;
		
		//更新配表
		_m_refSkinLvl = _lvlRef;
		//保存数据
		BM bmObj = getUSServer().getBM();
		_m_boSkin.setSkinLvl(bmObj, getLvl());
		_m_boSkin.saveAll(bmObj);
		
		//推送数据
	}
	
	/**
	 * 销毁数据
	 */
	protected void _discard() 
	{
		if(null != _m_boSkin)
		{
			_m_boSkin.del(getUSServer().getBM());
		}
	}
}
