package NPUSServer.CommonMarquee.Task;

import ALBasicServer.ALTask._IALSynTask;
import GS2GC.p007_CommOp.GS2GC_007_058_OnMarqueeDel;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;

import java.util.ArrayList;

public class SynTask_OnMarqueeDelBroadcastPlayer implements _IALSynTask
{
    private NPUserServer _m_server;

    private long _m_lMarqueeDbid;

    public SynTask_OnMarqueeDelBroadcastPlayer(NPUserServer _server, long _marqueeDbid)
    {
        _m_server = _server;

        _m_lMarqueeDbid = _marqueeDbid;
    }

    @Override
    public void run()
    {
    	ArrayList<NPUSUserData> userDataList = _m_server.getUsUserMgr().getAllCacheUserData();
    	
    	for(int i = 0; i < userDataList.size(); i++)
    	{
    		NPUSUserData userData = userDataList.get(i);
    		if(null == userData)
    			continue;
    		
    		GS2GC_007_058_OnMarqueeDel proto = new GS2GC_007_058_OnMarqueeDel();
            proto.setMarqueeDbid(_m_lMarqueeDbid);
            
            userData.sendMsgToGC(proto);
    	}
    }
}
