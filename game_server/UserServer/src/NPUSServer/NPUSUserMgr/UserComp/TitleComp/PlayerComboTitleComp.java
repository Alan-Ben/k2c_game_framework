package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPEnum.ENPPlayerComboTitleType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerComboTitleBO;

import java.util.List;

@SuppressWarnings("rawtypes")
public class PlayerComboTitleComp extends _ANPUserComponent
{
	private _AComboTitleUnitMgr[] _m_arrComboTitleMgr;
	
    public PlayerComboTitleComp(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.COMBO_TITLE);
        
        _m_arrComboTitleMgr = new _AComboTitleUnitMgr[ENPPlayerComboTitleType.ENPPlayerComboTitleType_Length];
        regUnitMgr(new ComboTitleUnitPreMgr(getUserData()));//组合称号-前缀
        regUnitMgr(new ComboTitleUnitSfxMgr(getUserData()));//组合称号-后缀
        regUnitMgr(new ComboTitleUnitBgMgr(getUserData()));//组合称号-底色
    }
    
    public void regUnitMgr(_AComboTitleUnitMgr _unitMgr)
    {
    	_m_arrComboTitleMgr[_unitMgr.getType().ordinal()] = _unitMgr;
    }
    public _AComboTitleUnitMgr getUnitMgr(ENPPlayerComboTitleType _type)
    {
    	return _m_arrComboTitleMgr[_type.ordinal()];
    }

    @Override
    protected void _init()
    {	
        getUSServer().getBM().getBM(PlayerComboTitleBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerComboTitleBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Combo Title Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerComboTitleBO> _list)
            {
                _initBo(_list);
            }
        });
    }
    //初始化数据
    @SuppressWarnings("unchecked")
	private void _initBo(List<PlayerComboTitleBO> _list)
    {
        //获取当前时间
        for (int i = 0; i < _list.size(); i++)
        {
        	PlayerComboTitleBO bo = _list.get(i);
            if (null == bo)
                continue;

            ENPPlayerComboTitleType type = ENPPlayerComboTitleType.ENPPlayerComboTitleType_FromInt(bo.getUnitType());
            if(null == type)
            {
            	USLog.error(getUSServer(), "player:{} title unit:{} load fail, not find type.", getUserData().getCid(), bo.getUnitType());
            	continue;
            }
            
            _AComboTitleUnitMgr unitMgr = getUnitMgr(type);
            if(null == unitMgr)
            {
            	USLog.error(getUSServer(), "player:{} title unit:{} load fail, not find mgr.", getUserData().getCid(), bo.getUnitType());
            	continue;
            }
            
            //检查重复数据，重复数据进行移除，同时发送日志，后期对日志进行再次检查
            if(unitMgr.hasItem(bo.getUnitId(), 1))
            {
            	bo.del(getBM());
            	USLog.error(getUSServer(), "player:{} unitType:{} unitId:{} duplicate bo.", getCid(), bo.getUnitType(), bo.getUnitId());
            	continue;
            }
            
            _AComboTitleUnit unit = unitMgr._createUnit(bo);
            unitMgr._initUnit(unit);
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
        //检查可以解锁的组合称号部件
        for(int i = 0; i < _m_arrComboTitleMgr.length; i++)
        {
        	_AComboTitleUnitMgr mgr = _m_arrComboTitleMgr[i];
        	if(null == mgr)
        		continue;
        	
        	mgr._initCheck();
        }
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    }

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////
    
    /**
     * 检查是否可以穿戴组合称号
     * @param _preId
     * @param _sfxId
     * @param _bgId
     * @return
     */
    public boolean canSet(long _preId, long _sfxId, long _bgId)
    {
    	return (_preId == 0 || getUnitMgr(ENPPlayerComboTitleType.PRE).hasItem(_preId, 1))
    			&& (_sfxId == 0 || getUnitMgr(ENPPlayerComboTitleType.SFX).hasItem(_sfxId, 1))
    			&& (_bgId == 0 || getUnitMgr(ENPPlayerComboTitleType.BG).hasItem(_bgId, 1));
    }
}
