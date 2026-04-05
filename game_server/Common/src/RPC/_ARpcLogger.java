package RPC;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

public abstract class _ARpcLogger
{
    private final HashSet<Integer> _m_needLogRpcIdSet = new HashSet<>();

    public _ARpcLogger()
    {
        List<Integer> idList = getNeedLogRpcIdList();
        for (Integer classId : idList)
        {
            _m_needLogRpcIdSet.add(classId);
        }
    }

    protected abstract List<Integer> getNeedLogRpcIdList();


    public void logSucc(_ARPCData _rpcData)
    {
        if (!checkNeedLog(_rpcData.getClassId()))
        {
            return;
        }
        _doLog(_rpcData, true, "");
    }

    public boolean checkNeedLog(int classId)
    {
        return _m_needLogRpcIdSet.contains(classId);
    }

    public void logFail(_ARPCData _rpcData, String _errMsg)
    {
        if (!checkNeedLog(_rpcData.getClassId()))
        {
            return;
        }
        _doLog(_rpcData, false, _errMsg);
    }

    public void _doLog(_ARPCData rpcData, boolean _isSucc, String _errMsg)
    {
        ALSynTaskManager.getInstance().regTask(() ->
        {
            String strReq = rpcData.getRequestString();
            String strRet = rpcData.getResponseString();
            doLog(rpcData.getClassId(), strReq, strRet, _isSucc, _errMsg);
        });
    }

    public abstract void doLog(int _classId, String _strReq, String _strRet, boolean _isSucc, String _errMsg);

    public void addClassId(int _classId)
    {
        _m_needLogRpcIdSet.add(_classId);
    }

    public void removeClassId(int _classId)
    {
        _m_needLogRpcIdSet.remove(_classId);
    }

    public List<Integer> getIdList()
    {
        return new ArrayList<>(_m_needLogRpcIdSet);
    }

}
