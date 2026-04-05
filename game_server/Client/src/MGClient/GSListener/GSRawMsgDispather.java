package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import MGClient.ClientRequestMgr.ClientRequestMgr;
import MGClient.ProtocolFactory.ResponseProtocolFactory;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGS2GC.p001_BasicOp.*;

import java.nio.ByteBuffer;

public class GSRawMsgDispather extends NPCustomMsgDispatcher
{
    private static GSRawMsgDispather _g_instance = new GSRawMsgDispather();

    public static GSRawMsgDispather getInstance()
    {
        return _g_instance;
    }

    public GSRawMsgDispather()
    {
        this.regHandler(new NPCustomMsgDealer<NPGS2GC_001_022_SendSerializeMsg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_022_SendSerializeMsg _msg)
            {
                //WCGGSListener dealer = (WCGGSListener) _receiver;
                boolean dealSucc = GSMsgDispather.getInstance().DealProtocol(_receiver, ByteBuffer.wrap(_msg.getMsg()));
                if (!dealSucc)
                {
                    ByteBuffer byteBuffer = ByteBuffer.wrap(_msg.getMsg());

                    _dealLogOutput((GSListener) _receiver, byteBuffer);

                }
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGS2GC_001_001_RetBasicInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_001_RetBasicInfo _msg)
            {

                //GlobalStorage.getInstance().saveRecentServerId(_msg.getUsServerId());
            }
        });
        this.regHandler(new NPCustomMsgDealer<NPGS2GC_001_023_RetClientRequest>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_023_RetClientRequest _msg)
            {
                //WCGGSListener dealer = (WCGGSListener) _receiver;
                ByteBuffer buffer = _msg.get_buffer_MsgBuffer();
                if (!ClientRequestMgr.getInstance().dealRequest(_msg.getClientRequestSerialize(), _msg.getErrCode(), buffer))
                {
                    boolean dealSucc = GSMsgDispather.getInstance().DealProtocol(_receiver, _msg.get_buffer_MsgBuffer());
                    if (!dealSucc)
                    {
                        ByteBuffer byteBuffer = _msg.get_buffer_MsgBuffer();

                        _dealLogOutput((GSListener) _receiver, byteBuffer);
                    }
                }
            }
        });
        this.regHandler(new NPCustomMsgDealer<NPGS2GC_001_012_RetMostRecommendedUSInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_012_RetMostRecommendedUSInfo _msg)
            {
                GSMsgDispather.getInstance().DealProtocol(_receiver, _msg.makeFullPackage());
            }
        });
        this.regHandler(new NPCustomMsgDealer<NPGS2GC_001_011_RetPlayerJoinedUSList>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_011_RetPlayerJoinedUSList _msg)
            {
                GSMsgDispather.getInstance().DealProtocol(_receiver, _msg.makeFullPackage());
            }
        });
        this.regHandler(new NPCustomMsgDealer<NPGS2GC_001_005_EnterUSRes>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_005_EnterUSRes _msg)
            {
                GSMsgDispather.getInstance().DealProtocol(_receiver, _msg.makeFullPackage());
            }
        });
        this.regHandler(new NPCustomMsgDealer<NPGS2GC_001_007_RetQueueInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_007_RetQueueInfo _msg)
            {
                GSMsgDispather.getInstance().DealProtocol(_receiver, _msg.makeFullPackage());
            }
        });
        this.regHandler(new NPCustomMsgDealer<NPGS2GC_001_004_OnUSEnterDone>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_004_OnUSEnterDone _msg)
            {
                GSMsgDispather.getInstance().DealProtocol(_receiver, _msg.makeFullPackage());
            }
        });
    }

    private static void _dealLogOutput(GSListener _receiver, ByteBuffer byteBuffer)
    {
        _IALProtocolStructure protocol = ResponseProtocolFactory.getInstance().createProtocol(byteBuffer.get(), byteBuffer.get());
        if (protocol != null)
        {
            try
            {
                protocol.readPackage(byteBuffer);
            } catch (Exception e)
            {
                CommLog.error("ClientProtoLogger readPackage error:", e);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.append(protocol.getClass().getSimpleName());
            sb.append(":\n");
            CommonFunc.GetInfoPropertys(protocol, sb);
            GSListener dealer = _receiver;
            dealer.getOwner().logProto(sb.toString());
        }
    }
}