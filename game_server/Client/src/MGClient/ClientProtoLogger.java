package MGClient;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import GS2GC.p007_CommOp.GS2GC_007_051_OnCommError;
import MGClient.GSListener.GSListener;
import NPCommon.Dispather._AWCGProtoLogger;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Util.CommonFunc;


public class ClientProtoLogger extends _AWCGProtoLogger
{
    @Override
    public void logProto(_IALProtocolReceiver _receiver, _IALProtocolStructure _msg)
    {
        if (_receiver instanceof GSListener)
        {
            GSListener dealer = (GSListener) _receiver;
            if (checkIsSkipp(_msg.getMainOrder(), _msg.getSubOrder()))
            {
                return;
            }

            String protoName = _msg.getClass().getSimpleName();
            if (protoName.compareToIgnoreCase("NPGS2GC_001_022_SendSerializeMsg") == 0)
                return;
            if (protoName.compareToIgnoreCase("NPGS2GC_001_023_RetClientRequest") == 0)
                return;
            if(_msg.getClass() == GS2GC_007_051_OnCommError.class)
            {
                GS2GC_007_051_OnCommError msg = (GS2GC_007_051_OnCommError)_msg;
                if(msg.getErrCode()>0)
                {
                    Result result = ResultMgr.getInstance().lookupResult(msg.getErrCode());
                    if(result!=null)
                    {
                        dealer.getOwner().logProto(result.toString());
                    }
                    else {
                        dealer.getOwner().logProto("unknown Error Code:"+msg.getErrCode());
                    }
                    return;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.append(protoName);
            sb.append(":\n");
            CommonFunc.GetInfoPropertys(_msg, sb);
            dealer.getOwner().logProto(sb.toString());
        }

    }

    private boolean checkIsSkipp(byte mainOrder, byte subOrder)
    {
        if (mainOrder == 4 && subOrder == 2) //GS2GC_004_002_RetGmCommand
            return true;

        return false;
    }

}
