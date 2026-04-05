package NPUSServer.CommonActivityMgr.Core.ActivityState;

/**
 * 活动变更状态
 * 用于区分活动是否完成了状态转换前的准备工作
 */
public enum EActivityTransState
{
    NONE,//无状态
    PROCESSING,//进行中
    DONE,//已完成
}
