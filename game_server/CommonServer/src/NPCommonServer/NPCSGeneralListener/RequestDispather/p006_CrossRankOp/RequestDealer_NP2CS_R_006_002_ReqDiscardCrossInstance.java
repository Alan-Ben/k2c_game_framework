package NPCommonServer.NPCSGeneralListener.RequestDispather.p006_CrossRankOp;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CRS_RB.p001_CrossRankOp.NP2CRS_RB_001_006_DiscardCrossInstance;
import NP2CS_R.np_p006_RankOp.NP2CS_R_006_002_ReqDiscardCrossInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.RankErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommonServer.CrossRankServerHandleMgr.CrossRankServerInfo;
import NPCommonServer.CrossRankServerHandleMgr.CrossRankServerMgr;
import NPCommonServer.NPCSGeneralListener.Writer.NP2CS_RB_Writer_006_CrossRankOp;
import NPCommonServer.NPCommonServer;
import NPServerProtocolWriter.NP2CRS.Request.NP2CRS_R_Writer_001_BasicOp;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum;

/**
 * 销毁跨服实例请求处理器
 *
 * 主要功能：
 * 1. 接收ScheduleServer的跨服实例销毁请求
 * 2. 转发请求到对应的CrossRankServer
 * 3. 处理各种错误情况（实例不存在、服务器离线等）
 * 4. 更新CommonServer上的负载权重
 *
 * 容错机制：
 * - 实例已不存在视为成功（幂等性）
 * - 服务器离线返回错误供上层重试
 * - 其他错误正常返回错误码
 */
public class RequestDealer_NP2CS_R_006_002_ReqDiscardCrossInstance extends NPRequestDealer<NP2CS_R_006_002_ReqDiscardCrossInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2CS_R_006_002_ReqDiscardCrossInstance _msg)
    {
        //获取对应的处理服务器对象
        _AWCGBSReceiverListener listener = (_AWCGBSReceiverListener) _committer.getRequestDealer();
        if (null == listener)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        int crsId = CommonFunc.parseCrossRankServerIdFromCrossInstanced(_msg.getCrossInstanceId());
        CrossRankServerInfo serverInfo = CrossRankServerMgr.getInstance().lookupServerById(crsId);
        if (serverInfo == null)
        {
            _committer.commitFailRes(RankErr.CROSS_RANK_HANDLE_FAIL.getCode());
            return;
        }

        //发送销毁请求到CrossRankServer
        NPCommonServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.CROSS_RANK.ordinal(), serverInfo.getServerTypeId(),
                NP2CRS_R_Writer_001_BasicOp.make_006_DiscardCrossInstance(_msg.getCrossInstanceId()), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_006_DiscardCrossInstance();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                        // 销毁成功，减少负载权重
                        CrossRankServerMgr.getInstance().reduceWeight(serverInfo.getServerTypeId(), 1);

                        CommLog.info("RequestDealer_NP2CS_R_006_002_ReqDiscardCrossInstance - discard success: crossInstanceId={}",
                                _msg.getCrossInstanceId());

                        //返回成功信息
                        _committer.commitSucRes(NP2CS_RB_Writer_006_CrossRankOp.make_002_RetDiscardCrossInstance());
                    }

                    @Override
                    public void dealFail(int _error)
                    {
                        // 区分错误类型进行处理
                        if (_error == RankErr.INSTANCE_NO_EXIST.getCode())
                        {
                            // 跨服实例已经不存在，视为成功（幂等性处理）
                            CommLog.warn("RequestDealer_NP2CS_R_006_002_ReqDiscardCrossInstance - instance already destroyed, treat as success: crossInstanceId={}",
                                    _msg.getCrossInstanceId());

                            // 减少负载权重
                            CrossRankServerMgr.getInstance().reduceWeight(serverInfo.getServerTypeId(), 1);

                            // 返回成功
                            _committer.commitSucRes(NP2CS_RB_Writer_006_CrossRankOp.make_002_RetDiscardCrossInstance());
                        }
                        else if (_error == CommErr.RPC_CALL_ERR.getCode())
                        {
                            // RPC调用失败（可能是服务器离线），返回错误让上层重试
                            CommLog.error("RequestDealer_NP2CS_R_006_002_ReqDiscardCrossInstance - RPC call failed (CRS may be offline): crossInstanceId={}, crsId={}, errorCode={}",
                                    _msg.getCrossInstanceId(), crsId, _error);
                            _committer.commitFailRes(_error);
                        }
                        else
                        {
                            // 其他错误，记录日志并返回错误码
                            CommLog.error("RequestDealer_NP2CS_R_006_002_ReqDiscardCrossInstance - discard failed: crossInstanceId={}, errorCode={}",
                                    _msg.getCrossInstanceId(), _error);
                            _committer.commitFailRes(_error);
                        }
                    }
                });
    }
}
