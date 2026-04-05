package NPScheduleServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPScheduleServer.NPGeneralListener.RequestDispather.p001_CrossServerGroupOp.*;

public class NPSSGeneral_001_RequestDispatcher_CrossServerGroupOp extends NPRequestDispatcher
{
    public static void init(NPSSGeneralRequestDispather _dispather)
    {
        //////////////////////////////// 分组系统相关协议  ////////////////////////////////
        _dispather.regHandler(new NPSSGeneralRequest_001_001_GetServerBelongingCrossGroup());

        //////////////////////////////// 排期系统相关协议  ////////////////////////////////
        _dispather.regHandler(new NPSSGeneralRequest_001_010_ReportUsActivityScheduleDone());
        _dispather.regHandler(new NPSSGeneralRequest_001_011_GetUsActivityScheduleList());
        _dispather.regHandler(new NPSSGeneralRequest_001_012_ReportUsActivityScheduleSettling());
        _dispather.regHandler(new NPSSGeneralRequest_001_013_CmdAddActivitySchedule());
        _dispather.regHandler(new NPSSGeneralRequest_001_014_QueryActivityByUs());
        _dispather.regHandler(new NPSSGeneralRequest_001_015_QueryActivityByUsAndActivityId());
        _dispather.regHandler(new NPSSGeneralRequest_001_016_ReportUsActivitySchedulePlaying());
        _dispather.regHandler(new NPSSGeneralRequest_001_017_CmdPushResUpdateInfo());

    }
}
