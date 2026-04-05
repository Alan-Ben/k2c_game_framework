package NPUSServer.NPUSUserMgr.UserComp.PlayerShowComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPEnum.EPlayerShowEnum;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerShowDataBO;

import java.util.List;

public class PlayerShowComponent extends _ANPUserComponent
{
	//玩家展示数据数组，key-EPlayerShowEnum
	private _APlayerShowInfo[] _m_arrPlayerShowArr;
	
	public PlayerShowComponent(NPUSUserData _userData) 
	{
		super(_userData, NPCommonEnum.ENPPlayerCompType.PLAYER_SHOW);
		
		_m_arrPlayerShowArr = new _APlayerShowInfo[EPlayerShowEnum.EPlayerShowEnum_Length];
		
		//玩家展示数据
	}
	
	/**********
	 * 注册处理对象
	 * @param _show
	 */
	public void regPlayerShow(_APlayerShowInfo _show)
	{
		_m_arrPlayerShowArr[_show.getShowType().ordinal()] = _show;
	}
	public _APlayerShowInfo getPlayerShow(EPlayerShowEnum _type)
	{
		return _m_arrPlayerShowArr[_type.ordinal()];
	}
	public _APlayerShowInfo getPlayerShow(int _type)
	{
		return _m_arrPlayerShowArr[_type];
	}

	@Override
	protected void _init() 
	{
		getUSServer().getBM().getBM(PlayerShowDataBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerShowDataBO>>()
		{
			@Override
			public void dealFail() 
			{
				USLog.error(getUSServer(), "player:{} init player show bo fail.", getUserData().getCid());
				getUserData().setDataLoadFail();
			}

			@Override
			public void dealSuc(List<PlayerShowDataBO> _boList) 
			{
				_initBoList(_boList);
			}
		});
	}
	private void _initBoList(List<PlayerShowDataBO> _boList)
	{
		for(int i = 0; i < _boList.size(); i++)
		{
			PlayerShowDataBO bo = _boList.get(i);
			if(null == bo)
				continue;
			
			_APlayerShowInfo info = getPlayerShow(bo.getType());
			if(null == info)
			{
				USLog.error(getUSServer(), "player:{} init player show type:{} fail.", getUserData().getCid(), bo.getType());
				continue;
			}
			
			info.initFromBo(bo);
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
}
