package NPUSServer.NPUSUserMgr.UserComp.PlayerComp;

import Common.CachedObj.CachedObj_CachedTitleObj;
import Common.NpPlayerInfoObj.PlayerInfo_ComboTitle;
import NPCommon.ErrMain.PlayerSkinErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.PlayerInfo_CurTitle;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerTitleType;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TitleComp.PlayerTitleInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerBO;

import java.nio.ByteBuffer;

public class PlayerCurTitleMgr 
{
	//玩家数据对象
	private NPUSUserData _m_usUserData;
	//玩家当前穿戴数据
	private PlayerInfo_CurTitle _m_pcPlayerCurTitle;
	//玩家是否对外展示
	private boolean _m_bShow;
	
	public PlayerCurTitleMgr(NPUSUserData _userData)
	{
		_m_usUserData = _userData;
		
		_m_pcPlayerCurTitle = new PlayerInfo_CurTitle();
		_m_bShow = false;
	}

	//玩家数据
    public NPUSUserData getUserData() {return _m_usUserData;}
    //US服务器
    public NPUserServer getUSServer() {return _m_usUserData.getUSServer();}
    
    //当前穿戴称号
    public PlayerInfo_CurTitle getCurTitle() {return _m_pcPlayerCurTitle;}
    //是否展示
    public boolean getShow() {return _m_bShow;}
    
    protected void _initFromBo(PlayerBO _bo)
    {
    	if(null != _bo.getCurTitle())
    	{
    		ByteBuffer buff = ByteBuffer.wrap(_bo.getCurTitle());
    		_m_pcPlayerCurTitle.readPackage(buff);
    	}
    	
    	_m_bShow = _bo.getCurTitleShow();
    }
    
    /**
     * 设置是否展示
     * @param _show
     * @param _context
     */
    public void setShow(boolean _show, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		_m_bShow = _show;
    		
    		getUserData().getPlayerComponent().getBo().saveCurTitleShow(getUSServer().getBM(), _m_bShow);

			//更新缓存数据
			CachedObj_CachedTitleObj cacheObj = new CachedObj_CachedTitleObj();
			cacheObj.setCurInfo(_m_pcPlayerCurTitle);
			cacheObj.setIsShow(_m_bShow);
			PlayerCacheFunc.updateTitleObjV2(getUserData(), cacheObj);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 穿戴通用称号
     * @param _titleId
     * @param _context
     * @return
     */
    public Result setCommTitle(long _titleId, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		PlayerTitleInfo title = getUserData().getTitleComponent().getEnableTitle(_titleId);
    		if(null == title)
    		{
    			return PlayerSkinErr.PLAYER_TITLE_NOT_FOUND;
        	}
    		
    		_m_pcPlayerCurTitle.setType(ENPPlayerTitleType.COMMON);
    		_m_pcPlayerCurTitle.setInfo(CommonFunc.ByteBfferToBytes(title.makeProto().makePackage()));
    		//保存数据
    		getUserData().getPlayerComponent().getBo().saveCurTitle(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_m_pcPlayerCurTitle.makePackage()));

            //更新缓存数据
    		CachedObj_CachedTitleObj cacheObj = new CachedObj_CachedTitleObj();
    		cacheObj.setCurInfo(_m_pcPlayerCurTitle);
    		cacheObj.setIsShow(_m_bShow);
            PlayerCacheFunc.updateTitleObjV2(getUserData(), cacheObj);
            
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_054_OnCurTitleChg(_m_pcPlayerCurTitle));
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 穿戴组合称号
     * @param _preId
     * @param _sfxId
     * @param _bgId
     * @param _context
     * @return
     */
    public Result setComboTitle(long _preId, long _sfxId, long _bgId, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(!getUserData().getPlayerComboTitleComp().canSet(_preId, _sfxId, _bgId))
    		{
    			return PlayerSkinErr.PLAYER_TITLE_NOT_FOUND;
    		}
    		
    		_m_pcPlayerCurTitle.setType(ENPPlayerTitleType.COMBO);
    		PlayerInfo_ComboTitle info = new PlayerInfo_ComboTitle();
    		info.setPreId(_preId);
    		info.setSfxId(_sfxId);
    		info.setBgId(_bgId);
    		_m_pcPlayerCurTitle.setInfo(CommonFunc.ByteBfferToBytes(info.makePackage()));
    		//保存数据
    		getUserData().getPlayerComponent().getBo().saveCurTitle(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_m_pcPlayerCurTitle.makePackage()));

            //更新缓存数据
    		CachedObj_CachedTitleObj cacheObj = new CachedObj_CachedTitleObj();
    		cacheObj.setCurInfo(_m_pcPlayerCurTitle);
    		cacheObj.setIsShow(_m_bShow);
            PlayerCacheFunc.updateTitleObjV2(getUserData(), cacheObj);
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_054_OnCurTitleChg(_m_pcPlayerCurTitle));
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
}
