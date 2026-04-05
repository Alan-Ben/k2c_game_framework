package NPCommonServer.NPCrossGameServerHandleMgr;

import NPCommonServer.CommonServerConf;

/******************
 * 公共服务器中间对应区域的房间信息对象
 * @author Administrator
 *
 */
public class NPCrossGameServerInfo
{
    /**
     * 服务器的类型Id
     */
    private int _m_iServerTypeId;
    /**
     * 当前服务器已承载的数值
     */
    private int _m_iHandleUserWeight;

    public NPCrossGameServerInfo(int _serverTypeId)
    {
        _m_iServerTypeId = _serverTypeId;

        _m_iHandleUserWeight = 0;
    }

    public int getServerTypeId()
    {
        return _m_iServerTypeId;
    }

    /**
     * 更新临时申请负载数值
     * @param _count
     */
    public void setHandleWeight(int _count)
    {
        _m_iHandleUserWeight = _count;
    }

    /**
     * 当前权重总值
     * @return
     */
    public int getHandleUserWeight()
    {
        return _m_iHandleUserWeight;
    }

    /**
     * 最大允许的负载，默认10000
     * @return
     */
    public boolean canHandle()
    {
        return getHandleUserWeight() < CommonServerConf.getInstance().getCrossGameWeightLimit();
    }

    /**
     * 增加临时负载数
     * @return
     */
    protected boolean addHandleUser(int _tmpAddWeight)
    {
        //临时增加的权重至少在1以上
        if (_tmpAddWeight <= 0)
            _tmpAddWeight = 1;

        int curWeight = _m_iHandleUserWeight + _tmpAddWeight;
        //检查当前权重是否超过最大权重
        if (curWeight > CommonServerConf.getInstance().getCrossGameWeightLimit())
            return false;

        setHandleWeight(curWeight);

        return true;
    }
}
