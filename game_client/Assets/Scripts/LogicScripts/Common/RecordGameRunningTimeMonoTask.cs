using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 记录游戏时间任务
    /// </summary>
    public class RecordGameRunningTimeMonoTask : _IALBaseMonoTask
    {
        //记录时间标记
        private float _m_fRunningTimeTag = 0;

        public void deal()
        {
            //已经发送过埋点不再记录
            if (GameSetting.instance.getIsSendGameRunningTime())
                return;

            if (_m_fRunningTimeTag == 0)
            {
                //Time.time游戏暂停后时间也会暂停
                _m_fRunningTimeTag = Time.time;
            }
            else if(Time.time - _m_fRunningTimeTag > 60f)
            {
                _m_fRunningTimeTag = Time.time;

                //每经过1分钟，记录一次时间到本地存储
                GameSetting.instance.addGameRunningTimeMin(1);

                //游戏运行超过半小时，上报埋点，关闭任务
                if (GameSetting.instance.getGameRunningTimeMin() >= 30)
                {
                    GCommon.sendStepReport(TraceConst.TIME_30);
                    GameSetting.instance.setIsSendGameRunningTime(true);
                    return;
                }
            }

            ALMonoTaskMgr.instance.addMonoTask(this, 1f);
        }
    }
}