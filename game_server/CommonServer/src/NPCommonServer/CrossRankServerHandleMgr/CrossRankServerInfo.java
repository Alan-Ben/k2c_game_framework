package NPCommonServer.CrossRankServerHandleMgr;

/******************
 * 公共服务器中间对应区域的房间信息对象
 * @author Administrator
 *
 */
public class CrossRankServerInfo
{
    //服务器的类型Id
    private int _m_iServerTypeId;
    //当前服务器已承载的数值
    private int _m_iHadHandleInstanceNum;
    //服务器承载上限值
    private int _m_iHandleInstanceLimit;
    //是否已经初始化
    private boolean _m_bHasInit;
    //服务器管理器
    private CrossRankServerMgr _m_mgr;

    public CrossRankServerInfo(int _serverTypeId, CrossRankServerMgr _mgr)
    {
        _m_iServerTypeId = _serverTypeId;
        _m_mgr = _mgr;

        _m_bHasInit = false;
    }

    protected void _lock()
    {
        _m_mgr._lock();
    }

    protected void _unlock()
    {
        _m_mgr._unlock();
    }

    /**
     * 获取服务器id
     * @return
     */
    public int getServerTypeId()
    {
        return _m_iServerTypeId;
    }

    /**
     * 返回当前的承载值
     * @return
     */
    public int getHandleUserWeight()
    {
        return _m_iHadHandleInstanceNum;
    }

    /**
     * 返回是否可以继续承载实例
     * @return
     */
    public boolean canHandle()
    {
        return _m_bHasInit && _m_iHadHandleInstanceNum < _m_iHandleInstanceLimit;
    }

    public void initServerInfo(int _hadHandleWeight, int _handleLimit)
    {
        _lock();
        try{
            _m_bHasInit = true;
            _m_iHadHandleInstanceNum = _hadHandleWeight;
            _m_iHandleInstanceLimit = _handleLimit;
        }finally{
            _unlock();
        }
    }

    /**
     * 增加临时负载数
     * @return
     */
    protected boolean addHandleWeight(int _tmpAddWeight)
    {
        _lock();
        try{
            //临时增加的权重至少在1以上
            if (_tmpAddWeight <= 0)
                _tmpAddWeight = 1;

            int curWeight = _m_iHadHandleInstanceNum + _tmpAddWeight;
            //检查当前权重是否超过最大权重
            if (curWeight > _m_iHandleInstanceLimit)
                return false;

            _m_iHadHandleInstanceNum = curWeight;

            return true;
        }finally{
            _unlock();
        }
    }

    /**
     * 降低权重
     * @param _weight 权重
     */
    public void reduceHandleWeight(int _weight)
    {
        _lock();
        try{
            _m_iHadHandleInstanceNum -= _weight;
            if (_m_iHadHandleInstanceNum < 0)
                _m_iHadHandleInstanceNum = 0;
        }finally{
            _unlock();
        }
    }
}
