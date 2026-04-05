package NPUSServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import NP2US_R.p001_BasicOp.NP2US_R_001_003_ChgRef;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GameResErr;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPGameRes.NPRefDataMgr;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_001_BasicOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2US_R_001_003_ChgRef extends _ABasicGeneralRequestDealer<NP2US_R_001_003_ChgRef>
{
    public RequestDealer_NP2US_R_001_003_ChgRef(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_001_003_ChgRef _msg)
    {
        // 通用配置
        RefContainerBase<?> refMgr = NPRefDataMgr.getInstance().lookupRefMgrByTableName(_msg.getTableName());
        if (null == refMgr)
        {
            USLog.error(getUSServer(), "NP2US_R_001_003_ChgRef can not find refMgr for class:" + _msg.getTableName());
            _committer.commitFailRes(GameResErr.GAME_RES_REF_TABLE_NOT_FOUND.getCode());
            return;
        }

        long keyV = Long.parseLong(_msg.getId());
        RefBase ref = refMgr.get(keyV);
        if (null == ref)
        {
            USLog.error(getUSServer(), "NP2US_R_001_003_ChgRef ref not found tableName:{} refId:{}", _msg.getTableName(), _msg.getId());
            _committer.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        boolean res = ref.gmSetValue(_msg.getKey(), _msg.getValue());
        if (!res)
        {
            USLog.error(getUSServer(), "NP2US_R_001_003_ChgRef ref set failed tableName:{} refId:{} value:{}", _msg.getTableName(), _msg.getId(), _msg.getValue());
            _committer.commitFailRes(GameResErr.GAME_RES_GM_CHG_FAIL.getCode());
            return;

        }

        ref.Assert();

        //返回结果
        _committer.commitSucRes(NP2US_RB_Writer_001_BasicOp.make_003_ChgRefRes(res));
    }
}
