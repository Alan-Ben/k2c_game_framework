using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 可追踪的任务接口
    /// </summary>
    public interface _IFollowableQuest
    {
        /// <summary> 任务类型 </summary>
        ENPFollowQuestType questType { get; }
        /// <summary> 唯一标识 </summary>
        long id { get; }
        /// <summary> 序号id </summary>
        long sortId { get; }
        /// <summary> 任务状态 </summary>
        ENPQuestStepStatusEnum questStepStatus { get; }
        /// <summary> 是否可以追踪 </summary>
        bool enableFollow { get; }
        /// <summary> 标题文本 </summary>
        string title { get; }
        /// <summary> 追踪显示的文本 </summary>
        string showStr { get; }
        /// <summary> 当前任务计数文本 </summary>
        string curCountStr { get; }
        /// <summary> 目标任务计数文本 </summary>
        string targetCountStr { get; }
        /// <summary> 任务进度文本 </summary>
        string progressStr { get; }
        /// <summary> 任务过期时间戳（秒），小于等于0则为永久 </summary>
        long expireTimeTagS { get; }

        /// <summary>
        /// 获取监听WinMsg列表
        /// </summary>
        /// <returns></returns>
        List<WinMsgType> getListenMsgList();

        /// <summary>
        /// 执行完成任务
        /// </summary>
        void dealFinish(Action<List<NPCommon_ItemInfo>> _callback);

        /// <summary>
        /// 执行前往
        /// </summary>
        void dealQuestGoto();
    }
}
