package NPUSServer.USGroup;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_CrossServerGroupInfo;
import Common.NpServerObj.NPServerObj_CrossServerGroupInfo;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.ADelegateOne;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.LocalCrossServerGroupMgrBO;

import java.nio.ByteBuffer;
import java.util.List;

/**
 * US本地跨服分组管理器
 */
public class LocalCrossServerGroupMgr
{
    private NPUserServer _m_usUSServer;

    private LocalCrossServerGroupMgrBO _m_bo;
    //监听对象-跨服实例变更
    public final ADelegateOne<NPServerObj_CrossServerGroupInfo> OnCrossServerGroupChg;

    //当前分组信息
    private NPServerObj_CrossServerGroupInfo _m_curGroupInfo;
    //预备分组信息
    private NPServerObj_CrossServerGroupInfo _m_prepareGroupInfo;

    public LocalCrossServerGroupMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;
        OnCrossServerGroupChg = new ADelegateOne<>(this);
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    /**
     * 初始化数据
     * @return
     */
    public boolean initFromDB()
    {
        if (!_initBo())
            return false;

        return true;
    }

    /**
     * 初始化控制器数据
     * @return 是否成功
     */
    private boolean _initBo()
    {
        List<LocalCrossServerGroupMgrBO> boList = getUSServer().getBM().getBM(LocalCrossServerGroupMgrBO.class).s_findAll();
        if (boList == null)
            return false;

        LocalCrossServerGroupMgrBO bo = null;

        if (boList.isEmpty())
        {
            bo = new LocalCrossServerGroupMgrBO();
            bo.setCrossGroupId(getUSServer().getBM(), -1);
            bo.insert(getUSServer().getBM());
        } else
        {
            bo = boList.get(0);

            if (bo.getCrossGroupInfo() != null && bo.getCrossGroupInfo().length != 0)
            {
                _m_curGroupInfo = new NPServerObj_CrossServerGroupInfo();
                _m_curGroupInfo.readPackage(ByteBuffer.wrap(bo.getCrossGroupInfo()));
            }
        }

        _m_bo = bo;

        return true;
    }

    /**
     * 返回当前的分组id
     */
    public long getCrossGroupId()
    {
        return _m_bo.getCrossGroupId();
    }

    /**
     * 返回当前的分组的排行实例
     */
    public long getCrsInstanceId()
    {
        NPServerObj_CrossServerGroupInfo curGroupInfo = _m_curGroupInfo;
        return curGroupInfo == null ? 0 : curGroupInfo.getCrsInstanceId();
    }

    /**
     * 尝试切换分组
     * @return 是否成功
     */
    public boolean tryChangeGroup()
    {
        //检查是否存在预备分组
        if (_m_prepareGroupInfo == null)
            return false;

        //检查是否新的分组id小于原有分组id
        if (_m_prepareGroupInfo.getGroupId() <= getCrossGroupId())
            return false;

        //检查是否满足切换条件
        if (!checkCanChangeGroup())
            return false;

        _m_curGroupInfo = _m_prepareGroupInfo;
        _m_prepareGroupInfo = null;

        _m_bo.setCrossGroupId(getUSServer().getBM(), _m_curGroupInfo.getGroupId());
        _m_bo.setCrossGroupInfo(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_m_curGroupInfo.makePackage()));
        _m_bo.saveAllMarked(getUSServer().getBM());

        USLog.info(_m_usUSServer, "NPLocalCrossServerGroupMgr success chg cross group crossGroupId:{}", _m_curGroupInfo.getGroupId());

        //通知监听对象
        OnCrossServerGroupChg.onAsyncEvent(_m_curGroupInfo);

        //广播分组信息
        ALSynTaskManager.getInstance().regTask(() ->
                getUSServer().getUsUserMgr().broadCastMessage(US2GCWriter_007_CommOp.make_057_OnCrossServerGroupInfoChg(makeGroupInfo())));

        return true;
    }

    /**
     * 检查是否可以切换分组
     */
    public boolean checkCanChangeGroup()
    {
    	//TODO
//        return LocalActivityScheduleMgr.getInstance().isGroupScheduleAllEnd(getCrossGroupId());
    	
    	return true;
    }

    /**
     * 设置待切换分组信息
     * @param _groupInfo 新的分组信息
     */
    public void setPrepareChangeGroupInfo(NPServerObj_CrossServerGroupInfo _groupInfo)
    {
        //检查是否新的分组id小于原有分组id
        if (_groupInfo.getGroupId() <= getCrossGroupId())
            return;

        if (_m_prepareGroupInfo != null)
        {
            //如果当前的预分区信息相同,则不处理
            if (_m_prepareGroupInfo.getGroupId() == _groupInfo.getGroupId())
                return;

            if (_m_prepareGroupInfo.getGroupId() != _groupInfo.getGroupId())
            {
                getUSServer().getDDAlert().warn("NPLocalCrossServerGroupMgr", "NPLocalCrossServerGroupMgr setPrepareChangeGroupInfo repeat oldGroupId:"
                        + _m_prepareGroupInfo.getGroupId() + " newGroupId:" + _groupInfo.getGroupId());
            }
        }

        _m_prepareGroupInfo = _groupInfo;

        USLog.info(_m_usUSServer, "NPLocalCrossServerGroupMgr setPrepareChangeGroupInfo groupId:{}", _groupInfo.getGroupId());
    }

    /**
     * 获取当前分组信息
     * @return 当前分组信息
     */
    public Common_CrossServerGroupInfo makeGroupInfo()
    {
        NPServerObj_CrossServerGroupInfo curGroupInfo = _m_curGroupInfo;

        if (curGroupInfo == null)
            return new Common_CrossServerGroupInfo();

        Common_CrossServerGroupInfo groupInfo = new Common_CrossServerGroupInfo();
        groupInfo.setGroupId(curGroupInfo.getGroupId());
        groupInfo.getUsIdList().addAll(curGroupInfo.getUsIdList());

        return groupInfo;
    }
}
