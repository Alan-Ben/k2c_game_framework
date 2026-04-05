using System;

namespace GOE
{
    /// <summary>
    /// 通用事件处理接口
    /// </summary>
    public interface _IEventDealAgent
    {
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="_startDeal">开始处理回调</param>
        /// <param name="_dealDone">处理完成回调</param>
        /// <param name="_break">中断处理回调</param>
        void dealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break);

        /// <summary>
        /// 数据层面事件是否已经完成
        /// </summary>
        bool isEventDoneDataLevel { get; }

        /// <summary>
        /// 自动处理方法
        /// </summary>
        /// <param name="_dealDone">处理完成回调</param>
        /// <param name="_break">中断处理回调</param>
        void autoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break);
    }
}