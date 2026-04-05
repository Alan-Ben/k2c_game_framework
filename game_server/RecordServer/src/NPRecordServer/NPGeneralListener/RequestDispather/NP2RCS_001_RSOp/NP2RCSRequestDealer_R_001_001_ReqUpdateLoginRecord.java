package NPRecordServer.NPGeneralListener.RequestDispather.NP2RCS_001_RSOp;

import NP2LCS_R.p001_BasicOp.NP2RCS_R_001_001_ReqUpdateLoginRecord;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPRecordServer.NPRecordMgr.RecordInfoListMgr;
import NPServerProtocolWriter.NP2RCS.RequestBack.NP2RCS_RB_Writer_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * @description: 更新登录记录
 * @author: ricci
 * @date: 2022-06-27 15:23:05
 */
public class NP2RCSRequestDealer_R_001_001_ReqUpdateLoginRecord extends NPRequestDealer<NP2RCS_R_001_001_ReqUpdateLoginRecord>
{


    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2RCS_R_001_001_ReqUpdateLoginRecord _msg)
    {
        //更新数据
        RecordInfoListMgr.getInstance().updateLoginRecord(_msg.getAccountId(), _msg.getCid(), _msg.getServerLogicId(), _msg.getServerTypeId());

        _receiver.commitSucRes(NP2RCS_RB_Writer_001_BasicOp.make_001_001_RetUpdateLoginRecord());
    }
}
