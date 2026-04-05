package NPUSServer.UsMars.MineCore.SynTask;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUserServer;
import NPUSServer.UsMars.MineCore.UsMarsMineCore;

/**
 * 火星矿单独的结算任务
 */
public class SynUsMarsMineSettleEndTask implements _IALSynTask
{
    private NPUserServer _m_server;
    private long _m_lMineDataId;

    public SynUsMarsMineSettleEndTask(NPUserServer _server, long _mineDataId)
    {
        _m_server = _server;
        _m_lMineDataId = _mineDataId;
    }

    @Override
    public void run()
    {
        //获取矿管理器
        UsMarsMineCore mineCore = _m_server.getMarsMineCore();

        //进行结束结算
        mineCore.settleEndMine(_m_lMineDataId);
    }
}
