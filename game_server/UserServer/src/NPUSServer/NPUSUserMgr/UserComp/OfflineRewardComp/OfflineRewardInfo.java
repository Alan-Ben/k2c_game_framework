package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.OfflineRewardObj.OfflineReward_Info;
import Common.OfflineRewardObj.Offline_CommonReward;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer._AOfflineDataDealer;
import USDB.Bo.PlayerOfflineRewardBO;

import java.nio.ByteBuffer;

public class OfflineRewardInfo
{
    //离线奖励组件
    private OfflineRewardComponent _m_comp;
    
    //数据实例ID
    private long _m_lID;
    //奖励类型
    private int _m_iRewardType;
    //是否已经预处理
    private boolean _m_bHasPreDeal;
    
    //离线数据
    private ByteBuffer _m_bbOfflineData;
    //实际奖励内容，这部分内容是通过客户端发起请求后领取
    private Offline_CommonReward _m_crOfflineRewardItemObj;
    //奖励展示内容，只用于客户端展示
    private ByteBuffer _m_bbRewardShow;
    
    //处理对象
    private _AOfflineDataDealer _m_odOfflineDealer;

    /**
     * 有创建BO的初始化
     * @param _comp
     * @param _bo
     * @param _dealer
     */
    public OfflineRewardInfo(OfflineRewardComponent _comp, PlayerOfflineRewardBO _bo, _AOfflineDataDealer _dealer)
    {
        _m_comp = _comp;

        _m_lID = _bo.getId();
        _m_iRewardType = _bo.getRewardType();
        _m_bHasPreDeal = _bo.getHasPreDeal();

        //离线数据
        if(null != _bo.getOfflineData())
        {
        	_m_bbOfflineData = ByteBuffer.wrap(_bo.getOfflineData());
        }
        //奖励列表
        if(null != _bo.getItemList())
        {
        	ByteBuffer buff = ByteBuffer.wrap(_bo.getItemList());
        	
        	_m_crOfflineRewardItemObj = new Offline_CommonReward();
        	_m_crOfflineRewardItemObj.readPackage(buff);
        }
        //展示内容
        if (null != _bo.getRewardShow())
        {
            _m_bbRewardShow = ByteBuffer.wrap(_bo.getRewardShow());
        }
        
        _m_odOfflineDealer = _dealer;
    }
    /**
     * 无需创建BO的初始化
     * @param _comp
     * @param _rewardType
     * @param _offlineData
     * @param _itemList
     * @param _rewardShow
     * @param _dealer
     */
    public OfflineRewardInfo(OfflineRewardComponent _comp, int _rewardType
    		, ByteBuffer _offlineData, ByteBuffer _itemList, ByteBuffer _rewardShow, _AOfflineDataDealer _dealer)
    {
        _m_comp = _comp;

        _m_iRewardType = _rewardType;

        //离线数据
        if(null != _offlineData)
        {
        	_m_bbOfflineData = _offlineData;
        }
        //奖励列表
        if(null != _itemList)
        {
        	_m_crOfflineRewardItemObj = new Offline_CommonReward();
        	_m_crOfflineRewardItemObj.readPackage(_itemList);
        }
        //展示内容
        _m_bbRewardShow = _rewardShow;
        
        _m_odOfflineDealer = _dealer;
    }

    public OfflineRewardComponent getComp() {return _m_comp;}
    public NPUSUserData getUserData() {return _m_comp.getUserData();}

    public long getId() {return _m_lID;}
    public int getRewardType() {return _m_iRewardType;}
    
    public ByteBuffer getOfflineData() {return _m_bbOfflineData;}
    public Offline_CommonReward getItemList() {return _m_crOfflineRewardItemObj;}
    
    public _AOfflineDataDealer getDealer() {return _m_odOfflineDealer;}

    public boolean hasPreDeal()
    {
        return _m_bHasPreDeal;
    }

    /**
     * 构造数据
     * @return
     */
    public OfflineReward_Info toProto()
    {
        OfflineReward_Info proto = new OfflineReward_Info();
        proto.setId(_m_lID);
        proto.setRewardType(_m_iRewardType);
        proto.setRewardShow(_m_bbRewardShow);

        return proto;
    }

    /**
     * 标记已经预处理
     * @param _context
     */
    public void markHasPreDeal(NPPlayerContext _context)
    {
        _m_bHasPreDeal = true;

        if (_m_lID > 0)
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("has_pre_deal", 1);
            _m_comp.getUSServer().getBM().getBM(PlayerOfflineRewardBO.class).update("id", _m_lID, updateValue);
        }
    }

    /**
     * 移除数据
     */
    protected void del()
    {
        _m_comp.getUSServer().getBM().getBM(PlayerOfflineRewardBO.class).delAll("id", _m_lID);
    }
}
