package NPCommon.DB;

import ALMySqlCommon.ALMySqlCommonObj.ALMySqlConnectionPool._AALMySqlBaseDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.DBVersionInfoBO;
import NPCommon.DB.Version.TbIDMgr.TBIdInfo;
import NPCommon.DB.Version.TbIDMgr.TBIdMgr;
import NPCommon.Enum.NPCommonEnum;

public class WCGDBObj extends _AALMySqlBaseDBObj
{
    private NPCommonEnum.EDBTag _m_sDBTag;
    //安全操作数据保存路径
    private String _m_sSafeOPSavePath;

    /**
     * 本数据库标记下的数据各表Id管理器
     */
    private TBIdMgr _m_tbIdMgr;
    private boolean _m_bIsDBIdInited;

    private int _m_iTaskIndex = 1;

    public WCGDBObj(String _host, String _dbName, String _userName, String _password, String _safeOpSavePath)
    {
        super(_host, _dbName, _userName, _password);

        _m_sSafeOPSavePath = _safeOpSavePath;
        //创建对象
        _m_tbIdMgr = new TBIdMgr(this);
        _m_bIsDBIdInited = false;
    }

    public String getSafeOpSavePath()
    {
        return _m_sSafeOPSavePath;
    }

    @Override
    public String getCurVersion()
    {
        return "0.0.0.1";
    }

    public void setDbTag(NPCommonEnum.EDBTag _sTag)
    {
        _m_sDBTag = _sTag;
    }

    public NPCommonEnum.EDBTag getDbTag()
    {
        return _m_sDBTag;
    }

    public void setTaskIndex(int _iIndex)
    {
        _m_iTaskIndex = _iIndex;
    }

    public int getTaskIndex()
    {
        return _m_iTaskIndex;
    }


    /**
     * 获取对应Tb的Id信息对象
     * @param _tbName
     * @return
     */
    public TBIdInfo ensureTbIdInfo(String _tbName)
    {
        if(null == _m_tbIdMgr)
            return null;

        return _m_tbIdMgr.ensureIdInfo(_tbName);
    }

    /**
     * 根据数据库版本信息，初始化数据表Id信息管理器
     * @param _versionBo
     */
    public void initTbIdMgr(DBVersionInfoBO _versionBo)
    {
        if(null == _versionBo)
        {
            ALServerLog.Fatal("BM.initTbIdMgr versionBo is null!");
            return ;
        }

        if(_m_bIsDBIdInited)
        {
            ALServerLog.Fatal("BM.initTbIdMgr TbIdMgr has been initialized!");
            return ;
        }

        _m_bIsDBIdInited = true;
        //初始化数据
        _m_tbIdMgr.init(_versionBo);
    }
}
