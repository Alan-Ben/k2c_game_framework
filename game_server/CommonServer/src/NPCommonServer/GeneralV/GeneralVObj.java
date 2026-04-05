package NPCommonServer.GeneralV;

import CSDB.Bo.GeneralVBO;
import NPCommon.DB.BM.BM;
import NPCommonServer.NPCommonServer;
import NPEnum.ENPCommonGeneralEnum;

public class GeneralVObj
{
    private GeneralVBO _m_bo;
    private ENPCommonGeneralEnum _m_eType;

    private ENPCommonGeneralEnum getType()
    {
        return _m_eType;
    }

    public GeneralVObj(ENPCommonGeneralEnum _eType)
    {
        _m_eType = _eType;
    }

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

            BM bmObj = NPCommonServer.getInstance().getBM();

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
            _m_bo.saveV(NPCommonServer.getInstance().getBM(), value);
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
