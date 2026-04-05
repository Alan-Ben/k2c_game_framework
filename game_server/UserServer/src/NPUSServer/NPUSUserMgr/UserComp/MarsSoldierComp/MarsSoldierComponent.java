package NPUSServer.NPUSUserMgr.UserComp.MarsSoldierComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPGameRes.Refs.Mars.RefMarsSoldierLevel;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsSoldierBO;

/**
 * 火星 - 士兵组件
 * @author mj
 *
 */
public class MarsSoldierComponent extends _ANPUserComponent
{
	//数据ID
	private long _m_lId;
	//兵种等级
	private int _m_iLvl;
	
	//兵种等级配置
	private RefMarsSoldierLevel _m_ref;
	
    public MarsSoldierComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MARS_SOLDIER);
    }
    
    public long getId() {return _m_lId;}
    public int getLvl() {return _m_iLvl;}
    
    public RefMarsSoldierLevel getRef() {return _m_ref;}
    
    @Override
    protected void _init()
    {
		getUSServer().getBM().getBM(PlayerMarsSoldierBO.class).findOne("cid", getUserData().getCid(), 
				new _ASelectCallback<PlayerMarsSoldierBO>() 
		{
			@Override
			public void dealSuc(PlayerMarsSoldierBO _bo) 
			{
				_m_lId = _bo.getId();
				_m_iLvl = _bo.getLvl();
				
				_m_ref = RefMarsSoldierLevel.getMgr().get(_m_iLvl);
				if(null == _m_ref)
				{
					USLog.error(getUSServer(), "player:{} lvl:{} init mars solider lvl ref fail, not find ref.", getUserData().getCid(), _m_iLvl);
				}
				
				setInited();
			}

			@Override
			public void dealFail() 
			{
                if (getHasErr())
                {
                    USLog.error(getUSServer(), "MarsSoldierComponent _init fail cid:{}", getUserData().getCid());
                    getUserData().setDataLoadFail();
                    return;
                }
				
                setInited();
			}
		});
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
}
