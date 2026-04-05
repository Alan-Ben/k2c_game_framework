using System;

namespace GOE
{
    /// <summary>
    /// 限制数量加载任务的接口
    /// </summary>
    public interface _ICountLimitedLoaderTask
    {
        string debugName { get; }
        /* internal C# 8.0 特性 */ void load(Action _complete);
        /* internal C# 8.0 特性 */ void discard();
    }
}