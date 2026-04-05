package NPCommonServer.WhiteAccMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2HS_RB.p001_HSOp.NP2HS_RB_001_007_RetWhiteAccList;
import NPCommon.Log.CommLog;
import NPCommonServer.NPCommonServer;
import NPServerProtocolWriter.NP2HS.Request.NP2HS_R_Writer_001_HSOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * CommonServer向HttpServer请求白名单列表的任务
 *
 * 执行流程：
 * 1. 发送NP2HS_R_001_007请求到HttpServer
 * 2. 成功：将白名单数据加载到CSWhiteAccMgr
 * 3. 失败：每秒重试，超过10次发送钉钉告警
 */
public class CSSendToGetWhiteAccListTask implements _IALSynTask
{
    // 失败次数
    private int _m_iFailCount;

    // 操作序列号，避免任务重启引发重新加载引发计数错误
    private long _m_opSerialize;

    public CSSendToGetWhiteAccListTask(long _opSerialize)
    {
        _m_iFailCount = 0;
        _m_opSerialize = _opSerialize;
    }

    @Override
    public void run()
    {
        final CSSendToGetWhiteAccListTask task = this;

        NPCommonServer.getInstance().sendRequestToBSServer(
            EServerType.SINGLE.ordinal(),
            ENPSingleServerType.HTTP.ordinal(),
            NP2HS_R_Writer_001_HSOp.make_007_ReqWhiteAccList(),
            new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new NP2HS_RB_001_007_RetWhiteAccList();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _retProto)
                {
                    NP2HS_RB_001_007_RetWhiteAccList ret = (NP2HS_RB_001_007_RetWhiteAccList) _retProto;
                    CSWhiteAccMgr.getInstance().loadWhiteAccList(ret.getWhiteAccList(), _m_opSerialize);
                }

                @Override
                public void dealFail(int _errCode)
                {
                    _m_iFailCount++;

                    if (_m_iFailCount > 10)
                    {
                        CommLog.error("Send Get WhiteAccList Fail, FailCount: {}, ErrCode: {}", _m_iFailCount, _errCode);
                        return;
                    }

                    // 1秒后重试
                    if (CSWhiteAccMgr.getInstance().getOpSerialize() != _m_opSerialize)
                    {
                        // 操作序列号已变更，停止重试
                        return;
                    }

                    ALSynTaskManager.getInstance().regTask(task, 1000);
                }
            });
    }
}
