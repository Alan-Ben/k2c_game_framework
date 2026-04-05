package NPUSServer.USGroup.LocalActivityController;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPUSServer.NPUserServer;
import NPUSServer.USGroup.LocalActivityController.LogicUnit.ChangeCrossServerGroupLogicUnit;
import NPUSServer.USGroup.LocalActivityController.LogicUnit.DefaultScheduleLogicUnit;
import NPUSServer.USGroup.LocalActivityController.Task.GetServerBelongingCrossGroupTask;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 服务器本地活动控制器
 * 对活动及排期、分组等进行管理和处理
 * 每10秒进行一次检测并执行逻辑单元
 */
public class LocalActivityController
{
    private NPUserServer _m_server;

    //逻辑单元列表
    private List<_ALocalActivityLogicUnit> _m_logicUnitList;

    public LocalActivityController(NPUserServer _server)
    {
        _m_server = _server;

        _m_logicUnitList = new ArrayList<>();

        _m_logicUnitList.add(new ChangeCrossServerGroupLogicUnit(getUSServer()));
        _m_logicUnitList.add(new DefaultScheduleLogicUnit());
    }

    public NPUserServer getUSServer(){return _m_server;}

    public void onInited()
    {
        //根据优先级对逻辑单元进行排序
        _m_logicUnitList.sort(Comparator.comparingLong(_ALocalActivityLogicUnit::executePriority));

        //开启本地活动控制器tick任务
        ALSynTaskManager.getInstance().regTask(new LocalActivityControllerTickTask(getUSServer()));
        //查询所在分组
        ALSynTaskManager.getInstance().regTask(new GetServerBelongingCrossGroupTask(getUSServer()));
    }

    /**
     * 10s的tick用于执行队列
     */
    public synchronized void tick10Sec()
    {
        //是否重新进行一次循环
        boolean needRepeatLoop;
        //已循环次数(避免陷入死循环)
        int alreadyLoopTimes = 0;

        do
        {
            //重置循环标志位
            needRepeatLoop = false;

            //避免陷入死循环
            if (alreadyLoopTimes >= 100)
            {
                getUSServer().getDDAlert().warn("NPLocalActivityController tick warn", "NPLocalActivityController loop more than 100 times in tick10Sec");
                break;
            }
            //记录循环次数
            alreadyLoopTimes++;

            //遍历执行逻辑单元
            for (_ALocalActivityLogicUnit logicUnit : _m_logicUnitList)
            {
                //是否执行成功
                boolean isSuc = logicUnit.tryExecuteLogic();
                //如果执行成功,且需要重新执行一遍逻辑单元,则把循环标志位置为true
                if (isSuc && logicUnit.needBreakAndRepeatLoopIfPass())
                {
                    needRepeatLoop = true;
                    break;
                }
            }
        } while (needRepeatLoop);
    }
}
