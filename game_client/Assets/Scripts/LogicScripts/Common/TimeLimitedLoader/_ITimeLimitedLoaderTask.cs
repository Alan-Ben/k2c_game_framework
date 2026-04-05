 
using System;


/// <summary>
/// 实现这个接口，可以被<see cref="TimeLimitedLoader"/>使用
/// </summary>
/// <remarks>
/// 需要注意！！！load 方法的 _complete 不应该调用另外一个 load，详细可以看 load 方法的 code 注释
/// </remarks>
public interface _ITimeLimitedLoaderTask
{
    /// <summary>
    /// 进行一小步加载
    /// </summary>
    /// <code>
    /// // 需要特别注意，_complete 回调，不可以依赖另外一个 load，比如下面是一个错误的实现
    /// void load(Action _complete)
    /// {
    ///     otherTask.regDoneDelegate(_complete);
    ///     TimeLimitedLoader.instance.addLoadTask(otherTask);
    /// }
    /// // 当执行 load 的时候，意味着 TimeLimitedLoader 正在处理当前这个任务，而不会同时处理其它任务，
    /// // 这时在内部再让 TimeLimitedLoader 处理其它任务就并不会被响应，整个 TimeLimitedLoader 就卡住了
    /// </code>
    void load(Action _complete);
    /// <summary>
    /// 加载时间过长时的提示字符串
    /// </summary>
    string debugTag { get; }
}
