package NPUSServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.NPGeneralListener.RequestDispather.p010_MarsMineOp.NPUS_010_RequestDispatcher_MarsMineOp;
import NPUSServer.NPUserServer;

public class NPUSGeneralRequestDispather extends NPRequestDispatcher
{
    private NPUserServer _m_server;

    public NPUSGeneralRequestDispather(NPUserServer _server)
    {
        _m_server = _server;

        NPUSGeneral_001_RequestDispatcher_BasicOp.init(this);
        NPUSGeneral_002_RequestDispatcher_GSOp.init(this);
        NPUsGeneral_003_RequestDispatcher_CommOp.init(this);
        NPUsGeneral_004_RequestDispatcher_PayOp.init(this);
        NPUsGeneral_005_RequestDispatcher_WebPayOp.init(this);
        NPUSGeneral_006_RequestDispatcher_CacheOp.init(this);
        NPUsGeneral_007_RequestDispatcher_CrossGameOp.init(this);
        NPUsGeneral_008_RequestDispatcher_CrossServerGroupOp.init(this);
        NPUsGeneral_009_RequestDispatcher_CrossRankOp.init(this);
        NPUS_010_RequestDispatcher_MarsMineOp.init(this);
        NPUsGeneral_255_RequestDispatcher.init(this);
    }

    public NPUserServer getUSServer() { return this._m_server; }
}
