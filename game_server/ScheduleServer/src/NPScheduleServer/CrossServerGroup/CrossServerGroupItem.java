package NPScheduleServer.CrossServerGroup;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._ITALProcessAction;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpServerObj.NPServerObj_CrossServerGroupInfo;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPScheduleServer.CrossServerGroup.WorkGroup.Task.ApplyCrossRankInstanceIdTask;
import NPScheduleServer.CrossServerGroup.WorkGroup.Task.CrossServerGroupInfoPushTask;
import NPScheduleServer.NPScheduleServer;
import SSDB.Bo.CrossServerGroupItemBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 跨服分组对象
 */
public class CrossServerGroupItem
{
    //数据库数据id
    protected long _m_dbId;
    //跨服分组id(由后台提供)
    protected long _m_groupId;
    //该分组包含的服务器id列表
    protected List<Integer> _m_usIdList;
    //对应的跨服排行集群Id
    protected long _m_crsGroupInstanceId;
    //是否已经推送
    protected boolean _m_hasPush;

    public CrossServerGroupItem(CrossServerGroupItemBO _bo, _ACrossServerGroupList _groupList)
    {
        _m_dbId = _bo.getId();
        _m_groupId = _bo.getGroupId();
        _m_usIdList = CommonFunc.listIntFromString(_bo.getUsIdList());
        _m_crsGroupInstanceId = _bo.getCrsGroupInstanceId();
        _m_hasPush = _bo.getHasPush();
    }

    public long getGroupId()
    {
        return _m_groupId;
    }

    public List<Integer> getUsIdList()
    {
        return _m_usIdList;
    }

    public boolean containsUs(int _usId)
    {
        return _m_usIdList.contains(_usId);
    }

    public long getCrsGroupInstanceId()
    {
        return _m_crsGroupInstanceId;
    }

    /**
     * 保存对应的跨服排行集群Id
     * @param _crsGroupInstanceId 跨服排行集群Id
     */
    public void saveCrsGroupInstanceId(long _crsGroupInstanceId)
    {
        _m_crsGroupInstanceId = _crsGroupInstanceId;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("crs_group_instance_id", _m_crsGroupInstanceId);
        NPScheduleServer.getInstance().getBM().getBM(CrossServerGroupItemBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 修改该跨服分组包含的服务器列表
     * @param _usIdList 该跨服分组包含的服务器列表
     */
    public void edit(ArrayList<Integer> _usIdList)
    {
        _m_usIdList = _usIdList;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("us_id_list", CommonFunc.list2String(_m_usIdList));
        NPScheduleServer.getInstance().getBM().getBM(CrossServerGroupItemBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 初始化分组的额外数据
     */
    public void initGroupExtInfoAndPush()
    {
        ALProcess process = ALProcess.CreateProcess("init_group_ext_info");

        //1.初始化对应的跨服排行集群Id
        process.addResDelegateProcess(this::initGroupCrossRankInfo, null, false);

        //2.推送分组信息
        //创建对应长度的process列表
        ALProcess[] list = new ALProcess[_m_usIdList.size()];
        for (int i = 0; i < _m_usIdList.size(); i++)
        {
            Integer usId = _m_usIdList.get(i);
            list[i] = ALProcess.CreateProcess("group_push_process_sub");
            list[i].addResDelegateProcess(action ->
            {
                //注册推送任务
                ALSynTaskManager.getInstance().regTask(new CrossServerGroupInfoPushTask(this, usId, action));
            }, "group_push_process_" + i);
        }
        process.addMultiProcess("group_push_process_main",list);

        //执行process初始化额外信息，成功后推送分组信息
        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                NPScheduleServer.getInstance().getDDAlert().err("CrossServerGroupItem initGroupExtInfoAndPush", "CrossServerGroupItem initGroupExtInfoAndPush process stop groupId:{}", _m_groupId);
            }

            @Override
            public void onRootProecssSuc()
            {
                markHasPush();
            }
        });
    }

    /**
     * 标记为已推送
     */
    public void markHasPush()
    {
        _m_hasPush = true;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("has_push", 1);
        NPScheduleServer.getInstance().getBM().getBM(CrossServerGroupItemBO.class).update("id", _m_dbId, updateValue);

        CommLog.info("CrossServerGroupItem markHasPush Suc, groupId:{}", _m_groupId);
    }

    /**
     * 请求分组的跨服排行实例id
     * @param _action process的回调
     */
    public void initGroupCrossRankInfo(_ITALProcessAction<Boolean> _action)
    {
        ALSynTaskManager.getInstance().regTask(new ApplyCrossRankInstanceIdTask(this, _action));
    }

    /**
     * 销毁数据
     */
    public void dispose()
    {
        NPScheduleServer.getInstance().getBM().getBM(CrossServerGroupItemBO.class).delAll("id", _m_dbId);
    }

    public NPServerObj_CrossServerGroupInfo toProto()
    {
        NPServerObj_CrossServerGroupInfo proto = new NPServerObj_CrossServerGroupInfo();
        proto.setGroupId(_m_groupId);
        proto.getUsIdList().addAll(_m_usIdList);
        proto.setCrsInstanceId(_m_crsGroupInstanceId);
        return proto;
    }

    public boolean hasPush()
    {
        return _m_hasPush;
    }
}
