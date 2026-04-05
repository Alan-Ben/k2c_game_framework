package NPCommonServer.HandleServerMgr;

import CSDB.Bo.HandleServerBO;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommonServer.NPCommonServer;

/**
 * 服务器权重数据
 */
public class HandleServerInfo
{
    //对应服务器权重管理对象
    private HandleServerMgr _m_mgrHandleServerMgr;
    //服务器的类型Id
    private int _m_iServerTypeId;
    //当前服务器已承载的数值
    private int _m_iHandleUserWeight;

    //是否有效
    public boolean _m_bIsEnable;

    //负载数据bo
    private HandleServerBO _m_bo;

    //保存负载数据的懒处理对象
    private LazyTaskDealer _m_ldSaveWeightDealer;

    public HandleServerInfo(HandleServerMgr _handleServerMgr, HandleServerBO _bo)
    {
        _m_mgrHandleServerMgr = _handleServerMgr;

        _m_iServerTypeId = _bo.getTypeId();

        _m_iHandleUserWeight = _bo.getWeight();

        _m_bo = _bo;

        _m_ldSaveWeightDealer = new LazyTaskDealer(()-> _saveWeight(), 5000);
    }

    public int getServerTypeId()
    {
        return _m_iServerTypeId;
    }

    public void doLazySaveWeight()
    {
        _m_ldSaveWeightDealer.setNeedDeal();
    }
    private void _saveWeight()
    {
        _m_bo.saveWeight(NPCommonServer.getInstance().getBM(), _m_iHandleUserWeight);
    }

    public int getHandleUserWeight()
    {
        return _m_iHandleUserWeight;
    }
    public void setHandleWeight(int _value)
    {
        _m_iHandleUserWeight = _value;

        doLazySaveWeight();
    }

    public boolean isEnable()
    {
        return _m_bIsEnable;
    }
    public void setEnable(boolean _value)
    {
        _m_bIsEnable = _value;
    }

    /**
     * 最大允许的负载，默认10000
     * @return
     */
    public boolean canHandle()
    {
        return _m_bIsEnable &&
                (_m_mgrHandleServerMgr.getHandleUserWeightLimit() == 0
                        || getHandleUserWeight() < _m_mgrHandleServerMgr.getHandleUserWeightLimit());
    }

    /**
     * 增加临时负载数
     * @return
     */
    public boolean addHandleUser(int _tmpAddWeight)
    {
        //临时增加的权重至少在1以上
        if (_tmpAddWeight <= 0)
            _tmpAddWeight = 1;

        int curWeight = _m_iHandleUserWeight + _tmpAddWeight;
        //检查当前权重是否超过最大权重
        if (_m_mgrHandleServerMgr.getHandleUserWeightLimit() > 0
                && curWeight > _m_mgrHandleServerMgr.getHandleUserWeightLimit())
            return false;

        setHandleWeight(curWeight);

        return true;
    }

    /**
     * 减少负载数
     * @param _tmpAddWeight
     */
    public void reduceHandleUser(int _tmpAddWeight)
    {
        int curWeight = Math.max(_m_iHandleUserWeight - _tmpAddWeight, 0);
        setHandleWeight(curWeight);
    }
}
