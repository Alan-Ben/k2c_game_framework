package NPUSServer.NPUSUserMgr.UserComp.PlayerSkinComp;

import Common.NpPlayerInfoObj.PlayerInfo_Skin;
import NPGameRes.Refs.PlayerSkin.RefPlayerSkin;
import NPGameRes.Refs.PlayerSkin.RefPlayerSkinLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerSkinBO;

public class PlayerSkinInfo 
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	
	//数据Bo
	private PlayerSkinBO _m_bo;
	
	//等级配置数据
	private RefPlayerSkin _m_refSkin;
	private RefPlayerSkinLevel _m_refSkinLevel;
	
	public PlayerSkinInfo(NPUSUserData _userData, PlayerSkinBO _bo)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;
		
		_m_refSkin = RefPlayerSkin.getMgr().get(getSkinId());
		
		_setSkinLevelRef();
	}
	
	//重置技能等级配置
	private void _setSkinLevelRef()
	{
		if(null == _m_refSkin)
			return;
		
		RefPlayerSkinLevel preLvlRef = _m_refSkinLevel;
		_m_refSkinLevel = null;
		
		_m_refSkinLevel = _m_refSkin.getLevelMapMgr().getLevelData(getSkinLvl());
		if(null == _m_refSkinLevel)
		{
			return;
		}

		//玩家属性
		getUserData().getConsortComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.player_property, _m_refSkinLevel.player_property);
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_udUserData;}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//玩家皮肤数据
	public PlayerSkinBO getBo() {return _m_bo;}
	public long getSkinId() {return _m_bo.getSkinId();}
	public int getSkinLvl() {return _m_bo.getSkinLvl();}
	
	//玩家皮肤等级配置
	public RefPlayerSkin getSkinRef() {return _m_refSkin;}
	public RefPlayerSkinLevel getSkinLvlRef() {return _m_refSkinLevel;}
	
	//构造协议数据
	public PlayerInfo_Skin toProto()
	{
		PlayerInfo_Skin proto = new PlayerInfo_Skin();
		proto.setSkinId(getSkinId());
		proto.setLvl(getSkinLvl());
		
		return proto;
	}
	
	/**
	 * 设置皮肤等级
	 * @param _lvl
	 * @param _context
	 */
	public void setLvl(int _lvl, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(getSkinLvl() == _lvl)
				return;
			
			_m_bo.setSkinLvl(getUSServer().getBM(), _lvl);
			_m_bo.saveAll(getUSServer().getBM());
			
			//更新等级配置
			_setSkinLevelRef();

			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_060_OnPlayerSkinChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
