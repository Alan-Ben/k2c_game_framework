package NPHsClient.HSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2HS_RB.p002_HsClientOp.ExeGmSuperResult;
import NP2HS_RB.p002_HsClientOp.NP2HS_RB_002_001_RetExecGmSuper;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPHsClient.HsClient;
import NPHsClient.HsClientMain;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

public class HSMsgDispather extends NPCustomMsgDispatcher
{
    private static HSMsgDispather _g_instance = new HSMsgDispather();

    public static HSMsgDispather getInstance()
    {
        return _g_instance;
    }

    public HSMsgDispather()
    {
        this.regHandler(new NPCustomMsgDealer<NP2HS_RB_002_001_RetExecGmSuper>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2HS_RB_002_001_RetExecGmSuper _msg)
            {
                HsClient dealer = (HsClient) _receiver;

                StringBuilder sb = new StringBuilder();
                sb.append("ServerType\tTypeId\tResult\tMessage\n");

                for (ExeGmSuperResult result : _msg.getResultList())
                {
                    sb.append(result.getServerType()).append("\t");
                    sb.append(result.getServerTypeId()).append("\t");
                    sb.append(result.getIsSucc() ? "true" : "false").append("\t");
                    sb.append(result.getResult()).append("\n");
                }
                String strResult = sb.toString();
                File file = new File(HsClientMain.outPutfileName);
                FileWriter fileWriter;
                try
                {
                    if (!file.exists())
                    {
                        file.createNewFile();
                    }

                    fileWriter = new FileWriter(file.getAbsoluteFile(), false);
                    BufferedWriter bw = new BufferedWriter(fileWriter);

                    bw.write(strResult);
                    bw.close();
                } catch (IOException e)
                {
                    e.printStackTrace();
                }
                System.out.print(strResult);
                dealer.exit();

            }
        });
    }
}