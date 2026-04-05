using System;
using Common.EventObj;

namespace GOE
{
    /// <summary>
    /// 事件处理时需要用到的外部操作
    /// </summary>
    public interface _ICommonSimpleEventDealExtOp
    {
        /// <summary>
        /// 请求处理事件方法
        /// </summary>
        Action<byte[], Action<bool, byte[]>> reqDealEvent { get; }
        
        /// <summary>
        /// 自动处理事件方法
        /// </summary>
        Action<byte[], Action<bool, byte[]>> reqAutoDealEvent { get; }
        
        /// <summary>
        /// 自动处理事件方法，不需要操作数据
        /// </summary>
        Action<Action<bool, byte[]>> reqAutoDealEventWithNoOp { get; }
        
        /// <summary>
        /// 通过处理事件回包获取事件处理结果
        /// </summary>
        Func<byte[], CommonEvent_DoneInfo> getEventDealDoneInfoByRetDealEventMsg { get; }
        
        /// <summary>
        /// 通过自动处理事件回包获取事件自动处理结果
        /// </summary>
        Func<byte[], CommonEvent_DoneInfo> getEventDealDoneInfoByRetAutoDealEventMsg { get; }
        
        /// <summary>
        /// 通过自动处理事件回包获取事件自动处理结果，不需要操作数据的结果
        /// </summary>
        Func<byte[], CommonEvent_DoneInfo> getEventDealDoneInfoByRetAutoWithNoOpDealEventMsg { get; }

        /// <summary>
        /// 处理事件时是否需要开启遮罩
        /// </summary>
        bool dealEventNeedOpenTransBk { get; }
        /// <summary>
        /// 是否是纯UI节点
        /// </summary>
        bool dealEventIsOnlyUINode { get; }
    }
}