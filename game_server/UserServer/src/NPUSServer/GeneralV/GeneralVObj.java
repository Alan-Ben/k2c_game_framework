package NPUSServer.GeneralV;

import NPCommon.DB.BM.BM;
import NPUSServer.NPUserServer;
import USDB.Bo.GeneralVBO;

public class GeneralVObj
{
    private NPUserServer _m_server;
    private GeneralVBO _m_bo;
    private EGeneralVType _m_eType;

    private EGeneralVType getType()
    {
        return _m_eType;
    }

    public GeneralVObj(NPUserServer _server, EGeneralVType _eType)
    {
        _m_server = _server;
        _m_eType = _eType;
    }

    public NPUserServer getUSServer(){return _m_server;}

    /***
     * 初始化加载
     * @param _bo
     */
    public void s_initBo(GeneralVBO _bo)
    {
        _m_bo = _bo;
    }

    /***
     * 加载时确保bo存在
     */
    public void s_ensureBo()
    {
        synchronized (this)
        {
            if (null != _m_bo)
            {
                return;
            }

            BM bmObj = getUSServer().getBM();

            _m_bo = new GeneralVBO();
            _m_bo.setType(bmObj, getType().ordinal());
            _m_bo.setV(bmObj, 0L);
            _m_bo.insert_sync(bmObj);
        }
    }

    /*****
     * 自增id
     * @return
     */
    public long makeNewId()
    {
        synchronized (this)
        {
            long value = _m_bo.getV();
            value++;
            _m_bo.saveV(getUSServer().getBM(), value);
            return value;
        }
    }

    public long getV()
    {
        synchronized (this)
        {
            return _m_bo.getV();
        }
    }
}
