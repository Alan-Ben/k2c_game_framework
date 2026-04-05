package NPCommon.DB.Version.TbIDMgr;

import WCGCommon.ALProtocolBuf;

/**
 * 管理表及其Id的具体信息
 */
public class TBIdInfo {
    private TBIdMgr _m_tbIdMgr;
    private String _m_sTbName;
    private long _m_lMaxId;

    public TBIdInfo(TBIdMgr _idMgr, String _tbName)
    {
        _m_tbIdMgr = _idMgr;
        this._m_sTbName = _tbName;
        this._m_lMaxId = 0;
    }
    public TBIdInfo(TBIdMgr _idMgr, String _tbName, long _maxId)
    {
        _m_tbIdMgr = _idMgr;
        this._m_sTbName = _tbName;
        this._m_lMaxId = _maxId;
    }

    public String getTbName() { return this._m_sTbName; }
    public long getMaxId() { return this._m_lMaxId; }

    public int getBufferSize(){return ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(_m_sTbName) + 8;}

    /**
     * 重新设置最大Id
     */
    public void initMaxId(long _maxId)
    {
        synchronized (this)
        {
            this._m_lMaxId = _maxId;
        }
    }

    /********
     * 尝试同步最大Id，如果最大Id大于本Id，需要设置脏标记
     * @param _maxId
     */
    public void trySyncMaxId(long _maxId)
    {
        synchronized (this)
        {
            if(_maxId <= this._m_lMaxId)
                return ;

            this._m_lMaxId = _maxId;
            _m_tbIdMgr.setDirty();
        }
    }


    /******
     * 取一个新Id
     * @return
     */
    public long popNewId()
    {
        //加锁避免互斥访问
        synchronized (this)
        {
            _m_tbIdMgr.setDirty();

            this._m_lMaxId += 1;
            return this._m_lMaxId;
        }
    }

    /**
     * 将数据写入ByteBuffer
     * @param _buffer
     */
    public void pushToBuffer(ALProtocolBuf _buffer)
    {
        if(null == _buffer)
            return ;

        //加锁避免互斥访问
        synchronized (this) {
            _buffer.putString(this._m_sTbName);
            _buffer.putLong(this._m_lMaxId);
        }
    }
}
