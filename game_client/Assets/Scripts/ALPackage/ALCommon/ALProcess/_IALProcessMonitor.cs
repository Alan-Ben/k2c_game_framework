using System;
using System.Collections.Generic;

/*********************
 * 流程处理的监控对象
 * 目前可以对耗时、执行过程报错进行监控
 * 监控结果会在回调的重载函数中调用，区分出错步骤是通过带入的步骤名称进行区分处理的
 **/
namespace ALPackage
{
    public interface _IALProcessMonitor
    {
        /// <summary>
        /// 返回对应过程标记的监控时长
        /// </summary>
        /// <param name="_processTag"></param>
        /// <returns></returns>
        long monitorTimeMS(string _processTag);
        /// <summary>
        /// 在超过监控时长的时候触发的回调
        /// </summary>
        /// <param name="_processTag"></param>
        void onTimeout(long _processTimeMS, string _processTag, string _exInfo);
        /// <summary>
        /// 在完成步骤的时候，如果超过监控时长时调用的完成超时回调
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="_processTag"></param>
        void onTimeoutDone(long _processTimeMS, string _processTag, string _exInfo);
        /// <summary>
        /// 在出现错误的时候的步骤错误回调
        /// </summary>
        /// <param name="_processTag"></param>
        /// <param name="_ex"></param>
        void onErr(long _processTimeMS, string _processTag, string _exInfo, Exception _ex);
        /// <summary>
        /// 在某个过程因为失败卡住的时候调用的事件函数
        /// </summary>
        /// <param name="_process"></param>
        void onProcessFailStop(_AALProcess _process);
        /// <summary>
        /// 在根节点的过程被终止时触发的函数
        /// </summary>
        void onRootProecssStop();
        /// <summary>
        /// 在根节点的过程完成时执行的处理，此函数必定触发
        /// </summary>
        void onRootProecssDone();
    }
}
