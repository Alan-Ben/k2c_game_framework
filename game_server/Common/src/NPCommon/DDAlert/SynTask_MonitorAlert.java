package NPCommon.DDAlert;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPEnum.ENPDDAlertType;

public class SynTask_MonitorAlert implements _IALSynTask
{
    private DDAlert _m_ddAlert;

    public SynTask_MonitorAlert(DDAlert _ddAlert)
    {
        _m_ddAlert = _ddAlert;
    }

    @Override
    public void run()
    {
        long mb = 1024 * 1024;
        long totalMem = Runtime.getRuntime().totalMemory();
        long freeMem = Runtime.getRuntime().freeMemory();
        long maxMem = Runtime.getRuntime().maxMemory();

        int usedPercent = (int) ((1.0f * (totalMem - freeMem) / maxMem) * 10000);
        float used = 1f * usedPercent / 100;

        //内存利用率超过80%或者在线数突破警戒阈值
        if (used >= 85)
        {
            StringBuilder sb = new StringBuilder();
            sb.append("used memory: ").append(used).append("%");
            sb.append(", left: ").append((maxMem - totalMem + freeMem) / mb).append("m");
            sb.append(", max: ").append(maxMem / mb).append("m");

            ALServerLog.ALServerLog.LogLevel logLvl;
            if (used >= 95)
            {
                logLvl = ALServerLog.ALServerLog.LogLevel.FATAL;
            } else
            {
                logLvl = ALServerLog.ALServerLog.LogLevel.WARNING;
            }

            _m_ddAlert.sendToHS(ENPDDAlertType.MEMORY, logLvl, sb.toString());
        }

        ALSynTaskManager.getInstance().regTask(this, 30000);
    }
}
