package NPUSServer.NPUSUserMgr.UserComp.MarsComp;

import Common.MarsEnum.EMarsPropertyType;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.GameObjs.PlayerMarsProperty.PlayerMarsPropertyContainer;
import NPGameRes.GameObjs.PlayerMarsProperty.PlayerMarsPropertyMgr;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;

/**
 * 火星各系统数据合并整理汇总
 * @author mj
 *
 */
public class MarsComponent extends _ANPUserComponent implements _IHandlerHolder
{
    //火星属性管理对象
    private PlayerMarsPropertyMgr _m_mgrMarsPropertyMgr;
    //火星属性容器
    private PlayerMarsPropertyContainer _m_mcMarsPropertyContainer;
    //火星属性延迟计算
    private LazyTaskDealer _m_ldCalMarsPropertyDealer;
	
	//火星实力汇总
	private long _m_lMarsPower;
    //健康指数延迟计算
    private LazyTaskDealer _m_ldCalMarsPowerDealer;

    //玩家属性加成容器
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;
    
    //公会互助管理对象
    private GuildMarsHelpMgr _m_mgrGuildMarsHelpMgr;

	public MarsComponent(NPUSUserData _userData) 
	{
		super(_userData, ENPPlayerCompType.MARS);

		_m_mgrMarsPropertyMgr = new PlayerMarsPropertyMgr();
		_m_mcMarsPropertyContainer = new PlayerMarsPropertyContainer();
		_m_ldCalMarsPropertyDealer = new LazyTaskDealer(()->getMarsPropertyMgr().calculateChgProperties(), 200);

		_m_ldCalMarsPowerDealer = new LazyTaskDealer(() -> MarsCalculate.calMarsPower(getUserData(), null), 200);

		_m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer();
		
		_m_mgrGuildMarsHelpMgr = new GuildMarsHelpMgr(getUserData());
	}
    
    public PlayerMarsPropertyMgr getMarsPropertyMgr() {return _m_mgrMarsPropertyMgr;}
    public PlayerMarsPropertyContainer getMarsPropertyContainer() {return _m_mcMarsPropertyContainer;}
	public void doLazyCalMarsProperty() {_m_ldCalMarsPropertyDealer.setNeedDeal();}
	
	public GuildMarsHelpMgr getGuildMarsHelpMgr() {return _m_mgrGuildMarsHelpMgr;}
	
	public long getMarsPower() {return _m_lMarsPower;}
	public void setMarsPower(long _value) 
	{
		_m_lMarsPower = _value;
		
		getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.MARS_MAX_POWER, _value, NPPlayerContext.createNew(ENPGameEvent.MARS_POWER_CHG));
	}
	public void doLazyCalMarsPower() {_m_ldCalMarsPowerDealer.setNeedDeal();}

    public NPPlayerPropertyContainer getPlayerPropertyContainer() {return _m_pcPlayerPropertyContainer;}

	@Override
	protected void _init() 
	{
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
    	//火星组件火星属性加成
    	_m_mgrMarsPropertyMgr.regPropertyContainer(_m_mcMarsPropertyContainer);
    	//buff组件火星属性加成
        _m_mgrMarsPropertyMgr.regPropertyContainer(getUserData().getBuffComponent().getMarsPropertyContainer());
        //初始计算火星属性
        _m_mgrMarsPropertyMgr.initProperties();
        
        //初始计算火星实力
		MarsCalculate.calMarsPower(getUserData(), null);

		//监听火星属性变化
		getMarsPropertyMgr().propertyChgDelegate().addHandler(this, 
				new HandlerThree<EMarsPropertyType, Long, Long>()
        {
            @Override
            public void handle(EMarsPropertyType _type, Long _preValue, Long _curVal)
            {
            	//发起重新计算指数
            	switch (_type) 
            	{
            		//健康指数相关
					case HEALTH_ADD_PER:
					case OXYGEN_ADD_PER:
					case SATIETY_ADD_PER:
					case SLEEP_ADD_PER:
						getUserData().getMarsBuildingComponent().doLazyCalHealthIndex();
						break;
				
					//幸福指数相关
					case HAPPY_ADD_PER:
					case COMFORT_ADD_PER:
					case MOOD_ADD_PER:
						getUserData().getMarsBuildingComponent().doLazyCalHappyIndex();
						break;
	
					default:
						break;
				}
            }
        });

    	//监听玩家属性，更新玩家开宴会加成数据
        getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().addHandler(this, new HandlerThree<ENPPlayerPropertyType, Long, Long>()
        {
            @Override
            public void handle(ENPPlayerPropertyType _type, Long _preValue, Long _curVal)
            {
            	switch (_type) 
            	{
					case MARS_ENERGY_OUTPUT_PER:
						getUserData().getMarsBuildingComponent().calAllBuildingOutput();
						break;
						
					case MARS_TEAM_TROOP_NUM:
					case MARS_TEAM_TROOP_NUM_PER:
					case MARS_TEAM_TROOP_EXT_NUM:
						getUserData().getMarsExploreComponent().getTeamMgr().doLazyCalMarsTroopNum();
						break;

					case MARS_TEAM_SOLDIER_POWER:
					case MARS_TEAM_SOLDIER_POWER_PER:
						getUserData().getMarsExploreComponent().getTeamMgr().doLazyCalMarsTeamPower();
						break;

					default:
						break;
				}
            }
        });
	}

	@Override
	public void dispose() 
	{
    	getMarsPropertyMgr().propertyChgDelegate().clear(this);
    	getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().clear(this);
	}
}
