using System;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 用于监控和处理任务关联的接口对象
    /// </summary>
    public interface _IALCommonTaskMonitorInterface
    {
        /// <summary>
        /// 释放本任务对象的相关资源或者关联
        /// </summary>
        void discard();
    }
}
