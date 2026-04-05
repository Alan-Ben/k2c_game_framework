package NPUSServer.CommonMarquee.Task;

import ALBasicServer.ALTask._IALSynTask;
import GS2GC.p007_CommOp.GS2GC_007_056_OnMarqueeAdd;
import NPUSServer.CommonMarquee.CommonMarqueeInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;

import java.util.ArrayList;

public class SynTask_OnMarqueeAddBroadcastPlayer implements _IALSynTask
{
    private NPUserServer _m_server;

    private int _m_showPosId;
    private CommonMarqueeInfo _m_marqueeInfo;

    public SynTask_OnMarqueeAddBroadcastPlayer(NPUserServer _server, int _showPosId, CommonMarqueeInfo _marqueeInfo)
    {
        _m_server = _server;

        _m_showPosId = _showPosId;
        _m_marqueeInfo = _marqueeInfo;
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
    		
    		GS2GC_007_056_OnMarqueeAdd proto = new GS2GC_007_056_OnMarqueeAdd();
            proto.setShowPosId(_m_showPosId);
            proto.setMarqueeInfo(_m_marqueeInfo.makeProto(userData.getSdkInfo().language));
            
            userData.sendMsgToGC(proto);
    	}
    }
}
