package NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp;

import ALBasicCommon.ALBasicCommonFun;
import NPGameRes.Refs.Player._ARefExpiredItem;

public abstract class _AExpiredItemInfo<R extends _ARefExpiredItem>
{
    //配置数据对象
    private R _m_ref;

    //数据库数据对象
    private long _m_dbId;
    //过期时间 如果小于等于0, 则不过期
    private int _m_expireTimeSec;
    private boolean _m_viewed;
    //用于检查是否需要发送过期提醒邮件, 如果道具没过期这个标志位要标为true. 如果检查时标志位为true, 且道具过期则直接发送邮件, 并标记为false
    private boolean _m_needCheckSendMail;

    public _AExpiredItemInfo(R _ref, long _dbId, int _expireTimeSec, boolean _viewed, boolean _needCheckSendMai)
    {
        _m_ref = _ref;
        _m_dbId = _dbId;
        _m_expireTimeSec = _expireTimeSec;
        _m_viewed = _viewed;
        _m_needCheckSendMail = _needCheckSendMai;
    }

    public R getRef()
    {
        return _m_ref;
    }

    public long getRefId()
    {
        return _m_ref.Id();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public int getExpireTimeSec()
    {
        return _m_expireTimeSec;
    }

    public boolean getViewed()
    {
        return _m_viewed;
    }

    public boolean getNeedCheckSendMail()
    {
        return _m_needCheckSendMail;
    }

    /**
     * 修改过期时间
     * @param _expireTimeSec 过期时间
     */
    public void chgExpireTimeSec(int _expireTimeSec)
    {
        _m_expireTimeSec = _expireTimeSec;

        if (!_m_needCheckSendMail)
        {
            _m_needCheckSendMail = !hasExpired();
        }

        //当过期时间改变时，需要更新数据库
        saveExpiredTimeSecChg();
    }

    /**
     * 检查是否需要发送邮件
     */
    public boolean checkNeedSendMailTagChg()
    {
        if (!_m_needCheckSendMail)
            return false;

        //如果目前还没过期, 则不处理
        if (!hasExpired())
            return false;

        _m_needCheckSendMail = false;

        //当检查需要发送邮件tag变化时，需要更新数据库
        saveCheckNeedSendMailTagChg();

        return true;
    }

    /**
     * 设置已查看
     */
    public void setViewed()
    {
        if (_m_viewed)
            return;

        _m_viewed = true;

        //当查看标志位改变时，需要更新数据库
        saveViewedTagChg();
    }
    
    /**
     * 增加获取次数
     */
    protected void _onGainAgain()
    {
    }

    /**
     * 判断数据是否有效
     */
    public boolean enable()
    {
        return enable(ALBasicCommonFun.getNowTime());
    }

    /**
     * 判断数据是否有效
     */
    public boolean enable(int _nowTimeS)
    {
        //暂时只判断超时，后续再判断global_res
        if (!hasExpired(_nowTimeS))
            return true;

        return false;
    }

    /**
     * 检查是否过期
     * @return
     */
    public boolean hasExpired()
    {
        return hasExpired(ALBasicCommonFun.getNowTime());
    }

    /**
     * 检查是否过期
     * @param _nowTimeS 当前时间戳
     * @return
     */
    public boolean hasExpired(int _nowTimeS)
    {
        //判断时间戳
        if (getExpireTimeSec() <= 0)
            return false;

        //当前时间比超时时间小，则未超时
        if (getExpireTimeSec() > _nowTimeS)
            return false;

        //返回超时
        return true;
    }

    /**
     * 当过期时间改变时
     */
    protected abstract void saveExpiredTimeSecChg();

    /**
     * 当查看标志位改变时
     */
    protected abstract void saveViewedTagChg();

    /**
     * 当检查需要发送邮件tag变化时
     */
    protected abstract void saveCheckNeedSendMailTagChg();

    /**
     * 删除信息
     */
    public abstract void del();
}
