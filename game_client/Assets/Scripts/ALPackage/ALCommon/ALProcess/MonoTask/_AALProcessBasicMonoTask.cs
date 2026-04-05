using System;
using System.Collections.Generic;

/*********************
 * 步骤管理器中使用到的monotask的基类，抽象了设置监控对象的接口
 **/
namespace ALPackage
{
    public abstract class _AALProcessBasicMonoTask : _IALBaseMonoTask
    {
        /// <summary>
        /// 基类任务函数
        /// </summary>
        public abstract void deal();

        /// <summary>
        /// 内部逻辑中部分脱节情况下可以在通过本函数设置monitor初始值
        /// 以此在task调用的时候设置步骤监控对象的调用方式
        /// </summary>
        public abstract void setMonitor(_IALProcessMonitor _monitor);
    }
}
