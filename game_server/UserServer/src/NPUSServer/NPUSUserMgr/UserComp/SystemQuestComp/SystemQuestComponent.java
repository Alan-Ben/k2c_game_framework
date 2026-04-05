package NPUSServer.NPUSUserMgr.UserComp.SystemQuestComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.QuestObj.SystemQuest_Info;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Quest.SystemQuest.RefSystemQuest;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerSystemQuestGroupBO;

import java.util.ArrayList;
import java.util.List;

public class SystemQuestComponent extends _ANPUserComponent
{
    //组id -> 任务组信息
    private List<SystemQuestGroupInfo> _m_questGroupList;

    public SystemQuestComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.SYSTEM_QUEST);

        _m_questGroupList = new ArrayList<>();
    }

    public void _lock()
    {
        getUserData().lockUser();
    }

    public void _unlock()
    {
        getUserData().unlockUser();
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("init_system_quest_comp");

        //1.初始化任务
        process.addResDelegateProcess(_action -> _initGroupFromDB(_action::dealAction),
                "init_quest_from_db", null, false);

        //执行初始化逻辑
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        for (Long groupId : RefSystemQuest.getMgr().getQuestGroupList())
        {
            if (lookupGroup(groupId) != null)
             continue;

            //创建任务组信息
            SystemQuestGroupInfo groupInfo = new SystemQuestGroupInfo(this, groupId);
            _m_questGroupList.add(groupInfo);
        }

        //遍历注册任务组任务监听
        _m_questGroupList.forEach(SystemQuestGroupInfo::_regEvtEntry);
    }

    @Override
    public void dispose()
    {
        //遍历反注册任务组任务监听
        _m_questGroupList.forEach(SystemQuestGroupInfo::_unRegEvtEntry);
    }

    /**
     * 从数据库初始化任务数据
     * @param _callBack _ICallBackBool
     */
    private void _initGroupFromDB(_ICallBackBool _callBack)
    {
        getUSServer().getBM().getBM(PlayerSystemQuestGroupBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerSystemQuestGroupBO>>()
        {
            @Override
            public void dealSuc(List<PlayerSystemQuestGroupBO> _boList)
            {
                for (PlayerSystemQuestGroupBO bo : _boList)
                {
                    if (bo == null)
                        continue;

                    if (lookupGroup(bo.getGroupId()) != null)
                    {
                        USLog.error(getUSServer(), "SystemQuestComponent _initGroupFromDB groupId:{} already exist, cid:{} groupId:{}",
                                getUserData().getCid(), bo.getGroupId());
                        continue;
                    }

                    //创建任务组信息
                    SystemQuestGroupInfo groupInfo = new SystemQuestGroupInfo(SystemQuestComponent.this, bo);
                    _m_questGroupList.add(groupInfo);
                }

                _callBack.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _callBack.onRunOver(false);
            }
        });
    }


    /**
     * 根据组id查找分组管理器
     * @param _groupId 组id
     */
    public SystemQuestGroupInfo lookupGroup(long _groupId)
    {
        getUserData().lockUser();
        try
        {
            for (SystemQuestGroupInfo groupInfo : _m_questGroupList)
            {
                if (groupInfo.getGroupId() == _groupId)
                {
                    return groupInfo;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造协议
     * @param _infoList
     */
    public void makeProtocol(ArrayList<SystemQuest_Info> _infoList)
    {
        getUserData().lockUser();
        try
        {
            for (SystemQuestGroupInfo groupInfo : _m_questGroupList)
            {
                _infoList.add(groupInfo.toProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查系统任务组是否已完成
     * @param _groupId 任务组ID
     * @return 是否完成
     */
    public boolean isSystemQuestGroupDone(long _groupId)
    {
        getUserData().lockUser();
        try
        {
            // 查找任务组
            SystemQuestGroupInfo groupInfo = lookupGroup(_groupId);
            if (groupInfo == null)
            {
                // 任务组不存在，检查是否有该组的配置
                List<RefSystemQuest> questList = RefSystemQuest.getMgr().getQuestListByGroupId(_groupId);
                if (questList == null || questList.isEmpty())
                    return false; // 配置不存在，未完成

                // 任务组存在配置但玩家还没开始，未完成
                return false;
            }

            // 检查当前任务组是否已完成所有步骤
            RefSystemQuest currentRef = RefSystemQuest.getMgr().getQuestRef(_groupId, groupInfo.getStep());
            if (currentRef == null)
            {
                // 当前步骤配置不存在，说明已超过最后一步，任务组完成
                return true;
            }

            // 还有后续步骤，未完成
            return false;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否已完成指定的系统任务 - 根据任务ID判断指定任务是否已完成
     * 
     * 执行流程：
     * 1. 根据任务ID查询任务配置，获取任务组ID和步骤号
     * 2. 查找玩家对应的任务组信息
     * 3. 比较玩家当前任务组的进度与目标步骤
     * 4. 返回是否已完成该指定任务
     * 
     * @param _id 系统任务ID
     * @return 是否已完成指定任务
     * 
     * 线程安全：通过getUserData().lockUser()保护任务组列表访问
     */
    public boolean hadDoneSystemQuest(long _id)
    {
        getUserData().lockUser();
        try
        {
            // 1. 根据任务ID查询配置获取组ID和步骤
            RefSystemQuest questRef = RefSystemQuest.getMgr().get(_id);
            if (questRef == null)
                return false;
            
            long groupId = questRef.group_id;
            int stepId = questRef.step;
            
            // 2. 查找玩家对应的任务组信息
            SystemQuestGroupInfo groupInfo = lookupGroup(groupId);
            if (groupInfo == null)
            {
                // 玩家还没开始该任务组，未完成
                return false;
            }
            
            // 3. 比较当前步骤与目标步骤
            // 如果玩家当前步骤大于目标步骤，说明已完成指定任务
            return groupInfo.getStep() > stepId;
            
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
